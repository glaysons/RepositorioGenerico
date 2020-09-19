// ReSharper disable ObjectCreationAsStatement

using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositorioGenerico.Entities.Anotacoes;
using RepositorioGenerico.Exceptions;
using RepositorioGenerico.Test.Objetos;

namespace RepositorioGenerico.Dictionary.Test
{
	[TestClass]
	public class DicionarioUnitTest
	{

		[TestMethod]
		public void SeCriarUmDicionarioNaoDeveGerarErro()
		{
			Action act = () => { new Dicionario(typeof(ObjetoDeTestes)); };
			act.Should().NotThrow();
		}

		[TestMethod]
		public void SeCriarUmDicionarioMapeadoNaoDeveGerarErro()
		{
			Action act = () => { new Dicionario(typeof(ObjetoMapeadoDeTestes)); };
			act.Should().NotThrow();
		}

		[TestMethod]
		public void SeCriarUmDicionarioMistoNaoDeveGerarErro()
		{
			Action act = () => { new Dicionario(typeof(ObjetoMistoDeTestes)); };
			act.Should().NotThrow();
		}

		[TestMethod]
		public void SeCriarUmDicionarioDeUmObjetoQueNaoHerdeObjetoDeBancoDeveGerarErro()
		{
			Action act = () => { new Dicionario(typeof(ObjetoSemHerancaCorreta)); };
			act.Should().Throw<NaoSeraPossivelCriarUmDicionarioDeUmObjetoQueNaoHerdeObjetoBancoException>();
		}

		[TestMethod]
		public void SeCriarUmDicionarioDeveCarregarPropriedadesCorretamente()
		{
			var dicionario = new Dicionario(typeof(ObjetoDeTestes));
			ValidarPropriedadesDaClasse(dicionario);
			ValidarItensDoDicionario(dicionario);
			ValidarItensDoDicionarioComValidadores(dicionario);
			ValidarItensDoDicionarioNaoMapeados(dicionario);
		}

		private static void ValidarPropriedadesDaClasse(Dicionario dicionario)
		{
			dicionario.Alias
				.Should().Be("ObjetoDeTestes");

			dicionario.AliasOuNome
				.Should().Be("ObjetoDeTestes");

			dicionario.Tipo
				.Should().Be(typeof(ObjetoDeTestes));

			dicionario.OrigemMapa
				.Should().BeNull();

			dicionario.AutoIncremento
				.Should().Be(OpcoesAutoIncremento.Identity);

			dicionario.Nome
				.Should().Be("ObjetoVirtual");

			dicionario.QuantidadeCamposNaChave
				.Should().Be(1);

			dicionario.QuantidadeCampos
				.Should().Be(10);

			dicionario.PossuiCamposFilho
				.Should().BeTrue();

			dicionario.Validador
				.Should().NotBeNull();

			dicionario.Validador.Quantidade
				.Should().Be(2);
		}

		private void ValidarItensDoDicionario(Dicionario dicionario)
		{
			dicionario.Itens.FirstOrDefault(i => string.Equals(i.Nome, "Codigo")).Should().NotBeNull();
			dicionario.Itens.FirstOrDefault(i => string.Equals(i.Nome, "CodigoNulo")).Should().NotBeNull();
			dicionario.Itens.FirstOrDefault(i => string.Equals(i.Nome, "Nome")).Should().NotBeNull();
			dicionario.Itens.FirstOrDefault(i => string.Equals(i.Nome, "Duplo")).Should().NotBeNull();
			dicionario.Itens.FirstOrDefault(i => string.Equals(i.Nome, "DuploNulo")).Should().NotBeNull();
			dicionario.Itens.FirstOrDefault(i => string.Equals(i.Nome, "Decimal")).Should().NotBeNull();
			dicionario.Itens.FirstOrDefault(i => string.Equals(i.Nome, "DecimalNulo")).Should().NotBeNull();
			dicionario.Itens.FirstOrDefault(i => string.Equals(i.Nome, "Logico")).Should().NotBeNull();
			dicionario.Itens.FirstOrDefault(i => string.Equals(i.Nome, "DataHora")).Should().NotBeNull();
			dicionario.Itens.FirstOrDefault(i => string.Equals(i.Nome, "DataHoraNulo")).Should().NotBeNull();
			dicionario.Itens.FirstOrDefault(i => string.Equals(i.Nome, "EstadoEntidade")).Should().BeNull();
		}

