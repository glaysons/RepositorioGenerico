using System;
using RepositorioGenerico.Entities.Anotacoes.Validadores;

namespace RepositorioGenerico.Test
{
	public class ValidadorDeClasse2Attribute : ValidadorEntidadeAttribute
	{
		public override void Validar(object objeto)
		{
			var teste = (ObjetoDeTestes)objeto;
			if (teste.Nome.Length % 5 == 0)
				throw new Exception("O tamanho do nome não pode ser multiplo de 5!");
		}
	}
}