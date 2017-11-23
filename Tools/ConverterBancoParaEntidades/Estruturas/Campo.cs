using ConverterBancoParaEntidades.Constantes;

namespace ConverterBancoParaEntidades.Estruturas
{
	public class Campo
	{

		public int Ordem { get; set; }

		public string NomeCampo { get; set; }

		public TipoCampo TipoInterno { get; set; }

		public int TipoBanco { get; set; }

		public string NomeTipo { get; set; }

		public bool Obrigatorio { get; set; }

		public bool Identity { get; set; }

		public bool Chave { get; set; }

		public int TamanhoMaximo { get; set; }

		public bool Calculado { get; set; }

	}
}
