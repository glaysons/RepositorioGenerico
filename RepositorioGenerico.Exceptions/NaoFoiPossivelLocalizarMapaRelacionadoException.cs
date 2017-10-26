using System;

namespace RepositorioGenerico.Exceptions
{
	public class NaoFoiPossivelLocalizarMapaRelacionadoException : Exception
	{

		public NaoFoiPossivelLocalizarMapaRelacionadoException(string nome)
			: base(string.Concat("Não foi possível localizar o mapa relacionado com o objeto [", nome, "]!"))
		{
			
		}

	}
}
