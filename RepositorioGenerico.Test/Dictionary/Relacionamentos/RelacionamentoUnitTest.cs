using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositorioGenerico.Dictionary;
using RepositorioGenerico.Dictionary.Helpers;
using RepositorioGenerico.Dictionary.Relacionamentos;
using RepositorioGenerico.Test.Objetos;

namespace RepositorioGenerico.Test.Dictionary.Relacionamentos
{
	[TestClass]
	public class RelacionamentoUnitTest
	{

		[TestMethod]
		public void SeDefinirUmaChaveAscendenteDeUmCampoRelacionadoDevePreencherObjetoCorretamente()
		{
			var filho = new FilhoDoObjetoDeTestes();
			var dicionario = new Dicionario(typeof(FilhoDoObjetoDeTestes));
			var chaveEstrangeira = DataAnnotationHelper.ConsultarForeignKey(dicionario.ConsultarPorCampo("Pai").Propriedade);
			var relacionamento = new Relacionamento(TiposRelacionamento.Ascendente, dicionario, chaveEstrangeira);

			relacionamento.AplicarChaveAscendente(new object[] { 123 }, filho);

			filho.CodigoPai
				.Should().Be(123);
		}

		[TestMethod]
		public void SeQuestionarPelaPropriedadeRelacionadaComDeterminadoValorPreenchidoDeveRetornarVerdadeiro()
		{
			var filho = new FilhoDoObjetoDeTestes()
			{
				CodigoPai = 123
			};

			var dicionario = new Dicionario(typeof(FilhoDoObjetoDeTestes));
			var chaveEstrangeira = DataAnnotationHelper.ConsultarForeignKey(dicionario.ConsultarPorCampo("Pai").Propriedade);
			var relacionamento = new Relacionamento(TiposRelacionamento.Ascendente, dicionario, chaveEstrangeira);

			relacionamento.PossuiChaveAscendente(new object[] { 123 }, filho)
				.Should().BeTrue();

		}

		[TestMethod]
		public void SeConsultarPorUmaPropriedadeInexistenteDeveRetornarNulo()
		{
			var dicionario = new Dicionario(typeof(ObjetoDeTestes));
			dicionario.ConsultarPorPropriedade("NomeDaPropriedadeInexistenteNoObjeto")
				.Should().BeNull();
		}

		[TestMethod]
		public void SeQuestionarPelaPropriedadeRelacinoadaDeUmObjetoQueNaoPossuiUmaDeterminadaChaveDeveRetornarFalso()
		{
			var filho = new FilhoDoObjetoDeTestes();
			var dicionario = new Dicionario(typeof(FilhoDoObjetoDeTestes));
			var chaveEstrangeira = DataAnnotationHelper.ConsultarForeignKey(dicionario.ConsultarPorCampo("Pai").Propriedade);
			var relacionamento = new Relacionamento(TiposRelacionamento.Ascendente, dicionario, chaveEstrangeira);

			relacionamento.PossuiChaveAscendente(new object[] { 123 }, filho)
				.Should().BeFalse();

		}
	}
}
