using RepositorioGenerico.Entities.Anotacoes;

namespace RepositorioGenerico.Entities.Annotations
{

	public class DefaultValueAttribute : ValorPadraoAttribute
	{

		public object Value { get { return Valor; } }

		public DefaultValueAttribute(object value)
			: base(value)
		{

		}

	}

}
