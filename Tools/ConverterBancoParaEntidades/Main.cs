﻿using ConverterBancoParaEntidades.Constantes;
using ConverterBancoParaEntidades.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ConverterBancoParaEntidades
{
	public partial class Main : Form, IConfiguracao
	{
		public string Conexao
		{
			get { return txtConnectionString.Text; }
		}

		public string ScriptConsultaTabelas
		{
			get { return txtScriptConsultaTabela.Text; }
		}

		public string PastaDeDestino
		{
			get { return txtPastaDestino.Text; }
		}

		public Linguagem Linguagem
		{
			get
			{
				if (comboLinguagem.SelectedIndex == 1)
					return Linguagem.VisualBasic;
				return Linguagem.CSharp;
			}
		}

		public string[] Usings
		{
			get { return txtUsings.Lines; }
		}

		public string Namespace
		{
			get { return txtNamespace.Text; }
		}

		public string HerancaPadrao
		{
			get { return txtHerancaPadrao.Text; }
		}

		public string[] Tabelas
		{
			get { return ConsultarTabelasSelecionadas(); }
		}

		private string[] ConsultarTabelasSelecionadas()
		{
			var itens = new List<string>();
			for (int indice = 0; indice < checkTabelas.Items.Count; indice++)
				if (checkTabelas.GetItemChecked(indice))
					itens.Add(checkTabelas.Items[indice].ToString());
			return itens.ToArray();
		}

		private void Salvar()
		{
			Properties.Settings.Default.Save();
		}

		public Main()
		{
			InitializeComponent();
		}

		private void Main_Load(object sender, EventArgs e)
		{
			this.Text = Program.TITULOPROGRAMA;
		}

		private void butConsultarTabelas_Click(object sender, EventArgs e)
		{
			try
			{
				var consultador = Consultadores.Factory.CriarConsultador(this);
				var itens = consultador.Consultar();
				checkTabelas.Items.Clear();
				checkTabelas.Items.AddRange(itens);
			}
			catch (Exception ex)
			{
				Utils.MensagemErro("Não foi possível consultar as tabelas devido ao seguinte erro: ", ex.Message);
			}
		}

		private void linkSelecionarTodas_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			for (int indice = 0; indice < checkTabelas.Items.Count; indice++)
				checkTabelas.SetItemChecked(indice, true);
		}

		private void linkSelecionarNenhuma_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			for (int indice = 0; indice < checkTabelas.Items.Count; indice++)
				checkTabelas.SetItemChecked(indice, false);
		}

		private void linkInverterSelecao_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			for (int indice = 0; indice < checkTabelas.Items.Count; indice++)
				checkTabelas.SetItemChecked(indice, !checkTabelas.GetItemChecked(indice));
		}

		private void txtTrocarPastaDestino_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(PastaDeDestino))
				folderDialog.SelectedPath = PastaDeDestino;
			if (folderDialog.ShowDialog() == DialogResult.OK)
				txtPastaDestino.Text = folderDialog.SelectedPath;
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
