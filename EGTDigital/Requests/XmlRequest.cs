using EGTDigital.Types;
using System.Xml.Serialization;

namespace EGTDigital.Requests
{
    [XmlRoot(ElementName = "command")]
    public class XmlRequest
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }

        [XmlElement(ElementName = "get")]
        public GetXml Get { get; set; }

        [XmlElement(ElementName = "history")]
        public HistoryXml History { get; set; }
    }
}
