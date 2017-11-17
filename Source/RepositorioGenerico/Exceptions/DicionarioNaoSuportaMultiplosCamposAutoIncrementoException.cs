using System;

namespace RepositorioGenerico.Exceptions
{
	public class DicionarioNaoSuportaMultiplosCamposAutoIncrementoException : Exception
	{
		public DicionarioNaoSuportaMultiplosCamposAutoIncrementoException()
			: base("Dicionário não suporta multiplos campos autoincremento!")
		{
			
		}
	}
}
