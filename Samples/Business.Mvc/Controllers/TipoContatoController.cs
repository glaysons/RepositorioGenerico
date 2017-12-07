using Business.TiposDosContatos;
using Entities;
using RepositorioGenerico.Entities;
using RepositorioGenerico.Pattern.Contextos;
using System;
using System.Web.Mvc;

namespace Business.Mvc.Controllers
{
	public class TipoContatoController : Controller
	{
		private IContexto _contexto;
		private ConsultaTipoContatoBusiness _consulta;
		private ManutencaoTipoContatoBusiness _manutencao;

		public TipoContatoController(IContexto contexto, ConsultaTipoContatoBusiness consulta, ManutencaoTipoContatoBusiness manutencao)
		{
			_contexto = contexto;
			_consulta = consulta;
			_manutencao = manutencao;
		}

		public ActionResult Index()
		{
			return View(_consulta.ConsultarTodosOsTiposDosContatos());
		}

		public ActionResult Create()
		{
			return ExibirPaginaParaCriarOuEditar();
		}

		private ActionResult ExibirPaginaParaCriarOuEditar(TipoContato tipoContato = null)
		{
			ViewBag.Novo = ((tipoContato == null) || (tipoContato.EstadoEntidade == EstadosEntidade.Novo));
			return View("CreateEdit", tipoContato);
		}

		[HttpPost]
		public ActionResult Create(TipoContato TipoContato)
		{
			ViewBag.MensagemErro = "";
			try
			{
				_manutencao.Cadastrar(TipoContato);
				_contexto.Salvar();
				return RedirecionarParaPaginaInicialDaTipoContato();
			}
			catch (Exception ex)
			{
				ViewBag.MensagemErro = ex.Message;
				return ExibirPaginaParaCriarOuEditar(TipoContato);
			}
		}

		private ActionResult RedirecionarParaPaginaInicialDaTipoContato()
		{
			return Redirect("~/TipoContato");
		}

		public ActionResult Edit(int id)
		{
			return ExibirPaginaParaCriarOuEditar(_consulta.ConsultarTipoContato(id));
		}

		[HttpPost]
		public ActionResult Edit(TipoContato TipoContato)
		{
			ViewBag.MensagemErro = "";
			try
			{
				_manutencao.Atualizar(TipoContato);
				_contexto.Salvar();
				return RedirecionarParaPaginaInicialDaTipoContato();
			}
			catch (Exception ex)
			{
				ViewBag.MensagemErro = ex.Message;
				return ExibirPaginaParaCriarOuEditar(TipoContato);
			}
		}

		public ActionResult Details(int id)
		{
			return View(_consulta.ConsultarTipoContato(id));
		}

		public ActionResult Delete(int id)
		{
			return View(_consulta.ConsultarTipoContato(id));
		}

		[HttpPost]
		public ActionResult Delete(TipoContato TipoContato)
		{
			ViewBag.MensagemErro = "";
			try
			{
				_manutencao.Excluir(TipoContato);
				_contexto.Salvar();
				return RedirecionarParaPaginaInicialDaTipoContato();
			}
			catch (Exception ex)
			{
				ViewBag.MensagemErro = ex.Message;
				return View(TipoContato);
			}
		}

	}
}