using ConverterBancoParaEntidades.Interfaces;
using System;
using System.Linq;

namespace ConverterBancoParaEntidades.Geradores
{
	public static class Factory
	{

		public static IGerador CriarGerador(IConfiguracao configuracao, IConsultador consultador)
		{
			Validar(configuracao, consultador);
			if (configuracao.Linguagem == Constantes.Linguagem.CSharp)
				return new CSharp.Gerador(configuracao, consultador);
			throw new NotImplementedException();
		}

		private static void Validar(IConfiguracao configuracao, IConsultador consultador)
		{
			if (consultador == null)
				throw new Exception("Favor consultar as tabelas para geração dos arquivos!");
			if (string.IsNullOrEmpty(configuracao.Namespace))
				throw new Exception("Favor informar um namespace válido para geração dos arquivos!");
			if ((string.IsNullOrEmpty(configuracao.PastaDeDestino)) || (!System.IO.Directory.Exists(configuracao.PastaDeDestino)))
				throw new Exception("Favor informar uma pasta de destino para geração dos arquivos!");
			if (string.IsNullOrEmpty(configuracao.HerancaPadrao))
				throw new Exception("Favor informar uma herança básica para geração dos arquivos! ");
			if ((configuracao.Tabelas == null) || (configuracao.Tabelas.Count(t => !string.IsNullOrEmpty(t)) == 0))
				throw new Exception("Favor selecionar ao menos uma tabela para geração dos arquivos!");
		}

	}
}
