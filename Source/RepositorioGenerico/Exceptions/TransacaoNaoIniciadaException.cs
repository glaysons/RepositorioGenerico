using System;

namespace RepositorioGenerico.Exceptions
{
	public class TransacaoNaoIniciadaException: Exception
	{

		public TransacaoNaoIniciadaException()
			: base("Transação não iniciada!")
		{

		}

	}
}
