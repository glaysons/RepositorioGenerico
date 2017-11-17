using System;
using System.Linq.Expressions;
using System.Reflection;
using RepositorioGenerico.Exceptions;

namespace RepositorioGenerico.Framework.Helpers
{
	public static class ExpressionHelper
	{

		public static PropertyInfo PropriedadeDaExpressao<T>(Expression<Func<T, object>> campo)
		{
			return PropriedadeDaExpressao(campo as LambdaExpression);
		}

		public static PropertyInfo PropriedadeDaExpressao(LambdaExpression campo)
		{
			var prop = campo.Body as MemberExpression;
			if (prop != null)
				return (PropertyInfo)prop.Member;
			var body = campo.Body as UnaryExpression;
			if (body == null)
				throw new PropriedadeInvalidaException();
			return (PropertyInfo)((MemberExpression)body.Operand).Member;
		}

		public static LambdaExpression CriarExpressaoDeConsultaDaPropriedade<TObjeto>(PropertyInfo propriedadeOrigem)
		{
			var origem = typeof(TObjeto);
			var parametro = Expression.Parameter(typeof(TObjeto));
			var tipoConvertido = Expression.TypeAs(parametro, origem);
			var propriedade = Expression.PropertyOrField(tipoConvertido, propriedadeOrigem.Name);
			return Expression.Lambda(propriedade, null);
		}

	}
}
