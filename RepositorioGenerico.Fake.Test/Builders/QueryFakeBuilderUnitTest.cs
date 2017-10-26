using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositorioGenerico.Dictionary;
using RepositorioGenerico.Fake.Builders;
using RepositorioGenerico.Pattern.Buscadores;
using RepositorioGenerico.Test;

namespace RepositorioGenerico.Fake.Test.Builders
{
	[TestClass]
	public class QueryFakeBuilderUnitTest
	{

		[TestMethod]
		public void SeTestarAConsultaDosOperadoresGeraisFakeDeveRetornarSinalValido()
		{
			var builder = new QueryFakeBuilder();
			builder.ConsultarOperador(Operadores.Igual).Should().Be("=");
			builder.ConsultarOperador(Operadores.Diferente).Should().Be("<>");
			builder.ConsultarOperador(Operadores.Menor).Should().Be("<");
			builder.ConsultarOperador(Operadores.MenorOuIgual).Should().Be("<=");
			builder.ConsultarOperador(Operadores.Maior).Should().Be(">");
			builder.ConsultarOperador(Operadores.MaiorOuIgual).Should().Be(">=");
		}

		[TestMethod]
		public void SeTestarAConsultaDosOperadoresTextoFakeDeveRetornarSinalValido()
		{
			var builder = new QueryFakeBuilder();
			builder.ConsultarOperador(OperadoresTexto.Contendo).Should().Be("like");
			builder.ConsultarOperador(OperadoresTexto.NaoContendo).Should().Be("not like");
		}

		[TestMethod]
		public void SeTestarAConsultaDosOperadoresEspeciaisFakeDeveRetornarSinalValido()
		{
			var builder = new QueryFakeBuilder();
			builder.ConsultarOperador(OperadoresEspeciais.In).Should().Be("in");
			builder.ConsultarOperador(OperadoresEspeciais.NotIn).Should().Be("not in");
			builder.ConsultarOperador(OperadoresEspeciais.Is).Should().Be("is");
			builder.ConsultarOperador(OperadoresEspeciais.IsNot).Should().Be("is not");
		}

		[TestMethod]
		public void SeDefinirLimiteNaQueryFakeAVariavelDeveSerDefinidaCorretamente()
		{
			var builder = new QueryFakeBuilder();

			builder.Limites
				.Should()
				.BeNull();

			builder.DefinirLimite(10);

			builder.Limites
				.Should()
				.Be(10);
		}

		[TestMethod]
		public void SeDefinirTabelaNaQueryFakeAVariavelDeveSerDefinidaCorretamente()
		{
			var builder = new QueryFakeBuilder();

			builder.Tabela
				.Should()
				.BeNull();

			builder.DefinirTabela("NomeTabela");

			builder.Tabela
				.Should()
				.Be("NomeTabela");
		}

		[TestMethod]
		public void SeDefinirResultadoNaQueryFakeAVariavelDeveSerDefinidaCorretamente()
		{
			var builder = new QueryFakeBuilder();

			builder.Selects
				.Should()
				.NotBeNull();

			builder.Selects
				.Should()
				.HaveCount(0);

			builder.AdicionarResultado("campoQueDeveSerExibido");

			builder.Selects
				.Should()
				.HaveCount(1);

			builder.Selects[0]
				.Should()
				.Be("[campoQueDeveSerExibido]");
		}

		[TestMethod]
		public void SeDefinirResultadoPersonalizadoNaQueryFakeAVariavelDeveSerDefinidaCorretamente()
		{
			var builder = new QueryFakeBuilder();

			builder.Selects
				.Should()
				.NotBeNull();

			builder.Selects
				.Should()
				.HaveCount(0);

			builder.AdicionarResultadoPersonalizado("campoQueDeveSerExibido");

			builder.Selects
				.Should()
				.HaveCount(1);

			builder.Selects[0]
				.Should()
				.Be("campoQueDeveSerExibido");
		}

