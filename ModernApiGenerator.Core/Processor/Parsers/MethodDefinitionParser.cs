using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using ModernApiGenerator.Core.Data.OpenApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ModernApiGenerator.Core.Processor.Parsers
{
	public class MethodDefinitionParser
	{
		private readonly HttpMethodParser httpMethodParser = new HttpMethodParser();

		public ApiMethodDefinition Parse(string resourcePath, string methodType, IEnumerable<JProperty> methodDefinitionJsonObject)
		{
			HttpMethod httpMethod = httpMethodParser.Parse(methodType);

			var summary = GetSummary(methodDefinitionJsonObject);
			var description = GetDescription(methodDefinitionJsonObject);
			var parameters = GetParameters(methodDefinitionJsonObject).ToList();


			return new ApiMethodDefinition()
			{
				Path = resourcePath,
				MethodType = httpMethod,
				Description = description,
				Summary = summary,
				Parameters = parameters
			};
		}

		private static string GetDescription(IEnumerable<JProperty> methodDefinitionJsonObject)
		{
			var descriptionJToken =
				methodDefinitionJsonObject.FirstOrDefault(x => x.Name.Equals("description", StringComparison.OrdinalIgnoreCase))?
					.Value;

			string description = string.Empty;

			if (descriptionJToken != null)
				description = descriptionJToken.Value<string>();
			return description;
		}

		private static string GetSummary(IEnumerable<JProperty> methodDefinitionJsonObject)
		{
			var summaryJToken =
				methodDefinitionJsonObject.FirstOrDefault(x => x.Name.Equals("summary", StringComparison.OrdinalIgnoreCase))?.Value;

			string summary = string.Empty;

			if (summaryJToken != null)
				summary = summaryJToken.Value<string>();
			return summary;
		}

		private static IEnumerable<ApiMethodParameter> GetParameters(IEnumerable<JProperty> methodDefinitionJsonObject)
		{
			var parameterJArray =
				methodDefinitionJsonObject.FirstOrDefault(x => x.Name.Equals("parameters", StringComparison.OrdinalIgnoreCase))?
					.Value as JArray;

			if (parameterJArray == null)
				yield break;

			foreach (var parameter in parameterJArray)
				yield return parameter.ToObject<ApiMethodParameter>();
		}
	}
}
