using System;

namespace RepositorioGenerico.Exceptions
{
	public class CampoPossuiTamanhoMinimoDePeenchimentoException : Exception
	{
		public CampoPossuiTamanhoMinimoDePeenchimentoException(string campo, int tamanhoMinimo)
			: base(string.Concat("O campo [", campo, "] precisa ser preenchido com no mínimo [", tamanhoMinimo, "] caracteres!"))
		{

		}
	}
}
