using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModernApiGenerator.Core.Data.CodeGen;

namespace ModernApiGenerator.Core.Data.Responses
{
	public class ApiGeneratorResponse
	{
		public ApiGeneratorResponse(IEnumerable<GeneratedCode> cSharpCodeOutput)
		{
			CSharpCodeOutput = cSharpCodeOutput;
		}

		public IEnumerable<GeneratedCode> CSharpCodeOutput { get; }
	}
}
