using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConverterBancoParaEntidades.Interfaces;

namespace ConverterBancoParaEntidades.Geradores.CSharp
{
	public class GeradorCamposTabela : IGeradorPropriedade
	{

		private IConsultador _consultador;

		public GeradorCamposTabela(IConsultador consultador)
		{
			_consultador = consultador;
		}

		public void Gerar(string tabela, StreamWriter arquivo)
		{
			throw new NotImplementedException();
		}

	}
}
