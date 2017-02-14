using System.Threading.Tasks;
using ModernApiGenerator.Core.Data.Responses.Base;

namespace ModernApiGenerator.Core.ApiSpecificationProvider
{
	public interface IOpenApiSpecificationProvider
	{
		Task<Response<string>> GetSpecificationAsJsonString();
	}
}