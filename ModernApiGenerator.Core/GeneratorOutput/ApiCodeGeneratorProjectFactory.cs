using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.Text;
using ModernApiGenerator.Core.Data.CodeGen;
using ModernApiGenerator.Core.Data.Responses;
using ModernApiGenerator.Core.Data.Responses.Base;

namespace ModernApiGenerator.Core.GeneratorOutput
{
    public class ApiCodeGeneratorProjectGenerator
    {
        private readonly OutputProjectConfiguration _projectConfig;

        public ApiCodeGeneratorProjectGenerator(OutputProjectConfiguration projectConfig)
        {
            _projectConfig = projectConfig;
        }

        public async Task<Response> CreateProject(ApiGeneratorResponse generatorResponse)
        {
            var names = typeof(ApiCodeGeneratorProjectGenerator).Assembly.GetManifestResourceNames();
            var resourcesPartOfPath = "";
            ExtractEmbeddedResource(Path.Combine(_projectConfig.OutputPath, "ModernApi.Template.NetCore"), string.Empty, new List<string>
            {
                "ModernApiGenerator.Core.ProjectGeneratorData.ModernApi.Template.NetCore.nuget.targets",
                "ModernApiGenerator.Core.ProjectGeneratorData.project.json",
                "ModernApiGenerator.Core.ProjectGeneratorData.project.lock.json",
                "ModernApiGenerator.Core.ProjectGeneratorData.ModernApi.Template.NetCore.csproj"
            });
            ExtractEmbeddedResource(Path.Combine(_projectConfig.OutputPath, "ModernApi.Template.NetCore", "Properties"), string.Empty,
                new List<string>
                {
                    "ModernApiGenerator.Core.ProjectGeneratorData.Properties.AssemblyInfo.cs"
                });
            ExtractEmbeddedResource(Path.Combine(_projectConfig.OutputPath, "Sln"), string.Empty, new List<string>()
            {
                "ModernApiGenerator.Core.ProjectGeneratorData.ModernApi.Template.sln"
            });


            var workspace = MSBuildWorkspace.Create();
            var solution =
                await workspace.OpenSolutionAsync(Path.Combine(_projectConfig.OutputPath, "Sln", "ModernApi.Template.sln"));

            var project =
                solution.Projects.First();


            foreach (var generatedCode in generatorResponse.CSharpCodeOutput)
            {
                var generatedCodeSourceText = SourceText.From(generatedCode.CSharpCode);
                var generatedCodeDocument = project.AddDocument(generatedCode.Name, generatedCodeSourceText, generatedCode.Namespace.Split('.').ToList());
                
                project = generatedCodeDocument.Project;
            }

            if (!workspace.TryApplyChanges(project.Solution))
                return new Response().AddErrorMessage("Adding generated classes/interfaces files to new solution failed.");

            return new Response();
        }


        private static void ExtractEmbeddedResource(string outputDir, string resourceLocation, IEnumerable<string> files)
        {
            foreach (var file in files)
            {
                using (var stream = typeof(ApiCodeGeneratorProjectGenerator).Assembly.GetManifestResourceStream(file))
                {
                    if (!Directory.Exists(outputDir))
                        Directory.CreateDirectory(outputDir);

                    using (var fileStream = new FileStream(Path.Combine(outputDir, file
                        .Replace("ModernApiGenerator.Core.ProjectGeneratorData.Properties.", "")
                        .Replace("ModernApiGenerator.Core.ProjectGeneratorData.", "")), FileMode.Create))
                    {
                        for (var i = 0; i < stream.Length; i++)
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