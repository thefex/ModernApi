using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using ModernApiGenerator.Core.Data.CodeGen;
using ModernApiGenerator.Core.Data.OpenApi;
using ModernApiGenerator.Core.Data.Responses;
using ModernApiGenerator.Core.Data.Responses.Base;
using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace ModernApiGenerator.Core.CodeGen
{
	public abstract class OpenApiCodeGenerator
	{
		private readonly GeneratorConfiguration _configuration;

		protected OpenApiCodeGenerator(GeneratorConfiguration configuration)
		{
			_configuration = configuration;
		}

		protected string DataNamespace => $"{_configuration.ProjectName}.Data";

		protected string ApiMethodNamespace => $"{_configuration.ProjectName}.API";

		public async Task<Response<ApiGeneratorResponse>> GenerateApiCode(
			ApiDefinitionProcessorResponse fromApiDefinition)
		{
			var generatedCode = new List<GeneratedCode>();

			var generatorTasks = new List<Task>();
			foreach (var dataModel in fromApiDefinition.DataModels)
				generatorTasks.Add(Task.Run(() => generatedCode.Add(BuildDataModel(dataModel))));
			foreach (var methodDefinition in fromApiDefinition.MethodDefinition)
				generatorTasks.Add(Task.Run(() => generatedCode.Add(BuildMethodDefinition(methodDefinition))));

			await Task.WhenAll(generatorTasks.ToArray());
			return new Response<ApiGeneratorResponse>(new ApiGeneratorResponse(generatedCode));
		}

		protected GeneratedCode BuildDataModel(DataDefinition dataDefinition)
			=> BuildCode(BuildDataModelCompilationUnitSytanx(dataDefinition), DataNamespace, dataDefinition.Name);

		protected GeneratedCode BuildMethodDefinition(ApiResourceDefinition resourceDefinition)
			=> BuildCode(BuildMethodDefinitionCompilationUnitSyntax(resourceDefinition), ApiMethodNamespace, string.Empty);

		private GeneratedCode BuildCode(CompilationUnitSyntax compilationUnitSyntax, string @namespace, string name)
		{
			var adhocWorkspace = new AdhocWorkspace();

			var options = adhocWorkspace.Options;
			options = options.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInMethods, false);
			options = options.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInTypes, false);
			var formattedSyntaxNode = Formatter.Format(compilationUnitSyntax, adhocWorkspace, options);

			var codeStringBuilder = new StringBuilder();
			using (var writer = new StringWriter(codeStringBuilder))
				formattedSyntaxNode.WriteTo(writer);

			return new GeneratedCode
			{
				CSharpCode = codeStringBuilder.ToString(),
				Namespace = @namespace,
				Name = name
			};
		}

		protected virtual CompilationUnitSyntax BuildDataModelCompilationUnitSytanx(DataDefinition dataDefinition)
		{
			NamespaceDeclarationSyntax namespaceDeclaration = SF.NamespaceDeclaration(SF.IdentifierName(DataNamespace));

		    var compilationUnitSyntax = SF.CompilationUnit()
		        .AddUsings(SF.UsingDirective(SF.IdentifierName("System")))
		        .AddUsings(SF.UsingDirective(SF.IdentifierName("System.Collections.Generic")));

		    ClassDeclarationSyntax classDeclarationSyntax = SF.ClassDeclaration(dataDefinition.Name);

		    foreach (var dataDefinitionProperty in dataDefinition.Properties)
		        classDeclarationSyntax =
                    classDeclarationSyntax.AddMembers(_configuration.PropertyBuilder.BuildParameterSyntax(dataDefinitionProperty));

		    namespaceDeclaration = namespaceDeclaration.AddMembers(classDeclarationSyntax);
		    return compilationUnitSyntax.AddMembers(namespaceDeclaration);
		}

		protected abstract CompilationUnitSyntax BuildMethodDefinitionCompilationUnitSyntax(ApiResourceDefinition methodDefinition);
	}
}