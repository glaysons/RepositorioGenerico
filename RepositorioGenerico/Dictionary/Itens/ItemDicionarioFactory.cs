using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using RepositorioGenerico.Dictionary.Helpers;
using RepositorioGenerico.Dictionary.Relacionamentos;
using RepositorioGenerico.Entities;
using RepositorioGenerico.Entities.Anotacoes.Validadores;
using RepositorioGenerico.Exceptions;
using RepositorioGenerico.Framework.Helpers;

namespace RepositorioGenerico.Dictionary.Itens
{

	public static class ItemDicionarioFactory
	{

		public static ItemDicionario CriarItemDicionario(int id, PropertyInfo propriedade, PropertyInfo propriedadeReferenciada = null)
		{
			if (propriedadeReferenciada == null)
				propriedadeReferenciada = propriedade;

			var coluna = DataAnnotationHelper.ConsultarColuna(propriedadeReferenciada);

			var alias = propriedade.Name;
			var nome = ((coluna == null) || (string.IsNullOrEmpty(coluna.Nome))) ? propriedadeReferenciada.Name : coluna.Nome;
			if (string.Equals(nome, alias))
				alias = null;

			var nullable = TipoColunaNullable(propriedade);

			var campo = new ItemDicionario(
				id: id,
				alias: alias,
				nome: nome,
				tipoBanco: (coluna == null) ? DbType.Object : ConverterSqlDbTypeParaDbType(EnumHelper.FromString<SqlDbType>(coluna.NomeDoTipo)),
				tipoLocal: (nullable) ? ConsultarTipoDaColunaNullable(propriedade) : propriedade.PropertyType,
				chave: DataAnnotationHelper.ChavePrimaria(propriedadeReferenciada),
				obrigatorio: DataAnnotationHelper.Obrigatorio(propriedadeReferenciada),
				tamanhoMaximo: DataAnnotationHelper.ConsultarTamanhoMaximo(propriedadeReferenciada),
				ordem: (coluna == null) ? 0 : coluna.Ordem,
				opcaoGeracao: DataAnnotationHelper.ConsultarOpcaoGeracao(propriedadeReferenciada),
				valorPadrao: DataAnnotationHelper.ConsultarValorPadrao(propriedadeReferenciada),
				mapeado: DataAnnotationHelper.Mapeado(propriedade),
				propriedade: propriedade,
				validacoes: ConsultarValidacoes(propriedade, propriedadeReferenciada),
				ligacao: CriarLigacaoEntreModels(propriedade, propriedadeReferenciada)
			);

			if (nullable && campo.Obrigatorio)
				throw new CampoNullableInvalidoException();

			return campo;
		}

		private static DbType ConverterSqlDbTypeParaDbType(SqlDbType sql)
		{
			switch (sql)
			{
				case SqlDbType.Text:
				case SqlDbType.VarChar:
					return DbType.String;

				case SqlDbType.Int:
					return DbType.Int32;

				case SqlDbType.Bit:
					return DbType.Boolean;

				case SqlDbType.Date:
				case SqlDbType.DateTime:
				case SqlDbType.DateTime2:
				case SqlDbType.DateTimeOffset:
				case SqlDbType.Decimal:
				case SqlDbType.Time:
				case SqlDbType.Xml:
				case SqlDbType.Binary:

					DbType retorno;
					if (Enum.TryParse(sql.ToString(), ignoreCase: true, result: out retorno))
						return retorno;
					return DbType.Object;

				case SqlDbType.BigInt:
					return DbType.Int64;

				case SqlDbType.Char:
					return DbType.StringFixedLength;

				case SqlDbType.Float:
				case SqlDbType.Money:
				case SqlDbType.Real:
				case SqlDbType.SmallMoney:
					return DbType.Double;

				case SqlDbType.NChar:
					return DbType.AnsiStringFixedLength;

				case SqlDbType.NText:
				case SqlDbType.NVarChar:
					return DbType.AnsiString;

				case SqlDbType.SmallDateTime:
					return DbType.Date;

				case SqlDbType.SmallInt:
					return DbType.Int16;

				case SqlDbType.TinyInt:
					return DbType.UInt16;

				case SqlDbType.UniqueIdentifier:
					return DbType.Guid;

				default:
					return DbType.Object;

			}
		}

