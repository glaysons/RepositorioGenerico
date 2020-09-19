using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RepositorioGenerico.Entities;
using RepositorioGenerico.Framework;
using RepositorioGenerico.Pattern;
using RepositorioGenerico.Pattern.Contextos;

namespace RepositorioGenerico.Test.Framework
{
	[TestClass]
	public class HistoricoTransacionalUnitTest
	{

		[TestMethod]
		public void SeCriarUmHistoricoTransacionalNaoDeveGerarErro()
		{
			var conexao = new Mock<IConexao>();
			Action act = () => new HistoricoTransacional(conexao.Object);
			act.Should().NotThrow();
		}

		[TestMethod]
		public void AoAdicionarTransacaoAMesmaDeveSerAdicionadaAoLog()
		{
			var historico = CriarHistoricoTransacional();
			var persistencia = Mock.Of<IPersistencia>();
			var registro = new Entidade();

			historico.Quantidade.Should().Be(0);

			historico.AdicionarTransacao(persistencia, registro);

			historico.Quantidade.Should().Be(1);
			historico[0].Persistencia.Should().Be(persistencia);
			historico[0].Registro.Should().Be(registro);
		}

		private static HistoricoTransacional CriarHistoricoTransacional()
		{
			return new HistoricoTransacional(Mock.Of<IConexao>());
		}

		[TestMethod]
		public void AoAcessarUmIndiceInvalidoDeveGerarErroIndexOutOfRangeException()
		{
			var historico = CriarHistoricoTransacional();
			historico.AdicionarTransacao(Mock.Of<IPersistencia>(), new Entidade());

			ItemHistoricoTransacional item = null;

			Action<int> acesso = indice => item = historico[indice];

			int[] enviarIndice = { 0 };
			Action act = () => acesso(enviarIndice[0]);

			enviarIndice[0] = -1;
			act.Should().Throw<IndexOutOfRangeException>();

			enviarIndice[0] = 1;
			act.Should().Throw<IndexOutOfRangeException>();

			item.Should().BeNull();
		}

		[TestMethod]
		public void AoSalvarOHistoricoDeTransacoesDeveSalvarItensNaOrdem()
		{
			var historico = CriarHistoricoTransacional();
			var mocks = CriarMockDeItensHistoricoTransacional().ToList();
	
			var registros = new List<Entidade>()
			{
				new Entidade() {EstadoEntidade = EstadosEntidade.Excluido},
				new Entidade() {EstadoEntidade = EstadosEntidade.Novo},
				new Entidade() {EstadoEntidade = EstadosEntidade.NaoModificado},
				new Entidade() {EstadoEntidade = EstadosEntidade.Modificado}
			};

			mocks.Count
				.Should().Be(registros.Count, "Porque os registros e os Mocks devem ter a mesma quantidade!");

			for (var n = 0; n < mocks.Count; n++)
				historico.AdicionarTransacao(mocks[n].Object, registros[n]);

			historico.Quantidade.Should().Be(4);

			historico.Salvar();

			historico.Quantidade.Should().Be(4);

			historico.Limpar();

			historico.Quantidade.Should().Be(0);

			for (var n = 0; n < mocks.Count; n++)
			{
				var mock = mocks[n];
				var registro = registros[n];
				mock.Verify(p => p.Salvar(It.IsAny<IConexao>(), registro));
			}
		}

		private IEnumerable<Mock<IPersistencia>> CriarMockDeItensHistoricoTransacional()
		{
			for (var n = 0; n < 4; n++)
			{
				var mockPersistencia = new Mock<IPersistencia>();
				mockPersistencia.Setup(p => p.Salvar(It.IsAny<IConexao>(), It.IsAny<Entidade>())).Verifiable();
				yield return mockPersistencia;
			}
		}
	}
}
