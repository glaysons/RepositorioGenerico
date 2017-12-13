using System;

namespace RepositorioGenerico.Exceptions
{
	public class TabelaNaoPossuiInformacoesDeCamposDaTabelaException : Exception
	{

		public TabelaNaoPossuiInformacoesDeCamposDaTabelaException(string tabela)
			: base(string.Concat("Não foi possível localizar as informações dos campos da tabela [", tabela, "]!"))
		{
			
		}

	}
}
