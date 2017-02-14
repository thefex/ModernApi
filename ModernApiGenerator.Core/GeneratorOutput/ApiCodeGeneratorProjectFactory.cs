using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.Text;
using ModernApiGenerator.Core.Data.CodeGen;
using ModernApiGenerator.Core.Data.Responses;
using ModernApiGenerator.Core.Data.Responses.Base;

namespace ModernApiGenerator.Core.GeneratorOutput
{
	public class ApiCodeGeneratorProjectFactory
	{
		private readonly OutputProjectConfiguration _projectConfig;

		public ApiCodeGeneratorProjectFactory(OutputProjectConfiguration projectConfig)
		{
			_projectConfig = projectConfig;
		}

		public async Task<Response> CreateProject(ApiGeneratorResponse generatorResponse)
		{
            string[] names = typeof(ApiCodeGeneratorProjectFactory).Assembly.GetManifestResourceNames();
            ExtractEmbeddedResource(_projectConfig.OutputPath, string.Empty, new List<string>()
		    {
		        "ModernApi.Template.NetCore.csproj",
		        "ModernApi.Template.NetCore.nuget.targets",
		        "project.json",
		        "project.lock.json"
		    });
		    ExtractEmbeddedResource(Path.Combine(_projectConfig.OutputPath, "Properties"), string.Empty, new List<string>()
		    {
		        "AssemblyInfo.cs"
		    });

		    var workspace = MSBuildWorkspace.Create();
		    var project = await workspace.OpenProjectAsync(Path.Combine(_projectConfig.OutputPath, "ModernApi.Template.NetCore.csproj"));

            foreach (var generatedCode in generatorResponse.CSharpCodeOutput)
            {
                var generatedCodeSourceText = SourceText.From(generatedCode.CSharpCode);
                var generatedCodeDocument = project.AddDocument(generatedCode.Name, generatedCodeSourceText);

                
            }

            return null;
        }
		

        private static void ExtractEmbeddedResource(string outputDir, string resourceLocation, IEnumerable<string> files)
        {
            foreach (string file in files)
            {
                using (System.IO.Stream stream = typeof(ApiCodeGeneratorProjectFactory).Assembly.GetManifestResourceStream(resourceLocation + @"." + file))
                {
                    using (System.IO.FileStream fileStream = new System.IO.FileStream(System.IO.Path.Combine(outputDir, file), System.IO.FileMode.Create))
                    {
                        for (int i = 0; i < stream.Length; i++)
                        {
                            fileStream.WriteByte((byte)stream.ReadByte());
                        }
                        fileStream.Close();
                    }
                }
            }
        }
    }
}
