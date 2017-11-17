using System;
using System.Collections.Generic;
using RepositorioGenerico.Dictionary;
using RepositorioGenerico.Dictionary.Builders;
using RepositorioGenerico.Entities;
using RepositorioGenerico.Pattern.Contextos;

namespace RepositorioGenerico.Fake.Contextos
{
	public class ContextoFake : ContextoFakeBase, IContextoFake, IContextoTransacional
	{

		public IRepositorio<TObjeto> Repositorio<TObjeto>() where TObjeto : IEntidade
		{
			var tipo = typeof(TObjeto);
			var nome = tipo.Name;
			if (!Repositorios.ContainsKey(nome))
				CriarNovoRepositorio(nome, tipo);
			return (IRepositorio<TObjeto>)Repositorios[nome];
		}

		public IRepositorioObject Repositorio(Type tipo)
		{
			var nome = tipo.Name;
			if (!Repositorios.ContainsKey(nome))
				CriarNovoRepositorio(nome, tipo);
			return (IRepositorioObject)Repositorios[nome];
		}

		private void CriarNovoRepositorio(string nome, Type tipo)
		{
			var tipoRepositorio = typeof(RepositorioFake<>);
			var repositorioGenerico = tipoRepositorio.MakeGenericType(tipo);
			var tipoPersistencia = typeof(PersistenciaFake<>);
			var persistenciaGenerica = tipoPersistencia.MakeGenericType(tipo);
			var dicionario = DicionarioCache.Consultar(tipo);
			var tabela = BancoDeDadosVirtual.Tables[tipo.Name] 
				?? DataTableBuilder.CriarDataTable(dicionario);
			if (!BancoDeDadosVirtual.Tables.Contains(tabela.TableName))
				BancoDeDadosVirtual.Tables.Add(tabela);
			var persistencia = Activator.CreateInstance(persistenciaGenerica, dicionario);
			var repositorio = Activator.CreateInstance(repositorioGenerico, this, persistencia, tabela);
			Repositorios.Add(nome, repositorio);
		}

		public void AdicionarRegistros<TObjeto>(IList<TObjeto> registros) where TObjeto : IEntidade
		{
			var repositorio = Repositorio<TObjeto>();
			foreach (var registro in registros)
				repositorio.Inserir(registro);
		}

		public void AdicionarRegistro<TObjeto>(TObjeto registro) where TObjeto : IEntidade
		{
			var repositorio = Repositorio<TObjeto>();
			repositorio.Inserir(registro);
		}
	}
}
