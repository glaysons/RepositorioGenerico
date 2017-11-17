using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositorioGenerico.Dictionary;
using RepositorioGenerico.Exceptions;
using RepositorioGenerico.Test.Objetos;

namespace RepositorioGenerico.Test.Dictionary.Validadores
{
	[TestClass]
	public class ValidadorDicionarioUnitTest
	{

		[TestMethod]
		public void SeValidarPreenchimentoObrigatorioDeveGerarErroEspecifico()
		{
			var objeto = new ObjetoDeTestes()
			{
				Nome = null
			};
			var dicionario = new Dicionario(typeof(ObjetoDeTestes));
			var validador = dicionario.Validador;
			
			Action validacao = () => validador.Validar(objeto);

			validacao.ShouldThrow<CampoPossuiPreenchimentoObrigatorioException>();

			var valido = validador.Valido(objeto).ToList();

			valido.Count
				.Should().BeGreaterThan(0);
			valido[0].Should().Be("O campo [Nome] é de preenchimento obrigatório!");
		}

		[TestMethod]
		public void SeCampoForPreenchidoCorretamenteNaoDeveGerarErro()
		{
			var objeto = new ObjetoDeTestes()
			{
				Nome = "Teste de Preenchimento Tranquilo"
			};
			var dicionario = new Dicionario(typeof(ObjetoDeTestes));
			var validador = dicionario.Validador;

			Action validacao = () => validador.Validar(objeto);

			validacao.ShouldNotThrow();

			var valido = validador.Valido(objeto).ToList();

			valido.Count
				.Should().Be(0);
		}


		[TestMethod]
		public void SeCampoForPreenchidoComTamanhoAcimaDoMaximoPermitidoDeveGerarErroEspecifico()
		{
			var objeto = new ObjetoDeTestes()
			{
				Nome = "12345678901234567890123456789012345678901234567890123456789012345678901234567890"
			};
			var dicionario = new Dicionario(typeof(ObjetoDeTestes));
			var validador = dicionario.Validador;
			
			Action validacao = () => validador.Validar(objeto);

			validacao.ShouldThrow<CampoPossuiTamanhoMaximoDePeenchimentoException>();

			var valido = validador.Valido(objeto).ToList();

			valido.Count
				.Should().BeGreaterThan(0);
			valido[0].Should().Be("O campo [Nome] permite ser preenchido com no máximo [50] caracteres!");
		}

		[TestMethod]
		public void SeValidarUmObjetoSemValidadoresNaoDeveGerarErro()
		{
			var objeto = new ObjetoSemValidacao();
			var dicionario = new Dicionario(typeof(ObjetoSemValidacao));
			var validador = dicionario.Validador;

			Action validacao = () => validador.Validar(objeto);

			validacao.ShouldNotThrow();

			var valido = validador.Valido(objeto).ToList();

			valido.Count
				.Should().Be(0);
		}

	}
}