		[TestMethod]
		public void SeDefinirResultadoAgregadoNaQueryFakeAVariavelDeveSerDefinidaCorretamente()
		{
			var builder = new QueryFakeBuilder();

			builder.Selects
				.Should()
				.NotBeNull();

			builder.Selects
				.Should()
				.HaveCount(0);

			builder.AdicionarResultadoAgregado(Agregadores.Avg, "campoQueDeveSerExibido1");
			builder.AdicionarResultadoAgregado(Agregadores.Count, null);
			builder.AdicionarResultadoAgregado(Agregadores.Count, "campoQueDeveSerExibido2");
			builder.AdicionarResultadoAgregado(Agregadores.Max, "campoQueDeveSerExibido3");
			builder.AdicionarResultadoAgregado(Agregadores.Min, "campoQueDeveSerExibido4");
			builder.AdicionarResultadoAgregado(Agregadores.Sum, "campoQueDeveSerExibido5");

			builder.Selects
				.Should()
				.HaveCount(6);

			builder.Selects[0].Should().Be(" Avg([campoQueDeveSerExibido1])as[campoQueDeveSerExibido1]");
			builder.Selects[1].Should().Be(" Count(*)");
			builder.Selects[2].Should().Be(" Count(distinct [campoQueDeveSerExibido2])as[campoQueDeveSerExibido2]");
			builder.Selects[3].Should().Be(" Max([campoQueDeveSerExibido3])as[campoQueDeveSerExibido3]");
			builder.Selects[4].Should().Be(" Min([campoQueDeveSerExibido4])as[campoQueDeveSerExibido4]");
			builder.Selects[5].Should().Be(" Sum([campoQueDeveSerExibido5])as[campoQueDeveSerExibido5]");
		}

		[TestMethod]
		public void SeAdicionarAgrupamentoNaQueryFakeAVariavelDeveSerDefinidaCorretamente()
		{
			var builder = new QueryFakeBuilder();

			builder.GroupBys
				.Should()
				.NotBeNull();

			builder.GroupBys
				.Should()
				.HaveCount(0);

			builder.AdicionarAgrupamento("campoQueDeveSerAgrupado");

			builder.GroupBys
				.Should()
				.HaveCount(1);

			builder.GroupBys[0]
				.Should()
				.Be("campoQueDeveSerAgrupado");
		}

		[TestMethod]
		public void SeAdicionarCondicaoAgrupamentoNaQueryFakeAVariavelDeveSerDefinidaCorretamente()
		{
			var builder = new QueryFakeBuilder();

			builder.Havings
				.Should()
				.NotBeNull();

			builder.Havings
				.Should()
				.HaveCount(0);

			builder.AdicionarCondicaoAgrupamento("campoQueDeveSerAgrupado>0");

			builder.Havings
				.Should()
				.HaveCount(1);

			builder.Havings[0]
				.Should()
				.Be("(campoQueDeveSerAgrupado>0)");
		}

		[TestMethod]
		public void SeAdicionarCondicaoPersonalizadaNaQueryFakeAVariavelDeveSerDefinidaCorretamente()
		{
			var builder = new QueryFakeBuilder();

			builder.Wheres
				.Should()
				.NotBeNull();

			builder.Wheres
				.Should()
				.HaveCount(0);

			builder.AdicionarCondicaoPersonalizada("campoQueDeveSerValidado>0");

			builder.Wheres
				.Should()
				.HaveCount(1);

			builder.Wheres[0]
				.Should()
				.Be("(campoQueDeveSerValidado>0)");
		}

