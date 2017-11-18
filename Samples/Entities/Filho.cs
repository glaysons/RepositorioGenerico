using RepositorioGenerico.Entities;
using RepositorioGenerico.Entities.Anotacoes;
using System;
using System.Collections.Generic;

namespace Entities
{
	[Tabela("Filhos")]
	public class Filho : Entidade
	{

		/// <summary>
		/// Estrutura da Tabela
		/// </summary>

		[Chave, Coluna(Nome = "CodFilho", NomeDoTipo = "int"), AutoIncremento(Incremento.Identity)]
		public int Id { get; set; }

		[Obrigatorio, Coluna(Nome = "CodCliente", NomeDoTipo = "int")]
		public int IdCliente { get; set; }

		[Obrigatorio, Coluna(NomeDoTipo = "varchar")]
		public string Nome { get; set; }

		[Coluna(NomeDoTipo = "bit")]
		public bool MoraComOsPais { get; set; }

		[Obrigatorio, Coluna(NomeDoTipo = "int")]
		public bool Idade { get; set; }

		[Coluna(NomeDoTipo = "datetime")]
		public DateTime? DataDeNascimento { get; set; }

		/// <summary>
		/// Relacionamento Ascendente
		/// </summary>

		[ChaveEstrangeira("IdCliente")]
		public virtual Cliente Cliente { get; set; }

		/// <summary>
		/// Relacionamento Descendente
		/// </summary>

		[PropriedadeDeLigacaoEstrangeira("Filho")]
		public virtual ICollection<ContatoDoFilho> Contatos { get; set; }



	}
}
