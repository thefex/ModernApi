using Microsoft.CodeAnalysis.CSharp.Syntax;
using ModernApiGenerator.Core.CodeGen.Builders;
using ModernApiGenerator.Core.Common;
using ModernApiGenerator.Core.Data.CodeGen;
using ModernApiGenerator.Core.Data.OpenApi;
using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace ModernApiGenerator.Core.CodeGen
{
	public class RefitOpenApiCodeGenerator : OpenApiCodeGenerator
	{
		private readonly RefitMethodGenerator _refitMethodGenerator;

		public RefitOpenApiCodeGenerator(GeneratorConfiguration configuration, RefitMethodGenerator refitMethodGenerator) : base(configuration)
		{
			_refitMethodGenerator = refitMethodGenerator;
		}

		protected override CompilationUnitSyntax BuildMethodDefinitionCompilationUnitSyntax(ApiResourceDefinition methodDefinition)
		{
			NamespaceDeclarationSyntax namespaceDeclaration = SF.NamespaceDeclaration(SF.IdentifierName(ApiMethodNamespace));

			var compilationUnitSyntax = SF.CompilationUnit()
				.AddUsings(SF.UsingDirective(SF.IdentifierName("System")))
				.AddUsings(SF.UsingDirective(SF.IdentifierName("System.Collections.Generic")))
				.AddUsings(SF.UsingDirective(SF.IdentifierName(DataNamespace)))
				.AddUsings(SF.UsingDirective(SF.IdentifierName(RefitConstants.RefitNamespace)))
				.AddMembers(namespaceDeclaration);

			InterfaceDeclarationSyntax interfaceDeclarationSyntax = SF.InterfaceDeclaration($"{methodDefinition.CSharpStyleFormattedResourceName}Api");
			return compilationUnitSyntax.AddMembers(interfaceDeclarationSyntax);
		}
	}
}