		[TestMethod]
		public void SeAdicionarCondicaoNaQueryFakeAVariavelDeveSerDefinidaCorretamente()
		{
			var builder = new QueryFakeBuilder();

			builder.Wheres
				.Should()
				.NotBeNull();

			builder.Wheres
				.Should()
				.HaveCount(0);

			builder.ProximoParametro
				.Should()
				.Be(0);

			builder.AdicionarCondicao("campoQueDeveSerValidado", (int)Operadores.Igual, 1);
			builder.AdicionarCondicao("campoQueDeveSerValidado", (int)Operadores.Igual, null);
			builder.AdicionarCondicao("campoQueDeveSerValidado", (int)Operadores.Igual, DBNull.Value);

			builder.AdicionarCondicao("campoQueDeveSerValidado", (int)Operadores.Diferente, 1);
			builder.AdicionarCondicao("campoQueDeveSerValidado", (int)Operadores.Maior, 1);
			builder.AdicionarCondicao("campoQueDeveSerValidado", (int)Operadores.MaiorOuIgual, 1);
			builder.AdicionarCondicao("campoQueDeveSerValidado", (int)Operadores.Menor, 1);
			builder.AdicionarCondicao("campoQueDeveSerValidado", (int)Operadores.MenorOuIgual, 1);

			builder.AdicionarCondicao("campoQueDeveSerValidado", (int)OperadoresTexto.Contendo, 1);
			builder.AdicionarCondicao("campoQueDeveSerValidado", (int)OperadoresTexto.NaoContendo, 1);

			builder.AdicionarCondicao("campoQueDeveSerValidado", (int)OperadoresEspeciais.Is, 1);
			builder.AdicionarCondicao("campoQueDeveSerValidado", (int)OperadoresEspeciais.IsNot, 1);
			builder.AdicionarCondicao("campoQueDeveSerValidado", (int)OperadoresEspeciais.In, 1);
			builder.AdicionarCondicao("campoQueDeveSerValidado", (int)OperadoresEspeciais.NotIn, 1);

			builder.Wheres
				.Should()
				.HaveCount(14);

			builder.Wheres[0].Should().Be("([campoQueDeveSerValidado]=@_p0)");
			builder.Wheres[1].Should().Be("([campoQueDeveSerValidado]IS NULL)");
			builder.Wheres[2].Should().Be("([campoQueDeveSerValidado]IS NULL)");
			builder.Wheres[3].Should().Be("([campoQueDeveSerValidado]<>@_p3)");
			builder.Wheres[4].Should().Be("([campoQueDeveSerValidado]>@_p4)");
			builder.Wheres[5].Should().Be("([campoQueDeveSerValidado]>=@_p5)");
			builder.Wheres[6].Should().Be("([campoQueDeveSerValidado]<@_p6)");
			builder.Wheres[7].Should().Be("([campoQueDeveSerValidado]<=@_p7)");
			builder.Wheres[8].Should().Be("([campoQueDeveSerValidado]like@_p8)");
			builder.Wheres[9].Should().Be("([campoQueDeveSerValidado]not like@_p9)");
			builder.Wheres[10].Should().Be("([campoQueDeveSerValidado]is@_p10)");
			builder.Wheres[11].Should().Be("([campoQueDeveSerValidado]is not@_p11)");
			builder.Wheres[12].Should().Be("([campoQueDeveSerValidado]in@_p12)");
			builder.Wheres[13].Should().Be("([campoQueDeveSerValidado]not in@_p13)");

			builder.ProximoParametro
				.Should()
				.Be(14);
		}

		[TestMethod]
		public void SeAdicionarCondicaoNaoNuloNaQueryFakeAVariavelDeveSerDefinidaCorretamente()
		{
			var builder = new QueryFakeBuilder();

			builder.Wheres
				.Should()
				.NotBeNull();

			builder.Wheres
				.Should()
				.HaveCount(0);

			builder.AdicionarCondicaoApenasCampoNaoNulo("campoQueDeveSerValidado");

			builder.Wheres
				.Should()
				.HaveCount(1);

			builder.Wheres[0]
				.Should()
				.Be("([campoQueDeveSerValidado]IS NOT NULL)");
		}

		[TestMethod]
		public void SeAdicionarCondicaoEntreNaQueryFakeAVariavelDeveSerDefinidaCorretamente()
		{
			var builder = new QueryFakeBuilder();

			builder.Wheres
				.Should()
				.NotBeNull();

			builder.Wheres
				.Should()
				.HaveCount(0);

			builder.ProximoParametro
				.Should()
				.Be(0);

			builder.AdicionarCondicaoEntre("campoQueDeveSerValidado", 1, 2);
			builder.AdicionarCondicaoEntre("campoQueDeveSerValidado", 1, null);
			builder.AdicionarCondicaoEntre("campoQueDeveSerValidado", null, 2);

			builder.Wheres
				.Should()
				.HaveCount(4);

			builder.Wheres[0].Should().Be("([campoQueDeveSerValidado]>=@_p0a)");
			builder.Wheres[1].Should().Be("([campoQueDeveSerValidado]<=@_p0b)");
			builder.Wheres[2].Should().Be("([campoQueDeveSerValidado]>=@_p1a)");
			builder.Wheres[3].Should().Be("([campoQueDeveSerValidado]<=@_p2b)");

			builder.ProximoParametro
			.Should()
			.Be(3);

		}

		[TestMethod]
		public void SeAdicionarRelacionamentoNaQueryFakeAVariavelDeveSerDefinidaCorretamente()
		{
			var builder = new QueryFakeBuilder();

			builder.Joins
				.Should()
				.NotBeNull();

			builder.Joins
				.Should()
				.HaveCount(0);

			builder.AdicionarRelacionamento("join correto qualquer");

			builder.Joins
				.Should()
				.HaveCount(1);

			builder.Joins[0]
				.Should()
				.Be("join correto qualquer");
		}

		[TestMethod]
		public void SeAdicionarOrdenacaoNaQueryFakeAVariavelDeveSerDefinidaCorretamente()
		{
			var builder = new QueryFakeBuilder();

			builder.OrderBys
				.Should()
				.NotBeNull();

			builder.OrderBys
				.Should()
				.HaveCount(0);

			builder.AdicionarOrdem("lista de campos qualquer");

			builder.OrderBys
				.Should()
				.HaveCount(1);

			builder.OrderBys[0]
				.Should()
				.Be("lista de campos qualquer");
		}

