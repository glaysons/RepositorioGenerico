using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositorioGenerico.Dictionary;
using RepositorioGenerico.Dictionary.Helpers;
using RepositorioGenerico.Dictionary.Relacionamentos;
using RepositorioGenerico.Pattern.Buscadores;
using RepositorioGenerico.SqlClient.Builders;
using RepositorioGenerico.Test.Objetos;

namespace RepositorioGenerico.SqlClient.Test.Builders
{
	[TestClass]
	public class RelacionamentoBuilderUnitTest
	{
		[TestMethod]
		public void SeCriarScriptConsultaRelacionamentoAscendenteDeveGerarUmScriptSqlCorretamente()
		{
			var queryBuilder = new QueryBuilder();

			var dicionarioFilhos = new Dicionario(typeof(FilhoDoObjetoDeTestes));
			queryBuilder.DefinirTabela(dicionarioFilhos.Nome);
			queryBuilder.AdicionarCondicao("CodigoFilho", (int)Operadores.Igual, 1);
			var scriptQueConsultaFilhoQueTeraPaiCarregado = queryBuilder.GerarScript(dicionarioFilhos);
			var chaveEstrangeira = DataAnnotationHelper.ConsultarForeignKey(dicionarioFilhos.ConsultarPorCampo("Pai").Propriedade);

			var dicionarioPai = new Dicionario(typeof(ObjetoDeTestes));
			var relacionamentoAscendente = new Relacionamento(TiposRelacionamento.Ascendente, dicionarioPai, chaveEstrangeira);

			var relacionamentoBuilder = new RelacionamentoBuilder();
			relacionamentoBuilder.CriarScriptConsultaRelacionamentoAscendente(relacionamentoAscendente, scriptQueConsultaFilhoQueTeraPaiCarregado)
				.Should()
				.Be("with[d]as(" +
						"select[CodigoFilho]as[Id],[NomeFilho]as[Nome],[CodigoPai]as[IdPai] " +
						"from[ObjetoVirtualFilho]" +
						"where([CodigoFilho]=@_p0) )" +
				    "select[Codigo],[CodigoNulo],[Nome],[Duplo],[DuploNulo]," +
						"[Decimal],[DecimalNulo],[Logico],[DataHora],[DataHoraNulo]" +
				    "from[ObjetoVirtual][t]" +
				    "where(exists(" +
						"select top 1 1 " +
						"from[d]" +
						"where(t.[Codigo]=d.[IdPai])))");
		}

		[TestMethod]
		public void SeCriarScriptConsultaRelacionamentoDescendenteDeveGerarUmScriptSqlCorretamente()
		{
			var queryBuilder = new QueryBuilder();
			var dicionarioPai = new Dicionario(typeof(ObjetoDeTestes));

			queryBuilder.DefinirTabela(dicionarioPai.Nome);
			queryBuilder.AdicionarCondicao("Codigo", (int)Operadores.Igual, 1);
			var scriptConsultaDoPai = queryBuilder.GerarScript(dicionarioPai);

			var dicionarioFilho = new Dicionario(typeof(FilhoDoObjetoDeTestes));
			var chaveEstrangeira = DataAnnotationHelper.ConsultarForeignKey(dicionarioFilho.ConsultarPorCampo("Pai").Propriedade);

			var relacionamentoBuilder = new RelacionamentoBuilder();
			var relacionamento = new Relacionamento(TiposRelacionamento.Descendente, dicionarioFilho, chaveEstrangeira);

			relacionamentoBuilder.CriarScriptConsultaRelacionamentoDescendente(relacionamento, scriptConsultaDoPai, dicionarioPai.ConsultarCamposChave())
				.Should()
				.Be("with[d]as(" +
						"select[Codigo],[CodigoNulo],[Nome],[Duplo],[DuploNulo],[Decimal]," +
							"[DecimalNulo],[Logico],[DataHora],[DataHoraNulo] " +
						"from[ObjetoVirtual]" +
						"where([Codigo]=@_p0) " +
				    ")" +
					"select[CodigoFilho]as[Id],[NomeFilho]as[Nome],[CodigoPai]as[IdPai]" +
				    "from[ObjetoVirtualFilho][t]" +
				    "where(exists(" +
						"select top 1 1 " +
						"from[d]" +
						"where(t.[IdPai]=d.[Codigo])))");
		}

	}
}
