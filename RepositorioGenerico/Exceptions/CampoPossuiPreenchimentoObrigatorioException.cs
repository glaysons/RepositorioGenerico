using System;

namespace RepositorioGenerico.Exceptions
{
	public class CampoPossuiPreenchimentoObrigatorioException : Exception
	{
		public CampoPossuiPreenchimentoObrigatorioException(string campo)
			: base(string.Concat("O campo [", campo, "] é de preenchimento obrigatório!"))
		{
			
		}
	}
}