		private void ValidarItensDoDicionarioComValidadores(Dicionario dicionario)
		{
			var campoCodigo = dicionario.ConsultarPorCampo("Codigo");
			campoCodigo.Should().NotBeNull();
			campoCodigo.Validacoes.Should().BeNull();

			var campoNome = dicionario.ConsultarPorCampo("Nome");
			campoNome.Should().NotBeNull();
			campoNome.Validacoes.Should().NotBeNull();
			campoNome.Validacoes.Count.Should().Be(2);
		}

		private void ValidarItensDoDicionarioNaoMapeados(Dicionario dicionario)
		{
			dicionario.ConsultarPorCampo("EstadoEntidade").Should().BeNull();
			dicionario.ConsultarPorPropriedade("EstadoEntidade").Should().BeNull();

			dicionario.ConsultarPorCampo("Filhos").Should().NotBeNull();
			dicionario.ConsultarPorPropriedade("Filhos").Should().NotBeNull();
		}

		[TestMethod]
		public void SeConsultarAChaveDeUmObjetoDeveRetornarOValorEsperado()
		{
			var dicionarioSimples = new Dicionario(typeof(ObjetoDeTestes));

			var objetoSimples = new ObjetoDeTestes()
			{
				Codigo = 123,
				Nome = "Testes ABC"
			};

			dicionarioSimples.ConsultarValoresDaChave(objetoSimples)
				.Should().BeEquivalentTo(123);

			var dicionarioDupla = new Dicionario(typeof(ObjetoComChaveDupla));

			var objetoDupla = new ObjetoComChaveDupla()
			{
				ChaveBase = 345,
				ChaveAutoIncremento = 789,
				Nome = "Testes CDF"
			};

			dicionarioDupla.ConsultarValoresDaChave(objetoDupla)
				.Should().BeEquivalentTo(345, 789);
		}

		[TestMethod]
		public void SeConsultarAChaveDeUmObjetoMapeadoDeveRetornarOValorEsperado()
		{
			var dicionarioSimples = new Dicionario(typeof(ObjetoMapeadoDeTestes));

			var objetoSimples = new ObjetoMapeadoDeTestes()
			{
				MapeadoComCodigo = 123,
				MapeadoComNome = "Testes ABC"
			};

			dicionarioSimples.ConsultarValoresDaChave(objetoSimples)
				.Should().BeEquivalentTo(123);

			var dicionarioDupla = new Dicionario(typeof(ObjetoMapeadoComChaveDupla));

			var objetoDupla = new ObjetoMapeadoComChaveDupla()
			{
				MapeadoComChaveBase = 345,
				MapeadoComChaveAutoIncremento = 789,
				MapeadoComNome = "Testes CDF"
			};

			dicionarioDupla.ConsultarValoresDaChave(objetoDupla)
				.Should().BeEquivalentTo(345, 789);
		}

		[TestMethod]
		public void SeConultarAChaveEstrangeiraDeUmObjetoDeveRetornarOValorEsperado()
		{
			var dicionario = new Dicionario(typeof(FilhoDoObjetoDeTestes));

			var objeto = new FilhoDoObjetoDeTestes()
			{
				Id = 123,
				Nome = "Testes ABC",
				IdPai = 775
			};

			dicionario.ConsultarValoresDaChave(objeto, new[] { "IdPai" })
				.Should().BeEquivalentTo(775);
		}

