using RepositorioGenerico.Entities;
using RepositorioGenerico.Entities.Anotacoes;
using System.Collections.Generic;

namespace Entities
{

	[Tabela("Clientes")]
	public class Cliente : Entidade
	{

		/// <summary>
		/// Estrutura da Tabela
		/// </summary>

		[Chave, Coluna(Nome = "CodCliente", NomeDoTipo = "int"), AutoIncremento(Incremento.Identity)]
		public int Id { get; set; }

		[Obrigatorio, Coluna(NomeDoTipo = "varchar"), TamanhoMinimo(10), TamanhoMaximo(50)]
		public string Nome { get; set; }

		[Obrigatorio, Coluna(NomeDoTipo = "int")]
		public int Idade { get; set; }

		[Coluna(NomeDoTipo = "varchar"), TamanhoMaximo(250)]
		public string Endereco { get; set; }

		[Coluna(Nome = "CreditoDisponivel", NomeDoTipo = "decimal")]
		public decimal? Credito { get; set; }

		[Coluna(NomeDoTipo = "varchar"), TamanhoMaximo(250)]
		public string Bairro { get; set; }

		[Coluna(Nome = "CodCidade", NomeDoTipo = "int")]
		public int IdCidade { get; set; }

		[Coluna(NomeDoTipo = "bit")]
		public bool RetemImpostos { get; set; }

		[Coluna(NomeDoTipo = "bit")]
		public bool Vip { get; set; }

		/// <summary>
		/// Relacionamento Ascendente
		/// </summary>

		[ChaveEstrangeira("IdCidade")]
		public virtual Cidade Cidade { get; set; }

		/// <summary>
		/// Relacionamento Descendente
		/// </summary>

		[PropriedadeDeLigacaoEstrangeira("Cliente")]
		public virtual ICollection<Filho> Filhos { get; set; }

		[PropriedadeDeLigacaoEstrangeira("Cliente")]
		public virtual ICollection<ContatoDoCliente> Contatos { get; set; }

	}
}
