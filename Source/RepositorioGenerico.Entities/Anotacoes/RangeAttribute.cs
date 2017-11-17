using System;

namespace RepositorioGenerico.Entities.Anotacoes
{

	[AttributeUsage(AttributeTargets.Property)]
	public class RangeAttribute : Attribute
	{

		public object De { get; private set; }

		public object Ate { get; private set; }

		public RangeAttribute(int de, int ate)
		{
			De = de;
			Ate = ate;
		}

		public RangeAttribute(double de, double ate)
		{
			De = de;
			Ate = ate;
		}

		public RangeAttribute(decimal de, decimal ate)
		{
			De = de;
			Ate = ate;
		}

		public RangeAttribute(DateTime de, DateTime ate)
		{
			De = de;
			Ate = ate;
		}

	}

}
