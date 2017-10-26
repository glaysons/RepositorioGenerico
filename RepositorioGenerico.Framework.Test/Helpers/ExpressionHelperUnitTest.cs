using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositorioGenerico.Exceptions;
using RepositorioGenerico.Framework.Helpers;
using RepositorioGenerico.Test;

namespace RepositorioGenerico.Framework.Test.Helpers
{
	[TestClass]
	public class ExpressionHelperUnitTest
	{

		private static ObjetoDeTestes ConsultarObjetoDeTestes()
		{
			var objeto = new ObjetoDeTestes
			{
				Filhos = new List<FilhoDoObjetoDeTestes>
				{
					new FilhoDoObjetoDeTestes() {CodigoFilho = 1, NomeFilho = "A"},
					new FilhoDoObjetoDeTestes() {CodigoFilho = 2, NomeFilho = "B"},
					new FilhoDoObjetoDeTestes() {CodigoFilho = 3, NomeFilho = "C"}
				}
			};
			return objeto;
		}

		[TestMethod]
		public void SeConsultarAPropriedadeDaExpressaoDeveTrazerOPropertyInfoCorreto()
		{
			var propriedade = typeof (ObjetoDeTestes).GetProperty("Nome");

			ExpressionHelper.PropriedadeDaExpressao<ObjetoDeTestes>(o => o.Nome)
				.Should().BeSameAs(propriedade);

			Expression<Func<ObjetoDeTestes, object>> func = o => o.Nome;

			ExpressionHelper.PropriedadeDaExpressao((LambdaExpression) func)
				.Should().BeSameAs(propriedade);
		}

		[TestMethod]
		public void SeCriarExpressaoDeConsultaDaPropriedadeDeveGerarUmaExpressaoValida()
		{
			var propriedade = typeof (ObjetoDeTestes).GetProperty("Filhos");

			var expressao = ExpressionHelper.CriarExpressaoDeConsultaDaPropriedade<ObjetoDeTestes>(propriedade);

			var outraPropriedade = ExpressionHelper.PropriedadeDaExpressao(expressao);

			outraPropriedade
				.Should()
				.BeSameAs(propriedade);
		}

		[TestMethod]
		public void SeConsultarAPropriedadeDaExpressaoInvalidaDeveGerarErroPropriedadeInvalidaException()
		{
			Action act = () => ExpressionHelper.PropriedadeDaExpressao<ObjetoDeTestes>(c => c.Nome + c.Nome);
			act.ShouldThrow<PropriedadeInvalidaException>();
		}

		[TestMethod]
		public void SeCriarExpressaoDeConsultaDaPropriedadeDoTipoInteiroDeveGerarUmaExpressaoValida()
		{
			var propriedade = typeof(ObjetoDeTestes).GetProperty("Codigo");

			var expressao = ExpressionHelper.CriarExpressaoDeConsultaDaPropriedade<ObjetoDeTestes>(propriedade);

			var outraPropriedade = ExpressionHelper.PropriedadeDaExpressao(expressao);

			outraPropriedade
				.Should()
				.BeSameAs(propriedade);
		}

	}
}
