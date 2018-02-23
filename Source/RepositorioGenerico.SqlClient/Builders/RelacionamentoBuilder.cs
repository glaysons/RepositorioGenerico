using System;
using System.Collections.Generic;
using System.Text;
using RepositorioGenerico.Dictionary.Itens;
using RepositorioGenerico.Dictionary.Relacionamentos;
using RepositorioGenerico.Exceptions;
using RepositorioGenerico.Search;

namespace RepositorioGenerico.SqlClient.Builders
{

	public class RelacionamentoBuilder : IRelacionamentoBuilder
	{

		public string CriarScriptConsultaRelacionamentoAscendente(Relacionamento relacionamento, string condicao)
		{
			var sql = CriarScriptConsultaRelacionamentoBasico(relacionamento, condicao);
			var n = 0;
			foreach (var chave in relacionamento.Dicionario.ConsultarCamposChave())
			{
				if (n < relacionamento.ChaveEstrangeira.Length)
				{
					sql.Append("(t.[");
					sql.Append(chave.AliasOuNome);
					sql.Append("]=d.[");
					sql.Append(relacionamento.ChaveEstrangeira[n]);
					sql.Append("])and");
				}
				n++;
			}
			if (n == 0 || n != relacionamento.ChaveEstrangeira.Length)
				throw new AQuantidadeDeCamposChaveNaLigacaoDoCampoEInvalidaException();
			sql.Length -= 3;
			AdicionarOrderBy(relacionamento, sql);
			return sql.ToString();
		}

		private StringBuilder CriarScriptConsultaRelacionamentoBasico(Relacionamento relacionamento, string condicao)
		{
			var sql = new StringBuilder();
			sql.Append("select");
			foreach (var item in relacionamento.Dicionario.Itens)
			{
				sql.Append("[");
				sql.Append(item.AliasOuNome);
				sql.Append("],");
			}
			sql.Length -= 1;
			sql.Append("from(select");
			var n = 0;
			foreach (var item in relacionamento.Dicionario.Itens)
			{
				sql.Append("[");
				sql.Append(item.Nome);
				sql.Append("]");
				if (!string.IsNullOrEmpty(item.Alias))
				{
					sql.Append("as[");
					sql.Append(item.Alias);
					sql.Append("]");
				}
				sql.Append(",");
				n++;
			}
			if (n == 0)
				throw new TabelaNaoPossuiInformacoesDeCamposDaTabelaException(relacionamento.Dicionario.Nome);
			sql.Length -= 1;
			sql.Append("from[");
			sql.Append(relacionamento.Dicionario.Nome);
			sql.Append("]");
			sql.Append(")[t]where(exists(select top 1 1 from(");
			var orderBy = condicao.IndexOf("order by", StringComparison.InvariantCultureIgnoreCase);
			if (orderBy > 0)
				sql.Append(condicao.Substring(0, orderBy));
			else
				sql.Append(condicao);
			sql.Append(")[d]where");
			return sql;
		}

		private void AdicionarOrderBy(Relacionamento relacionamento, StringBuilder sql)
		{
			sql.Append("))order by ");
			foreach (var chave in relacionamento.Dicionario.ConsultarCamposChave())
			{
				sql.Append("t.[");
				sql.Append(chave.AliasOuNome);
				sql.Append("],");
			}
			sql.Length -= 1;
		}

		public string CriarScriptConsultaRelacionamentoDescendente(Relacionamento relacionamento, string condicao, IEnumerable<ItemDicionario> camposChave)
		{
			var sql = CriarScriptConsultaRelacionamentoBasico(relacionamento, condicao);
			var n = 0;
			foreach (var campo in camposChave)
			{
				if (n < relacionamento.ChaveEstrangeira.Length)
				{
					sql.Append("(t.[");
					sql.Append(relacionamento.ChaveEstrangeira[n]);
					sql.Append("]=d.[");
					sql.Append(campo.AliasOuNome);
					sql.Append("])and");
				}
				n++;
			}
			if (n == 0 || n != relacionamento.ChaveEstrangeira.Length)
				throw new AQuantidadeDeCamposChaveNaLigacaoDoCampoEInvalidaException();
			sql.Length -= 3;
			AdicionarOrderBy(relacionamento, sql);
			return sql.ToString();
		}

	}
}
