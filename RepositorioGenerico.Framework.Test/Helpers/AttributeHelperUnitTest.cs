using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositorioGenerico.Entities;
using RepositorioGenerico.Entities.Anotacoes;
using RepositorioGenerico.Entities.Anotacoes.Validadores;
using RepositorioGenerico.Framework.Helpers;
using RepositorioGenerico.Test;

namespace RepositorioGenerico.Framework.Test.Helpers
{
	[TestClass]
	public class AttributeHelperUnitTest
	{

		[TestMethod]
		public void SeConsultarOAtributoTableAttributeDaClasseObjetoDeTestesDeveRetornarUmValorValido()
		{
			var atributo = AttributeHelper.Consultar<TabelaAttribute>(typeof(ObjetoDeTestes));
			atributo.Should().NotBeNull();
			atributo.Nome.Should().Be("ObjetoVirtual");
		}

		[TestMethod]
		public void SeConsultarOAtributoTableAttributeDaClasseObjetoBancoDeveRetornarUmValorNulo()
		{
			var atributo = AttributeHelper.Consultar<TabelaAttribute>(typeof(Entidade));
			atributo.Should().BeNull();
		}

		[TestMethod]
		public void SeConsultarOAtributoColumnAttributeDaPropriedadeCodigoDeveRetornarUmValorValido()
		{
			var codigo = typeof(ObjetoDeTestes).GetProperty("Codigo");
			var atributo = AttributeHelper.Consultar<ColunaAttribute>(codigo);
			atributo.Should().NotBeNull();
			atributo.Ordem.Should().Be(0);
			atributo.NomeDoTipo.Should().Be("int");
		}

		[TestMethod]
		public void SeConsultarOAtributoColumnAttributeDaPropriedadeEstadoObjetoDeveRetornarUmValorNulo()
		{
			var codigo = typeof(Entidade).GetProperty("EstadoEntidade");
			var atributo = AttributeHelper.Consultar<ColunaAttribute>(codigo);
			atributo.Should().BeNull();
		}

		[TestMethod]
		public void SeConsultarOAtributoDescriptionAttributeDeUmFieldDeveRetornarUmValorValido()
		{
			var tipo = typeof (EnumDeTestes);
			var field = tipo.GetField(EnumDeTestes.Opcao3.ToString());
			var atributo = AttributeHelper.Consultar<DescricaoAttribute>(field);
			atributo.Should().NotBeNull();
			atributo.Descricao.Should().Be("Terceira Opção");
		}

		[TestMethod]
		public void SeConsultarODefaultValueDeUmFieldDeveRetornarUmValorValido()
		{
			var tipo = typeof(EnumDeStrings);
			var field = tipo.GetField(EnumDeStrings.OpcaoC.ToString());
			var atributo = AttributeHelper.Consultar<ValorPadraoAttribute>(field);
			atributo.Should().NotBeNull();
			atributo.Valor.Should().Be("C");
		}

		[TestMethod]
		public void SeConsultarOAtributoDescriptionAttributeDeUmFieldQueNaoPossuiDescricaoDeveRetornarUmValorNulo()
		{
			var tipo = typeof(EnumDeTestes);
			var field = tipo.GetField(EnumDeTestes.SemOpcao.ToString());
			var atributo = AttributeHelper.Consultar<DescricaoAttribute>(field);
			atributo.Should().BeNull();
		}

		[TestMethod]
		public void SeConsultarTodosOsAtributosDeUmaClasseDeveRetornarUmaListaValida()
		{
			var lista = AttributeHelper.ConsultarTodos<ValidadorEntidadeAttribute>(typeof (ObjetoDeTestes))
				.ToList();
			lista.Should().NotBeNull();
			lista.Count().Should().Be(2);
			lista.SingleOrDefault(a => a.GetType() == typeof(ValidadorDeClasseAttribute)).Should().NotBeNull();
			lista.SingleOrDefault(a => a.GetType() == typeof(ValidadorDeClasse2Attribute)).Should().NotBeNull();
		}

		[TestMethod]
		public void SeConsultarTodosOsAtributosDeUmaClasseQueNaoPossuiEstesAtributosDeveRetornarUmaListaVazia()
		{
			var lista = AttributeHelper.ConsultarTodos<Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute>(typeof(ObjetoDeTestes))
				.ToList();
			lista.Should().NotBeNull();
			lista.Count().Should().Be(0);
		}

		[TestMethod]
		public void SeConsultarTodosOsAtributosDeUmaPropriedadeDeveRetornarUmaListaValida()
		{
			var lista = AttributeHelper.ConsultarTodos<ValidadorPropriedadeAttribute>(typeof(ObjetoDeTestes).GetProperty("Nome"))
				.ToList();
			lista.Should().NotBeNull();
			lista.Count().Should().Be(2);
			lista.SingleOrDefault(a => a.GetType() == typeof(ValidadorDePropriedadeAttribute)).Should().NotBeNull();
			lista.SingleOrDefault(a => a.GetType() == typeof(ValidadorDePropriedade2Attribute)).Should().NotBeNull();
		}

		[TestMethod]
		public void SeConsultarTodosOsAtributosDeUmaPropriedadeQueNaoPossuiEstesAtributosDeveRetornarUmaListaVazia()
		{
			var lista = AttributeHelper.ConsultarTodos<Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute>(typeof(ObjetoDeTestes).GetProperty("Nome"))
				.ToList();
			lista.Should().NotBeNull();
			lista.Count().Should().Be(0);
		}

	}
}
