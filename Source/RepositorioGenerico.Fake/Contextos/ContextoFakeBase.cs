﻿using System.Collections.Generic;
using RepositorioGenerico.Dictionary;
using RepositorioGenerico.Fake.Builders;
using RepositorioGenerico.Framework;
using RepositorioGenerico.Pattern.Buscadores;
using RepositorioGenerico.Search;
using RepositorioGenerico.Pattern;
using RepositorioGenerico.Pattern.Contextos;
using System;

namespace RepositorioGenerico.Fake.Contextos
{
	public class ContextoFakeBase : ConexaoFake
	{

		private Dictionary<Type, object> _repositorios;
		private IHistoricoTransacional _transacoes;
		private IBuscador _buscadorGenerico;
		private object _buscador;
		private ComandoFake _comando;

		private static readonly IQueryBuilder QueryBuilder = new QueryFakeBuilder();
		private static readonly IRelacionamentoBuilder RelacionamentoBuilder = CriarRelacionamentoBuilder();

		public bool LimparContextoAoSalvar { get; set; } = true;

		private static IRelacionamentoBuilder CriarRelacionamentoBuilder()
		{
			return new RelacionamentoBuilderFake();
		}

		protected Dictionary<Type, object> Repositorios
		{
			get { return _repositorios ?? (_repositorios = new Dictionary<Type, object>()); }
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
