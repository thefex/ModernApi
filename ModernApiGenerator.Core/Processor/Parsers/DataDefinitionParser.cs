using System.Collections.Generic;
using System.Linq;
using ModernApiGenerator.Core.Data.OpenApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ModernApiGenerator.Core.Processor.Parsers
{
	public class DataDefinitionParser
	{
		public DataDefinitionParser()
		{
			
		}

		public DataDefinition Parse(KeyValuePair<string, JToken> dataDefinitionJsonObject)
		{
			return new DataDefinition()
			{
				Name = dataDefinitionJsonObject.Key,
				Properties = GetModelProperties(dataDefinitionJsonObject).ToList()
			};
		}

	    private IEnumerable<PropertyDefinition> GetModelProperties(KeyValuePair<string, JToken> dataDefinitionJsonObject)
	    {
            if (!dataDefinitionJsonObject.Value.Where(x => x is JProperty).Cast<JProperty>().Any(x => x.Name.Equals("properties")))
                yield break;

	        JsonConvert.DefaultSettings =
	            () => new JsonSerializerSettings() {PreserveReferencesHandling = PreserveReferencesHandling.Objects , ReferenceLoopHandling = ReferenceLoopHandling.Error};
            foreach (var propertyValue in dataDefinitionJsonObject.Value["properties"].Cast<JProperty>())
            {
                var propertyName = propertyValue.Name;
                var parameterDetails = propertyValue.Value.ToObject<ParameterDetails>();

                yield return new PropertyDefinition(propertyName, parameterDetails);
            }
        }

	}
}
