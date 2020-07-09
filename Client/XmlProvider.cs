using System;
using System.Collections.Generic;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Client
{
    public class XmlProvider : IXmlProvider
    {
        public T Deserialize<T>(string uri, string rootElement = null)
        {
            try
            {
                XmlRootAttribute rootAttribute = null;
                if (rootElement != null)
                {
                    rootAttribute = new XmlRootAttribute(rootElement);
                }
                var doc = XDocument.Load(uri);
                using (var reader = doc.CreateReader())
                {
                    var serializer = new XmlSerializer(typeof(T), rootAttribute);
                    if (serializer.CanDeserialize(reader))
                        return (T)serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
                when (ex is WebException webEx
                      && webEx.Response is HttpWebResponse webResponse
                      && webResponse.StatusCode == HttpStatusCode.NotFound)
            {
            }
            return default;
        }

        public XDocument Serialize<T>(List<T> items, string rootElement)
        {
            XDocument doc = new XDocument();

            using (XmlWriter writer = doc.CreateWriter())
            {
                writer.WriteStartElement(rootElement);

                foreach (var item in items)
                {
                    new XmlSerializer(typeof(T)).Serialize(writer, item);
                }

                writer.WriteEndElement();
            }

            return doc;
        }
    }
}
