using ConverterBancoParaEntidades.Estruturas;
using System.Collections.Generic;

namespace ConverterBancoParaEntidades.Interfaces
{
	public interface IConsultador
	{

		string[] ConsultarTabelas();

		IList<Campo> ConsultarCamposDaTabela(string tabela);

		IEnumerable<Relacionamento> ConsultarRelacionamentosAscendentesDaTabela(string tabela);

		IEnumerable<Relacionamento> ConsultarRelacionamentosDescendentesDaTabela(string tabela);

	}
}
