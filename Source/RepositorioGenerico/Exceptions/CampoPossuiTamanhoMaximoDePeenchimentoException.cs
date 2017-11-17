using System;

namespace RepositorioGenerico.Exceptions
{
	public class CampoPossuiTamanhoMaximoDePeenchimentoException : Exception
	{
		public CampoPossuiTamanhoMaximoDePeenchimentoException(string campo, int tamanhoMaximo)
			: base(string.Concat("O campo [", campo, "] permite ser preenchido com no máximo [", tamanhoMaximo, "] caracteres!"))
		{

		}
	}
}
