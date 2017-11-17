using System;

namespace RepositorioGenerico.Exceptions
{
	public class ChavePrimariaInvalidaException: Exception
	{

		public ChavePrimariaInvalidaException()
			: base("A tabela informada não possui informações de chave primaria!")
		{

		}

	}
}
