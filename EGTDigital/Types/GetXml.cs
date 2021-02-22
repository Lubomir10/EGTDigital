using System.Xml.Serialization;

namespace EGTDigital.Types
{
    public class GetXml
    {
        [XmlAttribute(AttributeName = "consumer")]
        public string Consumer { get; set; }

        [XmlElement(ElementName = "currency")]
        public string Currency { get; set; }
    }
}
