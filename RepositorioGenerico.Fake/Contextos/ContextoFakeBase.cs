using System.Collections.Generic;
using Moq;
using RepositorioGenerico.Dictionary;
using RepositorioGenerico.Entities;
using RepositorioGenerico.Fake.Builders;
using RepositorioGenerico.Framework;
using RepositorioGenerico.Pattern.Buscadores;
using RepositorioGenerico.Search;

namespace RepositorioGenerico.Fake.Contextos
{
	public class ContextoFakeBase : ConexaoFake
	{

		private Dictionary<string, object> _repositorios;
		private IHistoricoTransacional _transacoes;
		private IBuscador _buscadorGenerico;
		private object _buscador;
		private ComandoFake _comando;

		private static readonly IQueryBuilder QueryBuilder = new QueryFakeBuilder();

		private static readonly IRelacionamentoBuilder RelacionamentoBuilder = CriarRelacionamentoBuilder();

		private static IRelacionamentoBuilder CriarRelacionamentoBuilder()
		{
			return new Mock<IRelacionamentoBuilder>().Object;
		}

		protected Dictionary<string, object> Repositorios
		{
			get { return _repositorios ?? (_repositorios = new Dictionary<string, object>()); }
		}

		internal IHistoricoTransacional Transacoes
		{
			get { return _transacoes ?? (_transacoes = new HistoricoTransacional(this)); }
		}

		public ContextoFakeBase()
		{
			
		}

		internal ContextoFakeBase(IHistoricoTransacional transacoes)
		{
			_transacoes = transacoes;
		}

		private ComandoFake Comando
		{
			get { return _comando ?? (_comando = new ComandoFake(this)); }
		}

		public IBuscador Buscar()
		{
			return _buscadorGenerico ?? (_buscadorGenerico = new Buscador(Comando, QueryBuilder));
		}

		public IBuscador<TObjeto> Buscar<TObjeto>() where TObjeto : IEntidade
		{
			var buscador = (_buscador as IBuscador<TObjeto>);
			if (buscador != null) return buscador;
			var dicionario = DicionarioCache.Consultar(typeof(TObjeto));
			buscador = new Buscador<TObjeto>(Comando, dicionario, QueryBuilder, RelacionamentoBuilder);
			_buscador = buscador;
			return buscador;
		}

		public void Salvar()
		{
			if (_transacoes == null)
				return;

			var gerenciarTransacao = !EmTransacao;
			if (gerenciarTransacao)
				IniciarTransacao();
			try
			{
				Transacoes.Salvar();
				if (gerenciarTransacao)
					ConfirmarTransacao();
			}
			catch
			{
				if (gerenciarTransacao)
					CancelarTransacao();
				throw;
			}
		}

	}
}
