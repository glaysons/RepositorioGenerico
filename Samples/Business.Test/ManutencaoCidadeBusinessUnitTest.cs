using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Entities;
using FluentAssertions;
using RepositorioGenerico.Entities;

namespace Business.Test
{
	[TestClass]
	public class ManutencaoCidadeBusinessUnitTest
	{

		[TestMethod]
		public void SeCadastrarUmaCidadeNaoDeveGerarErro()
		{
			var factory = new CidadeFactory();
			var manutencao = factory.CriarManutencao();

			var cidade = new Cidade()
			{
				Nome = "ABC do Zé",
				Estado = "MT"
			};

			Action cadastro = () => manutencao.Cadastrar(cidade);

			cadastro
				.ShouldNotThrow();
		}

		[TestMethod]
		public void SeCadastrarUmaCidadeDeveSerPossivelConsultala()
		{
			var factory = new CidadeFactory();
			var manutencao = factory.CriarManutencao();

			var cidade = new Cidade()
			{
				Nome = "ABC do Zé",
				Estado = "MT"
			};

			factory.Repositorio.Quantidade
				.Should().Be(0);

			manutencao.Cadastrar(cidade);

			factory.Repositorio.Quantidade
				.Should().Be(1);
		}

		[TestMethod]
		public void SeCadastrarUmaCidadeExistenteDeveGerarErro()
		{
			var factory = new CidadeFactory();
			var consultador = factory.CriarConsultador();
			var manutencao = factory.CriarManutencao();

			var cidade = new Cidade()
			{
				Nome = "Cuiabá",
				Estado = "MT"
			};

			Action cadastro = () => manutencao.Cadastrar(cidade);

			cadastro
				.ShouldThrow<Exception>()
				.WithMessage("Já existe uma cidade cadastrada com este nome!");

		}

		[TestMethod]
		public void SeAlterarUmaCidadeExistenteDeveEntrarNaListaDeAtualizacao()
		{
			var factory = new CidadeFactory();
			var consultador = factory.CriarConsultador();
			var manutencao = factory.CriarManutencao();

			factory.Repositorio.Quantidade
				.Should().Be(0);

			var cidade = consultador.ConsultarCidade(1);

			cidade
				.Should().NotBeNull();

			cidade.Nome = "CuiabáX";

			manutencao.Atualizar(cidade);

			factory.Repositorio.Quantidade
				.Should().Be(1);
		}

		[TestMethod]
		public void SeAlterarUmaCidadeExistenteSemExecutarAConsultaDeveEntrarNaListaDeAtualizacao()
		{
			var factory = new CidadeFactory();
			var manutencao = factory.CriarManutencao();

			factory.Repositorio.Quantidade
				.Should().Be(0);

			var cidade = new Cidade()
			{
				Id = 1,
				Nome = "CuiabáX",
				Estado = "XX",
				EstadoEntidade = EstadosEntidade.Novo
			};

			manutencao.Atualizar(cidade);

			cidade.EstadoEntidade
				.Should().Be(EstadosEntidade.Modificado);

			factory.Repositorio.Quantidade
				.Should().Be(1);
		}

		[TestMethod]
		public void SeExcluirUmaCidadeExistenteSemVinculoDeveEntrarNaListaDeAtualizacao()
		{
			var factory = new CidadeFactory();
			var consultador = factory.CriarConsultador();
			var manutencao = factory.CriarManutencao();

			factory.Repositorio.Quantidade
				.Should().Be(0);

			factory.ExistemClientesVinculados(existe: false);

			var cidade = consultador.ConsultarCidade(1);

			cidade
				.Should().NotBeNull();

			manutencao.Excluir(cidade);

			factory.Repositorio.Quantidade
				.Should().Be(0);
		}

		[TestMethod]
		public void SeExcluirUmaCidadeExistenteComVinculoDeveGerarErro()
		{
			var factory = new CidadeFactory();
			var consultador = factory.CriarConsultador();
			var manutencao = factory.CriarManutencao();

			factory.ExistemClientesVinculados(existe: true);

			var cidade = consultador.ConsultarCidade(1);

			cidade
				.Should().NotBeNull();

			Action exclusao = () => manutencao.Excluir(cidade);

			exclusao
				.ShouldThrow<Exception>()
				.WithMessage("Ainda existem clientes cadastrados para esta cidade!");
		}

	}
}
