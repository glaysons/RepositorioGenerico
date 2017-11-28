using ConverterBancoParaEntidades.Estruturas;
using System.Collections.Generic;
using System.IO;

namespace ConverterBancoParaEntidades.Interfaces
{
	public interface IGeradorPropriedade
	{

		void Gerar(string tabela, IList<Campo> campos, StreamWriter arquivo);

	}
}
