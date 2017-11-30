using System;
using System.Collections.Generic;
using RepositorioGenerico.Entities;
using RepositorioGenerico.Entities.Anotacoes;

namespace RepositorioGenerico.Test.Objetos
{

	[ValidadorDeClasse]
	[ValidadorDeClasse2]
	[Tabela("ObjetoVirtual")]
	public class ObjetoDeTestes : Entidade
	{

		[Chave, Obrigatorio, AutoIncremento(Incremento.Identity)]
		[Coluna(Ordem = 0, NomeDoTipo = "int")]
		public int Codigo { get; set; }

		[Coluna(Ordem = 1, NomeDoTipo = "int")]
		public int? CodigoNulo { get; set; }

		[Obrigatorio, TamanhoMinimo(5), TamanhoMaximo(50)]
		[Coluna(Ordem = 2, NomeDoTipo = "varchar")]
		[ValidadorDePropriedade]
		[ValidadorDePropriedade2]
		public string Nome { get; set; }

		[Obrigatorio, ValorPadrao(123.45)]
		[Coluna(Ordem = 3, NomeDoTipo = "float")]
		public double Duplo { get; set; }

		[Coluna(Ordem = 4, NomeDoTipo = "float")]
		public double? DuploNulo { get; set; }

		[Obrigatorio]
		[Coluna(Ordem = 5, NomeDoTipo = "decimal")]
		public decimal Decimal { get; set; }

		[Coluna(Ordem = 6, NomeDoTipo = "decimal")]
		public decimal? DecimalNulo { get; set; }

		[Obrigatorio, ValorPadrao(true)]
		[Coluna(Ordem = 7, NomeDoTipo = "bit")]
		public bool Logico { get; set; }

		[Obrigatorio]
		[Coluna(Ordem = 8, NomeDoTipo = "datetime")]
		public DateTime DataHora { get; set; }

		[Coluna(Ordem = 9, NomeDoTipo = "datetime")]
		public DateTime? DataHoraNulo { get; set; }

		[PropriedadeDeLigacaoEstrangeira("Pai")]
		public virtual ICollection<FilhoDoObjetoDeTestes> Filhos { get; set; }

	}

}