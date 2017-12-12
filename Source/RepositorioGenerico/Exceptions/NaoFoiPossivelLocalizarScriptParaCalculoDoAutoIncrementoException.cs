using System;

namespace RepositorioGenerico.Exceptions
{
	public class NaoFoiPossivelLocalizarScriptParaCalculoDoAutoIncrementoException : Exception
	{

		public NaoFoiPossivelLocalizarScriptParaCalculoDoAutoIncrementoException(string tabela)
			: base(string.Concat("Não foi possível localizar o script para calculo do autoincremento da tabela [", tabela, "]!"))
		{

		}

	}
}
