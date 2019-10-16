using System;
using System.Collections.Generic;
using RepositorioGenerico.Dictionary;
using RepositorioGenerico.Dictionary.Builders;
using RepositorioGenerico.Entities;
using RepositorioGenerico.Pattern.Contextos;
using System.Data;
using System.Collections;

namespace RepositorioGenerico.Fake.Contextos
{
	public class ContextoFake : ContextoFakeBase, IContextoFake
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

		private void CriarNovoRepositorio(Type tipo)
		{
			var tipoRepositorio = typeof(RepositorioFake<>);
			var repositorioGenerico = tipoRepositorio.MakeGenericType(tipo);
			var tipoPersistencia = typeof(PersistenciaFake<>);
			var persistenciaGenerica = tipoPersistencia.MakeGenericType(tipo);
			var dicionario = DicionarioCache.Consultar(tipo);
			var tabela = ConsultarTabelaDoBancoDeDadosVirtual(tipo);
			var persistencia = Activator.CreateInstance(persistenciaGenerica, dicionario);
			var repositorio = Activator.CreateInstance(repositorioGenerico, this, persistencia, tabela);
			Repositorios.Add(tipo, repositorio);
		}

		public DataTable ConsultarTabelaDoBancoDeDadosVirtual(Type tipo, string nomeTabela = null)
		{
			var dicionario = DicionarioCache.Consultar(tipo);
			var nome = nomeTabela ?? dicionario.Nome;
			if (!BancoDeDadosVirtual.Tables.Contains(nome))
			{
				var tabela = DataTableBuilder.CriarDataTable(dicionario);
				tabela.TableName = nome;
				BancoDeDadosVirtual.Tables.Add(tabela);
			}
			return BancoDeDadosVirtual.Tables[nome];
		}

		public void AdicionarRegistros<TObjeto>(IList<TObjeto> registros) where TObjeto : IEntidade
		{
			AdicionarRegistros(typeof(TObjeto), registros);
		}

		private void AdicionarRegistros(Type tipo, IEnumerable registros, Action<object> atualizar = null)
		{
			var dicionario = DicionarioCache.Consultar(tipo);
			var tabela = ConsultarTabelaDoBancoDeDadosVirtual(tipo);
			foreach (var registro in registros)
			{
				atualizar?.Invoke(registro);
				AdicionarRegistro(dicionario, tabela, registro);
			}
		}

		private void AdicionarRegistro(Dicionario dicionario, DataTable tabela, object registro)
		{
			tabela.Rows.Add(DataTableBuilder.ConverterItemEmDataRow(tabela, registro));
			if (dicionario.PossuiCamposFilho)
			{
				var chave = dicionario.ConsultarValoresDaChave(registro);
				foreach (var item in dicionario.ConsultarCamposFilho())
				{
					var valores = item.Propriedade.GetValue(registro, null);
					if (valores == null)
						continue;
					AdicionarRegistros(item.Ligacao.Dicionario.Tipo, (IEnumerable)valores,
						(r) => item.Ligacao.AplicarChaveAscendente(chave, r));
				}
			}
		}

		public void AdicionarRegistro<TObjeto>(TObjeto registro) where TObjeto : IEntidade
		{
			var tipo = typeof(TObjeto);
			var dicionario = DicionarioCache.Consultar(tipo);
			var tabela = ConsultarTabelaDoBancoDeDadosVirtual(tipo);
			AdicionarRegistro(dicionario, tabela, registro);
		}

		public void DefinirResultadoProcedure<TObjeto>(string nomeProcedure, IList<TObjeto> registros) where TObjeto : IEntidade
		{
			if (string.IsNullOrEmpty(nomeProcedure))
				throw new ArgumentNullException("nomeProcedure");
			var nomeRepositorio = "__proc__" + nomeProcedure;
			var tabela = ConsultarTabelaDoBancoDeDadosVirtual(typeof(TObjeto), nomeRepositorio);
			tabela.Rows.Clear();
			foreach (var registro in registros)
				tabela.Rows.Add(DataTableBuilder.ConverterItemEmDataRow(tabela, registro));
		}

		public void DefinirResultadoScalarProcedure(string nomeProcedure, object valor)
		{
			var tabela = ConsultarTabelaDoBancoDeDadosVirtual(typeof(Procedure));
			var registro = ConsultarOuCriarRegistroTabelaProcedures(tabela, nomeProcedure);
			registro["Valor"] = valor;
		}

		private DataRow ConsultarOuCriarRegistroTabelaProcedures(DataTable tabela, string nomeProcedure)
		{
			foreach (DataRow registro in tabela.Rows)
				if (string.Equals(registro["Nome"].ToString(), nomeProcedure))
					return registro;
			var novoRegistro = tabela.NewRow();
			novoRegistro["Nome"] = nomeProcedure;
			tabela.Rows.Add(novoRegistro);
			return novoRegistro;
		}

		public void DefinirResultadoNonQueryProcedure(string nomeProcedure, int registrosAfetados)
		{
			var tabela = ConsultarTabelaDoBancoDeDadosVirtual(typeof(Procedure));
			var registro = ConsultarOuCriarRegistroTabelaProcedures(tabela, nomeProcedure);
			registro["RegistrosAfetados"] = registrosAfetados;
		}

	}
}