		[TestMethod]
		public void SeConultarAChaveEstrangeiraDeUmObjetoMapeadoDeveRetornarOValorEsperado()
		{
			var dicionario = new Dicionario(typeof(FilhoMapeadoDoObjetoMapeadoDeTestes));

			var objeto = new FilhoMapeadoDoObjetoMapeadoDeTestes()
			{
				MapeadoComCodigoFilho = 123,
				MapeadoComNomeFilho = "Testes ABC",
				MapeadoComCodigoPai = 775
			};

			dicionario.ConsultarValoresDaChave(objeto, new[] { "MapeadoComCodigoPai" })
				.Should().BeEquivalentTo(775);
		}

		[TestMethod]
		public void SeConsultarAChaveEstrangeiraDeUmObjetoComNomeDeCampoInvalidoNaoDeveGerarErro()
		{
			var dicionario = new Dicionario(typeof(FilhoMapeadoDoObjetoMapeadoDeTestes));

			var objeto = new FilhoMapeadoDoObjetoMapeadoDeTestes()
			{
				MapeadoComCodigoFilho = 123,
				MapeadoComNomeFilho = "Testes ABC",
				MapeadoComCodigoPai = 775
			};

			Action consulta = () => dicionario.ConsultarValoresDaChave(objeto, new[] { "NomeDeCampoInexistenteNaTabela" });

			consulta
				.Should().NotThrow();
		}

		[TestMethod]
		public void SeConsultarOsCamposChavesDeveRetornarOsCamposEsperados()
		{
			var dicionarioDupla = new Dicionario(typeof(ObjetoComChaveDupla));

			dicionarioDupla.QuantidadeCamposNaChave
				.Should().Be(2);

			dicionarioDupla.QuantidadeCampos
				.Should().Be(3);

			var campos = dicionarioDupla.ConsultarCamposChave().GetEnumerator();

			campos.MoveNext().Should().BeTrue();
			campos.Current.Should().NotBeNull();
			campos.Current.Nome.Should().Be("ChaveBase");

			campos.MoveNext().Should().BeTrue();
			campos.Current.Should().NotBeNull();
			campos.Current.Nome.Should().Be("ChaveAutoIncremento");

			campos.MoveNext().Should().BeFalse();
		}

		[TestMethod]
		public void SeConsultarOsCamposChavesMapeadosDeveRetornarOsCamposEsperados()
		{
			var dicionarioDupla = new Dicionario(typeof(ObjetoMapeadoComChaveDupla));

			dicionarioDupla.QuantidadeCamposNaChave
				.Should().Be(2);

			var campos = dicionarioDupla.ConsultarCamposChave().GetEnumerator();

			campos.MoveNext().Should().BeTrue();
			campos.Current.Should().NotBeNull();
			campos.Current.Alias.Should().Be("MapeadoComChaveBase");
			campos.Current.Nome.Should().Be("ChaveBase");

			campos.MoveNext().Should().BeTrue();
			campos.Current.Should().NotBeNull();
			campos.Current.Alias.Should().Be("MapeadoComChaveAutoIncremento");
			campos.Current.Nome.Should().Be("ChaveAutoIncremento");

			campos.MoveNext().Should().BeFalse();
		}

		[TestMethod]
		public void SeConsultarOsCamposRelacionadosDeveRetornarOsCamposEsperados()
		{
			var dicionario = new Dicionario(typeof(ObjetoDeTestes));

			dicionario.PossuiCamposFilho
				.Should().BeTrue();

			var campos = dicionario.ConsultarCamposFilho().GetEnumerator();

			campos.MoveNext().Should().BeTrue();
			campos.Current.Should().NotBeNull();
			campos.Current.Nome.Should().Be("Filhos");

			campos.MoveNext().Should().BeFalse();
		}

