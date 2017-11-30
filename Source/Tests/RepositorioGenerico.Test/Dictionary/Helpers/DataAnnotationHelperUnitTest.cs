using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositorioGenerico.Dictionary.Helpers;
using RepositorioGenerico.Entities.Anotacoes;
using RepositorioGenerico.Test.Objetos;

namespace RepositorioGenerico.Test.Dictionary.Helpers
{
	[TestClass]
	public class DataAnnotationHelperUnitTest
	{

		[TestMethod]
		public void SeConsultarONomeDaTabelaObjetoDeTestesDeveSerObjetoVirtual()
		{
			DataAnnotationHelper.ConsultarNomeDaTabela(typeof(ObjetoDeTestes))
				.Should().Be("ObjetoVirtual");
		}

		[TestMethod]
		public void SeConsultarONomeDaTabelaQueNaoPossuiOAtributoDeveTrazerOProprioNomeDoObjeto()
		{
			DataAnnotationHelper.ConsultarNomeDaTabela(typeof(ObjetoSemHerancaCorreta))
				.Should().Be("ObjetoSemHerancaCorreta");
		}

		[TestMethod]
		public void SeConsultarPorUmaPropriedadeComAtributoColumnAttributoDeveTrazerUmValorValido()
		{
			var propriedade = typeof(ObjetoDeTestes).GetProperty("Codigo");
			var coluna = DataAnnotationHelper.ConsultarColuna(propriedade);
			coluna.Should().NotBeNull();
			coluna.Nome.Should().BeNull();
			coluna.Ordem.Should().Be(0);
			coluna.NomeDoTipo.Should().Be("int");
		}

		[TestMethod]
		public void SeConsultarPorUmaPropriedadeQueNaoPossuiOAtributoColumnAttributoDeveTrazerNulo()
		{
			var propriedade = typeof(ObjetoDeTestes).GetProperty("Filhos");
			var coluna = DataAnnotationHelper.ConsultarColuna(propriedade);
			coluna.Should().BeNull();
		}

		[TestMethod]
		public void SeVerificarSeAPropriedadeCodigoEhChavePrimariaDeveTrazerVerdadeiro()
		{
			var propriedade = typeof(ObjetoDeTestes).GetProperty("Codigo");
			DataAnnotationHelper.ChavePrimaria(propriedade)
				.Should().BeTrue();
		}

		[TestMethod]
		public void SeVerificarSeAPropriedadeNomeEhChavePrimariaDeveTrazerFalso()
		{
			var propriedade = typeof(ObjetoDeTestes).GetProperty("Nome");
			DataAnnotationHelper.ChavePrimaria(propriedade)
				.Should().BeFalse();
		}

		[TestMethod]
		public void SeVerificarSeAPropriedadeCodigoEhObrigatoriaDeveTrazerVerdadeiro()
		{
			var propriedade = typeof(ObjetoDeTestes).GetProperty("Codigo");
			DataAnnotationHelper.Obrigatorio(propriedade)
				.Should().BeTrue();
		}

		[TestMethod]
		public void SeVerificarSeAPropriedadeCodigoNuloEhObrigatoriaDeveTrazerFalso()
		{
			var propriedade = typeof(ObjetoDeTestes).GetProperty("CodigoNulo");
			DataAnnotationHelper.Obrigatorio(propriedade)
				.Should().BeFalse();
		}

		[TestMethod]
		public void SeConsultarOTamanhoMinimoDeUmaPropriedadeDeveTrazerValorCorreto()
		{
			var propriedade = typeof(ObjetoDeTestes).GetProperty("Nome");
			DataAnnotationHelper.ConsultarTamanhoMinimo(propriedade)
				.Should().Be(5);
		}

		[TestMethod]
		public void SeConsultarOTamanhoMinimoDeUmaPropriedadeQueNaoPossuiOAtributoDeveTrazerZero()
		{
			var propriedade = typeof(ObjetoDeTestes).GetProperty("Codigo");
			DataAnnotationHelper.ConsultarTamanhoMinimo(propriedade)
				.Should().Be(0);
		}

		[TestMethod]
		public void SeConsultarOTamanhoMaximoDeUmaPropriedadeDeveTrazerValorCorreto()
		{
			var propriedade = typeof(ObjetoDeTestes).GetProperty("Nome");
			DataAnnotationHelper.ConsultarTamanhoMaximo(propriedade)
				.Should().Be(50);
		}

		[TestMethod]
		public void SeConsultarOTamanhoMaximoDeUmaPropriedadeQueNaoPossuiOAtributoDeveTrazerZero()
		{
			var propriedade = typeof(ObjetoDeTestes).GetProperty("Codigo");
			DataAnnotationHelper.ConsultarTamanhoMaximo(propriedade)
				.Should().Be(0);
		}


		[TestMethod]
		public void SeConsultarAOpcaoDeGeracaoDaPropriedadeCodigoDeveTrazerValorCorreto()
		{
			var propriedade = typeof(ObjetoDeTestes).GetProperty("Codigo");
			DataAnnotationHelper.ConsultarOpcaoGeracao(propriedade)
				.Should().Be(Incremento.Identity);
		}

