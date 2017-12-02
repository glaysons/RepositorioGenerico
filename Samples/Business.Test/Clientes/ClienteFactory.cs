using Entities;
using RepositorioGenerico.Pattern.Contextos;
using RepositorioGenerico.Fake.Contextos;
using Business.Clientes;
using System.Collections.Generic;

namespace Business.Test.Clientes
{
	public class ClienteFactory
	{

		private IContextoFake _contexto;

		public IContextoFake Contexto
		{
			get { return _contexto ?? (_contexto = CriarContexto()); }
		}

		public IRepositorio<Cliente> Repositorio
		{
			get { return Contexto.Repositorio<Cliente>(); }
		}

		private IContextoFake CriarContexto()
		{
			var contexto = RepositorioGenerico.Fake.FabricaFake.CriarContexto();
			PreencherClientesDeExemplo(contexto);
			return contexto;
		}

		private void PreencherClientesDeExemplo(IContextoFake contexto)
		{
			contexto.AdicionarRegistro(new TipoContato() { Id = 1, Nome = "Pessoal" });
			contexto.AdicionarRegistro(new TipoContato() { Id = 2, Nome = "Trabalho" });
			contexto.AdicionarRegistro(new TipoContato() { Id = 3, Nome = "Parente" });

			contexto.AdicionarRegistro(new Cidade() { Id = 1, Nome = "São Paulo", Estado = "SP" });
			contexto.AdicionarRegistro(new Cidade() { Id = 2, Nome = "Rio Raro", Estado = "RS" });

			contexto.AdicionarRegistro(new Cliente()
			{
				Id = 1,
				Nome = "João Abc da Silva",
				Idade = 37,
				Endereco = "Rua Boa Vista",
				Credito = 10055.6M,
				Bairro = "Alto do Boa Vista",
				IdCidade = 1,
				RetemImpostos = false,
				Vip = true,

				Filhos = new List<Filho>()
				{
					new Filho()
					{
						Id = 1,
						IdCliente = 1,
						Nome = "Joãozinho Abc da Silva",
						MoraComOsPais = true,
						Idade = 12,
						DataDeNascimento = new System.DateTime(2005, 11, 5),
						Contatos = new List<ContatoDoFilho>()
						{
							new ContatoDoFilho()
							{
								Id = 1,
								IdFilho = 1,
								IdTipoContato = 1,
								Nome = "Amigo do Joãozinho",
								Telefone = "1234-4567"
							},

							new ContatoDoFilho()
							{
								Id = 2,
								IdFilho = 1,
								IdTipoContato = 2,
								Nome = "Abc Ltda.",
								Telefone = "3456-7789"
							}
						}
					},

					new Filho()
					{
						Id = 2,
						IdCliente = 1,
						Nome = "Joãninha Abc da Silva",
						MoraComOsPais = true,
						Idade = 10,
						DataDeNascimento = new System.DateTime(2007, 6, 17),
						Contatos = new List<ContatoDoFilho>() { }
					}
				},

				Contatos = new List<ContatoDoCliente>()
				{
					new ContatoDoCliente()
					{
						Id = 1,
						IdCliente = 1,
						IdTipoContato = 2,
						Nome = "Abc Ltda.",
						Telefone = "3456-7789"
					}
				}
			});

			contexto.AdicionarRegistro(new Cliente()
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
			});

			contexto.AdicionarRegistro(new Cliente()
			{
				Id = 3,
				Nome = "Ricardo Bcd dos Santos",
				Idade = 25,
				Endereco = "Rua do Rincão",
				Credito = 500556.44M,
				Bairro = "Bom Senhor",
				IdCidade = 3,
				RetemImpostos = false,
				Vip = false
			});
		}

		public ManutencaoClienteBusiness CriarManutencao()
		{
			return new ManutencaoClienteBusiness(Repositorio, CriarConsultador());
		}

		public ConsultaClienteBusiness CriarConsultador()
		{
			return new ConsultaClienteBusiness(Repositorio);
		}

	}
}
