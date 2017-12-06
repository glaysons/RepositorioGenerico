using Business.Cidades;
using Business.Clientes;
using Business.TiposDosContatos;
using Entities;
using RepositorioGenerico.Pattern.Contextos;
using System;
using System.Web.Mvc;

namespace Business.Mvc.Controllers
{
	public class ClienteController : Controller
	{

		private IContexto _contexto;
		private ConsultaClienteBusiness _consulta;
		private ManutencaoClienteBusiness _manutencao;
		private ConsultaCidadeBusiness _consultaCidade;
		private ConsultaTipoContatoBusiness _consultaTipoContato;

		public ClienteController(IContexto contexto, ConsultaClienteBusiness consulta, ManutencaoClienteBusiness manutencao,
			ConsultaCidadeBusiness consultaCidade, ConsultaTipoContatoBusiness consultaTipoContato)
		{
			_contexto = contexto;
			_consulta = consulta;
			_manutencao = manutencao;
			_consultaCidade = consultaCidade;
			_consultaTipoContato = consultaTipoContato;
		}

		public ActionResult Index()
		{
			return View(_consulta.ConsultarTodosOsClientes());
		}

		public ActionResult Create()
		{
			return ExibirPaginaParaCriarOuEditar();
		}

		private ActionResult ExibirPaginaParaCriarOuEditar(Cliente cliente = null)
		{
			ViewBag.Novo = (cliente == null);
			ViewBag.Cidades = _consultaCidade.ConsultarTodasAsCidades();
			ViewBag.TiposContatos = _consultaTipoContato.ConsultarTodosOsTiposDosContatos();
			return View("CreateEdit", cliente);
		}

		[HttpPost]
		public ActionResult Create(Cliente cliente)
		{
			ViewBag.MensagemErro = "";
			try
			{
				_manutencao.Cadastrar(cliente);
				_contexto.Salvar();
				return RedirecionarParaPaginaInicialDaCidade();
			}
			catch (Exception ex)
			{
				ViewBag.MensagemErro = ex.Message;
				return ExibirPaginaParaCriarOuEditar(cliente);
			}
		}

		private ActionResult RedirecionarParaPaginaInicialDaCidade()
		{
			return Redirect("~/Cliente");
		}

	}
}