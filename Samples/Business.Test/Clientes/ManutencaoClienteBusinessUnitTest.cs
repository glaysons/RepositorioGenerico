using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Entities;
using FluentAssertions;
using RepositorioGenerico.Entities;
using System.Collections.Generic;

namespace Business.Test.Clientes
{
	[TestClass]
	public class ManutencaoClienteBusinessUnitTest
	{

		[TestMethod]
		public void SeCadastrarUmClienteNaoDeveGerarErro()
		{
			var factory = new ClienteFactory();
			var manutencao = factory.CriarManutencao();

			var Cliente = new Cliente()
			{
				Nome = "Francisco dos Santos"
			};

			Action cadastro = () => manutencao.Cadastrar(Cliente);

			cadastro
				.Should().NotThrow();
		}

		[TestMethod]
		public void SeCadastrarUmClienteDeveSerPossivelConsultalo()
		{
			var factory = new ClienteFactory();
			var manutencao = factory.CriarManutencao();

			var Cliente = new Cliente()
			{
				Nome = "Francisco dos Santos"
			};

			factory.Repositorio.Quantidade
				.Should().Be(0);

			manutencao.Cadastrar(Cliente);

			factory.Repositorio.Quantidade
				.Should().Be(1);
		}

		[TestMethod]
		public void SeCadastrarUmClienteExistenteDeveGerarErro()
		{
			var factory = new ClienteFactory();
			var consultador = factory.CriarConsultador();
			var manutencao = factory.CriarManutencao();

			var Cliente = new Cliente()
			{
				Nome = "João Abc da Silva"
			};

			Action cadastro = () => manutencao.Cadastrar(Cliente);

			cadastro
				.Should().Throw<Exception>()
				.WithMessage("Já existe um Cliente cadastrado com este nome!");

		}

		[TestMethod]
		public void SeAlterarUmClienteExistenteDeveEntrarNaListaDeAtualizacao()
		{
			var factory = new ClienteFactory();
			var consultador = factory.CriarConsultador();
			var manutencao = factory.CriarManutencao();

			factory.Repositorio.Quantidade
				.Should().Be(0);

			var Cliente = consultador.ConsultarCliente(1);

			Cliente
				.Should().NotBeNull();

			Cliente.Nome = "Francisco dos Santos";

			manutencao.Atualizar(Cliente);

			factory.Repositorio.Quantidade
				.Should().Be(1);
		}

		[TestMethod]
		public void SeAlterarUmClienteExistenteSemExecutarAConsultaDeveEntrarNaListaDeAtualizacao()
		{
			var factory = new ClienteFactory();
			var manutencao = factory.CriarManutencao();

			factory.Repositorio.Quantidade
				.Should().Be(0);

			var Cliente = new Cliente()
			{
				Id = 1,
				Nome = "Francisco dos Santos",
				EstadoEntidade = EstadosEntidade.Novo
			};

			manutencao.Atualizar(Cliente);

			Cliente.EstadoEntidade
				.Should().Be(EstadosEntidade.Modificado);

			factory.Repositorio.Quantidade
				.Should().Be(1);
		}

		[TestMethod]
		public void SeExcluirUmClienteExistenteDeveEntrarNaListaDeAtualizacao()
		{
			var factory = new ClienteFactory();
			var consultador = factory.CriarConsultador();
			var manutencao = factory.CriarManutencao();

			factory.Repositorio.Quantidade
				.Should().Be(0);

			var Cliente = consultador.ConsultarCliente(1);

			Cliente
				.Should().NotBeNull();

			manutencao.Excluir(Cliente);

			factory.Repositorio.Quantidade
				.Should().Be(0);
		}

		[TestMethod]
		public void SeIncluirUmClientePreenchidoDeveInserirTodosOsRegistros()
		{
			var factory = new ClienteFactory();
			var manutencao = factory.CriarManutencao();

			var cliente = new Cliente()
			{
				Id = 2,
				Nome = "Zé Abc de Oliveira",
				Idade = 55,
				Endereco = "Rua Vista Velha",
				Bairro = "Prainha das Vistas",
				IdCidade = 2,
				RetemImpostos = true,
				Vip = true,

				Filhos = new List<Filho>()
				{
					new Filho()
					{
						Id = 3,
						IdCliente = 2,
						Nome = "Zézinho Abc de Oliveira",
						MoraComOsPais = false,
						Idade = 22,
						DataDeNascimento = new System.DateTime(1995, 3, 5),
						Contatos = new List<ContatoDoFilho>()
						{
							new ContatoDoFilho()
							{
								Id = 4,
								IdFilho = 3,
								Nome = "Amigo do Zézinho"
							}
						}
					}
				},

				Contatos = new List<ContatoDoCliente>()
				{
					new ContatoDoCliente()
					{
						Id = 2,
						IdCliente = 2,
						IdTipoContato = 2,
						Nome = "Bcd Ltda.",
						Telefone = "6543-9877"
					},
					new ContatoDoCliente()
					{
						Id = 3,
						IdCliente = 2,
						IdTipoContato = 1,
						Nome = "Parente Próximo",
						Telefone = "9876-5432"
					}
				}
			};

			manutencao.Cadastrar(cliente);

			factory.Contexto.Repositorio<Cliente>().Quantidade
				.Should().Be(1, "deve existir apenas um cliente no contexto!");

			factory.Contexto.Repositorio<Filho>().Quantidade
				.Should().Be(1, "deve existir apenas um filho no contexto!");

			factory.Contexto.Repositorio<ContatoDoFilho>().Quantidade
				.Should().Be(1, "deve existir apenas um contato do filho no contexto!");

			factory.Contexto.Repositorio<ContatoDoCliente>().Quantidade
				.Should().Be(2, "deve existir apenas dois contato do cliente no contexto!");
		}

	}
}
