using System.IO;

namespace ConverterBancoParaEntidades.Geradores.CSharp
{
	public static class GeradorRegiao
	{

		public static void GerarInicio(StreamWriter arquivo, string titulo)
		{
			arquivo.Write("\t\t#region ");
			arquivo.WriteLine(titulo);
			arquivo.WriteLine();
		}

		public static void GerarFim(StreamWriter arquivo)
		{
			arquivo.WriteLine("\t\t#endregion");
			arquivo.WriteLine();
		}

	}
}
