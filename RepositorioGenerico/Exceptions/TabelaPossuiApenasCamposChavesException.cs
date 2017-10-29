using System;

namespace RepositorioGenerico.Exceptions
{
	public class TabelaPossuiApenasCamposChavesException : Exception
	{

		public TabelaPossuiApenasCamposChavesException()
			: base("Não é possível realizar um update pois a tabela informada não possui campos para atualização!")
		{
			
		}

	}
}
