namespace ConverterBancoParaEntidades.Estruturas
{
	public class Relacionamento
	{

		public int Ordem { get; set; }

		public string Nome { get; set; }

		public string TabelaChavePrimaria { get; set; }

		public string ColunaChavePrimaria { get; set; }

		public string TabelaChaveEstrangeira { get; internal set; }

		public string ColunaChaveEstrangeira { get; internal set; }

	}
}
