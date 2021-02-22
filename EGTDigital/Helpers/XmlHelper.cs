using System.IO;
using System.Xml.Serialization;

namespace EGTDigital.Helpers
{

    //TODO: DELETE
    public static class XmlHelper
    {
        public static T XmlDeserializeFromString<T>(string objectData)
        {
            var serializer = new XmlSerializer(typeof(T));

            using (var reader = new StringReader(objectData))
            {
                return (T)serializer.Deserialize(reader);
            }
        }
    }
}
