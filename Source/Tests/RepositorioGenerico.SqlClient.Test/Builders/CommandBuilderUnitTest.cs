using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositorioGenerico.Dictionary;
using RepositorioGenerico.Entities.Anotacoes;
using RepositorioGenerico.Framework.Helpers;
using RepositorioGenerico.SqlClient.Builders;
using RepositorioGenerico.Test.Objetos;

namespace RepositorioGenerico.SqlClient.Test.Builders
{
	[TestClass]
	public class CommandBuilderUnitTest
	{

		[TestMethod]
		public void SeDefinirParametrosParaTodosOsCamposTodosOsParametrosDevemSerDefinidos()
		{
			var comando = new SqlCommand();
			var dicionario = new Dicionario(typeof(ObjetoDeTestes));

			CommandBuilder.DefinirParametrosParaTodosOsCampos(dicionario, comando);

			comando.Parameters.Count
				.Should()
				.Be(dicionario.QuantidadeCampos);

			foreach (var itemDicionario in dicionario.Itens)
			{
				var parametro = comando.Parameters["@p" + itemDicionario.Id.ToString()];

				parametro
					.Should()
					.NotBeNull();

				parametro.DbType
					.Should()
					.Be(itemDicionario.TipoBanco);
			}
		}

		[TestMethod]
		public void SeDefinirParametrosParaTodosOsCamposDaChaveApenasCamposChaveDevemSerDefinidos()
		{
			var comando = new SqlCommand();
			var dicionario = new Dicionario(typeof(ObjetoDeTestes));

			CommandBuilder.DefinirParametrosParaTodosOsCamposDaChave(dicionario, comando);

			comando.Parameters.Count
				.Should()
				.Be(dicionario.QuantidadeCamposNaChave);

			foreach (var itemDicionario in dicionario.ConsultarCamposChave())
			{
				var parametro = comando.Parameters["@p" + itemDicionario.Id.ToString()];

				parametro
					.Should()
					.NotBeNull();

				parametro.DbType
					.Should()
					.Be(itemDicionario.TipoBanco);
			}
		}

		[TestMethod]
		public void SeDefinirParametrosParaCamposDaChaveQueNaoSaoAutoIncrementoApenasCamposChaveQueNaoSaoAutoIncrementoDevemSerDefinidos()
		{
			var comando = new SqlCommand();
			var dicionario = new Dicionario(typeof(ObjetoComChaveDupla));

			CommandBuilder.DefinirParametrosParaCamposDaChaveQueNaoSaoAutoIncremento(dicionario, comando);

			comando.Parameters.Count
				.Should()
				.Be(dicionario.QuantidadeCamposNaChave - 1);

			foreach (var itemDicionario in dicionario.ConsultarCamposChave())
			{
				if (!itemDicionario.Chave || itemDicionario.OpcaoGeracao != Incremento.Nenhum)
					continue;

				var parametro = comando.Parameters["@p" + itemDicionario.Id.ToString()];

				parametro
					.Should()
					.NotBeNull();

				parametro.DbType
					.Should()
					.Be(itemDicionario.TipoBanco);
			}
		}

		[TestMethod]
		public void SeSincronizarParametroEmComandoSemOParametroDefinidoDeveGerarArgumentException()
		{
			var comando = new SqlCommand();
			var dicionario = new Dicionario(typeof(ObjetoComChaveDupla));

			var objeto = new ObjetoComChaveDupla()
			{
				ChaveBase = 123,
				Nome = "ABC"
			};

			Action sincronizar = () => CommandBuilder.SincronizarParametrosDeTodosOsCampos(dicionario, comando, objeto);

			sincronizar
				.Should().Throw<IndexOutOfRangeException>();

		}

		[TestMethod]
		public void SeSincronizarParametrosDosCamposChaveQueNaoSaoAutoIncrementoDeveDefinirValoresCorretamente()
		{
			var comando = new SqlCommand();
			var dicionario = new Dicionario(typeof(ObjetoComChaveDupla));

			CommandBuilder.DefinirParametrosParaCamposDaChaveQueNaoSaoAutoIncremento(dicionario, comando);

			var objeto = new ObjetoComChaveDupla()
			{
				ChaveBase = 123,
				Nome = "ABC"
			};

			CommandBuilder.SincronizarParametrosDosCamposChaveQueNaoSaoAutoIncremento(dicionario, comando, objeto);

			comando.Parameters["@p0"].Value
				.Should()
				.Be(123);

		}

		[TestMethod]
		public void SeSincronizarParametrosDeTodosOsCamposDeveDefinirValoresCorretamente()
		{
			var comando = new SqlCommand();
			var dicionario = new Dicionario(typeof(ObjetoComChaveDupla));

			CommandBuilder.DefinirParametrosParaTodosOsCampos(dicionario, comando);

			var objeto = new ObjetoComChaveDupla()
			{
				ChaveBase = 123,
				Nome = "ABC"
			};

			CommandBuilder.SincronizarParametrosDeTodosOsCampos(dicionario, comando, objeto);

			comando.Parameters["@p0"].Value
				.Should()
				.Be(123);

			comando.Parameters["@p2"].Value
				.Should()
				.Be("ABC");

		}

		[TestMethod]
		public void SeSincronizarParametrosDosCamposChaveDeveDefinirValoresCorretamente()
		{
			var comando = new SqlCommand();
			var dicionario = new Dicionario(typeof(ObjetoComChaveDupla));

			CommandBuilder.DefinirParametrosParaTodosOsCamposDaChave(dicionario, comando);

			var objeto = new ObjetoComChaveDupla()
			{
				ChaveBase = 123,
				Nome = "ABC"
			};

			CommandBuilder.SincronizarParametrosDosCamposChave(dicionario, comando, objeto);

			comando.Parameters["@p0"].Value
				.Should()
				.Be(123);

		}