		[TestMethod]
		public void SeAdicionarOrdenacaoDescendenteNaQueryFakeAVariavelDeveSerDefinidaCorretamente()
		{
			var builder = new QueryFakeBuilder();

			builder.OrderBys
				.Should()
				.NotBeNull();

			builder.OrderBys
				.Should()
				.HaveCount(0);

			builder.AdicionarOrdemDescendente("lista de campos qualquer");

			builder.OrderBys
				.Should()
				.HaveCount(1);

			builder.OrderBys[0]
				.Should()
				.Be("lista de campos qualquer DESC");
		}

		[TestMethod]
		public void SeGerarScriptSemDicionarioNaQueryFakeDeveGerarScriptCorretamente()
		{
			var builder = new QueryFakeBuilder();

			builder.DefinirTabela("Tabela");

			builder.GerarScript(null)
				.Should()
				.Be("select[Tabela].* from[Tabela]");

			builder.DefinirLimite(15);

			builder.GerarScript(null)
				.Should()
				.Be("select top 15 [Tabela].* from[Tabela]");

			builder.DefinirLimite(20);
			builder.AdicionarRelacionamento("inner join Relacionamento on Tabela.campo = Relacionamento.campo");
			builder.AdicionarRelacionamento("left join Parente on Tabela.Relacao = Parente.Relacao");

			builder.GerarScript(null)
				.Should()
				.Be("select top 20 [Tabela].* from[Tabela]" +
				    "inner join Relacionamento on Tabela.campo = Relacionamento.campo " +
				    "left join Parente on Tabela.Relacao = Parente.Relacao ");

			builder.DefinirLimite(25);
			builder.AdicionarCondicao("campoA", (int)Operadores.Igual, 1);
			builder.AdicionarCondicao("campoB", (int)Operadores.Igual, 2);

			builder.GerarScript(null)
				.Should()
				.Be("select top 25 [Tabela].* from[Tabela]" +
				    "inner join Relacionamento on Tabela.campo = Relacionamento.campo " +
				    "left join Parente on Tabela.Relacao = Parente.Relacao " +
				    "where([campoA]=@_p0)and([campoB]=@_p1) ");

			builder.DefinirLimite(30);
			builder.AdicionarAgrupamento("Tabela.CampoX");
			builder.AdicionarAgrupamento("Tabela.CampoY");
			builder.AdicionarAgrupamento("Tabela.CampoZ");

			builder.GerarScript(null)
				.Should()
				.Be("select top 30 [Tabela].* from[Tabela]" +
					"inner join Relacionamento on Tabela.campo = Relacionamento.campo " +
					"left join Parente on Tabela.Relacao = Parente.Relacao " +
					"where([campoA]=@_p0)and([campoB]=@_p1) " +
					"group by Tabela.CampoX,Tabela.CampoY,Tabela.CampoZ ");

			builder.DefinirLimite(35);
			builder.AdicionarCondicaoAgrupamento("Count(*)>0");
			builder.AdicionarCondicaoAgrupamento("Max(Tabela.Quantidade)>1000");

			builder.GerarScript(null)
				.Should()
				.Be("select top 35 [Tabela].* from[Tabela]" +
					"inner join Relacionamento on Tabela.campo = Relacionamento.campo " +
					"left join Parente on Tabela.Relacao = Parente.Relacao " +
					"where([campoA]=@_p0)and([campoB]=@_p1) " +
					"group by Tabela.CampoX,Tabela.CampoY,Tabela.CampoZ " +
					"having(Count(*)>0)and(Max(Tabela.Quantidade)>1000) ");

			builder.AdicionarOrdemDescendente("Tabela.OrdemB");
			builder.AdicionarOrdem("Tabela.OrdemA");

			builder.GerarScript(null)
				.Should()
				.Be("select top 35 [Tabela].* from[Tabela]" +
					"inner join Relacionamento on Tabela.campo = Relacionamento.campo " +
					"left join Parente on Tabela.Relacao = Parente.Relacao " +
					"where([campoA]=@_p0)and([campoB]=@_p1) " +
					"group by Tabela.CampoX,Tabela.CampoY,Tabela.CampoZ " +
					"having(Count(*)>0)and(Max(Tabela.Quantidade)>1000) " +
					"order by Tabela.OrdemB DESC,Tabela.OrdemA ");

		}