		private static bool TipoColunaNullable(PropertyInfo propriedade)
		{
			var tipo = propriedade.PropertyType;
			return ((tipo.IsGenericType) && (tipo.GetGenericTypeDefinition() == typeof(Nullable<>)));
		}

		private static Type ConsultarTipoDaColunaNullable(PropertyInfo propriedade)
		{
			var tipo = propriedade.PropertyType;
			return tipo.GetGenericArguments()[0];
		}

		private static IList<IValidadorPropriedadeAttribute> ConsultarValidacoes(PropertyInfo propriedade, PropertyInfo propriedadeReferenciada)
		{
			var lista = new List<IValidadorPropriedadeAttribute>();
			lista.AddRange(ConsultarValidadoresDaPropriedade(propriedade));
			if (propriedade != propriedadeReferenciada)
				lista.AddRange(ConsultarValidadoresDaPropriedade(propriedadeReferenciada));
			if (lista.Count > 0)
				return lista;
			return null;
		}

		private static IEnumerable<IValidadorPropriedadeAttribute> ConsultarValidadoresDaPropriedade(PropertyInfo propriedade)
		{
			var validacoes = AttributeHelper.ConsultarTodos<ValidadorPropriedadeAttribute>(propriedade);
			foreach (var item in validacoes)
			{
				var validacao = item as IValidadorPropriedadeAttribute;
				if (validacao != null)
					yield return validacao;
			}
		}

		private static Relacionamento CriarLigacaoEntreModels(PropertyInfo propriedade, PropertyInfo propriedadeReferenciada)
		{
			var tipoPropriedade = propriedade.PropertyType;
			if (!propriedade.GetGetMethod().IsVirtual)
				return null;
			var chaveEstrangeira = DataAnnotationHelper.ConsultarForeignKey(propriedadeReferenciada);
			if (!string.IsNullOrEmpty(chaveEstrangeira))
				return CriarLigacaoAscendenteEntreModels(propriedadeReferenciada, chaveEstrangeira);
			if (typeof(IEntidade).IsAssignableFrom(tipoPropriedade))
				GerarErroDeLigacao(propriedade);
			var inverse = DataAnnotationHelper.ConsultarForeignKeyDaInverseProperty(propriedadeReferenciada, tipoPropriedade);
			if (!string.IsNullOrEmpty(inverse))
				return GerarScriptCarregamentoDescendente(propriedadeReferenciada, inverse, tipoPropriedade);
			if ((tipoPropriedade.IsGenericType) && (tipoPropriedade.GetGenericTypeDefinition() == typeof(ICollection<>)))
				GerarErroDeLigacao(propriedade);
			return null;
		}

		private static void GerarErroDeLigacao(PropertyInfo propriedade)
		{
			var tipo = (propriedade.DeclaringType == null)
				? string.Empty
				: propriedade.DeclaringType.Name;
			throw new NaoFoiPossivelEncontrarUmaLigacaoParaOCampoException(tipo, propriedade.Name);
		}

		private static Relacionamento CriarLigacaoAscendenteEntreModels(PropertyInfo propriedade, string chaveEstrangeira)
		{
			var dicionario = DicionarioCache.Consultar(propriedade.PropertyType);
			return new Relacionamento(TiposRelacionamento.Ascendente, dicionario, chaveEstrangeira);
		}

		private static Relacionamento GerarScriptCarregamentoDescendente(PropertyInfo propriedade, string chaveEstrangeiraDaPropriedadeInversa, Type tipoPropriedade)
		{
			var tipo = propriedade.PropertyType;
			if ((!tipo.IsGenericType) || (tipo.GetGenericTypeDefinition() != typeof(ICollection<>)))
				return null;
			var tipoGenerico = tipo.GetGenericArguments()[0];
			var dicionarioTipoPropriedade = DicionarioCache.Consultar(tipoPropriedade.GetGenericArguments()[0]);
			if ((dicionarioTipoPropriedade.OrigemMapa != null) && (tipoGenerico == dicionarioTipoPropriedade.OrigemMapa))
				tipoGenerico = dicionarioTipoPropriedade.Tipo;
			var dicionario = DicionarioCache.Consultar(tipoGenerico);
			return new Relacionamento(TiposRelacionamento.Descendente, dicionario, chaveEstrangeiraDaPropriedadeInversa);
		}
	}

}
