using System;

namespace RepositorioGenerico.Exceptions
{
	public class ValoresChavePreenchimentoObrigatorioException: Exception
	{
		public ValoresChavePreenchimentoObrigatorioException()
			: base("Favor informar valores para todos os campos da chave primária da tabela!")
		{
			
		}

		public ValoresChavePreenchimentoObrigatorioException(int quantidadeCamposNaChave)
			: base("Favor informar valores para todos os campos da chave primária da tabela! " +
				   "Esta tabela exige o preenchimento de [" + quantidadeCamposNaChave.ToString() + "] valores.")
		{

		}
	}
}
