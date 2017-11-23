﻿using System.IO;
using ConverterBancoParaEntidades.Estruturas;
using ConverterBancoParaEntidades.Interfaces;
using ConverterBancoParaEntidades.Constantes;

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
			GerarInicioRegiao(arquivo);
			GerarPropriedades(tabela, arquivo);
			GerarFimRegiao(arquivo);
		}

		private void GerarInicioRegiao(StreamWriter arquivo)
		{
			arquivo.WriteLine("\t\t#region Estrutura da Tabela");
			arquivo.WriteLine();
		}

		private void GerarPropriedades(string tabela, StreamWriter arquivo)
		{
			var campos = _consultador.ConsultarCamposDaTabela(tabela);
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
			arquivo.Write("\t\t[Coluna(Nome = \"");
			arquivo.Write(campo.NomeCampo);
			arquivo.Write("\", NomeDoTipo = \"");
			arquivo.Write(campo.NomeTipo);
			arquivo.Write("\"");
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
			arquivo.Write(ConsultarTipoDoCampo(campo.TipoInterno));
			if ((!campo.Obrigatorio) && (campo.TipoInterno != TipoCampo.String))
				arquivo.Write("?");
			arquivo.Write(" ");
			arquivo.Write(campo.NomeCampo);
			arquivo.Write(" { get; set; }");
			arquivo.WriteLine(";");
			arquivo.WriteLine();
		}

		private string ConsultarTipoDoCampo(TipoCampo tipo)
		{
			switch (tipo)
			{
				case TipoCampo.String: return "string";
				case TipoCampo.Inteiro: return "int";
				case TipoCampo.Boolean: return "bool";
				case TipoCampo.Decimal: return "decimal";
				case TipoCampo.DateTime: return "System.DateTime";
				case TipoCampo.Double: return "double";
				case TipoCampo.Guid: return "System.Guid";
				case TipoCampo.Imagem: return "System.Drawing.Image";
				default:
					return "object";
			}
		}

		private void GerarFimRegiao(StreamWriter arquivo)
		{
			arquivo.WriteLine("\t\t#endregion");
			arquivo.WriteLine();
		}
	}
}
