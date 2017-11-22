using System;
using System.Windows.Forms;

namespace ConverterBancoParaEntidades
{
	static class Program
	{

		public const string TITULOPROGRAMA = ":: Converter Banco de Dados em Entidades ::";

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Main());
		}
	}
}
