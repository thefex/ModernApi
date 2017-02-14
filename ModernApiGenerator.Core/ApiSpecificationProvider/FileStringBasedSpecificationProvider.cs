using System.Threading.Tasks;
using ModernApiGenerator.Core.Data.Responses.Base;

namespace ModernApiGenerator.Core.ApiSpecificationProvider
{
	// used mainly during development to "speed-up" testing process..
	// could be useful during issue-fix (perhaps inside unit test project?)
	public class FileStringBasedSpecificationProvider : IOpenApiSpecificationProvider
	{
		private readonly string _filePath;

		public FileStringBasedSpecificationProvider(string filePath)
		{
			_filePath = filePath;
		}

		public Task<Response<string>> GetSpecificationAsJsonString()
		{
			return Task.FromResult(new Response<string>(System.IO.File.ReadAllText(_filePath)));
		}
	}
}
