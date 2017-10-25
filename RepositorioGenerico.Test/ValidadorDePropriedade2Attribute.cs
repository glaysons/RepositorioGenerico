using System;
using RepositorioGenerico.Entities.Anotacoes.Validadores;

namespace RepositorioGenerico.Test
{
	public class ValidadorDePropriedade2Attribute : ValidadorPropriedadeAttribute
	{
		public override void Validar(object valorDaPropriedade)
		{
			var nome = ((valorDaPropriedade == null) || (valorDaPropriedade == DBNull.Value))
				? string.Empty
				: valorDaPropriedade.ToString();
			if (nome.Contains("%"))
				throw new Exception("O nome não pode conter o simbolo de percentual!");
		}
	}
}