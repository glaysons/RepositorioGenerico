using Entities;
using RepositorioGenerico.Pattern.Contextos;
using RepositorioGenerico.Fake.Contextos;
using System;

namespace Business.Test
{
	public class CidadeFactory
	{

		private IRepositorio<Cidade> _repositorio;

		public IRepositorio<Cidade> Repositorio
		{
			get
			{
				return _repositorio
					?? (_repositorio = CriarRepositorio());
			}
		}

		private IRepositorio<Cidade> CriarRepositorio()
		{
			var contexto = RepositorioGenerico.Fake.FabricaFake.CriarContexto();
			PreencherCidadesDeExemplo(contexto);
			return contexto.Repositorio<Cidade>();
		}

		private void PreencherCidadesDeExemplo(IContextoFake contexto)
		{
			contexto.AdicionarRegistro(new Cidade() { Id = 1, Nome = "Cuiabá", Estado = "MT" });
			contexto.AdicionarRegistro(new Cidade() { Id = 2, Nome = "Rondonópolis", Estado = "MT" });
			contexto.AdicionarRegistro(new Cidade() { Id = 3, Nome = "São Paulo", Estado = "SP" });
			contexto.AdicionarRegistro(new Cidade() { Id = 4, Nome = "Campo Grande", Estado = "MS" });
			contexto.AdicionarRegistro(new Cidade() { Id = 5, Nome = "Curitiba", Estado = "PR" });
			contexto.AdicionarRegistro(new Cidade() { Id = 6, Nome = "Altônia", Estado = "PR" });
		}

		public ManutencaoCidadeBusiness CriarManutencao()
		{
			return new ManutencaoCidadeBusiness(Repositorio, CriarConsultador());
		}

		public ConsultaCidadeBusiness CriarConsultador()
		{
			return new ConsultaCidadeBusiness(Repositorio);
		}

		public void ExistemClientesVinculados(ConsultaCidadeBusiness consultador, bool v)
		{
			throw new NotImplementedException();
		}

	}
}