		[TestMethod]
		public void SeConsultarOsCamposMapeadosRelacionadosDeveRetornarOsCamposEsperados()
		{
			var dicionario = new Dicionario(typeof(ObjetoMapeadoDeTestes));

			dicionario.PossuiCamposFilho
				.Should().BeTrue();

			var campos = dicionario.ConsultarCamposFilho().GetEnumerator();

			campos.MoveNext().Should().BeTrue();
			campos.Current.Should().NotBeNull();
			campos.Current.Alias.Should().Be("MapeadoComFilhos");
			campos.Current.Nome.Should().Be("Filhos");

			campos.MoveNext().Should().BeFalse();
		}

		[TestMethod]
		public void SeCriarUmDicionarioDeUmObjetoMapeadoDeveCarregarPropriedadesCorretamente()
		{
			var dicionario = new Dicionario(typeof(ObjetoMapeadoDeTestes));
			ValidarPropriedadesMapeadasDaClasse(dicionario);
			ValidarItensDoDicionario(dicionario);
			ValidarAliasDosItensDoDicionario(dicionario);
			ValidarItensMapeadosDoDicionarioComValidadores(dicionario);
			ValidarItensMapeadosDoDicionarioNaoMapeados(dicionario);
		}

		private static void ValidarPropriedadesMapeadasDaClasse(Dicionario dicionario)
		{
			dicionario.Alias
				.Should().Be("ObjetoMapeadoDeTestes");

			dicionario.AliasOuNome
				.Should().Be("ObjetoMapeadoDeTestes");

			dicionario.Tipo
				.Should().Be(typeof(ObjetoMapeadoDeTestes));

			dicionario.OrigemMapa
				.Should().Be(typeof(ObjetoDeTestes));

			dicionario.AutoIncremento
				.Should().Be(OpcoesAutoIncremento.Identity);

			dicionario.Nome
				.Should().Be("ObjetoVirtual");

			dicionario.QuantidadeCamposNaChave
				.Should().Be(1);

			dicionario.QuantidadeCampos
				.Should().Be(10);

			dicionario.PossuiCamposFilho
				.Should().BeTrue();

			dicionario.Validador
				.Should().NotBeNull();

			dicionario.Validador.Quantidade
				.Should().Be(2);
		}

		private void ValidarAliasDosItensDoDicionario(Dicionario dicionario)
		{
			dicionario.Itens.FirstOrDefault(i => string.Equals(i.Alias, "MapeadoComCodigo")).Should().NotBeNull();
			dicionario.Itens.FirstOrDefault(i => string.Equals(i.Alias, "MapeadoComCodigoNulo")).Should().NotBeNull();
			dicionario.Itens.FirstOrDefault(i => string.Equals(i.Alias, "MapeadoComNome")).Should().NotBeNull();
			dicionario.Itens.FirstOrDefault(i => string.Equals(i.Alias, "MapeadoComDuplo")).Should().NotBeNull();
			dicionario.Itens.FirstOrDefault(i => string.Equals(i.Alias, "MapeadoComDuploNulo")).Should().NotBeNull();
			dicionario.Itens.FirstOrDefault(i => string.Equals(i.Alias, "MapeadoComDecimal")).Should().NotBeNull();
			dicionario.Itens.FirstOrDefault(i => string.Equals(i.Alias, "MapeadoComDecimalNulo")).Should().NotBeNull();
			dicionario.Itens.FirstOrDefault(i => string.Equals(i.Alias, "MapeadoComLogico")).Should().NotBeNull();
			dicionario.Itens.FirstOrDefault(i => string.Equals(i.Alias, "MapeadoComDataHora")).Should().NotBeNull();
			dicionario.Itens.FirstOrDefault(i => string.Equals(i.Alias, "MapeadoComDataHoraNulo")).Should().NotBeNull();
			dicionario.Itens.FirstOrDefault(i => string.Equals(i.Alias, "EstadoEntidade")).Should().BeNull();
		}

