using System;

namespace RepositorioGenerico.Exceptions
{
	public class TransacaoJaIniciadaException: Exception
	{

		public TransacaoJaIniciadaException()
			: base("Já existe uma transação em andamento!")
		{

		}

	}
}
