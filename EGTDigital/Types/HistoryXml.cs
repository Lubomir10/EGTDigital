using System.Xml.Serialization;

namespace EGTDigital.Types
{
    public class HistoryXml
    {
        [XmlAttribute(AttributeName = "consumer")]
        public string Consumer { get; set; }

        [XmlAttribute(AttributeName = "currency")]
        public string Currency { get; set; }

        [XmlAttribute(AttributeName = "period")]
        public string Period { get; set; }
    }
}
