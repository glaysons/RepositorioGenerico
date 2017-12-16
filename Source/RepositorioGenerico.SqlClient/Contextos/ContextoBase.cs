using System.Collections.Generic;
using RepositorioGenerico.Dictionary;
using RepositorioGenerico.Framework;
using RepositorioGenerico.Pattern.Buscadores;
using RepositorioGenerico.Search;
using RepositorioGenerico.SqlClient.Builders;

namespace RepositorioGenerico.SqlClient.Contextos
{
	public abstract class ContextoBase : Conexao
	{

		private Dictionary<string, object> _repositorios;
		private HistoricoTransacional _transacoes;
		private IBuscador _buscadorGenerico;
		private object _buscador;
		private Comando _comando;

		private static readonly IQueryBuilder QueryBuilder = new QueryBuilder();
		private static readonly IRelacionamentoBuilder RelacionamentoBuilder = new RelacionamentoBuilder();

		protected Dictionary<string, object> Repositorios
		{
			get { return _repositorios ?? (_repositorios = new Dictionary<string, object>()); }
		}

		internal HistoricoTransacional Transacoes
		{
			get { return _transacoes ?? (_transacoes = new HistoricoTransacional(this)); }
		}

		private Comando Comando
		{
			get { return _comando ?? (_comando = new Comando(this)); }
		}

		protected ContextoBase(string stringConexao)
			: base(stringConexao)
		{

		}

		public IBuscador Buscar()
		{
			return _buscadorGenerico ?? (_buscadorGenerico = new Buscador(Comando, QueryBuilder));
		}

		public IBuscador<TObjeto> Buscar<TObjeto>()
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
