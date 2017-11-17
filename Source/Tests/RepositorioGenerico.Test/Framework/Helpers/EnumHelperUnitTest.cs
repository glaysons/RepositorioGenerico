using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositorioGenerico.Framework.Helpers;
using RepositorioGenerico.Test.Objetos;

namespace RepositorioGenerico.Test.Framework.Helpers
{
	[TestClass]
	public class EnumHelperUnitTest
	{

		[TestMethod]
		public void SeConverterUmaStringDeveConverterNumaEnumValida()
		{
			EnumHelper.FromString<EnumDeTestes>("Opcao1")
				.Should().Be(EnumDeTestes.Opcao1);
		}

		[TestMethod]
		public void SeConverterUmaStringInvalidaDeveGerarErro()
		{
			Action act = () => EnumHelper.FromString<EnumDeTestes>("OpcaoInvalida");
			act
				.ShouldThrow<ArgumentException>();
		}

	}
}
