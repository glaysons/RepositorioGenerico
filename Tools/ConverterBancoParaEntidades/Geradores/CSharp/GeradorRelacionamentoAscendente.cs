using ConverterBancoParaEntidades.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConverterBancoParaEntidades.Geradores.CSharp
{
	public class GeradorRelacionamentoAscendente : IGeradorPropriedade
	{
		private IConsultador _consultador;

		public GeradorRelacionamentoAscendente(IConsultador consultador)
		{
			_consultador = consultador;
		}

		public void Gerar(string tabela, StreamWriter arquivo)
		{
			throw new NotImplementedException();
		}

	}
}
