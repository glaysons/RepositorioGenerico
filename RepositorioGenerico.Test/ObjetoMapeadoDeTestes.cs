using System;
using System.Collections.Generic;
using RepositorioGenerico.Entities;

namespace RepositorioGenerico.Test
{
	public class ObjetoMapeadoDeTestes: Entidade
	{

		public int MapeadoComCodigo { get; set; }

		public int? MapeadoComCodigoNulo { get; set; }

		public string MapeadoComNome { get; set; }

		public double MapeadoComDuplo { get; set; }

		public double? MapeadoComDuploNulo { get; set; }

		public decimal MapeadoComDecimal { get; set; }

		public decimal? MapeadoComDecimalNulo { get; set; }

		public bool MapeadoComLogico { get; set; }

		public DateTime MapeadoComDataHora { get; set; }

		public DateTime? MapeadoComDataHoraNulo { get; set; }

		public virtual ICollection<FilhoMapeadoDoObjetoMapeadoDeTestes> MapeadoComFilhos { get; set; }

	}
}
