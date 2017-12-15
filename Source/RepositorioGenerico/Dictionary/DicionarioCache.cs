using System;
using System.Collections.Concurrent;

namespace RepositorioGenerico.Dictionary
{

	public static class DicionarioCache
	{

		private static readonly ConcurrentDictionary<Type, Dicionario> Dicionarios = new ConcurrentDictionary<Type, Dicionario>();

		public static Dicionario Consultar(Type tipo)
		{
			return Dicionarios.GetOrAdd(tipo, CriarDicionarioGenerico);
		}

		private static Dicionario CriarDicionarioGenerico(Type tipo)
		{
			if (typeof(Entities.Entidade).IsAssignableFrom(tipo))
				return new Dicionario(tipo);
			return null;
		}

	}

}
