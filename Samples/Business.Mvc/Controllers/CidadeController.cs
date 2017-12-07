using Business.Cidades;
using Entities;
using RepositorioGenerico.Entities;
using RepositorioGenerico.Pattern.Contextos;
using System;
using System.Web.Mvc;

namespace Business.Mvc.Controllers
{
	public class CidadeController : Controller
	{
		private IContexto _contexto;
		private ConsultaCidadeBusiness _consulta;
		private ManutencaoCidadeBusiness _manutencao;

		public CidadeController(IContexto contexto, ConsultaCidadeBusiness consulta, ManutencaoCidadeBusiness manutencao)
		{
			_contexto = contexto;
			_consulta = consulta;
			_manutencao = manutencao;
		}

		public ActionResult Index()
		{
			return View(_consulta.ConsultarTodasAsCidades());
		}

		public ActionResult Create()
		{
			return ExibirPaginaParaCriarOuEditar();
		}

		private ActionResult ExibirPaginaParaCriarOuEditar(Cidade cidade = null)
		{
			ViewBag.Novo = ((cidade == null) || (cidade.EstadoEntidade == EstadosEntidade.Novo));
			return View("CreateEdit", cidade);
		}

		[HttpPost]
		public ActionResult Create(Cidade cidade)
		{
			ViewBag.MensagemErro = "";
			try
			{
				_manutencao.Cadastrar(cidade);
				_contexto.Salvar();
				return RedirecionarParaPaginaInicialDaCidade();
			}
			catch (Exception ex)
			{
				ViewBag.MensagemErro = ex.Message;
				return ExibirPaginaParaCriarOuEditar(cidade);
			}
		}

		private ActionResult RedirecionarParaPaginaInicialDaCidade()
		{
			return Redirect("~/Cidade");
		}

		public ActionResult Edit(int id)
		{
			return ExibirPaginaParaCriarOuEditar(_consulta.ConsultarCidade(id));
		}

		[HttpPost]
		public ActionResult Edit(Cidade cidade)
		{
			ViewBag.MensagemErro = "";
			try
			{
				_manutencao.Atualizar(cidade);
				_contexto.Salvar();
				return RedirecionarParaPaginaInicialDaCidade();
			}
			catch (Exception ex)
			{
				ViewBag.MensagemErro = ex.Message;
				return ExibirPaginaParaCriarOuEditar(cidade);
			}
		}

		public ActionResult Details(int id)
		{
			return View(_consulta.ConsultarCidade(id));
		}

		public ActionResult Delete(int id)
		{
			return View(_consulta.ConsultarCidade(id));
		}

		[HttpPost]
		public ActionResult Delete(Cidade cidade)
		{
			ViewBag.MensagemErro = "";
			try
			{
				_manutencao.Excluir(cidade);
				_contexto.Salvar();
				return RedirecionarParaPaginaInicialDaCidade();
			}
			catch (Exception ex)
			{
				ViewBag.MensagemErro = ex.Message;
				return View(cidade);
			}
		}

	}
}