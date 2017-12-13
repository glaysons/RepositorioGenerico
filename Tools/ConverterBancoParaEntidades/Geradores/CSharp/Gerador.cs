using ConverterBancoParaEntidades.Interfaces;
using System;
using System.IO;

namespace ConverterBancoParaEntidades.Geradores.CSharp
{
	public class Gerador : IGerador
	{

		private IConfiguracao _configuracao;
		private IConsultador _consultador;

		private IGeradorPropriedade _geradorCampos;
		private IGeradorPropriedade _geradorAscendente;
		private IGeradorPropriedade _geradorDescendente;

		public Gerador(IConfiguracao configuracao, IConsultador consultador)
		{
			_configuracao = configuracao;
			_consultador = consultador;
			_geradorCampos = new GeradorCamposTabela();
			_geradorAscendente = new GeradorRelacionamentoAscendente(consultador);
			_geradorDescendente = new GeradorRelacionamentoDescendente(consultador);
		}

		public void Gerar()
		{
			var t = _configuracao.Tabelas.Length;
			for (int n = 0; n < t; n++)
			{
				var tabela = _configuracao.Tabelas[n];
				try
				{
					_configuracao.AtualizarStatusGeracaoTabela(string.Concat("[", (n / (double)t * 100.0).ToString("F2"), "%] ", tabela));
					GerarArquivo(tabela);
				}
				catch (Exception ex)
				{
					_configuracao.AdicionarLog("Não foi possível gerar a tabela [", tabela, "] devido ao seguinte erro: ", ex.Message);
				}
			}
			_configuracao.AtualizarStatusGeracaoTabela(string.Concat("[", 100.0.ToString("F2"), "%]"));
		}

		private void GerarArquivo(string tabela)
		{
			var arquivo = tabela + ".cs";
			var campos = _consultador.ConsultarCamposDaTabela(tabela);
			CriadorArquivos.Salvar(_configuracao.PastaDeDestino, arquivo, a =>
			{
				EscreverUsings(a);
				EscreverInicioNamespace(a);
				EscreverInicioTabela(tabela, a);
				_geradorCampos.Gerar(tabela, campos, a);
				_geradorAscendente.Gerar(tabela, campos, a);
				_geradorDescendente.Gerar(tabela, campos, a);
				EscreverFimTabela(a);
				EscreverFimNamespace(a);
			});
		}

		private void EscreverUsings(StreamWriter a)
		{
			foreach (var _using in _configuracao.Usings)
			{
				if (string.IsNullOrWhiteSpace(_using))
					continue;
				if (!_using.StartsWith("using "))
					a.Write("using ");
				a.Write(_using);
				if (!_using.EndsWith(";"))
					a.Write(";");
				a.WriteLine();
			}
			a.WriteLine();
		}

		private void EscreverInicioNamespace(StreamWriter a)
		{
			a.WriteLine(string.Concat("namespace ", _configuracao.Namespace));
			a.WriteLine("{");
			a.WriteLine();
		}

		private void EscreverInicioTabela(string tabela, StreamWriter a)
		{
			a.Write("\t[Tabela(\"");
			a.Write(tabela);
			a.WriteLine("\")]");
			a.WriteLine(string.Concat("\tpublic partial class ", tabela, " : ", _configuracao.HerancaPadrao));
			a.WriteLine("\t{");
			a.WriteLine();
		}

		private void EscreverFimTabela(StreamWriter a)
		{
			a.WriteLine("\t}");
			a.WriteLine();
		}

		private void EscreverFimNamespace(StreamWriter a)
		{
			a.WriteLine("}");
			a.WriteLine();
		}
	}
}
