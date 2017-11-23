using ConverterBancoParaEntidades.Estruturas;
using System.Collections.Generic;

namespace ConverterBancoParaEntidades.Interfaces
{
	public interface IConsultador
	{

		string[] ConsultarTabelas();

		IEnumerable<Campo> ConsultarCamposDaTabela(string tabela);

	}
}
