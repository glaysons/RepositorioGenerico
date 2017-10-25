using System;

namespace RepositorioGenerico.Entities.Anotacoes.Validadores
{

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public abstract class ValidadorEntidadeAttribute : Attribute, IValidadorEntidadeAttribute
	{

		public abstract void Validar(object objeto);

	}

}
