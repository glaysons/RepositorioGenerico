using Entities;
using RepositorioGenerico.Pattern.Contextos;
using RepositorioGenerico.Fake.Contextos;
using Business.TiposDosContatos;

namespace Business.Test.TiposDosContatos
{
	public class TipoContatoFactory
	{

		private IContextoFake _contexto;

		public IContextoFake Contexto
		{
			get { return _contexto ?? (_contexto = CriarContexto()); }
		}

		public IRepositorio<TipoContato> Repositorio
		{
			get { return Contexto.Repositorio<TipoContato>(); }
		}

		private IContextoFake CriarContexto()
		{
			var contexto = RepositorioGenerico.Fake.FabricaFake.CriarContexto();
			PreencherTiposDosContatosDeExemplo(contexto);
			return contexto;
		}

		private void PreencherTiposDosContatosDeExemplo(IContextoFake contexto)
		{
			contexto.AdicionarRegistro(new TipoContato() { Id = 1, Nome = "Telefone" });
			contexto.AdicionarRegistro(new TipoContato() { Id = 2, Nome = "E-mail" });
			contexto.AdicionarRegistro(new TipoContato() { Id = 3, Nome = "Facebook" });
			contexto.AdicionarRegistro(new TipoContato() { Id = 4, Nome = "Twitter" });
			contexto.AdicionarRegistro(new TipoContato() { Id = 5, Nome = "Whattsapp" });
			contexto.AdicionarRegistro(new TipoContato() { Id = 6, Nome = "Telepatia" });
		}

		public ManutencaoTipoContatoBusiness CriarManutencao()
		{
			return new ManutencaoTipoContatoBusiness(Repositorio, CriarConsultador());
		}

		public ConsultaTipoContatoBusiness CriarConsultador()
		{
			return new ConsultaTipoContatoBusiness(Repositorio);
		}

		public void ExistemContatosVinculados(bool existe)
		{
			Contexto.DefinirResultadoScalarProcedure("spExistemContatosVinculados", existe);
		}

	}
}
