using System;
using RepositorioGenerico.Dictionary;
using RepositorioGenerico.Entities;
using RepositorioGenerico.Pattern.Contextos;
using System.Data;

namespace RepositorioGenerico.SqlClient.Contextos
{
	public class Contexto : ContextoBase, IContextoTransacional
	{

		public IRepositorio<TObjeto> Repositorio<TObjeto>() where TObjeto : IEntidade
		{
			var tipo = typeof(TObjeto);
			return (IRepositorio<TObjeto>)Repositorios.GetOrAdd(tipo, CriarNovoRepositorio);
		}

		public IRepositorioObject Repositorio(Type tipo)
		{
			return (IRepositorioObject)Repositorios.GetOrAdd(tipo, CriarNovoRepositorio);
		}

		public Contexto(string stringConexao)
			: base(stringConexao)
		{

		}

		public Contexto(string stringConexao, IDbTransaction transacao) : base(stringConexao, transacao)
		{

		}

		private object CriarNovoRepositorio(Type tipo)
		{
			var tipoRepositorio = typeof(Repositorio<>);
			var repositorioGenerico = tipoRepositorio.MakeGenericType(tipo);
			var tipoPersistencia = typeof(Persistencia<>);
			var persistenciaGenerica = tipoPersistencia.MakeGenericType(tipo);
			var dicionario = DicionarioCache.Consultar(tipo);
			var persistencia = Activator.CreateInstance(persistenciaGenerica, dicionario);
			return Activator.CreateInstance(repositorioGenerico, this, persistencia);
		}

	}
}
