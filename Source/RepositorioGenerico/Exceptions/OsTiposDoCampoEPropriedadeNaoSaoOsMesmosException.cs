using System;

namespace RepositorioGenerico.Exceptions
{
	public class OsTiposDoCampoEPropriedadeNaoSaoOsMesmosException : Exception
	{

		public OsTiposDoCampoEPropriedadeNaoSaoOsMesmosException()
			: base("Os tipos do campo e da propriedade que estão sendo mapeados não são iguais!")
		{
			
		}

	}
}
