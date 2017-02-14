using System;
using System.Net.Http;

namespace ModernApiGenerator.Core.Processor.Parsers
{
	public class HttpMethodParser
	{
		public HttpMethod Parse(string httpMethodString)
		{
			switch (httpMethodString.ToLower())
			{
				case "get":
					return HttpMethod.Get;
				case "post":
					return HttpMethod.Post;
				case "put":
					return HttpMethod.Put;
				case "delete":
					return HttpMethod.Delete;
				case "head":
					return HttpMethod.Head;
				case "options":
					return HttpMethod.Options;
				case "trace":
					return HttpMethod.Trace;
                case "patch":
			        return new HttpMethod("PATCH");
			}

			throw new InvalidOperationException($"There is no {nameof(HttpMethod)} with name: {httpMethodString}.");
		}
	}
}
