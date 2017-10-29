using System;

namespace RepositorioGenerico.Exceptions
{
	public class PropriedadeInvalidaException : Exception
	{

		public PropriedadeInvalidaException()
			: base("Favor informar um nome de propriedade válida!")
		{
			
		}

	}
}
