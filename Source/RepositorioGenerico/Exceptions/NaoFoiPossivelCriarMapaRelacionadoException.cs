using System;

namespace RepositorioGenerico.Exceptions
{
	public class NaoFoiPossivelCriarMapaRelacionadoException : Exception
	{

		public NaoFoiPossivelCriarMapaRelacionadoException(string nome)
			: base(string.Concat("Não foi possível criar um mapa relacionado com o objeto [", nome, "]!"))
		{

		}

	}
}
