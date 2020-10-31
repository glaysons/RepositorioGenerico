using System;
using RepositorioGenerico.Dictionary;
using RepositorioGenerico.Entities;
using RepositorioGenerico.Pattern.Contextos.Tables;
using System.Data;

namespace RepositorioGenerico.SqlClient.Contextos.Tables
{
	public class Contexto : ContextoBase, IContextoTransacional
	{

		public IRepositorio Repositorio<TObjeto>() where TObjeto : class, IEntidade
		{
			var tipo = typeof(TObjeto);
			return (IRepositorio)Repositorios.GetOrAdd(tipo, t => CriarNovoRepositorio<TObjeto>());
		}

		public Contexto(string stringConexao)
			: base(stringConexao)
		{

		}

		public Contexto(string stringConexao, IDbTransaction transacao)
			: base(stringConexao, transacao)
		{

		}

		private object CriarNovoRepositorio<TObjeto>() where TObjeto : class, IEntidade
		{
			var tipoRepositorio = typeof(Repositorio<>);
			var tipoGenerico = tipoRepositorio.MakeGenericType(typeof(TObjeto));
			var dicionario = DicionarioCache.Consultar(typeof(TObjeto));
			var persistencia = new Persistencia<TObjeto>(dicionario);
			return (IRepositorio)Activator.CreateInstance(tipoGenerico, this, persistencia);
		}

	}
}
