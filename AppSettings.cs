using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiValidator
{
    public class AppSettings
    {
        public string BaseUrl { get; set; }
        public string ProviderUserToken { get; set; }
        public string BearerToken { get; set; }
        public string ContentType { get; set; }

        public string FetchEndPoint { get; set; }

        public string CreateEndPoint { get; set; }

        public string GetEndPoint { get; set; }
    }
}
