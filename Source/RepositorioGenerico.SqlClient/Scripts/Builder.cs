using System.Linq;
using System.Text;
using RepositorioGenerico.Dictionary;
using RepositorioGenerico.Entities.Anotacoes;
using RepositorioGenerico.Exceptions;

namespace RepositorioGenerico.SqlClient.Scripts
{
	internal static class Builder
	{

		public static string CriarScriptInsert(Dicionario dicionario)
		{
			var sql = new StringBuilder();
			sql.Append("insert into[");
			sql.Append(dicionario.Nome);
			sql.Append("](");
			foreach (var campo in dicionario.Itens)
				if (campo.OpcaoGeracao != Incremento.Identity)
				{
					sql.Append("[");
					sql.Append(campo.Nome);
					sql.Append("],");
				}
			sql.Length -= 1;
			sql.Append(")values(");
			foreach (var campo in dicionario.Itens)
				if (campo.OpcaoGeracao != Incremento.Identity)
				{
					sql.Append("@p");
					sql.Append(campo.Id);
					sql.Append(",");
				}
			sql.Length -= 1;
			sql.Append(")");
			if (dicionario.AutoIncremento == OpcoesAutoIncremento.Identity)
				sql.Append(" select @@identity");
			return sql.ToString();
		}

		public static string CriarScriptUpdate(Dicionario dicionario)
		{
			if (dicionario.QuantidadeCamposNaChave == 0)
				throw new ChavePrimariaInvalidaException();
			var sql = new StringBuilder();
			var temCampos = false;
			sql.Append("update[");
			sql.Append(dicionario.Nome);
			sql.Append("]set");
			foreach (var campo in dicionario.Itens)
			{
				if (campo.Chave)
					continue;
				sql.Append("[");
				sql.Append(campo.Nome);
				sql.Append("]=@p");
				sql.Append(campo.Id);
				sql.Append(",");
				temCampos = true;
			}
			if (!temCampos)
				return string.Empty;
			sql.Length -= 1;
			sql.Append(" where");
			AcrescentarCamposChave(dicionario, sql);
			return sql.ToString();
		}

		private static void AcrescentarCamposChave(Dicionario dicionario, StringBuilder sql)
		{
			foreach (var campo in dicionario.ConsultarCamposChave())
			{
				sql.Append("([");
				sql.Append(campo.Nome);
				sql.Append("]=@p");
				sql.Append(campo.Id);
				sql.Append(")and");
			}
			sql.Length -= 3;
		}

		public static string CriarScriptDelete(Dicionario dicionario)
		{
			if (dicionario.QuantidadeCamposNaChave == 0)
				throw new ChavePrimariaInvalidaException();
			var sql = new StringBuilder();
			sql.Append("delete[");
			sql.Append(dicionario.Nome);
			sql.Append("]where");
			AcrescentarCamposChave(dicionario, sql);
			return sql.ToString();
		}

		public static string CriarScriptAutoIncremento(Dicionario dicionario)
		{
			var chave = dicionario.Itens.FirstOrDefault(i => i.OpcaoGeracao == Incremento.Calculado);
			if (chave == null)
				return null;
			var sql = new StringBuilder();
			sql.Append("select isnull((select max([");
			sql.Append(chave.Nome);
			sql.Append(string.Concat("])from[", dicionario.Nome, "]"));
			if (dicionario.QuantidadeCamposNaChave > 1)
			{
				sql.Append("where");
				foreach (var campo in dicionario.ConsultarCamposChave())
					if (campo.OpcaoGeracao == Incremento.Nenhum)
					{
						sql.Append("([");
						sql.Append(campo.Nome);
						sql.Append("]=@p");
						sql.Append(campo.Id);
						sql.Append(")and");
					}
				sql.Length -= 3;
			}
			sql.Append("),0)+1");
			return sql.ToString();
		}

	}
}
