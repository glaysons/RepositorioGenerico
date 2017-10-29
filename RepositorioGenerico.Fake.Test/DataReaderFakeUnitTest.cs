using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositorioGenerico.Framework.Helpers;
using RepositorioGenerico.Test.Objetos;

namespace RepositorioGenerico.Fake.Test
{
	[TestClass]
	public class DataReaderFakeUnitTest
	{

		private IList<ObjetoDeTestes> CriarListaParaConsulta()
		{
			return new List<ObjetoDeTestes>
			{
				new ObjetoDeTestes() {Codigo = 1, Nome = "A"},
				new ObjetoDeTestes() {Codigo = 2, Nome = "B"},
				new ObjetoDeTestes() {Codigo = 3, Nome = "C"},
				new ObjetoDeTestes() {Codigo = 4, Nome = "D"},
				new ObjetoDeTestes() {Codigo = 5, Nome = "E"}
			};
		}

		private IDataReader CriarDataReader(IList<ObjetoDeTestes> dados = null)
		{
			var tabela = DataTableHelper.ConverterListaEmDataTable(dados ?? CriarListaParaConsulta());
			return new DataReaderFake(tabela.DefaultView);
		}

		[TestMethod]
		public void SeCriarUmDataReaderNaoDeveGerarErro()
		{

			Action act = () => CriarDataReader();
			act.ShouldNotThrow();

		}

		[TestMethod]
		public void SeCriarUmDataReaderDeveRepresentarUmConjuntoDeDadosPassadoComoParametro()
		{
			var dados = CriarListaParaConsulta();
			var reader = CriarDataReader(dados);

			foreach (var item in dados)
			{
				reader.Read().Should().BeTrue();

				reader.GetName(0).Should().Be("Codigo");
				reader.GetName(1).Should().Be("CodigoNulo");
				reader.GetName(2).Should().Be("Nome");
				reader.GetName(3).Should().Be("Duplo");
				reader.GetName(4).Should().Be("DuploNulo");
				reader.GetName(5).Should().Be("Decimal");
				reader.GetName(6).Should().Be("DecimalNulo");
				reader.GetName(7).Should().Be("Logico");
				reader.GetName(8).Should().Be("DataHora");
				reader.GetName(9).Should().Be("DataHoraNulo");

				Action act = () => reader.GetName(10);
				act.ShouldThrow<IndexOutOfRangeException>();

				reader.FieldCount.Should().Be(10);

				reader.GetFieldType(0).Should().Be(typeof(int));
				reader.GetFieldType(1).Should().Be(typeof(int?));
				reader.GetFieldType(2).Should().Be(typeof(string));
				reader.GetFieldType(3).Should().Be(typeof(double));
				reader.GetFieldType(4).Should().Be(typeof(double?));
				reader.GetFieldType(5).Should().Be(typeof(decimal));
				reader.GetFieldType(6).Should().Be(typeof(decimal?));
				reader.GetFieldType(7).Should().Be(typeof(bool));
				reader.GetFieldType(8).Should().Be(typeof(DateTime));
				reader.GetFieldType(9).Should().Be(typeof(DateTime?));

				reader[0].Should().Be(item.Codigo);
				reader[1].Should().BeNull();
				reader[2].Should().Be(item.Nome);

				reader["Codigo"].Should().Be(item.Codigo);
				reader["CodigoNulo"].Should().Be(item.CodigoNulo);
				reader["Nome"].Should().Be(item.Nome);
				reader["Duplo"].Should().Be(item.Duplo);
				reader["DuploNulo"].Should().Be(item.DuploNulo);
				reader["Decimal"].Should().Be(item.Decimal);
				reader["DecimalNulo"].Should().Be(item.DecimalNulo);
				reader["Logico"].Should().Be(item.Logico);
				reader["DataHora"].Should().Be(item.DataHora);
				reader["DataHoraNulo"].Should().Be(item.DataHoraNulo);
			}

			reader.Read().Should().BeFalse();
		}

		[TestMethod]
		public void AoCriarObjetoDataReaderOMesdoDeveSerDoTipoDbDataReader()
		{
			var reader = CriarDataReader();
			reader.Should().BeAssignableTo<DbDataReader>();
		}

	}
}
