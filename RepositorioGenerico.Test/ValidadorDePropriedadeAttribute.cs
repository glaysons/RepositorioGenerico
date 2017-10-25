using System;
using RepositorioGenerico.Entities.Anotacoes.Validadores;

namespace RepositorioGenerico.Test
{
	public class ValidadorDePropriedadeAttribute : ValidadorPropriedadeAttribute
	{
		public override void Validar(object valorDaPropriedade)
		{
			var nome = ((valorDaPropriedade == null) || (valorDaPropriedade == DBNull.Value))
				? string.Empty
				: valorDaPropriedade.ToString();
			if (string.IsNullOrEmpty(nome))
				throw new Exception("Favor informar um nome válido!");
			if (nome.Length < 10)
				throw new Exception("Favor informar no mínimo 10 caracteres para o nome!");
		}
	}
}
