using System;
using System.Collections.Generic;
using RepositorioGenerico.Entities;
using RepositorioGenerico.Entities.Anotacoes;

namespace RepositorioGenerico.Test.Objetos
{

	[ValidadorDeClasse]
	[ValidadorDeClasse2]
	[Tabela("ObjetoVirtual")]
	public class ObjetoMistoDeTestes : Entidade
	{

		[Chave]
		[Obrigatorio]
		[AutoIncremento(Incremento.Identity)]
		[Coluna(Ordem = 0, Nome = "Codigo", NomeDoTipo = "int")]
		public int AliasCodigo { get; set; }

		[Coluna(Ordem = 1, Nome = "CodigoNulo", NomeDoTipo = "int")]
		public int? AliasCodigoNulo { get; set; }

		[Obrigatorio]
		[Coluna(Ordem = 2, Nome = "Nome", NomeDoTipo = "varchar")]
		[TamanhoMaximo(50)]
		[ValidadorDePropriedade]
		[ValidadorDePropriedade2]
		public string AliasNome { get; set; }

		[Obrigatorio]
		[Coluna(Ordem = 3, Nome = "Duplo", NomeDoTipo = "float")]
		public double AliasDuplo { get; set; }

		[Coluna(Ordem = 4, Nome = "DuploNulo", NomeDoTipo = "float")]
		public double? AliasDuploNulo { get; set; }

		[Obrigatorio]
		[Coluna(Ordem = 5, Nome = "Decimal", NomeDoTipo = "decimal")]
		public decimal AliasDecimal { get; set; }

		[Coluna(Ordem = 6, Nome = "DecimalNulo", NomeDoTipo = "decimal")]
		public decimal? AliasDecimalNulo { get; set; }

		[Obrigatorio]
		[ValorPadrao(false)]
		[Coluna(Ordem = 7, Nome = "Logico", NomeDoTipo = "bit")]
		public bool AliasLogico { get; set; }

		[Obrigatorio]
		[Coluna(Ordem = 8, Nome = "DataHora", NomeDoTipo = "datetime")]
		public DateTime AliasDataHora { get; set; }

		[Coluna(Ordem = 9, Nome = "DataHoraNulo", NomeDoTipo = "datetime")]
		public DateTime? AliasDataHoraNulo { get; set; }

		[PropriedadeDeLigacaoEstrangeira("AliasPai")]
		public virtual ICollection<FilhoMistoDoObjetoDeTestes> AliasFilhos { get; set; }

	}

}