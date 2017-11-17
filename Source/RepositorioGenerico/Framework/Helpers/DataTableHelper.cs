using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using RepositorioGenerico.Entities.Anotacoes;

namespace RepositorioGenerico.Framework.Helpers
{
	public static class DataTableHelper
	{
		public static DataTable ConverterListaEmDataTable<TObjeto>(IList<TObjeto> itens)
		{
			var tipo = typeof(TObjeto);
			var tabela = new DataTable(tipo.Name);
			var campos = ConsultarColunasPossiveis(tipo);
			CriarColunas(campos, tabela);
			ConverterItensEmDataRows(campos, tabela, itens);
			return tabela;
		}

		private static List<PropertyInfo> ConsultarColunasPossiveis(Type tipo)
		{
			var campos = tipo.GetProperties()
				.Where(property => (property.PropertyType.IsValueType || property.PropertyType == typeof(string))
								   && (!property.GetCustomAttributes(typeof(NaoMapeadoAttribute), inherit:true).Any()))
				.ToList();
			return campos;
		}

		private static void CriarColunas(IList<PropertyInfo> campos, DataTable tabela)
		{
			foreach (var campo in campos)
			{
				var tipoCampo = campo.PropertyType;
				var nullable = tipoCampo.Name.Contains("Nullable`");
				if (nullable)
					tipoCampo = tipoCampo.GetGenericArguments()[0];
				var coluna = tabela.Columns.Add(campo.Name, tipoCampo);
				coluna.AllowDBNull = (nullable || tipoCampo.Name.Contains("string"));
			}
		}

		private static void ConverterItensEmDataRows<TObjeto>(IList<PropertyInfo> campos, DataTable tabela, IList<TObjeto> itens)
		{
			foreach (var item in itens)
			{
				var registro = tabela.NewRow();
				for (var coluna = 0; coluna < tabela.Columns.Count; coluna++)
					registro[coluna] = campos[coluna].GetValue(item, null)
						?? DBNull.Value;
				tabela.Rows.Add(registro);
			}
		}

	}
}
