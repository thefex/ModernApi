using System;
using ModernApiGenerator.Core;
using ModernApiGenerator.Core.ApiSpecificationProvider;
using ModernApiGenerator.Core.CodeGen;
using ModernApiGenerator.Core.CodeGen.Builders;
using ModernApiGenerator.Core.Data.CodeGen;
using ModernApiGenerator.Core.Processor;

namespace ModernApi
{
    internal class Program
    {
        private const int versionBase = 1;
        private const int versionRevision = 0;

        private static void Main(string[] args)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Green;
                var generatorService = BuildGeneratorService();


                Console.WriteLine("Przemysław Raciborski [thefex] - Swagger/OpenAPI Refit Code Generator " + versionBase +
                                  "." + versionRevision);
                if (args.Length != 2)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(
                        "You have to pass two launch arguments. \n" +
                        "1. File path or http/https url to Swagger/OpenAPI doc. \n" +
                        "2. Path to save output generated code. \n" +
                        "-------------------------------------------\n");

                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("EXAMPLE: \n" +
                        "ModernApi.exe https://raw.githubusercontent.com/OAI/OpenAPI-Specification/master/examples/v2.0/json/petstore.json C:/MySecretProject/API/");
                    return;
                }

                var path = args[0];
                var specificationProvider = new SpecificationProviderFactory().Build(path);
                var outputResponse = generatorService.GenerateApiCode(specificationProvider).Result;

                if (!outputResponse.IsSuccess)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(outputResponse.FormattedErrorMessages);
                    return;
                }

                Console.WriteLine("Successfully generated API!");
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.ToString());
            }
            finally
            {
                ResetColors();
                Console.ReadLine();

            }
        }

        private static void ResetColors()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine("");
        }

        private static OpenApiGeneratorService BuildGeneratorService()
        {
            var codeGenerator = new RefitOpenApiCodeGenerator(
                new GeneratorConfiguration("TestProject",
                    new PropertyBuilder()),
                new RefitMethodGenerator());


            return new OpenApiGeneratorService(
                new OpenApiDefinitionProcessor(),
                codeGenerator
                );
        }
    }
}