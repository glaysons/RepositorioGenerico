namespace ConverterBancoParaEntidades
{
	partial class Main
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.checkTabelas = new System.Windows.Forms.CheckedListBox();
			this.linkSelecionarTodas = new System.Windows.Forms.LinkLabel();
			this.linkSelecionarNenhuma = new System.Windows.Forms.LinkLabel();
			this.linkInverterSelecao = new System.Windows.Forms.LinkLabel();
			this.butConsultarTabelas = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.comboLinguagem = new System.Windows.Forms.ComboBox();
			this.label6 = new System.Windows.Forms.Label();
			this.txtTrocarPastaDestino = new System.Windows.Forms.Button();
			this.folderDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.butGerar = new System.Windows.Forms.Button();
			this.butSair = new System.Windows.Forms.Button();
			this.txtPastaDestino = new System.Windows.Forms.TextBox();
			this.txtNamespace = new System.Windows.Forms.TextBox();
			this.txtUsings = new System.Windows.Forms.TextBox();
			this.txtScriptConsultaTabela = new System.Windows.Forms.TextBox();
			this.txtConnectionString = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.txtHerancaPadrao = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(189, 21);
			this.label1.TabIndex = 0;
			this.label1.Text = "Conexão SQL Server:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 64);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(74, 21);
			this.label2.TabIndex = 0;
			this.label2.Text = "Tabelas:";
			// 
			// checkTabelas
			// 
			this.checkTabelas.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.checkTabelas.Font = new System.Drawing.Font("Courier New", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.checkTabelas.FormattingEnabled = true;
			this.checkTabelas.Location = new System.Drawing.Point(16, 235);
			this.checkTabelas.Name = "checkTabelas";
			this.checkTabelas.Size = new System.Drawing.Size(228, 251);
			this.checkTabelas.TabIndex = 5;
			// 
			// linkSelecionarTodas
			// 
			this.linkSelecionarTodas.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.linkSelecionarTodas.AutoSize = true;
			this.linkSelecionarTodas.Location = new System.Drawing.Point(12, 502);
			this.linkSelecionarTodas.Name = "linkSelecionarTodas";
			this.linkSelecionarTodas.Size = new System.Drawing.Size(55, 21);
			this.linkSelecionarTodas.TabIndex = 6;
			this.linkSelecionarTodas.TabStop = true;
			this.linkSelecionarTodas.Text = "Todas";
			this.linkSelecionarTodas.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkSelecionarTodas_LinkClicked);
			// 
			// linkSelecionarNenhuma
			// 
			this.linkSelecionarNenhuma.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.linkSelecionarNenhuma.AutoSize = true;
			this.linkSelecionarNenhuma.Location = new System.Drawing.Point(73, 502);
			this.linkSelecionarNenhuma.Name = "linkSelecionarNenhuma";
			this.linkSelecionarNenhuma.Size = new System.Drawing.Size(80, 21);
			this.linkSelecionarNenhuma.TabIndex = 7;
			this.linkSelecionarNenhuma.TabStop = true;
			this.linkSelecionarNenhuma.Text = "Nenhuma";
			this.linkSelecionarNenhuma.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkSelecionarNenhuma_LinkClicked);
			// 
			// linkInverterSelecao
			// 
			this.linkInverterSelecao.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.linkInverterSelecao.AutoSize = true;
			this.linkInverterSelecao.Location = new System.Drawing.Point(159, 502);
			this.linkInverterSelecao.Name = "linkInverterSelecao";
			this.linkInverterSelecao.Size = new System.Drawing.Size(69, 21);
			this.linkInverterSelecao.TabIndex = 8;
			this.linkInverterSelecao.TabStop = true;
			this.linkInverterSelecao.Text = "Inverter";
			this.linkInverterSelecao.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkInverterSelecao_LinkClicked);
			// 
			// butConsultarTabelas
			// 
			this.butConsultarTabelas.BackColor = System.Drawing.Color.Silver;
			this.butConsultarTabelas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.butConsultarTabelas.Font = new System.Drawing.Font("Courier New", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.butConsultarTabelas.Location = new System.Drawing.Point(16, 199);
			this.butConsultarTabelas.Name = "butConsultarTabelas";
			this.butConsultarTabelas.Size = new System.Drawing.Size(228, 30);
			this.butConsultarTabelas.TabIndex = 4;
			this.butConsultarTabelas.Text = "Consultar Tabelas";
			this.butConsultarTabelas.UseVisualStyleBackColor = false;
			this.butConsultarTabelas.Click += new System.EventHandler(this.butConsultarTabelas_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(246, 64);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(65, 21);
			this.label3.TabIndex = 0;
			this.label3.Text = "Usings:";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(250, 208);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(102, 21);
			this.label4.TabIndex = 0;
			this.label4.Text = "Namespace:";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(250, 262);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(96, 21);
			this.label5.TabIndex = 0;
			this.label5.Text = "Linguagem:";
			// 
			// comboLinguagem
			// 
			this.comboLinguagem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboLinguagem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboLinguagem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.comboLinguagem.FormattingEnabled = true;
			this.comboLinguagem.Items.AddRange(new object[] {
            "C#"});
			this.comboLinguagem.Location = new System.Drawing.Point(254, 286);
			this.comboLinguagem.Name = "comboLinguagem";
			this.comboLinguagem.Size = new System.Drawing.Size(532, 29);
			this.comboLinguagem.TabIndex = 15;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(250, 318);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(142, 21);
			this.label6.TabIndex = 0;
			this.label6.Text = "Pasta de Destino:";
			// 
			// txtTrocarPastaDestino
			// 
			this.txtTrocarPastaDestino.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.txtTrocarPastaDestino.BackColor = System.Drawing.Color.Silver;
			this.txtTrocarPastaDestino.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.txtTrocarPastaDestino.Font = new System.Drawing.Font("Courier New", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtTrocarPastaDestino.Location = new System.Drawing.Point(711, 342);
			this.txtTrocarPastaDestino.Name = "txtTrocarPastaDestino";
			this.txtTrocarPastaDestino.Size = new System.Drawing.Size(75, 27);
			this.txtTrocarPastaDestino.TabIndex = 8;
			this.txtTrocarPastaDestino.Text = "Trocar";
			this.txtTrocarPastaDestino.UseVisualStyleBackColor = false;
			this.txtTrocarPastaDestino.Click += new System.EventHandler(this.txtTrocarPastaDestino_Click);
			// 
			// butGerar
			// 
			this.butGerar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butGerar.BackColor = System.Drawing.Color.Silver;
			this.butGerar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.butGerar.Font = new System.Drawing.Font("Courier New", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.butGerar.Location = new System.Drawing.Point(534, 480);
			this.butGerar.Name = "butGerar";
			this.butGerar.Size = new System.Drawing.Size(123, 40);
			this.butGerar.TabIndex = 9;
			this.butGerar.Text = "Gerar";
			this.butGerar.UseVisualStyleBackColor = false;
			this.butGerar.Click += new System.EventHandler(this.butGerar_Click);
			// 
			// butSair
			// 
			this.butSair.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butSair.BackColor = System.Drawing.Color.Silver;
			this.butSair.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.butSair.Font = new System.Drawing.Font("Courier New", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.butSair.Location = new System.Drawing.Point(663, 480);
			this.butSair.Name = "butSair";
			this.butSair.Size = new System.Drawing.Size(123, 40);
			this.butSair.TabIndex = 10;
			this.butSair.Text = "Sair";
			this.butSair.UseVisualStyleBackColor = false;
			this.butSair.Click += new System.EventHandler(this.butSair_Click);
			// 
			// txtPastaDestino
			// 
			this.txtPastaDestino.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtPastaDestino.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ConverterBancoParaEntidades.Properties.Settings.Default, "PastaDestino", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.txtPastaDestino.Font = new System.Drawing.Font("Courier New", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtPastaDestino.Location = new System.Drawing.Point(254, 342);
			this.txtPastaDestino.Name = "txtPastaDestino";
			this.txtPastaDestino.Size = new System.Drawing.Size(451, 27);
			this.txtPastaDestino.TabIndex = 7;
			this.txtPastaDestino.Text = global::ConverterBancoParaEntidades.Properties.Settings.Default.PastaDestino;
			// 
			// txtNamespace
			// 
			this.txtNamespace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtNamespace.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ConverterBancoParaEntidades.Properties.Settings.Default, "Namespace", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.txtNamespace.Font = new System.Drawing.Font("Courier New", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtNamespace.Location = new System.Drawing.Point(254, 235);
			this.txtNamespace.Name = "txtNamespace";
			this.txtNamespace.Size = new System.Drawing.Size(532, 27);
			this.txtNamespace.TabIndex = 6;
			this.txtNamespace.Text = global::ConverterBancoParaEntidades.Properties.Settings.Default.Namespace;
			// 
			// txtUsings
			// 
			this.txtUsings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtUsings.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ConverterBancoParaEntidades.Properties.Settings.Default, "Usings", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.txtUsings.Font = new System.Drawing.Font("Courier New", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtUsings.Location = new System.Drawing.Point(250, 88);
			this.txtUsings.Multiline = true;
			this.txtUsings.Name = "txtUsings";
			this.txtUsings.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtUsings.Size = new System.Drawing.Size(536, 105);
			this.txtUsings.TabIndex = 3;
			this.txtUsings.Text = global::ConverterBancoParaEntidades.Properties.Settings.Default.Usings;
			this.txtUsings.WordWrap = false;
			// 
			// txtScriptConsultaTabela
			// 
			this.txtScriptConsultaTabela.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ConverterBancoParaEntidades.Properties.Settings.Default, "ScriptConsultaTabelas", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.txtScriptConsultaTabela.Font = new System.Drawing.Font("Courier New", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtScriptConsultaTabela.Location = new System.Drawing.Point(16, 88);
			this.txtScriptConsultaTabela.Multiline = true;
			this.txtScriptConsultaTabela.Name = "txtScriptConsultaTabela";
			this.txtScriptConsultaTabela.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtScriptConsultaTabela.Size = new System.Drawing.Size(228, 105);
			this.txtScriptConsultaTabela.TabIndex = 2;
			this.txtScriptConsultaTabela.Text = global::ConverterBancoParaEntidades.Properties.Settings.Default.ScriptConsultaTabelas;
			this.txtScriptConsultaTabela.WordWrap = false;
			// 
			// txtConnectionString
			// 
			this.txtConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtConnectionString.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ConverterBancoParaEntidades.Properties.Settings.Default, "Conexao", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.txtConnectionString.Font = new System.Drawing.Font("Courier New", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtConnectionString.Location = new System.Drawing.Point(16, 31);
			this.txtConnectionString.Name = "txtConnectionString";
			this.txtConnectionString.Size = new System.Drawing.Size(770, 27);
			this.txtConnectionString.TabIndex = 1;
			this.txtConnectionString.Text = global::ConverterBancoParaEntidades.Properties.Settings.Default.Conexao;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(250, 382);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(133, 21);
			this.label7.TabIndex = 16;
			this.label7.Text = "Herança Padrão:";
			// 
			// txtHerancaPadrao
			// 
			this.txtHerancaPadrao.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtHerancaPadrao.Font = new System.Drawing.Font("Courier New", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtHerancaPadrao.Location = new System.Drawing.Point(254, 406);
			this.txtHerancaPadrao.Name = "txtHerancaPadrao";
			this.txtHerancaPadrao.Size = new System.Drawing.Size(532, 27);
			this.txtHerancaPadrao.TabIndex = 17;
			this.txtHerancaPadrao.Text = "Entidade";
			// 
			// Main
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(798, 532);
			this.Controls.Add(this.txtHerancaPadrao);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.butSair);
			this.Controls.Add(this.butGerar);
			this.Controls.Add(this.txtTrocarPastaDestino);
			this.Controls.Add(this.txtPastaDestino);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.comboLinguagem);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.txtNamespace);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.txtUsings);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.butConsultarTabelas);
			this.Controls.Add(this.linkInverterSelecao);
			this.Controls.Add(this.linkSelecionarNenhuma);
			this.Controls.Add(this.linkSelecionarTodas);
			this.Controls.Add(this.checkTabelas);
			this.Controls.Add(this.txtScriptConsultaTabela);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtConnectionString);
			this.Controls.Add(this.label1);
			this.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "Main";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = ":: Converter Banco de Dados em Entidades ::";
			this.Load += new System.EventHandler(this.Main_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtConnectionString;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtScriptConsultaTabela;
		private System.Windows.Forms.CheckedListBox checkTabelas;
		private System.Windows.Forms.LinkLabel linkSelecionarTodas;
		private System.Windows.Forms.LinkLabel linkSelecionarNenhuma;
		private System.Windows.Forms.LinkLabel linkInverterSelecao;
		private System.Windows.Forms.Button butConsultarTabelas;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtUsings;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtNamespace;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ComboBox comboLinguagem;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox txtPastaDestino;
		private System.Windows.Forms.Button txtTrocarPastaDestino;
		private System.Windows.Forms.FolderBrowserDialog folderDialog;
		private System.Windows.Forms.Button butGerar;
		private System.Windows.Forms.Button butSair;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox txtHerancaPadrao;
	}
}

