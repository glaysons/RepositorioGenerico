using System;

namespace RepositorioGenerico.Exceptions
{
	public class NaoFoiPossivelDeterminarONomeDaTabelaFakeException : Exception
	{
		public NaoFoiPossivelDeterminarONomeDaTabelaFakeException()
			: base("Não foi possível determinar o nome da tabela fake!")
		{

		}
	}
}
