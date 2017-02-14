using Microsoft.CodeAnalysis.CSharp.Syntax;
using ModernApiGenerator.Core.Data.OpenApi;
using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace ModernApiGenerator.Core.CodeGen.Builders
{
	public class PropertyBuilder
	{
		public MemberDeclarationSyntax BuildParameterSyntax(PropertyDefinition propertyDefinition)
		{
			PropertyDeclarationSyntax property =
				SF.PropertyDeclaration(SF.ParseTypeName(propertyDefinition.Details.GetParameterType().ToString()), propertyDefinition.Name)
					.AddModifiers(SF.Token(Microsoft.CodeAnalysis.CSharp.SyntaxKind.PublicKeyword))
					.AddAccessorListAccessors(
						SF.AccessorDeclaration(Microsoft.CodeAnalysis.CSharp.SyntaxKind.GetAccessorDeclaration)
							.WithSemicolonToken(SF.Token(Microsoft.CodeAnalysis.CSharp.SyntaxKind.SemicolonToken)),
						SF.AccessorDeclaration(Microsoft.CodeAnalysis.CSharp.SyntaxKind.SetAccessorDeclaration)
							.WithSemicolonToken(SF.Token(Microsoft.CodeAnalysis.CSharp.SyntaxKind.SemicolonToken))
					);

			return property;
		}
	}
}
