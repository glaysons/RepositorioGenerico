using System;
using RepositorioGenerico.Dictionary;
using RepositorioGenerico.Entities;
using RepositorioGenerico.Pattern.Contextos.Tables;

namespace RepositorioGenerico.SqlClient.Contextos.Tables
{
	public class Contexto : ContextoBase, IContextoTransacional
	{

		public IRepositorio Repositorio<TObjeto>() where TObjeto : class, IEntidade
		{
			var tipo = typeof(TObjeto).FullName;
			if (!Repositorios.ContainsKey(tipo))
				CriarNovoRepositorio<TObjeto>(tipo);
			return (IRepositorio)Repositorios[tipo];
		}

		public Contexto(string stringConexao)
			: base(stringConexao)
		{

		}

		private void CriarNovoRepositorio<TObjeto>(string tipo) where TObjeto : class, IEntidade
		{
			var tipoRepositorio = typeof(Repositorio<>);
			var tipoGenerico = tipoRepositorio.MakeGenericType(typeof(TObjeto));
			var dicionario = DicionarioCache.Consultar(typeof(TObjeto));
			var persistencia = new Persistencia<TObjeto>(dicionario);
			Repositorios.Add(tipo, (IRepositorio)Activator.CreateInstance(tipoGenerico, this, persistencia));
		}

	}
}
