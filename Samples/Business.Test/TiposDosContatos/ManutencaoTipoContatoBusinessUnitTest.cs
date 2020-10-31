using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Entities;
using FluentAssertions;
using RepositorioGenerico.Entities;

namespace Business.Test.TiposDosContatos
{
	[TestClass]
	public class ManutencaoTipoContatoBusinessUnitTest
	{

		[TestMethod]
		public void SeCadastrarUmTipoContatoNaoDeveGerarErro()
		{
			var factory = new TipoContatoFactory();
			var manutencao = factory.CriarManutencao();

			var tipoContato = new TipoContato()
			{
				Nome = "Fumaça"
			};

			Action cadastro = () => manutencao.Cadastrar(tipoContato);

			cadastro
				.Should().NotThrow();
		}

		[TestMethod]
		public void SeCadastrarUmTipoContatoDeveSerPossivelConsultalo()
		{
			var factory = new TipoContatoFactory();
			var manutencao = factory.CriarManutencao();

			var tipoContato = new TipoContato()
			{
				Nome = "Fumaça"
			};

			factory.Repositorio.Quantidade
				.Should().Be(0);

			manutencao.Cadastrar(tipoContato);

			factory.Repositorio.Quantidade
				.Should().Be(1);
		}

		[TestMethod]
		public void SeCadastrarUmTipoContatoExistenteDeveGerarErro()
		{
			var factory = new TipoContatoFactory();
			var consultador = factory.CriarConsultador();
			var manutencao = factory.CriarManutencao();

			var tipoContato = new TipoContato()
			{
				Nome = "Telefone"
			};

			Action cadastro = () => manutencao.Cadastrar(tipoContato);

			cadastro
				.Should().Throw<Exception>()
				.WithMessage("Já existe um Tipo de Contato cadastrado com este nome!");

		}

		[TestMethod]
		public void SeAlterarUmTipoContatoExistenteDeveEntrarNaListaDeAtualizacao()
		{
			var factory = new TipoContatoFactory();
			var consultador = factory.CriarConsultador();
			var manutencao = factory.CriarManutencao();

			factory.Repositorio.Quantidade
				.Should().Be(0);

			var tipoContato = consultador.ConsultarTipoContato(1);

			tipoContato
				.Should().NotBeNull();

			tipoContato.Nome = "Fumaça";

			manutencao.Atualizar(tipoContato);

			factory.Repositorio.Quantidade
				.Should().Be(1);
		}

		[TestMethod]
		public void SeAlterarUmTipoContatoExistenteSemExecutarAConsultaDeveEntrarNaListaDeAtualizacao()
		{
			var factory = new TipoContatoFactory();
			var manutencao = factory.CriarManutencao();

			factory.Repositorio.Quantidade
				.Should().Be(0);

			var tipoContato = new TipoContato()
			{
				Id = 1,
				Nome = "Fumaça",
				EstadoEntidade = EstadosEntidade.Novo
			};

			manutencao.Atualizar(tipoContato);

			tipoContato.EstadoEntidade
				.Should().Be(EstadosEntidade.Modificado);

			factory.Repositorio.Quantidade
				.Should().Be(1);
		}

		[TestMethod]
		public void SeExcluirUmTipoContatoExistenteSemVinculoDeveEntrarNaListaDeAtualizacao()
		{
			var factory = new TipoContatoFactory();
			var consultador = factory.CriarConsultador();
			var manutencao = factory.CriarManutencao();

			factory.Repositorio.Quantidade
				.Should().Be(0);

			factory.ExistemContatosVinculados(existe: false);

			var tipoContato = consultador.ConsultarTipoContato(1);

			tipoContato
				.Should().NotBeNull();

			manutencao.Excluir(tipoContato);

			factory.Repositorio.Quantidade
				.Should().Be(0);
		}

		[TestMethod]
		public void SeExcluirUmTipoContatoExistenteComVinculoDeveGerarErro()
		{
			var factory = new TipoContatoFactory();
			var consultador = factory.CriarConsultador();
			var manutencao = factory.CriarManutencao();

			factory.ExistemContatosVinculados(existe: true);

			var tipoContato = consultador.ConsultarTipoContato(1);

			tipoContato
				.Should().NotBeNull();

			Action exclusao = () => manutencao.Excluir(tipoContato);

			exclusao
				.Should().Throw<Exception>()
				.WithMessage("Ainda existem contatos vinculados com este Tipo de Contato!");
		}

	}
}
