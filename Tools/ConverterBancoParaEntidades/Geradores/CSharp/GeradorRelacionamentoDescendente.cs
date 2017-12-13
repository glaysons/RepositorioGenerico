using ConverterBancoParaEntidades.Interfaces;
using System.IO;
using ConverterBancoParaEntidades.Estruturas;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConverterBancoParaEntidades.Geradores.CSharp
{
	public class GeradorRelacionamentoDescendente : IGeradorPropriedade
	{

		private IConsultador _consultador;

		public GeradorRelacionamentoDescendente(IConsultador consultador)
		{
			_consultador = consultador;
		}

		public void Gerar(string tabela, IList<Campo> campos, StreamWriter arquivo)
		{
			var camposRelacionados = _consultador.ConsultarRelacionamentosDescendentesDaTabela(tabela).ToList();
			if (camposRelacionados.Count == 0)
				return;
			GeradorRegiao.GerarInicio(arquivo, "Relacionamentos Descendentes");
			GerarPropriedades(tabela, camposRelacionados, arquivo);
			GeradorRegiao.GerarFim(arquivo);
		}

		private void GerarPropriedades(string tabela, IList<Relacionamento> camposRelacionados, StreamWriter arquivo)
		{
			foreach (var campoRelacionado in camposRelacionados.Select(c => c.Nome).Distinct())
			{
				var relacionamento = camposRelacionados.Where(c => string.Equals(c.Nome, campoRelacionado)).First();
				GerarAtributos(arquivo, relacionamento);
				GerarPropriedade(arquivo, relacionamento);
			}
		}

		private void GerarAtributos(StreamWriter arquivo, Relacionamento relacionamento)
		{
			arquivo.Write("\t\t[PropriedadeDeLigacaoEstrangeira(\"");
			arquivo.Write(relacionamento.TabelaChavePrimaria);
			arquivo.Write(relacionamento.Nome);
			arquivo.WriteLine("\")]");
		}

		private void GerarPropriedade(StreamWriter arquivo, Relacionamento relacionamento)
		{
			arquivo.Write("\t\tpublic virtual ICollection<");
			arquivo.Write(relacionamento.TabelaChaveEstrangeira);
			arquivo.Write("> ");
			arquivo.Write(relacionamento.TabelaChaveEstrangeira);
			arquivo.WriteLine("s { get; set; }");
			arquivo.WriteLine();
		}

	}
}
