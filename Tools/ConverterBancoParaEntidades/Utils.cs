using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConverterBancoParaEntidades
{
	public static class Utils
	{

		public static void MensagemErro(params object[] mensagem)
		{
			MessageBox.Show(string.Concat(mensagem), Program.TITULOPROGRAMA, MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

	}
}
