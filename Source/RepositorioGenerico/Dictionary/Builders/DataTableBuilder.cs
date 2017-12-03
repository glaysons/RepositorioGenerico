using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using RepositorioGenerico.Dictionary.Itens;

namespace RepositorioGenerico.Dictionary.Builders
{
	public static class DataTableBuilder
	{

		public static DataTable CriarDataTable(Dicionario dicionario)
		{
			var tabela = new DataTable(dicionario.Nome);
			CriarColunas(dicionario, tabela);
			DefinirChavePrimaria(dicionario, tabela);
			return tabela;
		}

		private static void CriarColunas(Dicionario dicionario, DataTable tabela)
		{
			foreach (var campo in dicionario.Itens)
				CriarColuna(campo, tabela);
		}

		private static void CriarColuna(ItemDicionario itemDicionario, DataTable tabela)
		{
			var coluna = tabela.Columns.Add(itemDicionario.Nome, itemDicionario.TipoLocal);
			coluna.Caption = itemDicionario.AliasOuNome;
			coluna.AllowDBNull = !itemDicionario.Obrigatorio;
			if ((coluna.DataType == typeof(string)) && (itemDicionario.TamanhoMaximo > 0))
				coluna.MaxLength = itemDicionario.TamanhoMaximo;
			if (itemDicionario.OpcaoGeracao == Entities.Anotacoes.Incremento.Identity)
			{
				coluna.AutoIncrement = true;
				coluna.AutoIncrementSeed = 1;
				coluna.AutoIncrementStep = 1;
			}
		}

		private static void DefinirChavePrimaria(Dicionario dicionario, DataTable tabela)
		{
			var campos = new List<DataColumn>();
			foreach (var campo in dicionario.ConsultarCamposChave())
				campos.Add(tabela.Columns[campo.Nome]);
			tabela.PrimaryKey = campos.ToArray();
		}

		public static DataRow ConverterItemEmDataRow(DataTable tabela, object item, bool novoRegistro = false)
		{
			var registro = tabela.NewRow();
			if (item != null)
			{
				var propriedades = item.GetType().GetProperties();
				foreach (DataColumn coluna in tabela.Columns)
				{
					if (novoRegistro && coluna.AutoIncrement)
						continue;
					var propriedade = propriedades.SingleOrDefault(p => p.Name == coluna.ColumnName || p.Name == coluna.Caption);
					if (propriedade != null)
							registro[coluna] = propriedade.GetValue(item, null) ?? DBNull.Value;
				}
			}
			return registro;
		}

	}
}