		[TestMethod]
		public void SeGerarScriptQueVerificaExistenciaNaQueryFakeSemDicionarioDeveGerarScriptCorretamente()
		{
			var builder = new QueryFakeBuilder();

			builder.DefinirTabela("Tabela");

			builder.GerarScriptExistencia(null)
				.Should()
				.Be("select top 1 1 from[Tabela]");

			builder.DefinirLimite(15);

			builder.GerarScriptExistencia(null)
				.Should()
				.Be("select top 1 1 from[Tabela]");

			builder.DefinirLimite(20);
			builder.AdicionarRelacionamento("inner join Relacionamento on Tabela.campo = Relacionamento.campo");
			builder.AdicionarRelacionamento("left join Parente on Tabela.Relacao = Parente.Relacao");

			builder.GerarScriptExistencia(null)
				.Should()
				.Be("select top 1 1 from[Tabela]" +
					"inner join Relacionamento on Tabela.campo = Relacionamento.campo " +
					"left join Parente on Tabela.Relacao = Parente.Relacao ");

			builder.DefinirLimite(25);
			builder.AdicionarCondicao("campoA", (int)Operadores.Igual, 1);
			builder.AdicionarCondicao("campoB", (int)Operadores.Igual, 2);

			builder.GerarScriptExistencia(null)
				.Should()
				.Be("select top 1 1 from[Tabela]" +
					"inner join Relacionamento on Tabela.campo = Relacionamento.campo " +
					"left join Parente on Tabela.Relacao = Parente.Relacao " +
					"where([campoA]=@_p0)and([campoB]=@_p1) ");

			builder.DefinirLimite(30);
			builder.AdicionarAgrupamento("Tabela.CampoX");
			builder.AdicionarAgrupamento("Tabela.CampoY");
			builder.AdicionarAgrupamento("Tabela.CampoZ");

			builder.GerarScriptExistencia(null)
				.Should()
				.Be("select top 1 1 from[Tabela]" +
					"inner join Relacionamento on Tabela.campo = Relacionamento.campo " +
					"left join Parente on Tabela.Relacao = Parente.Relacao " +
					"where([campoA]=@_p0)and([campoB]=@_p1) " +
					"group by Tabela.CampoX,Tabela.CampoY,Tabela.CampoZ ");

			builder.DefinirLimite(35);
			builder.AdicionarCondicaoAgrupamento("Count(*)>0");
			builder.AdicionarCondicaoAgrupamento("Max(Tabela.Quantidade)>1000");

			builder.GerarScriptExistencia(null)
				.Should()
				.Be("select top 1 1 from[Tabela]" +
					"inner join Relacionamento on Tabela.campo = Relacionamento.campo " +
					"left join Parente on Tabela.Relacao = Parente.Relacao " +
					"where([campoA]=@_p0)and([campoB]=@_p1) " +
					"group by Tabela.CampoX,Tabela.CampoY,Tabela.CampoZ " +
					"having(Count(*)>0)and(Max(Tabela.Quantidade)>1000) ");

			builder.AdicionarOrdemDescendente("Tabela.OrdemB");
			builder.AdicionarOrdem("Tabela.OrdemA");

			builder.GerarScriptExistencia(null)
				.Should()
				.Be("select top 1 1 from[Tabela]" +
					"inner join Relacionamento on Tabela.campo = Relacionamento.campo " +
					"left join Parente on Tabela.Relacao = Parente.Relacao " +
					"where([campoA]=@_p0)and([campoB]=@_p1) " +
					"group by Tabela.CampoX,Tabela.CampoY,Tabela.CampoZ " +
					"having(Count(*)>0)and(Max(Tabela.Quantidade)>1000) " +
					"order by Tabela.OrdemB DESC,Tabela.OrdemA ");

		}

		[TestMethod]
		public void SeGerarScriptsFakeAPartirDeUmDicionarioDeveGerarScriptCorretamente()
		{
			var builder = new QueryFakeBuilder();
			var dicionario = new Dicionario(typeof(ObjetoDeTestes));
			builder.DefinirTabela(dicionario.Nome);

			builder.GerarScript(dicionario)
				.Should()
				.Be("select" +
						"[Codigo],[CodigoNulo],[Nome],[Duplo],[DuploNulo],[Decimal]," +
						"[DecimalNulo],[Logico],[DataHora],[DataHoraNulo] " +
					"from[ObjetoVirtual]");

			builder.GerarScriptExistencia(dicionario)
				.Should()
				.Be("select top 1 1 from[ObjetoVirtual]");
		}

	}
}
