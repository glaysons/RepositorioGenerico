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

		public static string CamihoDaExpressao<T>(Expression<Func<T, object>> expressao)
		{
			if (expressao == null)
				return null;
			string caminho = null;
			EhPossivelConverterExpressaoEmCaminho(expressao.Body, out caminho);
			return caminho;
		}

		public static bool EhPossivelConverterExpressaoEmCaminho(Expression expressao, out string caminho)
		{
			caminho = null;
			var expressaoSemConversao = RemoverConversoesDaExpressao(expressao);
			var member = expressaoSemConversao as MemberExpression;
			var call = expressaoSemConversao as MethodCallExpression;

			if (member != null)
			{
				var caminhoAtual = member.Member.Name;
				string caminhoPai;
				if (!EhPossivelConverterExpressaoEmCaminho(member.Expression, out caminhoPai))
					return false;
				caminho = caminhoPai == null 
					? caminhoAtual 
					: (caminhoPai + "." + caminhoAtual);
			}
			else if (call != null)
			{
				if (call.Method.Name == "Select"
					&& call.Arguments.Count == 2)
				{
					string caminhoPai;
					if (!EhPossivelConverterExpressaoEmCaminho(call.Arguments[0], out caminhoPai))
						return false;
					if (caminhoPai != null)
					{
						var subExpressao = call.Arguments[1] as LambdaExpression;
						if (subExpressao != null)
						{
							string caminhoAtual;
							if (!EhPossivelConverterExpressaoEmCaminho(subExpressao.Body, out caminhoAtual))
								return false;
							if (caminhoAtual != null)
							{
								caminho = caminhoPai + "." + caminhoAtual;
								return true;
							}
						}
					}
				}
				return false;
			}

			return true;
		}

		private static Expression RemoverConversoesDaExpressao(Expression expressao)
		{
			while (expressao.NodeType == ExpressionType.Convert || expressao.NodeType == ExpressionType.ConvertChecked)
				expressao = ((UnaryExpression)expressao).Operand;
			return expressao;
		}

	}
}
