using System;
using System.Collections.Generic;
using System.Reflection;

namespace RepositorioGenerico.Framework.Helpers
{
	public static class AttributeHelper
	{

		public static TTipo Consultar<TTipo>(Type origem) where TTipo : Attribute
		{
			foreach (var atributo in origem.GetCustomAttributes(typeof(TTipo), false))
				if (atributo is TTipo)
					return (TTipo) atributo;
			return null;
		}

		public static TTipo Consultar<TTipo>(PropertyInfo origem) where TTipo : Attribute
		{
			foreach (var atributo in origem.GetCustomAttributes(typeof(TTipo), false))
				if (atributo is TTipo)
					return (TTipo)atributo;
			return null;
		}

		public static TTipo Consultar<TTipo>(FieldInfo origem) where TTipo : Attribute
		{
			foreach (var atributo in origem.GetCustomAttributes(typeof(TTipo), false))
				if (atributo is TTipo)
					return (TTipo)atributo;
			return null;
		}

		public static IEnumerable<TTipo> ConsultarTodos<TTipo>(Type origem) where TTipo : Attribute
		{
			foreach (var atributo in origem.GetCustomAttributes(true))
				if (atributo is TTipo)
					yield return (TTipo)atributo;
		}

		public static IEnumerable<TTipo> ConsultarTodos<TTipo>(PropertyInfo origem) where TTipo : Attribute
		{
			foreach (var atributo in origem.GetCustomAttributes(true))
				if (atributo is TTipo)
					yield return (TTipo)atributo;
		}

	}
}
