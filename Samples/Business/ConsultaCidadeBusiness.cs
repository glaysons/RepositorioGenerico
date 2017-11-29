using Entities;
using RepositorioGenerico.Pattern.Buscadores;
using RepositorioGenerico.Pattern.Contextos;

namespace Business
{
	public class ConsultaCidadeBusiness
	{

		private IRepositorio<Cidade> _repositorio;

		public ConsultaCidadeBusiness(IRepositorio<Cidade> repositorio)
		{
			_repositorio = repositorio;
		}

		public Cidade ConsultarCidade(int codigo)
		{
			var config = _repositorio.Buscar.CriarQuery()
				.AdicionarCondicao(c => c.Id).Igual(codigo);
			return _repositorio.Buscar.Um(config);
		}

		public bool ExisteCidadeCadastrada(Cidade cidade)
		{
			var config = _repositorio.Buscar.CriarQuery()
				.AdicionarCondicao(c => c.Id).Seja(Operadores.Diferente, cidade.Id)
				.AdicionarCondicao(c => c.Nome).Igual(cidade.Nome)
				.AdicionarCondicao(c => c.Estado).Igual(cidade.Estado);
			return _repositorio.Buscar.Existe(config);
		}

		public bool ExistemClientesVinculados(Cidade cidade)
		{
			var config = _repositorio.Buscar.CriarProcedure("spExistemClientesVinculados");
			config.DefinirParametro(c => c.Id).Valor(cidade.Id);
			return _repositorio.Buscar.Existe(config);
		}

	}
}
