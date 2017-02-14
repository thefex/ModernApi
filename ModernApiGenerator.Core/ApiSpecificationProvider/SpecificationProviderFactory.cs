namespace ModernApiGenerator.Core.ApiSpecificationProvider
{
    public class SpecificationProviderFactory
    {
        public IOpenApiSpecificationProvider Build(string forPath)
        {
            if (forPath.StartsWith("http://") || forPath.StartsWith("https://"))
                return new UrlBasedSpecificationProvider(forPath);

            return new FileStringBasedSpecificationProvider(forPath);
        }
    }
}