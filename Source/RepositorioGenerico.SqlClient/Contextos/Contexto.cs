using System;
using RepositorioGenerico.Dictionary;
using RepositorioGenerico.Entities;
using RepositorioGenerico.Pattern.Contextos;
using System.Data;

namespace RepositorioGenerico.SqlClient.Contextos
{
	public class Contexto : ContextoBase, IContexto, IContextoTransacional
	{

		public IRepositorio<TObjeto> Repositorio<TObjeto>() where TObjeto : IEntidade
		{
			var tipo = typeof(TObjeto);
			if (!Repositorios.ContainsKey(tipo))
				CriarNovoRepositorio(tipo);
			return (IRepositorio<TObjeto>)Repositorios[tipo];
		}

		public IRepositorioObject Repositorio(Type tipo)
		{
			if (!Repositorios.ContainsKey(tipo))
				CriarNovoRepositorio(tipo);
			return (IRepositorioObject)Repositorios[tipo];
		}

		public Contexto(string stringConexao)
			: base(stringConexao)
		{

		}

		public Contexto(string stringConexao, IDbTransaction transacao) : base(stringConexao, transacao)
		{

		}

		private void CriarNovoRepositorio(Type tipo)
		{
			var tipoRepositorio = typeof(Repositorio<>);
			var repositorioGenerico = tipoRepositorio.MakeGenericType(tipo);
			var tipoPersistencia = typeof(Persistencia<>);
			var persistenciaGenerica = tipoPersistencia.MakeGenericType(tipo);
			var dicionario = DicionarioCache.Consultar(tipo);
			var persistencia = Activator.CreateInstance(persistenciaGenerica, dicionario);
			var repositorio = Activator.CreateInstance(repositorioGenerico, this, persistencia);
			Repositorios.Add(tipo, repositorio);
		}

	}
}
