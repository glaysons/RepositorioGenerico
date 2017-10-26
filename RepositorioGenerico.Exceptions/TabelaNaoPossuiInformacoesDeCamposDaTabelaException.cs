using System;

namespace RepositorioGenerico.Exceptions
{
	public class TabelaNaoPossuiInformacoesDeCamposDaTabelaException : Exception
	{

		public TabelaNaoPossuiInformacoesDeCamposDaTabelaException()
			: base("Não foi possível localizar as informações dos campos da tabela!")
		{
			
		}

	}
}
