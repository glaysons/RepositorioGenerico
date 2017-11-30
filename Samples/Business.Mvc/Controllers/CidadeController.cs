using Entities;
using RepositorioGenerico.Pattern.Contextos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
			return View();
		}

		[HttpPost]
		public ActionResult Create(Cidade cidade)
		{
			throw new NotImplementedException();
		}

	}
}