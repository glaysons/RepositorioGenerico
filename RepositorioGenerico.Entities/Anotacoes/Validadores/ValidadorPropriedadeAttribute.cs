using System;

namespace RepositorioGenerico.Entities.Anotacoes.Validadores
{

	[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
	public abstract class ValidadorPropriedadeAttribute : Attribute, IValidadorPropriedadeAttribute
	{

		public abstract void Validar(object valorDaPropriedade);

	}

}
