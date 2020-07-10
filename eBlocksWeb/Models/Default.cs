using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBlocksWeb.Models
{
    public class Default
    {
        public static string GetClassificationEndpoint(string typeName)
        {
            return "/service/classificationservice/" + typeName.ToLower();
        }

        public static string GetProductEndpoint(string typeName)
        {
            return "/service/ProductService/" + typeName.ToLower();
        }
    }
}
