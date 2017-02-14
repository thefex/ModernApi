using System.Threading.Tasks;
using ModernApiGenerator.Core.ApiSpecificationProvider;
using ModernApiGenerator.Core.CodeGen;
using ModernApiGenerator.Core.Data.Responses;
using ModernApiGenerator.Core.Data.Responses.Base;
using ModernApiGenerator.Core.Processor;

namespace ModernApiGenerator.Core
{
	public class OpenApiGeneratorService
	{
		private readonly OpenApiDefinitionProcessor _apiSpecificationProcessor;
		private readonly OpenApiCodeGenerator _apiCodeGenerator;

		public OpenApiGeneratorService(OpenApiDefinitionProcessor apiSpecificationProcessor, OpenApiCodeGenerator apiCodeGenerator)
		{
			_apiSpecificationProcessor = apiSpecificationProcessor;
			_apiCodeGenerator = apiCodeGenerator;
		}

		public async Task<Response<ApiGeneratorResponse>> GenerateApiCode(IOpenApiSpecificationProvider specificationProvider)
		{
			var specificationResponse = await specificationProvider.GetSpecificationAsJsonString()
				.ConfigureAwait(false);

			if (!specificationResponse.IsSuccess)
				return specificationResponse.CloneFailedResponse<ApiGeneratorResponse>();

			var processedSpecificationResponse = await _apiSpecificationProcessor.Process(specificationResponse.Results)
				.ConfigureAwait(false);

			if (!processedSpecificationResponse.IsSuccess)
				return processedSpecificationResponse.CloneFailedResponse<ApiGeneratorResponse>();

			var apiCodeGeneratorResponse = await _apiCodeGenerator.GenerateApiCode(processedSpecificationResponse.Results)
				.ConfigureAwait(false);

			return apiCodeGeneratorResponse;
		}
	}
}
