using RepositorioGenerico.Entities;
using RepositorioGenerico.Entities.Anotacoes;
using System.Collections.Generic;

namespace Entities
{

	[Tabela("Clientes")]
	public class Cliente : Entidade
	{

		#region Estrutura da Tabela

		[Chave, Coluna("CodCliente", NomeDoTipo = "int"), AutoIncremento(Incremento.Identity)]
		public int Id { get; set; }

		[Obrigatorio, Coluna(NomeDoTipo = "varchar"), TamanhoMinimo(10), TamanhoMaximo(50)]
		public string Nome { get; set; }

		[Obrigatorio, Coluna(NomeDoTipo = "int")]
		public int Idade { get; set; }

		[Descricao("Endereço")]
		[Coluna(NomeDoTipo = "varchar"), TamanhoMaximo(250)]
		public string Endereco { get; set; }

		[Descricao("Crédito Disp.")]
		[Coluna("CreditoDisponivel", NomeDoTipo = "decimal")]
		public decimal? Credito { get; set; }

		[Coluna(NomeDoTipo = "varchar"), TamanhoMaximo(250)]
		public string Bairro { get; set; }

		[Descricao("Cidade")]
		[Coluna("CodCidade", NomeDoTipo = "int")]
		public int? IdCidade { get; set; }

		[Descricao("Retem Imp.?")]
		[Coluna(NomeDoTipo = "bit")]
		public bool RetemImpostos { get; set; }

		[Descricao("V.I.P.?")]
		[Coluna(NomeDoTipo = "bit")]
		public bool Vip { get; set; }

		#endregion

		#region Relacionamentos Ascendentes

		[ChaveEstrangeira("IdCidade")]
		public virtual Cidade Cidade { get; set; }

		#endregion

		#region Relacionamentos Descendentes

		[PropriedadeDeLigacaoEstrangeira("Cliente")]
		public virtual ICollection<Filho> Filhos { get; set; }

		[PropriedadeDeLigacaoEstrangeira("Cliente")]
		public virtual ICollection<ContatoDoCliente> Contatos { get; set; }

		#endregion

	}
}
