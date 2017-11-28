using ConverterBancoParaEntidades.Estruturas;
using ConverterBancoParaEntidades.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConverterBancoParaEntidades.Geradores.CSharp
{
	public class GeradorRelacionamentoAscendente : IGeradorPropriedade
	{

		private IConsultador _consultador;

		public GeradorRelacionamentoAscendente(IConsultador consultador)
		{
			_consultador = consultador;
		}

		public void Gerar(string tabela, IList<Campo> campos, StreamWriter arquivo)
		{
			var camposRelacionados = _consultador.ConsultarRelacionamentosAscendentesDaTabela(tabela).ToList();
			if (camposRelacionados.Count == 0)
				return;
			GeradorRegiao.GerarInicio(arquivo, "Relacionamentos Ascendentes");
			GerarPropriedades(tabela, campos, camposRelacionados, arquivo);
			GeradorRegiao.GerarFim(arquivo);
		}

		private void GerarPropriedades(string tabela, IList<Campo> campos, IList<Relacionamento> camposRelacionados, StreamWriter arquivo)
		{
			foreach (var campoRelacionado in camposRelacionados.Select(c => c.Nome).Distinct())
			{
				var relacionamentos = camposRelacionados.Where(c => string.Equals(c.Nome, campoRelacionado)).ToList();
				GerarAtributos(arquivo, campos, relacionamentos);
				GerarPropriedade(arquivo, campos, relacionamentos);
			}
		}

		private void GerarAtributos(StreamWriter arquivo, IList<Campo> campos, IList<Relacionamento> relacionamentos)
		{
			var chaveMultipla = false;
			arquivo.Write("\t\t[ChaveEstrangeira(\"");
			foreach (var campo in campos.OrderBy(c => c.Ordem))
				if (relacionamentos.Any(r => string.Equals(campo.NomeCampo, r.ColunaChaveEstrangeira, StringComparison.InvariantCultureIgnoreCase)))
				{
					if (chaveMultipla)
						arquivo.Write(",");
					else
						chaveMultipla = true;
					arquivo.Write(campo.NomeCampo);
				}
			arquivo.WriteLine("\"]");
		}

		private void GerarPropriedade(StreamWriter arquivo, IList<Campo> campos, IList<Relacionamento> relacionamentos)
		{
			var relacionamento = relacionamentos.First();
			arquivo.Write("\t\tpublic virtual ");
			arquivo.Write(relacionamento.TabelaChavePrimaria);
			arquivo.Write(" ");
			arquivo.Write(relacionamento.TabelaChavePrimaria);
			arquivo.Write(relacionamento.Nome);
			arquivo.WriteLine(" { get; set; }");
			arquivo.WriteLine();
		}

	}
}
