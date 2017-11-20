using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositorioGenerico.Dictionary.Builders;
using RepositorioGenerico.Dictionary;
using RepositorioGenerico.Test.Objetos;
using FluentAssertions;
using System.Data;

namespace RepositorioGenerico.Test.Dictionary.Builders
{
	[TestClass]
	public class DataTableBuilderUnitTest
	{

		[TestMethod]
		public void SeCriarDataTableDoObjetoNaoDeveGerarErro()
		{
			var dicionario = new Dicionario(typeof(ObjetoDeTestes));
			Action criar = () => DataTableBuilder.CriarDataTable(dicionario);
			criar
				.ShouldNotThrow();
		}

		[TestMethod]
		public void SeCriarDataTableDeveGerarUmDataTableComEstruturaEsperada()
		{
			var dicionario = new Dicionario(typeof(ObjetoDeTestes));
			var tabela = DataTableBuilder.CriarDataTable(dicionario);

			tabela
				.Should()
				.NotBeNull();

			tabela.Columns
				.Should()
				.HaveCount(10, "devem existir 10 colunas!");

			ValidarColuna(tabela.Columns[0], nome: "Codigo", tipo: typeof(int), permiteNulo: false);
			ValidarColuna(tabela.Columns[1], nome: "CodigoNulo", tipo: typeof(int), permiteNulo: true);
			ValidarColuna(tabela.Columns[2], nome: "Nome", tipo: typeof(string), permiteNulo: false, tamanhoMaximo: 50);
			ValidarColuna(tabela.Columns[3], nome: "Duplo", tipo: typeof(double), permiteNulo: false);
			ValidarColuna(tabela.Columns[4], nome: "DuploNulo", tipo: typeof(double), permiteNulo: true);
			ValidarColuna(tabela.Columns[5], nome: "Decimal", tipo: typeof(decimal), permiteNulo: false);
			ValidarColuna(tabela.Columns[6], nome: "DecimalNulo", tipo: typeof(decimal), permiteNulo: true);
			ValidarColuna(tabela.Columns[7], nome: "Logico", tipo: typeof(bool), permiteNulo: false);
			ValidarColuna(tabela.Columns[8], nome: "DataHora", tipo: typeof(DateTime), permiteNulo: false);
			ValidarColuna(tabela.Columns[9], nome: "DataHoraNulo", tipo: typeof(DateTime), permiteNulo: true);
		}

		private void ValidarColuna(DataColumn coluna, string nome, Type tipo, bool permiteNulo, int tamanhoMaximo = 0)
		{
			coluna.ColumnName
				.Should()
				.Be(nome);

			coluna.DataType
				.Should()
				.Be(tipo);

			coluna.AllowDBNull
				.Should()
				.Be(permiteNulo);

			if (tamanhoMaximo > 0)
				coluna.MaxLength
					.Should()
					.Be(tamanhoMaximo);
		}

		[TestMethod]
		public void SeCriarDataTableNaoDevePermitirValoresDuplicados()
		{
			var dicionario = new Dicionario(typeof(ObjetoDeTestes));
			var tabela = DataTableBuilder.CriarDataTable(dicionario);

			var registro = tabela.NewRow();
			registro["Codigo"] = 1;
			registro["Nome"] = "ABC";
			registro["Duplo"] = 123.56;
			registro["Decimal"] = 234.67M;
			registro["Logico"] = true;
			registro["DataHora"] = DateTime.Now;
			tabela.Rows.Add(registro);

			var novoRegistro = tabela.NewRow();
			novoRegistro["Codigo"] = 1;
			novoRegistro["Nome"] = "ABC";
			novoRegistro["Duplo"] = 123.56;
			novoRegistro["Decimal"] = 234.67M;
			novoRegistro["Logico"] = true;
			novoRegistro["DataHora"] = DateTime.Now;

			Action incluir = () => tabela.Rows.Add(novoRegistro);
			incluir
				.ShouldThrow<ConstraintException>();
		}

		[TestMethod]
		public void SeConverterItemEmDataRowDeveGerarUmDataRowEsperado()
		{
			var dicionario = new Dicionario(typeof(ObjetoDeTestes));
			var tabela = DataTableBuilder.CriarDataTable(dicionario);
			var agora = DateTime.Now;

			var objeto = new ObjetoDeTestes()
			{
				Codigo = 1,
				CodigoNulo = 2,
				Nome = "ABC",
				Duplo = 123.56,
				DuploNulo = 123.5,
				Decimal = 234.67M,
				DecimalNulo = 234.6M,
				Logico = true,
				DataHora = agora,
				DataHoraNulo = agora.AddDays(1)
			};

			var registro = DataTableBuilder.ConverterItemEmDataRow(tabela, objeto);

			registro.RowState
				.Should()
				.Be(DataRowState.Detached);

			registro["Codigo"]
				.Should()
				.Be(1);

			registro["CodigoNulo"]
				.Should()
				.Be(2);

			registro["Nome"]
				.Should()
				.Be("ABC");

			registro["Duplo"]
				.Should()
				.Be(123.56);

			registro["DuploNulo"]
				.Should()
				.Be(123.5);

			registro["Decimal"]
				.Should()
				.Be(234.67M);

			registro["DecimalNulo"]
				.Should()
				.Be(234.6M);

			registro["Logico"]
				.Should()
				.Be(true);

			registro["DataHora"]
				.Should()
				.Be(agora);

			registro["DataHoraNulo"]
				.Should()
				.Be(agora.AddDays(1));

		}

	}
}
