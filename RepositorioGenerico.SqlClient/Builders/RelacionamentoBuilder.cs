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
					sql.Append(string.Concat("(t.[", chave.Nome, "]=d.[", relacionamento.ChaveEstrangeira[n], "])and"));
				n++;
			}
			if (n == 0 || n != relacionamento.ChaveEstrangeira.Length)
				throw new AQuantidadeDeCamposChaveNaLigacaoDoCampoEInvalidaException();
			sql.Length -= 3;
			sql.Append("))");
			return sql.ToString();
		}

		private StringBuilder CriarScriptConsultaRelacionamentoBasico(Relacionamento relacionamento, string condicao)
		{
			var sql = new StringBuilder();
			sql.Append(string.Concat("with[d]as(", condicao, ")"));
			sql.Append("select");
			foreach (var item in relacionamento.Dicionario.Itens)
			{
				var alias = (string.IsNullOrEmpty(item.Alias))
					? string.Empty
					: string.Concat("as[", item.Alias, "]");
				sql.Append(string.Concat("[", item.Nome, "]", alias, ","));
			}
			sql.Length -= 1;
			sql.Append(string.Concat("from[", relacionamento.Dicionario.Nome, "][t]where(exists(select top 1 1 from[d]where"));
			return sql;
		}

		public string CriarScriptConsultaRelacionamentoDescendente(Relacionamento relacionamento, string condicao, IEnumerable<ItemDicionario> camposChave)
		{
			var sql = CriarScriptConsultaRelacionamentoBasico(relacionamento, condicao);
			var n = 0;
			foreach (var campo in camposChave)
			{
				if (n < relacionamento.ChaveEstrangeira.Length)
					sql.Append(string.Concat("(t.[", relacionamento.ChaveEstrangeira[n], "]=d.[", campo.AliasOuNome, "])and"));
				n++;
			}
			if (n == 0 || n != relacionamento.ChaveEstrangeira.Length)
				throw new AQuantidadeDeCamposChaveNaLigacaoDoCampoEInvalidaException();
			sql.Length -= 3;
			sql.Append("))");
			return sql.ToString();
		}

	}
}
