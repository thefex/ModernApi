using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ModernApiGenerator.Core.Data.OpenApi;
using ModernApiGenerator.Core.Data.OpenApi.Specification;
using ModernApiGenerator.Core.Data.Responses;
using ModernApiGenerator.Core.Data.Responses.Base;
using ModernApiGenerator.Core.Processor.Parsers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ModernApiGenerator.Core.Processor
{
	public class OpenApiDefinitionProcessor
	{
		private readonly DataDefinitionParser dataDefinitionParser = new DataDefinitionParser();
		private readonly MethodDefinitionParser methodDefinitionParser = new MethodDefinitionParser();

		public OpenApiDefinitionProcessor()
		{
			
		}

		public async Task<Response<ApiDefinitionProcessorResponse>> Process(string openApiDefinitionJson)
		{
			try
			{
				OpenApiJsonSpecification specificationObject = null;
				await Task.Run(() =>
				{
					specificationObject = JsonConvert.DeserializeObject<OpenApiJsonSpecification>(openApiDefinitionJson);
				}).ConfigureAwait(false);

				var dataDefinitions = GetDataDefinitions(specificationObject.Definitions).ToList();
				var resourceDefinitions = GetResourceDefinitions(specificationObject.Paths).ToList();

				var apiDefinitionProcessorResponse = new ApiDefinitionProcessorResponse(dataDefinitions, resourceDefinitions);

				return new Response<ApiDefinitionProcessorResponse>(apiDefinitionProcessorResponse);
			}
			catch (JsonException e)
			{
				return new Response<ApiDefinitionProcessorResponse>()
					.AddErrorMessage("This does not look like a valid specification object.\n----FAILED WITH STACKTRACE:---\n")
					.AddErrorMessage(e.ToString());

			}
		}

		private IEnumerable<DataDefinition> GetDataDefinitions(JObject definitionJsonObject)
		{
			foreach (var definition in definitionJsonObject)
				yield return dataDefinitionParser.Parse(definition);
		}

		private IEnumerable<ApiResourceDefinition> GetResourceDefinitions(JObject paths)
		{
			foreach (var resourcePathJson in paths)
			{
				var resourceName = resourcePathJson.Key.Trim('/').Split('/').First();

				List<ApiMethodDefinition> apiMethodDefinition = new List<ApiMethodDefinition>();
				string resourcePath = resourcePathJson.Key;

				foreach (var methodForResource in resourcePathJson.Value.Cast<JProperty>())
					apiMethodDefinition.Add(methodDefinitionParser.Parse(resourcePath, methodForResource.Name, methodForResource.Value.Cast<JProperty>()));

				yield return new ApiResourceDefinition(resourceName, apiMethodDefinition);
			}
		}



	}
}
