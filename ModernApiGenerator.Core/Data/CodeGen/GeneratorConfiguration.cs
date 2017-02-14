using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModernApiGenerator.Core.CodeGen.Builders;

namespace ModernApiGenerator.Core.Data.CodeGen
{
	public class GeneratorConfiguration
	{
		public GeneratorConfiguration(string projectName, PropertyBuilder propertyBuilder)
		{
			ProjectName = projectName;
			PropertyBuilder = propertyBuilder;
		}

		public string ProjectName { get; }

		public PropertyBuilder PropertyBuilder { get; }
	}
}
