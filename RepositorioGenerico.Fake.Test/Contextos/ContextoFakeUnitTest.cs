using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositorioGenerico.Fake.Contextos;
using RepositorioGenerico.Test;

namespace RepositorioGenerico.Fake.Test.Contextos
{
	[TestClass]
	public class ContextoFakeUnitTest
	{

		[TestMethod]
		public void SeConsultarRepositorioDeFormasDiferentesDeveEncontrarMesmoRepositorio()
		{
			var contexto = new ContextoFake();

			var repositorioUm = contexto.Repositorio<ObjetoDeTestes>();

			var repositorioDois = contexto.Repositorio(typeof(ObjetoDeTestes));

			repositorioUm
				.Should()
				.BeSameAs(repositorioDois);
		}

		[TestMethod]
		public void SeAdicionarRegistrosNoContextoORepositorioCorretoDeveserModificado()
		{
			var contexto = new ContextoFake();

			var repositorio = contexto.Repositorio<ObjetoDeTestes>();

			repositorio.Quantidade
				.Should()
				.Be(0);

			contexto.AdicionarRegistro(new ObjetoDeTestes() { Codigo = 1, Nome = "Nome Valido Para Adicao" });

			repositorio.Quantidade
				.Should()
				.Be(1);
		}

		[TestMethod]
		public void SeAdicionarUmaListaDeRegistrosNoContextoORepositorioCorretoDeveserModificado()
		{
			var contexto = new ContextoFake();

			var repositorio = contexto.Repositorio<ObjetoDeTestes>();

			repositorio.Quantidade
				.Should()
				.Be(0);

			var lista = new List<ObjetoDeTestes>
			{
				new ObjetoDeTestes() {Codigo = 1, Nome = "Nome Valido Para Adicao"},
				new ObjetoDeTestes() {Codigo = 2, Nome = "Novo Nome Valido Para Adicao"}
			};

			contexto.AdicionarRegistros(lista);

			repositorio.Quantidade
				.Should()
				.Be(2);
		}

	}
}
