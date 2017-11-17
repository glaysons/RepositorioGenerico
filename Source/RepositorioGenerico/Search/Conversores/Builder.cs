using System;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using RepositorioGenerico.Entities.Anotacoes;

namespace RepositorioGenerico.Search.Conversores
{
	public class Builder
	{

		private static readonly ParameterExpression Reader = Expression.Parameter(typeof(DbDataReader), "reader");
		private static readonly MethodInfo MetodoGetBoolean = typeof(DbDataReader).GetMethod("GetBoolean");
		private static readonly MethodInfo MetodoGetByte = typeof(DbDataReader).GetMethod("GetByte");
		private static readonly MethodInfo MetodoGetChar = typeof(DbDataReader).GetMethod("GetChar");
		private static readonly MethodInfo MetodoGetDateTime = typeof(DbDataReader).GetMethod("GetDateTime");
		private static readonly MethodInfo MetodoGetDecimal = typeof(DbDataReader).GetMethod("GetDecimal");
		private static readonly MethodInfo MetodoGetDouble = typeof(DbDataReader).GetMethod("GetDouble");
		private static readonly MethodInfo MetodoGetGuid = typeof(DbDataReader).GetMethod("GetGuid");
		private static readonly MethodInfo MetodoGetInt16 = typeof(DbDataReader).GetMethod("GetInt16");
		private static readonly MethodInfo MetodoGetInt32 = typeof(DbDataReader).GetMethod("GetInt32");
		private static readonly MethodInfo MetodoGetInt64 = typeof(DbDataReader).GetMethod("GetInt64");
		private static readonly MethodInfo MetodoGetString = typeof(DbDataReader).GetMethod("GetString");
		private static readonly MethodInfo MetodoGetValue = typeof(DbDataReader).GetMethod("GetValue");
		private static readonly MethodInfo MetodoIsDbNull = typeof(DbDataReader).GetMethod("IsDBNull");

		public Delegate CriarBinderDoObjeto(Type tipo)
		{
			var lambdaDoLeitor = CriarLambdaDoLeitor(tipo);
			return lambdaDoLeitor.Compile();
		}

		private LambdaExpression CriarLambdaDoLeitor(Type tipo)
		{
			var todasPropriedades = (
				from property in tipo.GetProperties()
				where (property.PropertyType.IsValueType || property.PropertyType == typeof(string))
					&& !property.GetCustomAttributes(typeof(NaoMapeadoAttribute), inherit: true).Any()
				select property).ToArray();

			var propriedades = (from property in todasPropriedades
								select tipo.GetField(string.Format("<{0}>k__BackingField", property.Name),
															 BindingFlags.Instance | BindingFlags.NonPublic)).ToArray();

			var bindings = new MemberBinding[propriedades.Length];
			for (var i = 0; i < propriedades.Length; i++)
			{
				MemberInfo parametro = propriedades[i];
				if (parametro == null)
					parametro = todasPropriedades[i];
				var metodo = ConsultarLeitorDaPropriedade(
					Expression.MakeMemberAccess(Expression.Parameter(tipo, "param"), parametro), i);
				bindings[i] = Expression.Bind(parametro, metodo);
			}

			return Expression.Lambda(Expression.MemberInit(Expression.New(tipo), bindings), Reader);
		}

		private Expression ConsultarLeitorDaPropriedade(Expression parametro, int posicao)
		{
			var campo = Expression.Constant(posicao, typeof(int));
			var expressaoAceitaDbNull = Expression.Call(Reader, MetodoIsDbNull, campo);
			var expressaoDoReader = CriarExpressaoDoReader(parametro, campo);

			var expressaoCondicional =
				Expression.Condition(expressaoAceitaDbNull,
					Expression.Convert(Expression.Constant(null), expressaoDoReader.Type), expressaoDoReader);

			return expressaoCondicional;
		}

		private Expression CriarExpressaoDoReader(Expression parametro, ConstantExpression campo)
		{
			var metodoDoReader = ConsultarMetodoDoReader(parametro);
			var readerExpression = Expression.Call(Reader, metodoDoReader, campo);
			if (metodoDoReader.ReturnType == parametro.Type)
				return readerExpression;
			return Expression.Convert(readerExpression, parametro.Type);
		}

		private static MethodInfo ConsultarMetodoDoReader(Expression parametro)
		{
			var tipoDoCampo = ConsultarTipoDoParametro(parametro);
			MethodInfo metodo;
			switch (Type.GetTypeCode(tipoDoCampo))
			{
				case TypeCode.String:
					metodo = MetodoGetString;
					break;
				case TypeCode.Int32:
					metodo = MetodoGetInt32;
					break;
				case TypeCode.Decimal:
					metodo = MetodoGetDecimal;
					break;
				case TypeCode.Boolean:
					metodo = MetodoGetBoolean;
					break;
				case TypeCode.DateTime:
					metodo = MetodoGetDateTime;
					break;
				case TypeCode.Double:
					metodo = MetodoGetDouble;
					break;
				case TypeCode.Char:
					metodo = MetodoGetChar;
					break;
				case TypeCode.Int16:
					metodo = MetodoGetInt16;
					break;
				case TypeCode.Int64:
					metodo = MetodoGetInt64;
					break;
				case TypeCode.Byte:
					metodo = MetodoGetByte;
					break;
				default:
					metodo = (parametro.Type == typeof(Guid))
						? MetodoGetGuid
						: MetodoGetValue;
					break;
			}
			return metodo;
		}

		private static Type ConsultarTipoDoParametro(Expression m)
		{
			return (m.Type.Name == "Nullable`1")
				? m.Type.GetGenericArguments()[0]
				: m.Type;
		}

	}
}