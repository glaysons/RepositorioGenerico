using System;
using RepositorioGenerico.Entities.Anotacoes.Validadores;

namespace RepositorioGenerico.Test
{
	public class ValidadorDeClasseAttribute : ValidadorEntidadeAttribute
	{
		public override void Validar(object objeto)
		{
			var teste = (ObjetoDeTestes)objeto;
			if (teste.Nome.Length % 3 == 0)
				throw new Exception("O tamanho do nome não pode ser multiplo de 3!");
		}
	}
}
