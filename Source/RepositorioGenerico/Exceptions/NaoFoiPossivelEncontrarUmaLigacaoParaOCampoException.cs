using System;

namespace RepositorioGenerico.Exceptions
{
	public class NaoFoiPossivelEncontrarUmaLigacaoParaOCampoException : Exception
	{
		public NaoFoiPossivelEncontrarUmaLigacaoParaOCampoException(string tipo, string nome)
			: base(string.Concat("Não foi possível encontrar uma ligação válida para o campo [", nome, "] do objeto [", tipo, "]!"))
		{
			
		}
	}
}
