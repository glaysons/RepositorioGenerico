using System;
using System.Windows.Forms;

namespace ConverterBancoParaEntidades
{
	public partial class Main : Form
	{

		private void Salvar()
		{
			Properties.Settings.Default.Save();
		}

		public Main()
		{
			InitializeComponent();
		}

		private void butConsultarTabelas_Click(object sender, EventArgs e)
		{

		}

		private void linkSelecionarTodas_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{

		}

		private void linkSelecionarNenhuma_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{

		}

		private void linkInverterSelecao_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{

		}

		private void txtTrocarPastaDestino_Click(object sender, EventArgs e)
		{

		}

		private void butGerar_Click(object sender, EventArgs e)
		{
			Salvar();
		}

		private void butSair_Click(object sender, EventArgs e)
		{
			Salvar();
			Application.Exit();
		}

	}
}
