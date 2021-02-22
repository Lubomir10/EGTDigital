using System.Xml.Serialization;

namespace EGTDigital.Types
{
    public class CommandXml
    {
        [XmlElement(ElementName = "id")]
        public string Id;
    }
}