		[TestMethod]
		public void SeConsultarAOpcaoDeGeracaoDeUmapropriedadeQueNaoPossuiOAtributoDeveTrazerNone()
		{
			var propriedade = typeof(ObjetoDeTestes).GetProperty("Nome");
			DataAnnotationHelper.ConsultarOpcaoGeracao(propriedade)
				.Should().Be(Incremento.Nenhum);
		}

		[TestMethod]
		public void SeConsultarSeAPropriedadeCodigoEstaMapeadaDeveRetornarVerdadeiro()
		{
			var propriedade = typeof(ObjetoDeTestes).GetProperty("Codigo");
			DataAnnotationHelper.Mapeado(propriedade)
				.Should().BeTrue();
		}

		[TestMethod]
		public void SeConsultarSeAPropriedadeEstadoObjetoEstaMapeadaDeveRetornarFalso()
		{
			var propriedade = typeof(ObjetoDeTestes).GetProperty("EstadoEntidade");
			DataAnnotationHelper.Mapeado(propriedade)
				.Should().BeFalse();
		}

		[TestMethod]
		public void SeConsultarSeAPropriedadeFilhosEstaMapeadaDeveRetornarFalso()
		{
			var propriedade = typeof(ObjetoDeTestes).GetProperty("Filhos");
			DataAnnotationHelper.Mapeado(propriedade)
				.Should().BeFalse();
		}

		[TestMethod]
		public void SeConsultarOValorPadraoDaPropriedadeEstadoObjetoDeveRetornarFalso()
		{
			var propriedade = typeof(ObjetoDeTestes).GetProperty("Logico");
			var valor = (bool)DataAnnotationHelper.ConsultarValorPadrao(propriedade);
			valor.Should().BeTrue();
		}

		[TestMethod]
		public void SeConsultarOValorPadraoDaPropriedadeFilhosDeveRetornarNull()
		{
			var propriedade = typeof(ObjetoDeTestes).GetProperty("Filhos");
			DataAnnotationHelper.ConsultarValorPadrao(propriedade)
				.Should().BeNull();
		}

		[TestMethod]
		public void SeConsultarAForeingKeyDaPropriedadPaiDeveRetornarCodigoPai()
		{
			var propriedade = typeof(FilhoDoObjetoDeTestes).GetProperty("Pai");
			DataAnnotationHelper.ConsultarForeignKey(propriedade)
				.Should().Be("CodigoPai");
		}

		[TestMethod]
		public void SeConsultarAForeingKeyDaPropriedadeFilhosDeveRetornarNull()
		{
			var propriedade = typeof(ObjetoDeTestes).GetProperty("Filhos");
			DataAnnotationHelper.ConsultarForeignKey(propriedade)
				.Should().BeNull();
		}

		[TestMethod]
		public void SeConsultarAForeingKeyDaInversePropertyDaPropriedadPaiDeveRetornarNulo()
		{
			var propriedade = typeof(FilhoDoObjetoDeTestes).GetProperty("Pai");
			DataAnnotationHelper.ConsultarForeignKeyDaInverseProperty(propriedade, typeof(ICollection<FilhoDoObjetoDeTestes>))
				.Should().BeNull();
		}

		[TestMethod]
		public void SeConsultarAForeingKeyDaInversePropertyDeUmaPropriedadeInvalidaDeveRetornarNulo()
		{
			var propriedadeNaoGenerica = typeof(ObjetoDeTestes).GetProperty("DataHora");
			DataAnnotationHelper.ConsultarForeignKeyDaInverseProperty(propriedadeNaoGenerica, typeof(DateTime))
				.Should().BeNull();

			var propriedadeGenericaNaoCollection = typeof(ObjetoDeTestes).GetProperty("CodigoNulo");
			DataAnnotationHelper.ConsultarForeignKeyDaInverseProperty(propriedadeGenericaNaoCollection, typeof(int?))
				.Should().BeNull();
		}

		[TestMethod]
		public void SeConsultarAForeingKeyDaInversePropertyDeUmaPropriedadeConfiguradaDeveRetornarONomeDaForeingKey()
		{
			var propriedade = typeof(ObjetoDeTestes).GetProperty("Filhos");
			DataAnnotationHelper.ConsultarForeignKeyDaInverseProperty(propriedade, typeof(ICollection<FilhoDoObjetoDeTestes>))
				.Should().Be("CodigoPai");
		}

		[TestMethod]
		public void SeConsultarAForeingKeyDaInversePropertyDeUmaPropriedadeMapeadaConfiguradaDeveRetornarONomeDaForeingKey()
		{
			var propriedade = typeof(ObjetoMapeadoDeTestes).GetProperty("MapeadoComFilhos");
			DataAnnotationHelper.ConsultarForeignKeyDaInverseProperty(propriedade, typeof(ICollection<FilhoMapeadoDoObjetoMapeadoDeTestes>))
				.Should().Be("CodigoPai");
		}

	}
}
