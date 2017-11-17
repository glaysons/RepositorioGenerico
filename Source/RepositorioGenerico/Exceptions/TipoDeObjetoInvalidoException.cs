using System;

namespace RepositorioGenerico.Exceptions
{
	public class TipoDeObjetoInvalidoException : Exception
	{

		public TipoDeObjetoInvalidoException(string tipo)
			: base(string.Concat("Favor informar um tipo [", tipo, "] de objeto válido para salvar!"))
		{

		}

	}
}
