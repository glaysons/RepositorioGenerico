using System;
using System.Collections.Generic;
using System.Linq;
using RepositorioGenerico.Dictionary.Helpers;
using RepositorioGenerico.Dictionary.Itens;
using RepositorioGenerico.Dictionary.Mapas;
using RepositorioGenerico.Dictionary.Mapas.Patterns;
using RepositorioGenerico.Dictionary.Relacionamentos;
using RepositorioGenerico.Dictionary.Validadores;
using RepositorioGenerico.Entities;
using RepositorioGenerico.Entities.Anotacoes;
using RepositorioGenerico.Entities.Anotacoes.Validadores;
using RepositorioGenerico.Entities.Mapas;
using RepositorioGenerico.Exceptions;
using RepositorioGenerico.Framework.Helpers;
using System.Data;

namespace RepositorioGenerico.Dictionary
{
	public class Dicionario : IDicionario
	{

		private readonly Type _tipo;
		private readonly string _nome;
		private ValidadorDicionario _validador;
		private IList<IValidadorEntidadeAttribute> _validacoes;
		private Dictionary<string, ItemDicionario> _itens;
		private Dictionary<string, ItemDicionario> _itensNaoMapeados;
		private Dictionary<string, ItemDicionario> _chaves;
		private Dictionary<string, ItemDicionario> _propriedades;
		private Dictionary<string, ItemDicionario> _propriedadesNaoMapeadas;
		private OpcoesAutoIncremento _autoIncremento;
		private int _quantidade;
		private int _quantidadeCamposNaChave;
		private readonly bool _possuiReferencial;
		private bool _possuiCamposFilhos;
		private readonly IMapa _mapa;

		public string Alias { get; private set; }

		public string Nome { get { return _nome; } }

		public string AliasOuNome { get { return Alias ?? Nome; } }

		public OpcoesAutoIncremento AutoIncremento
		{
			get
			{
				CarregarDefinicoesDoModel();
				return _autoIncremento;
			}
		}

		public IEnumerable<ItemDicionario> Itens
		{
			get
			{
				CarregarDefinicoesDoModel();
				return _itens.Select(item => item.Value);
			}
		}

		public int QuantidadeCampos
		{
			get
			{
				CarregarDefinicoesDoModel();
				return _quantidade;
			}
		}

		public int QuantidadeCamposNaChave
		{
			get
			{
				CarregarDefinicoesDoModel();
				return _quantidadeCamposNaChave;
			}
		}

		public ValidadorDicionario Validador
		{
			get { return _validador ?? (_validador = new ValidadorDicionario(this, Validacoes)); }
		}

		protected IList<IValidadorEntidadeAttribute> Validacoes
		{
			get { return _validacoes ?? (_validacoes = new List<IValidadorEntidadeAttribute>()); }
		}

		public Type Tipo
		{
			get { return _tipo; }
		}

		public Type OrigemMapa
		{
			get
			{
				return (_possuiReferencial)
					? _mapa.TipoDaTabela
					: null;
			}
		}

		public bool PossuiCamposFilho
		{
			get
			{
				CarregarDefinicoesDoModel();
				return _possuiCamposFilhos;
			}
		}

		public Dicionario(Type tipo)
		{
			if (!typeof(Entidade).IsAssignableFrom(tipo))
				throw new NaoSeraPossivelCriarUmDicionarioDeUmObjetoQueNaoHerdeObjetoBancoException();
			_tipo = tipo;
			_nome = DataAnnotationHelper.ConsultarNomeDaTabela(tipo);
			Alias = tipo.Name;
			_possuiReferencial = ObjetoPossuiOutroObjetoReferenciado(tipo);
			_mapa = (_possuiReferencial)
				? MapaItemDicionarioBuilder.CriarMapaBuilder(tipo)
				: null;
			if (_mapa != null)
			{
				Alias = tipo.Name;
				_nome = _mapa.NomeDaTabela;
			}
			if (string.Equals(Nome, Alias) || string.IsNullOrEmpty(Alias))
				Alias = null;
			_possuiCamposFilhos = false;
		}

		private bool ObjetoPossuiOutroObjetoReferenciado(Type tipo)
		{
			var tipoMapa = typeof(IMapaEntidade<>);
			var tipoGenerico = tipoMapa.MakeGenericType(tipo);
			return (tipo.Assembly.GetTypes().Count(t => tipoGenerico.IsAssignableFrom(t)) == 1);
		}

		private void CarregarDefinicoesDoModel()
		{
			if (_itens != null)
				return;
			_itens = new Dictionary<string, ItemDicionario>();
			_itensNaoMapeados = new Dictionary<string, ItemDicionario>();
			_propriedades = new Dictionary<string, ItemDicionario>();
			_propriedadesNaoMapeadas = new Dictionary<string, ItemDicionario>();
			_chaves = new Dictionary<string, ItemDicionario>();
			CarregarCamposDaTabela();
			CarregarValidadoresDoModel();
		}

