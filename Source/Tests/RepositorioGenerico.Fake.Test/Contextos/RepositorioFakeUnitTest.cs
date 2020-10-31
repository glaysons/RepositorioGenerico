using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositorioGenerico.Entities;
using RepositorioGenerico.Fake.Contextos;
using RepositorioGenerico.Test.Objetos;

namespace RepositorioGenerico.Fake.Test.Contextos
{
	[TestClass]
	public class RepositorioFakeUnitTest
	{

		[TestMethod]
		public void SeConsultarPelaChaveDeUmRegistroDeveRetornarUmRegistroValido()
		{
			var repositorio = CriarRepositorioComDados();
			var objeto = repositorio.Consultar(3);

			objeto
				.Should()
				.NotBeNull();

			objeto.Nome
				.Should()
				.Be("C");
		}

		private RepositorioFake<ObjetoDeTestes> CriarRepositorioComDados(bool validar = false, IContextoFake contextoFixo = null)
		{
			var contexto = contextoFixo ?? FabricaFake.CriarContexto();
			var repositorio = (RepositorioFake<ObjetoDeTestes>)contexto.Repositorio<ObjetoDeTestes>();
			repositorio.DesativarValidacoes();
			GerarRegistrosDoObjetoDeTestesNoContexto(contexto);
			if (validar)
				repositorio.AtivarValidacoes();
			return repositorio;
		}

		private static void GerarRegistrosDoObjetoDeTestesNoContexto(IContextoFake contexto)
		{
			contexto.AdicionarRegistro(new ObjetoDeTestes() {Codigo = 1, Nome = "A", Logico = true});
			contexto.AdicionarRegistro(new ObjetoDeTestes() {Codigo = 2, Nome = "B", Logico = false});
			contexto.AdicionarRegistro(new ObjetoDeTestes() {Codigo = 3, Nome = "C", Logico = false});
			contexto.AdicionarRegistro(new ObjetoDeTestes() {Codigo = 4, Nome = "D", Logico = true});
			contexto.AdicionarRegistro(new ObjetoDeTestes() {Codigo = 5, Nome = "E", Logico = false});
			contexto.AdicionarRegistro(new ObjetoDeTestes() {Codigo = 6, Nome = "F", Logico = false});
		}

		[TestMethod]
		public void SeExcluirUmItemOMesmoNaoDeveraSerConsultado()
		{
			var contexto = FabricaFake.CriarContexto();
			var repositorio = CriarRepositorioComDados(contextoFixo: contexto);

			repositorio.Quantidade
				.Should()
				.Be(0);

			repositorio.Buscar.Todos()
				.Should()
				.HaveCount(6);

			var registro = repositorio.Consultar(3);
			repositorio.Excluir(registro);

			repositorio.Quantidade
				.Should()
				.Be(0);

			contexto.Salvar();

			repositorio.Buscar.Todos()
				.Should()
				.HaveCount(5);

		}

		[TestMethod]
		public void SeCriarNovoObjetoDeveGerarObjetoDoTipoCorreto()
		{
			var repositorio = CriarRepositorioComDados();
			var novo = repositorio.Criar();

			novo
				.Should()
				.NotBeNull();

			novo
				.Should()
				.BeOfType<ObjetoDeTestes>();

			novo.Logico
				.Should()
				.BeTrue();

			novo.Duplo
				.Should()
				.Be(123.45);

			novo.Filhos
				.Should()
				.BeNull();

			novo.EstadoEntidade
				.Should()
				.Be(EstadosEntidade.Novo);

			repositorio.Quantidade
				.Should()
				.Be(0);
		}

		[TestMethod]
		public void SeValidarUmObjetoInvalidoAntesDeInserirDeveRetornarListaDeErrosValida()
		{
			var repositorio = CriarRepositorioComDados();

			var novo = new ObjetoDeTestes
			{
				Nome = "Pequeno%"
			};

			var valido = repositorio.Valido(novo).ToList();

			valido
				.Should()
				.NotBeNull();

			valido.Count.Should().BeGreaterThan(1);

			valido
				.Should()
				.Contain("Favor informar no mínimo 10 caracteres para o nome!");
		}