		[TestMethod]
		public void SeSincronizarValoresDePropriedadesEnumOValorDoGetHashCodeQueDeveSerSincronizado()
		{
			var comando = new SqlCommand();
			var dicionario = new Dicionario(typeof(NetoDoObjetoDeTestes));

			CommandBuilder.DefinirParametrosParaTodosOsCampos(dicionario, comando);

			var objeto = new NetoDoObjetoDeTestes()
			{
				CodigoFilho = 123,
				NomeNeto = "ABC",
				Opcao = EnumDeTestes.Opcao3
			};

			CommandBuilder.SincronizarParametrosDeTodosOsCampos(dicionario, comando, objeto);

			var campo = dicionario.ConsultarPorPropriedade("Opcao");
			comando.Parameters["@p" + campo.Id.ToString()].Value
				.Should()
				.Be(3);
		}

		[TestMethod]
		public void SeSincronizarValoresDePropriedadesEnumOValorDoDefaultValueQueDeveSerSincronizado()
		{
			var comando = new SqlCommand();
			var dicionario = new Dicionario(typeof(NetoDoObjetoDeTestes));

			CommandBuilder.DefinirParametrosParaTodosOsCampos(dicionario, comando);

			var objeto = new NetoDoObjetoDeTestes()
			{
				CodigoFilho = 123,
				NomeNeto = "ABC",
				Letra = EnumDeStrings.OpcaoC
			};

			CommandBuilder.SincronizarParametrosDeTodosOsCampos(dicionario, comando, objeto);

			var campoNome = dicionario.ConsultarPorPropriedade("Letra");
			comando.Parameters["@p" + campoNome.Id.ToString()].Value
				.Should()
				.Be("C");
		}

		[TestMethod]
		public void SeSincronizarValoresDePropriedadesEnumSemDefaultValueOEnumQueDeveSerSincronizado()
		{
			var comando = new SqlCommand();
			var dicionario = new Dicionario(typeof(NetoDoObjetoDeTestes));

			CommandBuilder.DefinirParametrosParaTodosOsCampos(dicionario, comando);

			var objeto = new NetoDoObjetoDeTestes()
			{
				CodigoFilho = 123,
				NomeNeto = "ABC",
				Letra = EnumDeStrings.SemOpcao
			};

			CommandBuilder.SincronizarParametrosDeTodosOsCampos(dicionario, comando, objeto);

			var campo = dicionario.ConsultarPorPropriedade("Letra");
			comando.Parameters["@p" + campo.Id.ToString()].Value
				.Should()
				.Be("SemOpcao");
		}

		[TestMethod]
		public void SeSincronizarParametrosDeTodosOsCamposDeUmObjetoNuloDeveZerarTodosOsParametros()
		{
			var comando = new SqlCommand();
			var dicionario = new Dicionario(typeof(NetoDoObjetoDeTestes));

			CommandBuilder.DefinirParametrosParaTodosOsCampos(dicionario, comando);

			NetoDoObjetoDeTestes objeto = null;

			CommandBuilder.SincronizarParametrosDeTodosOsCampos(dicionario, comando, objeto);

			foreach (var item in dicionario.Itens)
				comando.Parameters["@p" + item.Id.ToString()].Value.Should().Be(DBNull.Value);
		}

		[TestMethod]
		public void SeSincronizarParametrosDosCamposChaveQueNaoSaoAutoIncrementoComBaseNumDatarowDeveDefinirValoresCorretamente()
		{
			var comando = new SqlCommand();
			var dicionario = new Dicionario(typeof(ObjetoComChaveDupla));

			CommandBuilder.DefinirParametrosParaCamposDaChaveQueNaoSaoAutoIncremento(dicionario, comando);

			var registro = ConsultarRegistro();

			CommandBuilder.SincronizarParametrosDosCamposChaveQueNaoSaoAutoIncremento(dicionario, comando, registro);

			comando.Parameters["@p0"].Value
				.Should()
				.Be(123);

		}

		private DataRow ConsultarRegistro()
		{
			return DataTableHelper.ConverterListaEmDataTable(new List<ObjetoComChaveDupla>()
			{
				new ObjetoComChaveDupla()
				{
					ChaveBase = 123,
					Nome = "ABC"
				}
			}).Rows[0];
		}

		[TestMethod]
		public void SeSincronizarParametrosDeTodosOsCamposComBaseNumDatarowDeveDefinirValoresCorretamente()
		{
			var comando = new SqlCommand();
			var dicionario = new Dicionario(typeof(ObjetoComChaveDupla));

			CommandBuilder.DefinirParametrosParaTodosOsCampos(dicionario, comando);

			var registro = ConsultarRegistro();

			CommandBuilder.SincronizarParametrosDeTodosOsCampos(dicionario, comando, registro);

			comando.Parameters["@p0"].Value
				.Should()
				.Be(123);

			comando.Parameters["@p2"].Value
				.Should()
				.Be("ABC");

		}

		[TestMethod]
		public void SeSincronizarParametrosDosCamposChaveComBaseNumDatarowDeveDefinirValoresCorretamente()
		{
			var comando = new SqlCommand();
			var dicionario = new Dicionario(typeof(ObjetoComChaveDupla));

			CommandBuilder.DefinirParametrosParaTodosOsCamposDaChave(dicionario, comando);

			var registro = ConsultarRegistro();

			CommandBuilder.SincronizarParametrosDosCamposChave(dicionario, comando, registro);

			comando.Parameters["@p0"].Value
				.Should()
				.Be(123);

		}

	}
}
