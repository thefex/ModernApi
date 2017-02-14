using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace ModernApiGenerator.Core.Data.OpenApi.Specification
{
	internal class OpenApiJsonSpecification
	{
		public string Host { get; set; }

		public string BasePath { get; set; }

		public JObject Paths { get; set; }
		public JObject Definitions { get; set; }
	}
}
