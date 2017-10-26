using System.Collections.Generic;
using System.Data;
using System.Linq;
using RepositorioGenerico.Dictionary.Builders;
using RepositorioGenerico.Entities;
using RepositorioGenerico.Exceptions;
using RepositorioGenerico.Pattern.Buscadores;
using RepositorioGenerico.Pattern.Contextos;

namespace RepositorioGenerico.Fake.Contextos
{
	public class RepositorioFake<TObjeto> : IRepositorioObject, IRepositorio<TObjeto> where TObjeto : class, IEntidade
	{

		private readonly ContextoFake _contexto;
		private readonly PersistenciaFake<TObjeto> _persistencia;
		private readonly DataTable _bancoDeDadosVirtual;
		private bool _validarAoSalvar;
		private bool _filhosVerificados;
		private FilhosRepositorioFake<TObjeto> _filhosRepositorio;

		public int Quantidade
		{
			get { return _persistencia.Quantidade; }
		}

		public IBuscador<TObjeto> Buscar
		{
			get { return _contexto.Buscar<TObjeto>(); }
		}

		public IQueryable<TObjeto> Query
		{
			get
			{
				return _persistencia.Dados.AsQueryable();
			}
		}

		public bool SalvarFilhos { get; set; }

		internal FilhosRepositorioFake<TObjeto> FilhosRepositorio
		{
			get
			{
				if (_filhosVerificados)
					return _filhosRepositorio;
				_filhosVerificados = true;
				if (_persistencia.Dicionario.PossuiCamposFilho)
					_filhosRepositorio = new FilhosRepositorioFake<TObjeto>(_contexto, _persistencia.Dicionario.ConsultarCamposFilho());
				return _filhosRepositorio;
			}
		}

		public RepositorioFake(IContexto contexto, PersistenciaFake<TObjeto> persistencia, DataTable bancoDeDadosVirtual)
		{
			_contexto = contexto as ContextoFake;
			_persistencia = persistencia;
			_bancoDeDadosVirtual = bancoDeDadosVirtual;
			_validarAoSalvar = true;
			_filhosVerificados = false;
			SalvarFilhos = true;
		}

		public void AtivarValidacoes()
		{
			_validarAoSalvar = true;
		}

		public void DesativarValidacoes()
		{
			_validarAoSalvar = false;
		}

		public IEnumerable<TObjeto> Itens()
		{
			return _persistencia.Dados;
		}

		IEnumerable<object> IRepositorioObject.Itens()
		{
			return _persistencia.Dados;
		}

		public TObjeto Consultar(params object[] chave)
		{
			if (chave.Length == 0)
				throw new ValoresChavePreenchimentoObrigatorioException();
			if (chave.Length != _persistencia.Dicionario.QuantidadeCamposNaChave)
				throw new ValoresChavePreenchimentoObrigatorioException(_persistencia.Dicionario.QuantidadeCamposNaChave);
			var consulta = Buscar.CriarQuery();
			var indice = 0;
			foreach (var campo in _persistencia.Dicionario.ConsultarCamposChave())
			{
				consulta.AdicionarCondicaoPersonalizada(campo.Nome + "=@p" + indice.ToString());
				consulta.DefinirParametro("p" + indice.ToString()).Tipo(campo.TipoBanco, chave[indice]);
				indice++;
			}
			return Buscar.Um(consulta);
		}

		public TObjeto Criar()
		{
			return _persistencia.Criar();
		}

		object IRepositorioObject.Criar()
		{
			return _persistencia.Criar();
		}

		public void Validar(TObjeto model)
		{
			_persistencia.Dicionario.Validador.Validar(model);
		}

		void IRepositorioObject.Validar(object model)
		{
			Validar((TObjeto)model);
		}

		public IEnumerable<string> Valido(TObjeto model)
		{
			return _persistencia.Dicionario.Validador.Valido(model);
		}

		IEnumerable<string> IRepositorioObject.Valido(object model)
		{
			return Valido((TObjeto)model);
		}

		public void Inserir(TObjeto model)
		{
			if (_validarAoSalvar)
				Validar(model);
			model.EstadoEntidade = EstadosEntidade.Novo;
			_persistencia.Dados.Add(model);
			_contexto.Transacoes.AdicionarTransacao(_persistencia, model);
			_bancoDeDadosVirtual.Rows.Add(DataTableBuilder.ConverterItemEmDataRow(_bancoDeDadosVirtual, model));
			if (SalvarFilhos && FilhosRepositorio != null)
			{
				var chave = _persistencia.Dicionario.ConsultarValoresDaChave(model);
				FilhosRepositorio.InserirFilhos(model, chave);
			}
		}

		void IRepositorioObject.Inserir(object model)
		{
			Inserir((TObjeto)model);
		}

		public void Inserir(IList<TObjeto> models)
		{
			foreach (var model in models)
				Inserir(model);
		}

		public void Atualizar(TObjeto model)
		{
			if (_validarAoSalvar)
				Validar(model);
			model.EstadoEntidade = EstadosEntidade.Modificado;
			_persistencia.Dados.Add(model);
			_contexto.Transacoes.AdicionarTransacao(_persistencia, model);
			if (SalvarFilhos && FilhosRepositorio != null)
			{
				var chave = _persistencia.Dicionario.ConsultarValoresDaChave(model);
				FilhosRepositorio.AtualizarFilhos(model, chave);
			}
		}

		void IRepositorioObject.Atualizar(object model)
		{
			Atualizar((TObjeto)model);
		}

		public void Atualizar(IList<TObjeto> models)
		{
			foreach (var model in models)
				Atualizar(model);
		}

		public void Excluir(TObjeto model)
		{
			if (SalvarFilhos && FilhosRepositorio != null)
				FilhosRepositorio.ExcluirFilhos(model);
			model.EstadoEntidade = EstadosEntidade.Excluido;
			_persistencia.Dados.Remove(model);
			_contexto.Transacoes.AdicionarTransacao(_persistencia, model);
		}

		void IRepositorioObject.Excluir(object model)
		{
			Excluir((TObjeto)model);
		}

		public void Excluir(IList<TObjeto> models)
		{
			foreach (var model in models)
				Excluir(model);
		}


	}
}