		private void ValidarItensMapeadosDoDicionarioComValidadores(Dicionario dicionario)
		{
			var campoCodigo = dicionario.ConsultarPorPropriedade("MapeadoComCodigo");
			campoCodigo.Should().NotBeNull();
			campoCodigo.Validacoes.Should().BeNull();

			var campoNome = dicionario.ConsultarPorPropriedade("MapeadoComNome");
			campoNome.Should().NotBeNull();
			campoNome.Validacoes.Should().NotBeNull();
			campoNome.Validacoes.Count.Should().Be(2);
		}

		private void ValidarItensMapeadosDoDicionarioNaoMapeados(Dicionario dicionario)
		{
			dicionario.ConsultarPorCampo("EstadoEntidade").Should().BeNull();
			dicionario.ConsultarPorPropriedade("EstadoEntidade").Should().BeNull();

			dicionario.ConsultarPorCampo("Filhos").Should().NotBeNull();
			dicionario.ConsultarPorPropriedade("MapeadoComFilhos").Should().NotBeNull();
		}

		[TestMethod]
		public void SeCriarUmDicionarioDeUmFilhoDoObjetoDeTestesDeveCarregarAliasComNomeDaClasse()
		{
			var dicionario = new Dicionario(typeof(FilhoDoObjetoDeTestes));
			dicionario.Nome
				.Should().Be("ObjetoVirtualFilho");

			dicionario.Alias
				.Should().Be("FilhoDoObjetoDeTestes");

			dicionario.AliasOuNome
				.Should().Be("FilhoDoObjetoDeTestes");
		}

		[TestMethod]
		public void SeCriarUmDicionarioDeUmObjetoMistoDeveCarregarPropriedadesCorretamente()
		{
			var dicionario = new Dicionario(typeof(ObjetoMistoDeTestes));
			ValidarPropriedadesMistaDaClasse(dicionario);
			ValidarItensDoDicionario(dicionario);
			ValidarAliasDosItensMistoDoDicionario(dicionario);
			ValidarItensMistoDoDicionarioComValidadores(dicionario);
			ValidarItensMistoDoDicionarioNaoMapeados(dicionario);
		}

		private static void ValidarPropriedadesMistaDaClasse(Dicionario dicionario)
		{
			dicionario.Alias
				.Should().Be("ObjetoMistoDeTestes");

			dicionario.AliasOuNome
				.Should().Be("ObjetoMistoDeTestes");

			dicionario.Tipo
				.Should().Be(typeof(ObjetoMistoDeTestes));

			dicionario.OrigemMapa
				.Should().BeNull();

			dicionario.AutoIncremento
				.Should().Be(OpcoesAutoIncremento.Identity);

			dicionario.Nome
				.Should().Be("ObjetoVirtual");

			dicionario.QuantidadeCamposNaChave
				.Should().Be(1);

			dicionario.QuantidadeCampos
				.Should().Be(10);

			dicionario.PossuiCamposFilho
				.Should().BeTrue();

			dicionario.Validador
				.Should().NotBeNull();

			dicionario.Validador.Quantidade
				.Should().Be(2);
		}

		private void ValidarAliasDosItensMistoDoDicionario(Dicionario dicionario)
		{
			dicionario.Itens.FirstOrDefault(i => string.Equals(i.Alias, "AliasCodigo")).Should().NotBeNull();
			dicionario.Itens.FirstOrDefault(i => string.Equals(i.Alias, "AliasCodigoNulo")).Should().NotBeNull();
			dicionario.Itens.FirstOrDefault(i => string.Equals(i.Alias, "AliasNome")).Should().NotBeNull();
			dicionario.Itens.FirstOrDefault(i => string.Equals(i.Alias, "AliasDecimal")).Should().NotBeNull();
			dicionario.Itens.FirstOrDefault(i => string.Equals(i.Alias, "AliasDecimalNulo")).Should().NotBeNull();
			dicionario.Itens.FirstOrDefault(i => string.Equals(i.Alias, "AliasLogico")).Should().NotBeNull();
			dicionario.Itens.FirstOrDefault(i => string.Equals(i.Alias, "AliasDataHora")).Should().NotBeNull();
			dicionario.Itens.FirstOrDefault(i => string.Equals(i.Alias, "AliasDataHoraNulo")).Should().NotBeNull();
			dicionario.Itens.FirstOrDefault(i => string.Equals(i.Alias, "EstadoEntidade")).Should().BeNull();
		}

