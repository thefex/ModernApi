using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ModernApiGenerator.Core.Data.Responses.Base;

namespace ModernApiGenerator.Core.ApiSpecificationProvider
{
    public class UrlBasedSpecificationProvider : IOpenApiSpecificationProvider
    {
        private readonly string _url;

        public UrlBasedSpecificationProvider(string url)
        {
            _url = url;
        }

        public async Task<Response<string>> GetSpecificationAsJsonString()
        {
            HttpClient httpClient = new HttpClient();

            string apiSpecification = await httpClient.GetStringAsync(_url);
            return new Response<string>(apiSpecification);
        }
    }
}
