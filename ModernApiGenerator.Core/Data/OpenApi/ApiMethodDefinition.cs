using System.Collections.Generic;
using System.Net.Http;

namespace ModernApiGenerator.Core.Data.OpenApi
{
	public class ApiMethodDefinition
	{
		public HttpMethod MethodType { get; internal set; }

		public string Path { get; internal set; }

		public string Description { get; internal set; }

		public string Summary { get; internal set; }

		public IEnumerable<ApiMethodParameter> Parameters { get; internal set; }
	}


}
