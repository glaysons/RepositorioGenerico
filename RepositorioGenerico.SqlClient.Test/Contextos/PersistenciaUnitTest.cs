using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositorioGenerico.Dictionary;
using RepositorioGenerico.Exceptions;
using RepositorioGenerico.SqlClient.Contextos;
using RepositorioGenerico.Test.Objetos;

namespace RepositorioGenerico.SqlClient.Test.Contextos
{
	[TestClass]
	public class PersistenciaUnitTest
	{

		[TestMethod]
		public void SeConsultarUltimoRegistroDeUmNucleoVazioDeveRetornarNulo()
		{
			var persistencia = CriarPersistencia();

			persistencia.ConsultarUltimoRegistro()
				.Should()
				.BeNull();
		}

		private static Persistencia<ObjetoDeTestes> CriarPersistencia()
		{
			var dicionario = new Dicionario(typeof (ObjetoDeTestes));
			var persistencia = new Persistencia<ObjetoDeTestes>(dicionario);
			return persistencia;
		}

		[TestMethod]
		public void SeSalvarPersistenciaSemObjetoDeveGerarErro()
		{
			var contexto = new Contexto(ConnectionStringHelper.Consultar());
			var persistencia = CriarPersistencia();

			Action salvar = () => persistencia.Salvar(contexto, null);

			salvar
				.ShouldThrow<ArgumentNullException>();
		}

		[TestMethod]
		public void SeSalvarPersistenciaComObjetoSemHerancaCorretaDeveGerarErro()
		{
			var contexto = new Contexto(ConnectionStringHelper.Consultar());
			var persistencia = CriarPersistencia();
			var objeto = new ObjetoSemHerancaCorreta();

			Action salvar = () => persistencia.Salvar(contexto, objeto);

			salvar
				.ShouldThrow<TipoDeObjetoInvalidoException>();
		}

		[TestMethod]
		public void SeInserirItensNosDadosDoRepositorioAQuantidadeDeveEstarCorreta()
		{
			var persistencia = CriarPersistencia();

			persistencia.Quantidade
				.Should()
				.Be(0);

			persistencia.Dados.Add(new ObjetoDeTestes());

			persistencia.Quantidade
				.Should()
				.Be(1);
		}

	}
}
