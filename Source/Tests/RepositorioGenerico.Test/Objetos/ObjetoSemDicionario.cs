using System;

namespace RepositorioGenerico.Test.Objetos
{

	public class ObjetoSemDicionario 
	{
		public int Codigo { get; set; }
		public int? CodigoNulo { get; set; }
		public string Nome { get; set; }
		public double Duplo { get; set; }
		public double? DuploNulo { get; set; }
		public decimal Decimal { get; set; }
		public decimal? DecimalNulo { get; set; }
		public bool Logico { get; set; }
		public DateTime DataHora { get; set; }
		public DateTime? DataHoraNulo { get; set; }
	}

}