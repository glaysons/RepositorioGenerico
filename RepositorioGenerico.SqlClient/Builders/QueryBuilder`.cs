using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using RepositorioGenerico.Entities.Anotacoes;

namespace RepositorioGenerico.SqlClient.Builders
{
	public class QueryBuilder<TObjeto>
	{

		private readonly Expression _expression;
		private IEnumerable<PropertyInfo> _propriedades;

		public bool PodeAvaliarUltimaExpressao { get; set; }

		public QueryBuilder(Expression expression)
		{
			_expression = expression;
			CarregarCampos();
		}

		private void CarregarCampos()
		{
			var tipo = typeof(TObjeto);
			_propriedades =
				from property in tipo.GetProperties()
				where (property.PropertyType.IsValueType || property.PropertyType == typeof(string))
					  && !property.GetCustomAttributes(typeof(NaoMapeadoAttribute), inherit:true).Any()
				select property;
		}

		public override string ToString()
		{
			return "select " + ConsultarCampos() + " from " + typeof(TObjeto).Name;
			//LinqToSql.SqlExpressionParser.TranslateExpression(_expression);
			//var objSqlExpression = EvaluationVisitor.Evaluate(_expression);
			//return objSqlExpression.ToString();
		}

		private string ConsultarCampos()
		{
			var campos = new StringBuilder();
			foreach (var propriedade in _propriedades)
			{
				campos.Append(propriedade.Name);
				campos.Append(",");
			}
			if (campos.Length > 0)
				campos.Length -= 1;
			return campos.ToString();
		}

	}

	public class EvaluationVisitor : ExpressionVisitor
	{
		private static readonly EvaluationVisitor Singleton;

		/// <summary>
		/// Static constructor to nitialize the singleton visitor instance.
		/// </summary>
		static EvaluationVisitor()
		{
			Singleton = new EvaluationVisitor();
		}

		/// <summary>
		/// Private constructor to prevent non-singleton use. All external interaction 
		/// with this type should be done via its public <see cref="Evaluate"/> method.
		/// </summary>
		private EvaluationVisitor()
		{

		}

		#region Methods

		/// <summary>
		/// Perform immediate evaluates of an expression.
		/// </summary>
		/// <param name="expression">The expression tree to evaluate.</param>
		/// <returns>The result of the evaluation.</returns>
		/// <exception cref="NotImplementedException">The expression tree provided 
		/// contains a node type that is not supported.</exception>
		public static object Evaluate(Expression expression)
		{
			// Visit our expression, and cast the result to a ConstantExpression
			var constantExpression = (ConstantExpression)Singleton.Visit(expression);

			// Return the value of the ConstantExpression
			return constantExpression.Value;
		}

		public override Expression Visit(Expression node)
		{
			// Test if one of our implemented visitor methods was able to transform
			// the input expression into a ConstantExpression.
			var visited = base.Visit(node);
			if (visited is ConstantExpression)
			{
				// If visit was successful, then return the visited expression
				return visited;
			}
			return null;
		}

		protected override Expression VisitConstant(ConstantExpression node)
		{
			// The base case, simply return the constant expression.
			return node;
		}

		protected override Expression VisitUnary(UnaryExpression node)
		{
			// Visit the Operand expression and extract its value
			var operandConstant = (ConstantExpression)Visit(node.Operand);
			dynamic operand = operandConstant.Value;

			return Transform(node, () =>
			{
				if (node.Method != null)
					return InvokeMethod(node.Method, operand);

				switch (node.NodeType)
				{
					case ExpressionType.Decrement:
						return --operand;
					case ExpressionType.Increment:
						return ++operand;
					case ExpressionType.IsFalse:
						return operand == false;
					case ExpressionType.IsTrue:
						return operand == true;
					case ExpressionType.Negate:
						unchecked { return -operand; }
					case ExpressionType.NegateChecked:
						checked { return -operand; }
					case ExpressionType.Not:
						return !operand;
					case ExpressionType.OnesComplement:
						return ~operand;
					case ExpressionType.UnaryPlus:
						return +operand;
					default:
						throw new NotSupportedException();
				}
			});
		}

		protected override Expression VisitBinary(BinaryExpression node)
		{
			// Visit the Left expression and extract its value
			var leftConstant = (ConstantExpression)Visit(node.Left);
			dynamic left = leftConstant.Value;

			// Visit the Right expression and extract its value
			var rightConstant = (ConstantExpression)Visit(node.Right);
			dynamic right = rightConstant.Value;

			return Transform(node, () =>
			{
				if (node.Method != null)
					return InvokeMethod(node.Method, left, right);

				switch (node.NodeType)
				{
					case ExpressionType.Add:
						unchecked { return left + right; };
					case ExpressionType.AddChecked:
						checked { return left + right; };
					case ExpressionType.Divide:
						return left / right;
					case ExpressionType.Equal:
						return left == right;
					case ExpressionType.GreaterThan:
						return left > right;
					case ExpressionType.GreaterThanOrEqual:
						return left >= right;
					case ExpressionType.LeftShift:
						return left << right;
					case ExpressionType.LessThan:
						return left < right;
					case ExpressionType.LessThanOrEqual:
						return left <= right;
					case ExpressionType.Modulo:
						return left % right;
					case ExpressionType.Multiply:
						unchecked { return left * right; }
					case ExpressionType.MultiplyChecked:
						checked { return left * right; }
					case ExpressionType.NotEqual:
						return left != right;
					case ExpressionType.RightShift:
						return left >> right;
					case ExpressionType.Subtract:
						unchecked { return left - right; }
					case ExpressionType.SubtractChecked:
						checked { return left - right; };
					case ExpressionType.And:
						return left & right;
					case ExpressionType.AndAlso:
						return left && right;
					case ExpressionType.Or:
						return left | right;
					case ExpressionType.OrElse:
						return left || right;
					case ExpressionType.ExclusiveOr:
						return left ^ right;
					default:
						throw new NotSupportedException();
				}
			});
		}

		private static Expression Transform(Expression input, Func<object> transform)
		{
			var result = transform();

			var convertedResult = Convert.ChangeType(result, input.Type);
			return Expression.Constant(convertedResult, input.Type);
		}

		private static object InvokeMethod(MethodInfo method, params object[] args)
		{
			try
			{
				return method.Invoke(null, args);
			}
			catch (TargetInvocationException ex)
			{
				throw ex.InnerException;
			}
		}

		#endregion
	}

}
