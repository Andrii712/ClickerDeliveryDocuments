using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ClickerDeliveryDocuments.MicrosoftOfficeUtil
{
    internal static class DataUtil
    {
        // Need a calendar.  Culture's irrelevent since we specify start day of week
        internal static Calendar cal = CultureInfo.InvariantCulture.Calendar;

        // Return CultureInfo object.
        internal static CultureInfo GetCultureInfoProvider(string name = "en-US")
        {
            //  Culture-specific formatting information (names): "en-US", "uk-UA", "de-DE".
            return new CultureInfo(name);
        }
    }
}
