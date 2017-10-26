using System.Collections.Concurrent;
using RepositorioGenerico.Dictionary;

namespace RepositorioGenerico.SqlClient.Scripts
{
	internal static class Cache
	{

		private static readonly ConcurrentDictionary<Dicionario, Script> Scripts = new ConcurrentDictionary<Dicionario, Script>();

		public static Script Consultar(Dicionario dicionario)
		{
			return Scripts.GetOrAdd(dicionario, CriarScriptParaObjeto);
		}

		private static Script CriarScriptParaObjeto(Dicionario dicionario)
		{
			return new Script(
				Builder.CriarScriptInsert(dicionario),
				Builder.CriarScriptUpdate(dicionario),
				Builder.CriarScriptDelete(dicionario),
				Builder.CriarScriptAutoIncremento(dicionario));
		}

	}
}
