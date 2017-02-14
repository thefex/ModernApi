using System.Collections.Generic;

namespace ModernApiGenerator.Core.Data.OpenApi
{
	public class DataDefinition
	{
		public string Name { get; internal set; }

		public IEnumerable<PropertyDefinition> Properties { get; internal set; }

	}
}
