using System.IO;
using ConverterBancoParaEntidades.Estruturas;
using ConverterBancoParaEntidades.Interfaces;
using ConverterBancoParaEntidades.Constantes;
using System.Collections.Generic;

namespace ConverterBancoParaEntidades.Geradores.CSharp
{
	public class GeradorCamposTabela : IGeradorPropriedade
	{

		public void Gerar(string tabela, IList<Campo> campos, StreamWriter arquivo)
		{
			GeradorRegiao.GerarInicio(arquivo, "Estrutura da Tabela");
			GerarPropriedades(tabela, campos, arquivo);
			GeradorRegiao.GerarFim(arquivo);
		}

		private void GerarPropriedades(string tabela, IList<Campo> campos, StreamWriter arquivo)
		{
			foreach (var campo in campos)
			{
				GerarAtributosChave(arquivo, campo);
				GerarAtributosBasicos(arquivo, campo);
				GerarPropriedade(arquivo, campo);
			}
		}

		private void GerarAtributosChave(StreamWriter arquivo, Campo campo)
		{
			if (campo.Chave || campo.Identity || campo.Calculado)
			{
				arquivo.Write("\t\t[");
				if (campo.Chave)
					arquivo.Write("Chave, ");
				arquivo.Write("AutoIncremento(Incremento.");
				if (campo.Identity)
					arquivo.Write("Identity");
				else if (campo.Calculado)
					arquivo.Write("Calculado");
				else
					arquivo.Write("Nenhum");
				arquivo.WriteLine(")]");
			}
		}

		private static void GerarAtributosBasicos(StreamWriter arquivo, Campo campo)
		{
			arquivo.Write("\t\t[Coluna(\"");
			arquivo.Write(campo.NomeCampo);
			arquivo.Write("\", NomeDoTipo = \"");
			arquivo.Write(campo.NomeTipo);
			arquivo.Write("\")");
			if (campo.TamanhoMaximo > 0)
			{
				arquivo.Write(", TamanhoMaximo(");
				arquivo.Write(campo.TamanhoMaximo);
				arquivo.Write(")");
			}
			if (campo.Obrigatorio)
				arquivo.Write(", Obrigatorio");
			arquivo.WriteLine("]");
		}

		private void GerarPropriedade(StreamWriter arquivo, Campo campo)
		{
			arquivo.Write("\t\tpublic ");
			arquivo.Write(ConversorDeTipos.ConsultarTipoDoCampo(campo.TipoInterno));
			if ((!campo.Obrigatorio) && (campo.TipoInterno != TipoCampo.String))
				arquivo.Write("?");
			arquivo.Write(" ");
			arquivo.Write(campo.NomeCampo);
			arquivo.WriteLine(" { get; set; }");
			arquivo.WriteLine();
		}

	}
}
