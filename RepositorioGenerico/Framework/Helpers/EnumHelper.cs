using System;

namespace RepositorioGenerico.Framework.Helpers
{
	public static class EnumHelper
	{

		public static T FromString<T>(string value)
		{
			return (T)Enum.Parse(typeof(T), value, true);
		}

	}
}