		private void CarregarCamposDaTabela()
		{
			var itens = (_possuiReferencial)
				? MapaItemDicionarioBuilder.ConsultarItensDicionario(_mapa, _tipo)
				: ItemDicionarioBuilder.ConsultarItensDicionario(_tipo);

			_quantidade = 0;
			_quantidadeCamposNaChave = 0;
			_possuiCamposFilhos = false;

			foreach (var item in itens)
			{
				if (string.Equals(item.Nome, "EstadoEntidade"))
					continue;

				if (item.Mapeado)
				{
					_quantidade++;
					_itens.Add(item.Nome, item);
					_propriedades[item.Propriedade.Name] = item;
				}
				else
				{
					_itensNaoMapeados.Add(item.Nome, item);
					_propriedadesNaoMapeadas[item.Propriedade.Name] = item;
				}
				if (item.Chave)
				{
					_chaves[item.Nome] = item;
					_quantidadeCamposNaChave++;
				}
				if (CampoFilho(item))
					_possuiCamposFilhos = true;
			}

			if (_quantidade == 0)
				throw new TabelaNaoPossuiInformacoesDeCamposDaTabelaException();

			_autoIncremento = OpcoesAutoIncremento.Nenhum;

			var camposIdentity = itens.Count(i => i.OpcaoGeracao == Incremento.Identity);
			if (camposIdentity > 0)
			{
				if (camposIdentity > 1)
					throw new DicionarioNaoSuportaMultiplosCamposAutoIncrementoException();
				_autoIncremento = OpcoesAutoIncremento.Identity;
			}

			var camposAgrupados = itens.Count(i => i.OpcaoGeracao == Incremento.Calculado);
			if (camposAgrupados > 0)
			{
				if (camposAgrupados > 1)
					throw new DicionarioNaoSuportaMultiplosCamposAutoIncrementoException();
				_autoIncremento = OpcoesAutoIncremento.Calculado;
			}
			
			if ((camposIdentity > 0) && (camposAgrupados > 0))
				throw new DicionarioNaoSuportaMultiplosCamposAutoIncrementoException();
		}

		private bool CampoFilho(ItemDicionario item)
		{
			return ((item.Ligacao != null) && (item.Ligacao.Tipo == TiposRelacionamento.Descendente));
		}

		private void CarregarValidadoresDoModel()
		{
			CarregarValidadoresDoModel(_tipo);
			if (OrigemMapa != null)
				CarregarValidadoresDoModel(OrigemMapa);
		}

		private void CarregarValidadoresDoModel(Type tipo)
		{
			var validacoes = AttributeHelper.ConsultarTodos<ValidadorEntidadeAttribute>(tipo);
			foreach (var item in validacoes)
				AdicionarValidacao(item);
		}

		private void AdicionarValidacao(IValidadorEntidadeAttribute validacao)
		{
			if (validacao != null)
				Validacoes.Add(validacao);
		}

		public ItemDicionario ConsultarPorPropriedade(string nome)
		{
			CarregarDefinicoesDoModel();
			ItemDicionario item;
			if (_propriedades.TryGetValue(nome, out item) || _propriedadesNaoMapeadas.TryGetValue(nome, out item))
				return item;
			return null;
		}

		public ItemDicionario ConsultarPorCampo(string nome)
		{
			CarregarDefinicoesDoModel();
			ItemDicionario item;
			if (_itens.TryGetValue(nome, out item) || _itensNaoMapeados.TryGetValue(nome, out item))
				return item;
			return null;
		}

		public object[] ConsultarValoresDaChave(object registro)
		{
			var resultado = new object[QuantidadeCamposNaChave];
			var n = 0;
			foreach (var campo in ConsultarCamposChave())
			{
				resultado[n] = campo.Propriedade.GetValue(registro, null);
				n++;
			}
			return resultado;
		}

		public object[] ConsultarValoresDaChave(DataRow registro)
		{
			var resultado = new object[QuantidadeCamposNaChave];
			var n = 0;
			foreach (var campo in ConsultarCamposChave())
			{
				resultado[n] = registro[campo.Nome];
				n++;
			}
			return resultado;
		}

		public object[] ConsultarValoresDaChave(object registro, string[] foreign)
		{
			var resultado = new object[foreign.Length];
			var n = 0;
			foreach (var campo in foreign)
			{
				var item = ConsultarPorPropriedade(campo);
				if (item == null)
					continue;
				resultado[n] = item.Propriedade.GetValue(registro, null);
				n++;
			}
			return resultado;
		}

		public IEnumerable<ItemDicionario> ConsultarCamposChave()
		{
			CarregarDefinicoesDoModel();
			return _chaves.Select(i => i.Value);
		}

		public IEnumerable<ItemDicionario> ConsultarCamposFilho()
		{
			CarregarDefinicoesDoModel();
			return
				from item in _itensNaoMapeados
				where CampoFilho(item.Value)
				select item.Value;
		}
	}
}
