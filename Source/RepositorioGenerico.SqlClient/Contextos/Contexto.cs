using System;
using RepositorioGenerico.Dictionary;
using RepositorioGenerico.Entities;
using RepositorioGenerico.Pattern.Contextos;

namespace RepositorioGenerico.SqlClient.Contextos
{
	public class Contexto : ContextoBase, IContextoTransacional
	{

		public IRepositorio<TObjeto> Repositorio<TObjeto>() where TObjeto : IEntidade
		{
			var tipo = typeof(TObjeto);
			var nome = tipo.FullName;
			if (!Repositorios.ContainsKey(nome))
				CriarNovoRepositorio(nome, tipo);
			return (IRepositorio<TObjeto>)Repositorios[nome];
		}

		public IRepositorioObject Repositorio(Type tipo)
		{
			var nome = tipo.FullName;
			if (!Repositorios.ContainsKey(nome))
				CriarNovoRepositorio(nome, tipo);
			return (IRepositorioObject)Repositorios[nome];
		}

		public Contexto(string stringConexao)
			: base(stringConexao)
		{

		}

		private void CriarNovoRepositorio(string nome, Type tipo)
		{
			var tipoRepositorio = typeof(Repositorio<>);
			var repositorioGenerico = tipoRepositorio.MakeGenericType(tipo);
			var tipoPersistencia = typeof(Persistencia<>);
			var persistenciaGenerica = tipoPersistencia.MakeGenericType(tipo);
			var dicionario = DicionarioCache.Consultar(tipo);
			var persistencia = Activator.CreateInstance(persistenciaGenerica, dicionario);
			var repositorio = Activator.CreateInstance(repositorioGenerico, this, persistencia);
			Repositorios.Add(nome, repositorio);
		}

	}
}