		[TestMethod]
		public void SeInserirUmObjetoInvalidoDeveGerarErroEaQuantidadeDeRegistrosNaoDeveSerModificada()
		{
			var repositorio = CriarRepositorioComDados(validar: true);

			var novo = new ObjetoDeTestes
			{
				Nome = "Pequeno"
			};

			Action insercao = () => repositorio.Inserir(novo);

			insercao
				.Should().Throw<Exception>()
				.WithMessage("Favor informar no mínimo 10 caracteres para o nome!");

			repositorio.Quantidade
				.Should()
				.Be(0);

			Action validacao = () => repositorio.Validar(novo);

			validacao
				.Should().Throw<Exception>()
				.WithMessage("Favor informar no mínimo 10 caracteres para o nome!");

		}

		[TestMethod]
		public void SeValidarUmObjetoValidoAntesDeInserirDeveRetornarListaDeErrosVazia()
		{
			var repositorio = CriarRepositorioComDados();

			var novo = new ObjetoDeTestes
			{
				Nome = "Nome Considerado Correto"
			};

			var valido = repositorio.Valido(novo).ToList();

			valido
				.Should()
				.NotBeNull();

			valido.Count
				.Should()
				.Be(0);
		}

		[TestMethod]
		public void SeInserirUmObjetoValidoNaoDeveGerarErro()
		{
			var repositorio = CriarRepositorioComDados(validar: true);

			repositorio.Quantidade
				.Should()
				.Be(0);

			var novo = new ObjetoDeTestes
			{
				Nome = "Nomes Corretos"
			};

			Action insercao = () => repositorio.Inserir(novo);

			insercao
				.Should().NotThrow();

			Action validacao = () => repositorio.Validar(novo);

			validacao
				.Should().NotThrow();

			repositorio.Quantidade
				.Should()
				.Be(1);
		}

		[TestMethod]
		public void SeAtivarAsValidacoesNaoDeveSerPossivelInserirUmObjetoInvalido()
		{
			var repositorio = CriarRepositorioComDados();

			repositorio.AtivarValidacoes();

			var novo = new ObjetoDeTestes
			{
				Nome = "Pequeno"
			};

			Action insercao = () => repositorio.Inserir(novo);

			insercao
				.Should().Throw<Exception>()
				.WithMessage("Favor informar no mínimo 10 caracteres para o nome!");

			repositorio.Quantidade
				.Should()
				.Be(0);
		}

		[TestMethod]
		public void SeDesativarAsValidacoesDeveSerPossivelInserirUmObjetoInvalido()
		{
			var repositorio = CriarRepositorioComDados();

			repositorio.Quantidade
				.Should()
				.Be(0);

			repositorio.DesativarValidacoes();

			var novo = new ObjetoDeTestes
			{
				Nome = "Pequeno"
			};

			Action insercao = () => repositorio.Inserir(novo);

			insercao
				.Should().NotThrow();

			repositorio.Quantidade
				.Should()
				.Be(1);
		}

		[TestMethod]
		public void SeVerificarExistenciaDeUmRegistroComBaseNumCampoInteiroDeveEncontrar()
		{
			var repositorio = CriarRepositorioComDados();

			var config = repositorio.Buscar.CriarQuery()
				.AdicionarCondicao(c => c.Codigo).Igual(1);

			repositorio.Buscar.Existe(config)
				.Should()
				.BeTrue();
		}

		[TestMethod]
		public void SeVerificarExistenciaDeUmRegistroComBaseNumCampoTextoDeveEncontrar()
		{
			var repositorio = CriarRepositorioComDados();

			var config = repositorio.Buscar.CriarQuery()
				.AdicionarCondicao(c => c.Nome).Igual("A");

			repositorio.Buscar.Existe(config)
				.Should()
				.BeTrue();
		}

