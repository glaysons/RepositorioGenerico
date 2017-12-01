using Entities;
using RepositorioGenerico.Pattern.Contextos;
using RepositorioGenerico.Fake.Contextos;
using Business.Cidades;

namespace Business.Test.Cidades
{
	public class CidadeFactory
	{

		private IContextoFake _contexto;

		public IContextoFake Contexto
		{
			get { return _contexto ?? (_contexto = CriarContexto()); }
		}

		public IRepositorio<Cidade> Repositorio
		{
			get { return Contexto.Repositorio<Cidade>(); }
		}

		private IContextoFake CriarContexto()
		{
			var contexto = RepositorioGenerico.Fake.FabricaFake.CriarContexto();
			PreencherCidadesDeExemplo(contexto);
			return contexto;
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

		public void ExistemClientesVinculados(bool existe)
		{
			Contexto.DefinirResultadoScalarProcedure("spExistemClientesVinculados", existe);
		}

	}
}
