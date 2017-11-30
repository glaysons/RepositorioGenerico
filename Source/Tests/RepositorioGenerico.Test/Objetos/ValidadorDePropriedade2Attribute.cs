using System;
using RepositorioGenerico.Entities.Anotacoes.Validadores;
using System.Reflection;

namespace RepositorioGenerico.Test.Objetos
{
	public class ValidadorDePropriedade2Attribute : ValidadorPropriedadeAttribute
	{
		public override void Validar(PropertyInfo propriedade, object valorDaPropriedade)
		{
			var nome = ((valorDaPropriedade == null) || (valorDaPropriedade == DBNull.Value))
				? string.Empty
				: valorDaPropriedade.ToString();
			if (nome.Contains("%"))
				throw new Exception("O nome não pode conter o simbolo de percentual!");
		}
	}
}