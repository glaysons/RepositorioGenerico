using System;

namespace RepositorioGenerico.Exceptions
{
	public class AQuantidadeDeCamposChaveNaLigacaoDoCampoEInvalidaException : Exception
	{
		public AQuantidadeDeCamposChaveNaLigacaoDoCampoEInvalidaException()
			: base("A quantidade de campos chave na ligação é inválida!")
		{
			
		}
	}
}
