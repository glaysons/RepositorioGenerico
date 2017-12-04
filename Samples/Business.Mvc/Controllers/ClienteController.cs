using Business.Clientes;
using RepositorioGenerico.Pattern.Contextos;
using System.Web.Mvc;

namespace Business.Mvc.Controllers
{
	public class ClienteController : Controller
	{

		private IContexto _contexto;
		private ConsultaClienteBusiness _consulta;
		private ManutencaoClienteBusiness _manutencao;

		public ClienteController(IContexto contexto, ConsultaClienteBusiness consulta, ManutencaoClienteBusiness manutencao)
		{
			_contexto = contexto;
			_consulta = consulta;
			_manutencao = manutencao;
		}

		public ActionResult Index()
		{
			return View(_consulta.ConsultarTodosOsClientes());
		}
	}
}