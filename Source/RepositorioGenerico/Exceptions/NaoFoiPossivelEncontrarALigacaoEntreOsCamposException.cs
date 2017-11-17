using System;

namespace RepositorioGenerico.Exceptions
{
	public class NaoFoiPossivelEncontrarALigacaoEntreOsCamposException : Exception
	{
		public NaoFoiPossivelEncontrarALigacaoEntreOsCamposException()
			: base("Não foi possível encontrar a ligação entre os campos!")
		{
			
		}
	}
}