		private void ValidarItensMistoDoDicionarioComValidadores(Dicionario dicionario)
		{
			var campoCodigo = dicionario.ConsultarPorPropriedade("AliasCodigo");
			campoCodigo.Should().NotBeNull();
			campoCodigo.Validacoes.Should().BeNull();

			var campoNome = dicionario.ConsultarPorPropriedade("AliasNome");
			campoNome.Should().NotBeNull();
			campoNome.Validacoes.Should().NotBeNull();
			campoNome.Validacoes.Count.Should().Be(2);
		}

		private void ValidarItensMistoDoDicionarioNaoMapeados(Dicionario dicionario)
		{
			dicionario.ConsultarPorCampo("EstadoEntidade").Should().BeNull();
			dicionario.ConsultarPorPropriedade("EstadoEntidade").Should().BeNull();

			dicionario.ConsultarPorCampo("AliasFilhos").Should().NotBeNull();
			dicionario.ConsultarPorPropriedade("AliasFilhos").Should().NotBeNull();
		}

		[TestMethod]
		public void SeConsultarOsItensDeUmObjetoSemEstruturaDefinidaDeveGerarErroEspecifico()
		{
			var dicionario = new Dicionario(typeof(ObjetoSemEstruturaDefinida));
			Action consulta = () => dicionario.Itens.Count();
			consulta
				.Should().Throw<TabelaNaoPossuiInformacoesDeCamposDaTabelaException>();
		}

		[TestMethod]
		public void SeConsultarOsItensDeUmObjetoSemChaveDefinidaNaoDeveGerarErro()
		{
			var dicionario = new Dicionario(typeof(ObjetoSemChaveDefinida));

			dicionario.QuantidadeCampos
				.Should()
				.Be(2);
		}

		[TestMethod]
		public void SeCriarObjetoComDoisCamposAutoIncrementoNaChaveDeveGerarErroEspecifico()
		{
			var dicionario = new Dicionario(typeof(ObjetoComDoisCamposAutoIncremento));
			Action consulta = () => dicionario.Itens.Count();
			consulta
				.Should().Throw<DicionarioNaoSuportaMultiplosCamposAutoIncrementoException>();

		}

		[TestMethod]
		public void SeCriarObjetoComDoisCamposComputadosNaChaveDeveGerarErroEspecifico()
		{
			var dicionario = new Dicionario(typeof(ObjetoComDoisCamposComputados));
			Action consulta = () => dicionario.Itens.Count();
			consulta
				.Should().Throw<DicionarioNaoSuportaMultiplosCamposAutoIncrementoException>();

		}

		[TestMethod]
		public void SeCriarObjetoComChaveComputadaDeveRetornarCampoComputado()
		{
			var dicionario = new Dicionario(typeof(ObjetoComChaveComputada));
			dicionario.ConsultarPorCampo("Codigo").OpcaoGeracao
				.Should().Be(Incremento.Calculado);
		}

		[TestMethod]
		public void SeCriarObjetoComCampoAutoIncrementoEComputadoDeveGerarErroEspecifico()
		{
			var dicionario = new Dicionario(typeof(ObjetoComChaveAutoIncrementoEComputada));
			Action consulta = () => dicionario.Itens.Count();
			consulta
				.Should().Throw<DicionarioNaoSuportaMultiplosCamposAutoIncrementoException>();
			
		}

	}
}
