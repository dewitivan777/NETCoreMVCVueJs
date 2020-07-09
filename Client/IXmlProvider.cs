using System.Collections.Generic;
using System.Xml.Linq;

namespace Client
{
   public interface IXmlProvider
    {
        XDocument Serialize<T>(List<T> items, string rootElement);
        T Deserialize<T>(string uri, string rootElement = null);
    }
}
