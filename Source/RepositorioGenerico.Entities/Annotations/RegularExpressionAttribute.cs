using RepositorioGenerico.Entities.Anotacoes;

namespace RepositorioGenerico.Entities.Annotations
{

	public class RegularExpressionAttribute : ExpressaoRegularAttribute
	{

		public string Expression { get { return Expressao; } }

		public RegularExpressionAttribute(string expression)
			: base(expression)
		{
		}

	}

}
