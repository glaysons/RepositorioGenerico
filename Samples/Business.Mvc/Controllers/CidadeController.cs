using Entities;
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
			ViewBag.Novo = true;
			return ExibirPaginaParaCriarOuEditar();
		}

		private ActionResult ExibirPaginaParaCriarOuEditar(Cidade cidade = null)
		{
			return View("CreateEdit", cidade);
		}

		[HttpPost]
		public ActionResult Create(Cidade cidade)
		{
			ViewBag.Novo = true;
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

	}
}