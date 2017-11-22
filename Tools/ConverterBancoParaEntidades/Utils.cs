using System.Windows.Forms;

namespace ConverterBancoParaEntidades
{
	public static class Utils
	{

		public static void MensagemErro(params object[] mensagem)
		{
			MessageBox.Show(string.Concat(mensagem), Program.TITULOPROGRAMA, 
				MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		public static void MensagemInformacao(params object[] mensagem)
		{
			MessageBox.Show(string.Concat(mensagem), Program.TITULOPROGRAMA, 
				MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		public static bool UsuarioConfirma(params object[] mensagem)
		{
			return (MessageBox.Show(string.Concat(mensagem), Program.TITULOPROGRAMA, 
				MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes);
		}

	}
}
