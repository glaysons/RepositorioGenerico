using System;
using System.Data;
using RepositorioGenerico.Dictionary;
using RepositorioGenerico.Dictionary.Itens;
using RepositorioGenerico.Entities;
using RepositorioGenerico.Entities.Anotacoes;
using RepositorioGenerico.Pattern;
using RepositorioGenerico.Fake.Contextos;
using RepositorioGenerico.Dictionary.Builders;

namespace RepositorioGenerico.Fake
{
	internal class AdapterFake<TObjeto> where TObjeto : IEntidade
	{

		private readonly Dicionario _dicionario;

		public AdapterFake(Dicionario dicionario)
		{
			_dicionario = dicionario;
		}

		public void Salvar(IConexao conexao, TObjeto model)
		{
			if ((model == null) || (model.EstadoEntidade == EstadosEntidade.NaoModificado))
				return;

			var tabela = (conexao as ContextoFake).ConsultarTabelaDoBancoDeDadosVirtual(typeof(TObjeto));
			var registro = DataTableBuilder.ConverterItemEmDataRow(tabela, model, novoRegistro: model.EstadoEntidade == EstadosEntidade.Novo);
			switch (model.EstadoEntidade)
			{
				case EstadosEntidade.Novo:
					tabela.Rows.Add(registro);
					if (_dicionario.AutoIncremento == OpcoesAutoIncremento.Identity)
					{
						var autoIncremento = Convert.ToInt32(registro[ConsultarColunaIdentity(tabela)]);
						AtribuirValorAutoIncremento(Incremento.Identity, item => item.Propriedade.SetValue(model, autoIncremento, null));
					}
					break;

				case EstadosEntidade.Modificado:
					AtualizarRegistro(tabela, registro);
					break;

				case EstadosEntidade.Excluido:
					ExcluirRegistro(tabela, registro);
					break;
			}
		}

		private int ConsultarColunaIdentity(DataTable tabela)
		{
			for (var indice = 0; indice < tabela.Columns.Count; indice++)
			{
				var coluna = tabela.Columns[indice];
				if (coluna.AutoIncrement)
					return indice;
			}
			return - 1;
		}

		private void AtribuirValorAutoIncremento(Incremento opcao, Action<ItemDicionario> atribuir)
		{
			foreach (var item in _dicionario.Itens)
				if (item.OpcaoGeracao == opcao)
				{
					atribuir(item);
					return;
				}
		}

		private void AtualizarRegistro(DataTable tabela, DataRow registro)
		{
			var chave = _dicionario.ConsultarValoresDaChave(registro);
			var registroExistente = tabela.Rows.Find(chave);
			var tabelaImportada = registro.Table;
			if ((registroExistente == null) || (tabelaImportada == null))
				throw new DBConcurrencyException();
			for (int indice = 0; indice < tabela.Columns.Count; indice++)
			{
				var campoImportado = tabelaImportada.Columns.IndexOf(tabela.Columns[indice].ColumnName);
				if (campoImportado > -1)
					registroExistente[indice] = registro[campoImportado];
			}
		}

		private void ExcluirRegistro(DataTable tabela, DataRow registro)
		{
			var chave = _dicionario.ConsultarValoresDaChave(registro);
			var registroExistente = tabela.Rows.Find(chave);
			if (registroExistente == null)
				throw new DBConcurrencyException();
			registroExistente.Delete();
		}

		public void Salvar(IConexao conexao, DataRow registro)
		{
			if ((registro == null) || (registro.RowState == DataRowState.Unchanged) || (registro.RowState == DataRowState.Detached))
				return;

			var tabela = (conexao as ContextoFake).ConsultarTabelaDoBancoDeDadosVirtual(typeof(TObjeto));

			switch (registro.RowState)
			{
				case DataRowState.Added:
					tabela.Rows.Add(registro);
					break;

				case DataRowState.Modified:
					AtualizarRegistro(tabela, registro);
					break;

				case DataRowState.Deleted:
					ExcluirRegistro(tabela, registro);
					break;
			}
		}

	}
}
