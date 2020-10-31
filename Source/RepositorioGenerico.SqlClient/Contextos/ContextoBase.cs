using System.Collections.Generic;
using RepositorioGenerico.Dictionary;
using RepositorioGenerico.Framework;
using RepositorioGenerico.Pattern.Buscadores;
using RepositorioGenerico.Search;
using RepositorioGenerico.SqlClient.Builders;
using System.Data;
using RepositorioGenerico.Pattern;
using RepositorioGenerico.Pattern.Contextos;
using System;
using System.Collections.Concurrent;

namespace RepositorioGenerico.SqlClient.Contextos
{
	public abstract class ContextoBase : Conexao
	{

		private ConcurrentDictionary<Type, object> _repositorios;
		private HistoricoTransacional _transacoes;
		private IBuscador _buscadorGenerico;
		private object _buscador;
		private Comando _comando;

		private static readonly IQueryBuilder QueryBuilder = new QueryBuilder();
		private static readonly IRelacionamentoBuilder RelacionamentoBuilder = new RelacionamentoBuilder();

		public bool LimparContextoAoSalvar { get; set; } = true;

		protected ConcurrentDictionary<Type, object> Repositorios
		{
			get { return _repositorios ?? (_repositorios = new ConcurrentDictionary<Type, object>()); }
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

		protected ContextoBase(string stringConexao, IDbTransaction transacao)
			: base(stringConexao, transacao)
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

			ITransacao transacao = null;
			var gerenciarTransacao = !EmTransacao;

			if (gerenciarTransacao)
				transacao = IniciarTransacao();

			try
			{
				Transacoes.Salvar();
				if (gerenciarTransacao)
					transacao.ConfirmarTransacao();
				if (LimparContextoAoSalvar)
					Limpar();
			}
			catch
			{
				if (gerenciarTransacao)
					transacao.CancelarTransacao();
				throw;
			}
		}

		public void Limpar()
		{
			Transacoes.Limpar();
			foreach (var nome in Repositorios.Keys)
				(Repositorios[nome] as IRepositorioObject).Limpar();
		}

	}
}
