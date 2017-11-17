using System;

namespace RepositorioGenerico.Entities.Anotacoes
{

	[AttributeUsage(AttributeTargets.Property)]
	public class AutoIncrementoAttribute : Attribute
	{

		public Incremento Incremento { get; set; }

		public AutoIncrementoAttribute(Incremento incremento)
		{
			Incremento = incremento;
		}

	}

}
