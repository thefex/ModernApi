using System.Collections.Generic;
using ModernApiGenerator.Core.Data.OpenApi;

namespace ModernApiGenerator.Core.Data.Responses
{
	public sealed class ApiDefinitionProcessorResponse
	{
		public ApiDefinitionProcessorResponse(IEnumerable<DataDefinition> dataModels, IEnumerable<ApiResourceDefinition> methodDefinition)
		{
			DataModels = dataModels;
			MethodDefinition = methodDefinition;
		}
		public string Host { get; set; }

		public string BasePath { get; set; }

		public IEnumerable<DataDefinition> DataModels { get; }

		public IEnumerable<ApiResourceDefinition> MethodDefinition { get; }
	}
}
