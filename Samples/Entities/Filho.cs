using RepositorioGenerico.Entities;
using RepositorioGenerico.Entities.Anotacoes;
using System;
using System.Collections.Generic;

namespace Entities
{
	[Tabela("Filhos")]
	public class Filho : Entidade
	{

		#region Estrutura da Tabela

		[Chave, Coluna("CodFilho", NomeDoTipo = "int"), AutoIncremento(Incremento.Identity)]
		public int Id { get; set; }

		[Obrigatorio, Coluna("CodCliente", NomeDoTipo = "int")]
		public int IdCliente { get; set; }

		[Obrigatorio, Coluna(NomeDoTipo = "varchar"), TamanhoMaximo(50)]
		public string Nome { get; set; }

		[Coluna(NomeDoTipo = "bit")]
		public bool MoraComOsPais { get; set; }

		[Obrigatorio, Coluna(NomeDoTipo = "int")]
		public int Idade { get; set; }

		[Coluna(NomeDoTipo = "datetime")]
		public DateTime? DataDeNascimento { get; set; }

		#endregion

		#region Relacionamentos Ascendentes

		[ChaveEstrangeira("IdCliente")]
		public virtual Cliente Cliente { get; set; }

		#endregion

		#region Relacionamentos Descendentes

		[PropriedadeDeLigacaoEstrangeira("Filho")]
		public virtual ICollection<ContatoDoFilho> Contatos { get; set; }

		#endregion

	}
}
