using System;

namespace RepositorioGenerico.Entities.Annotations
{

	public class RangeAttribute : Anotacoes.RangeAttribute
	{

		public object Minimum { get { return De; } }

		public object Maximum { get { return Ate; } }

		public RangeAttribute(int de, int ate)
			: base(de, ate)
		{
		}

		public RangeAttribute(double de, double ate)
			: base(de, ate)
		{
		}

		public RangeAttribute(decimal de, decimal ate)
			: base(de, ate)
		{
		}

		public RangeAttribute(DateTime de, DateTime ate)
			: base(de, ate)
		{
		}

	}

}
