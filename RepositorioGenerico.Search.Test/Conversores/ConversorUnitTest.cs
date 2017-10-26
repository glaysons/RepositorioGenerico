using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositorioGenerico.Fake;
using RepositorioGenerico.Framework.Helpers;
using RepositorioGenerico.Search.Conversores;
using RepositorioGenerico.Test;

namespace RepositorioGenerico.Search.Test.Conversores
{
	[TestClass]
	public class ConversorUnitTest
	{

		[TestMethod]
		public void SeConverterDataReaderUtilizandoConversorTipadoDeveRetornarUmaListaValida()
		{
			var reader = CriarDataReaderFakeParaTestes();
			var objetosConvertidos = Conversor.ConverterDataReaderParaObjeto<ObjetoDeTestes>(reader);

			objetosConvertidos
				.Should()
				.NotBeNull();

			objetosConvertidos
				.Should()
				.HaveCount(6);

			var n = 1;
			foreach (var objeto in objetosConvertidos)
			{
				objeto.Codigo
					.Should()
					.Be(n);

				objeto.Nome
					.Should()
					.Be("Nome " + n.ToString());

				n++;
			}
		}

		private DataReaderFake CriarDataReaderFakeParaTestes()
		{
			var objetos = CriarListaDeObjetos();
			var dados = DataTableHelper.ConverterListaEmDataTable(objetos).DefaultView;
			var reader = new DataReaderFake(dados);
			return reader;
		}

		private IList<ObjetoDeTestes> CriarListaDeObjetos()
		{
			return new List<ObjetoDeTestes>
			{
				new ObjetoDeTestes() {Codigo = 1, Nome = "Nome 1"},
				new ObjetoDeTestes() {Codigo = 2, Nome = "Nome 2"},
				new ObjetoDeTestes() {Codigo = 3, Nome = "Nome 3"},
				new ObjetoDeTestes() {Codigo = 4, Nome = "Nome 4"},
				new ObjetoDeTestes() {Codigo = 5, Nome = "Nome 5"},
				new ObjetoDeTestes() {Codigo = 6, Nome = "Nome 6"}
			};
		}

		[TestMethod]
		public void SeConverterDataReaderUtilizandoConversorGenericoDeveRetornarUmaListaValida()
		{
			var reader = CriarDataReaderFakeParaTestes();
			var objetosConvertidos = Conversor.ConverterDataReaderParaObjeto(typeof(ObjetoDeTestes), reader);

			objetosConvertidos
				.Should()
				.NotBeNull();

			objetosConvertidos
				.Should()
				.HaveCount(6);

			var n = 1;
			foreach (var objeto in objetosConvertidos)
			{
				objeto
					.Should()
					.BeOfType<ObjetoDeTestes>();

				((ObjetoDeTestes)objeto).Codigo
					.Should()
					.Be(n);

				((ObjetoDeTestes)objeto).Nome
					.Should()
					.Be("Nome " + n.ToString());

				n++;
			}
		}

	}
}
