using System;

namespace RepositorioGenerico.Exceptions
{
	public class NaoFoiPossivelOResultadoDaProcedureFakeException : Exception
	{
		public NaoFoiPossivelOResultadoDaProcedureFakeException(string nomeProcedure)
			: base(string.Concat("Não foi possível determinar um resultado para execução da procedure [", 
				nomeProcedure, "]!"))
		{

		}
	}
}
