using System;

namespace RepositorioGenerico.Exceptions
{
	public class NaoEhPossivelConfirmarOuCancelarTransacaoExternaException : Exception
	{
		public NaoEhPossivelConfirmarOuCancelarTransacaoExternaException()
			: base("Não é possível confirmar ou cancelar uma transação externa!")
		{

		}
	}
}
