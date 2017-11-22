using System.IO;

namespace ConverterBancoParaEntidades.Interfaces
{
	public interface IGeradorPropriedade
	{

		void Gerar(string tabela, StreamWriter arquivo);

	}
}
