using System.Collections.Generic;

namespace ModernApiGenerator.Core.Data.OpenApi
{
	public class ApiResourceDefinition
	{
		public ApiResourceDefinition(string resourceName, IEnumerable<ApiMethodDefinition> methodDefinitions)
		{
			MethodDefinitions = methodDefinitions;
			ResourceName = resourceName;
		}

		public string ResourceName { get; }

		public IEnumerable<ApiMethodDefinition> MethodDefinitions { get; }

		public string CSharpStyleFormattedResourceName => ResourceName.Replace("_", "").Replace("-", "").Replace(" ", "");
	}


}