		[TestMethod]
		public void SeVerificarExistenciaDeUmRegistroComBaseNaChaveTextoDeveEncontrar()
		{
			var repositorio = CriarRepositorioComChaveTexto();

			var config = repositorio.Buscar.CriarQuery()
				.AdicionarCondicao(c => c.ChaveTexto).Igual("C");

			repositorio.Buscar.Existe(config)
				.Should()
				.BeTrue();
		}

		private static Pattern.Contextos.IRepositorio<ObjetoComChaveTexto> CriarRepositorioComChaveTexto(IContextoFake contexto = null)
		{
			contexto = contexto ?? FabricaFake.CriarContexto();
			contexto.AdicionarRegistro(new ObjetoComChaveTexto() { ChaveTexto = "A", Nome = "Nome A" });
			contexto.AdicionarRegistro(new ObjetoComChaveTexto() { ChaveTexto = "B", Nome = "Nome B" });
			contexto.AdicionarRegistro(new ObjetoComChaveTexto() { ChaveTexto = "C", Nome = "Nome C" });
			contexto.AdicionarRegistro(new ObjetoComChaveTexto() { ChaveTexto = "D", Nome = "Nome D" });

			var repositorio = contexto.Repositorio<ObjetoComChaveTexto>();
			return repositorio;
		}

		[TestMethod]
		public void SeConsultarChaveDeUmRegistroComBaseNaChaveTextoDeveEncontrar()
		{
			var repositorio = CriarRepositorioComChaveTexto();

			var registro = repositorio.Consultar("B");

			registro
				.Should()
				.NotBeNull();

			registro.Nome
				.Should()
				.Be("Nome B");
		}


		[TestMethod]
		public void SeAdicionarEDepoisConsultarChaveDeUmRegistroComBaseNaChaveTextoNaoDeveEncontrar()
		{
			var repositorio = CriarRepositorioComChaveTexto();

			repositorio.Inserir(new ObjetoComChaveTexto() { ChaveTexto = "E", Nome = "Nome E" });

			var registro = repositorio.Consultar("E");

			registro
				.Should()
				.BeNull();
		}

		[TestMethod]
		public void SeAdicionarESalvarEDepoisConsultarChaveDeUmRegistroComBaseNaChaveTextoDeveEncontrar()
		{
			var contexto = FabricaFake.CriarContexto();
			var repositorio = CriarRepositorioComChaveTexto(contexto);

			repositorio.Inserir(new ObjetoComChaveTexto() { ChaveTexto = "E", Nome = "Nome E" });

			contexto.Salvar();

			var registro = repositorio.Consultar("E");

			registro
				.Should()
				.NotBeNull();

			registro.Nome
				.Should()
				.Be("Nome E");
		}

		//[TestMethod]
		//public void SeGerarConsultaComEstruturaDiferenteDoObjetoAtualDeveSerPossivelConsultar()
		//{
		//	var repositorio = CriarRepositorioComDados();
		//	var config = repositorio.Buscar.CriarQuery()
		//		.AdicionarResultado(o => o.Logico)
		//		.AdicionarResultadoAgregado(Pattern.Buscadores.Agregadores.Count)
		//		.AdicionarAgrupamento(o => o.Logico)
		//		.AdicionarOrdem(o => o.Logico);

		//	var valor = false;
		//	var registros = 0;
		//	foreach (var registro in repositorio.Buscar.Registros(config))
		//	{
		//		registro[0]
		//			.Should()
		//			.Be(valor);
		//		((int)registro[1])
		//			.Should()
		//			.BeGreaterThan(0);
		//		registros += 1;
		//		valor = !valor;
		//	}

		//	registros
		//		.Should()
		//		.Be(2);
		//}

	}
}
