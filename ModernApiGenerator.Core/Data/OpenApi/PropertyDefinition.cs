namespace ModernApiGenerator.Core.Data.OpenApi
{
	public class PropertyDefinition
	{

		public PropertyDefinition(string name, ParameterDetails details)
		{
			Name = name;
			Details = details;
		}

		public string Name { get; }

		public ParameterDetails Details { get; }
	}
}
