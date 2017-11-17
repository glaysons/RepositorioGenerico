using System;
using System.Collections.Generic;
using System.Data;
using RepositorioGenerico.Dictionary;
using RepositorioGenerico.Entities;
using RepositorioGenerico.Pattern.Contextos.Tables;

namespace RepositorioGenerico.Fake.Contextos.Tables
{
	public class ContextoFake : ContextoFakeBase, IContextoFake, IContextoTransacional
	{

		private readonly Dictionary<string, object> _persistencias;

		public IRepositorio Repositorio<TObjeto>() where TObjeto : class, IEntidade
		{
			var tipo = typeof(TObjeto).FullName;
			if (!Repositorios.ContainsKey(tipo))
				CriarNovoRepositorio<TObjeto>(tipo);
			return (IRepositorio)Repositorios[tipo];
		}

		public ContextoFake()
		{
			_persistencias = new Dictionary<string, object>();
		}

		private void CriarNovoRepositorio<TObjeto>(string tipo) where TObjeto : IEntidade
		{
			var tipoRepositorio = typeof(RepositorioFake<>);
			var tipoGenerico = tipoRepositorio.MakeGenericType(typeof(TObjeto));
			var dicionario = DicionarioCache.Consultar(typeof(TObjeto));
			var persistencia = new PersistenciaFake<TObjeto>(dicionario);
			Repositorios.Add(tipo, (IRepositorio)Activator.CreateInstance(tipoGenerico, this, persistencia));
			_persistencias.Add(tipo, persistencia);
		}

		public void AdicionarRegistros<TObjeto>(DataTable registros) where TObjeto : IEntidade
		{
			var repositorioVirtual = ConsultarRepositorioVirtual<TObjeto>();
			foreach (DataRow registro in registros.Rows)
				repositorioVirtual.Dados.ImportRow(registro);
		}

		private PersistenciaFake<TObjeto> ConsultarRepositorioVirtual<TObjeto>() where TObjeto : IEntidade
		{
			object persistencia;
			var tipo = typeof (TObjeto).FullName;
			if (_persistencias.TryGetValue(tipo, out persistencia))
				return (PersistenciaFake<TObjeto>)persistencia;
			CriarNovoRepositorio<TObjeto>(tipo);
			persistencia = _persistencias[tipo];
			return (PersistenciaFake<TObjeto>)persistencia;
		}

		public void AdicionarRegistro<TObjeto>(DataRow registro) where TObjeto : IEntidade
		{
			var repositorioVirtual = ConsultarRepositorioVirtual<TObjeto>();
			repositorioVirtual.Dados.ImportRow(registro);
		}
	}
}
