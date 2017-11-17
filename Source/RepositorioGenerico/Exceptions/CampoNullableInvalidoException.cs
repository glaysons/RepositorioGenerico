using System;

namespace RepositorioGenerico.Exceptions
{
	public class CampoNullableInvalidoException : Exception
	{

		public CampoNullableInvalidoException()
			: base("Não é possível definir uma propriedade Nullable como obrigatória!")
		{

		}

	}
}
