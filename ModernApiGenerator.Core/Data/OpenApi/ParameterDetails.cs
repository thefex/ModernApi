using System;
using Newtonsoft.Json;

namespace ModernApiGenerator.Core.Data.OpenApi
{
	public class ParameterDetails
	{
		[JsonConstructor]
		public ParameterDetails(string type, string format)
		{
			Type = type ?? "NULL";
			Format = format ?? "NULL";
		}



	    public string Type { get; }

		public string Format { get; }

        public string Reference { get; }

		public Type GetParameterType()
		{
			if (Type.Equals("integer"))
			{
				if (Format.Equals("int32"))
					return typeof(int);
				if (Format.Equals("int64"))
					return typeof(long);
			}

			if (Type.Equals("number"))
			{
				if (Format.Equals("float"))
					return typeof(float);
				if (Format.Equals("double"))
					return typeof(double);
			}

			if (Type.Equals("boolean"))
				return typeof(bool);

			if (Type.Equals("string"))
			{
				if (Format.Equals("byte"))
					return typeof(byte[]);
				if (Format.Equals("binary"))
					return typeof(string);
				if (Format.Equals("date"))
					return typeof(DateTime);
				if (Format.Equals("date-time"))
					return typeof(DateTime);
				if (Format.Equals("password"))
					return typeof(string);
				return typeof(string);
			}

		    throw new InvalidOperationException("Type: " + Type + " of format: " + Format + " is not implemented.");
		}
	}
}