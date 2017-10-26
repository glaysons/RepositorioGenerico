using System;

namespace RepositorioGenerico.Exceptions
{
	public class NaoSeraPossivelCriarUmDicionarioDeUmObjetoQueNaoHerdeObjetoBancoException : Exception
	{
		public NaoSeraPossivelCriarUmDicionarioDeUmObjetoQueNaoHerdeObjetoBancoException()
			: base("Não é possível criar um dicionário para um objeto que não herde o objeto [ObjetoBanco]!")
		{
			
		}
	}
}
