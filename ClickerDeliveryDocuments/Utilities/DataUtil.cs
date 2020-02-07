using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace ClickerDeliveryDocuments.Utilities
{
    internal static class DataUtil
    {
        // Need a calendar.  Culture's irrelevent since we specify start day of week.
        internal static Calendar cal = CultureInfo.InvariantCulture.Calendar;

        // Culture information provider by default for application.
        internal static CultureInfo defaultCultureProvider = DataUtil.GetCultureInfoProvider();


        /// <summary>
        /// Application culture names.
        /// </summary>
        internal struct CulterName
        {
            internal const string en_US = "en-US";
            internal const string uk_UA = "uk-UA";
            internal const string de_DE = "de-DE";
        }

        /// <summary>
        /// Create CultureInfo instance which based on the culture specified by name.
        /// </summary>
        /// <param name="name">A predefined System.Globalization.CultureInfo name: en_US, uk_UA, de_DE.</param>
        /// <returns>The instance of CultureInfo.</returns>
        internal static CultureInfo GetCultureInfoProvider(string name = CulterName.en_US)
        {
            //  Culture-specific formatting information (names): "en-US", "uk-UA", "de-DE".
            return new CultureInfo(name);
        }

        /// <summary>
        /// Displays the message window which present a message as an error to the user 
        /// about an incorect parameter value that was typed. 
        /// </summary>
        /// <param name="value">The typed value.</param>
        internal static void ShowErrorMessageInputParameters(object value = null)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("An error occurred: typed an incorrect value of the parameter.");

            if (value != null)
                stringBuilder.Append($"\nThe value is typed: \"{value.ToString()}\"");

            stringBuilder.Append("\nCheck input parameters.");

            MessageBox.Show(stringBuilder.ToString(),
                    String.Format(defaultCultureProvider, "Error"),
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
        }

        /// <summary>
        /// Displays the message window which present a message as an error to the user.
        /// </summary>
        /// <param name="message">The error's text description.</param>
        internal static void ShowErrorMessage(string message = "")
        {
            MessageBox.Show(message,
                String.Format(defaultCultureProvider, "Error"),
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        /// <summary>
        /// Convert string value to Int32 equivalent.
        /// </summary>
        /// <param name="value">The string value which need converting to Itn32.</param>
        /// <returns>Int32 value</returns>
        internal static int StringToInt32(string value)
        {
            int number = 0;
            try
            {
                number = Int32.Parse(value, NumberStyles.Integer, defaultCultureProvider);
            }
            catch
            {
                ShowErrorMessageInputParameters(value);
            }
            return number;
        }

        /// <summary>
        /// Checks the input value is correct.
        /// </summary>
        /// <param name="input">The string to search for a disallowed match and null or empty value.</param>
        /// <returns>true if the input value is only numeric; otherwise, false.</returns>
        internal static bool IsNumericValue(string input)
        {
            // The regular expression pattern that matches disallowed text. 
            string regexPattern = "[^0-9]+";

            if (String.IsNullOrEmpty(input) || (Regex.IsMatch(input, regexPattern)))
                return false;
            else
                return true;
        }

        /// <summary>
        /// Call the garbage collector for forces a garbage collection.
        /// </summary>
        internal static void CallGarbageCollector()
        {
            // Forces an immediate garbage collection of all generations.
            GC.Collect();

            // Suspends the current thread until the thread that is processing the queue of finalizers has emptied that queue.
            GC.WaitForPendingFinalizers();
        }
    }
}
