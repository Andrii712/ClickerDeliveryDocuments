using System;

namespace ClickerDeliveryDocuments.Models
{
    public class DocumentHyperlinkModel
    {
        public Uri Url { get; set; }
        public string LinkTitle { get; set; } = String.Empty;
    }
}
