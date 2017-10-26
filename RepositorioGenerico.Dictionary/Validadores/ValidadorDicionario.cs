using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using RepositorioGenerico.Dictionary.Itens;
using RepositorioGenerico.Entities;
using RepositorioGenerico.Entities.Anotacoes.Validadores;
using RepositorioGenerico.Exceptions;

namespace RepositorioGenerico.Dictionary.Validadores
{
	public class ValidadorDicionario
	{

		private readonly IList<IValidadorEntidadeAttribute> _validacoes;

		public Dicionario Dicionario { get; private set; }

		public int Quantidade { get; private set; }

		public ValidadorDicionario(Dicionario dicionario, IList<IValidadorEntidadeAttribute> validacoes)
		{
			_validacoes = validacoes;
			Dicionario = dicionario;
			Quantidade = _validacoes.Count;
		}

		public void Validar<TObjeto>(TObjeto registro) where TObjeto : IEntidade
		{
			ValidarRegistro(i => ConsultarValorDoRegistro(registro, i));
			ExecutarValidacoesGerais(registro);
		}

		private void ValidarRegistro(Func<ItemDicionario, object> consultadorValor)
		{
			foreach (var item in Dicionario.Itens)
			{
				var valor = consultadorValor(item);
				Validar(item, valor);
			}
		}

		private object ConsultarValorDoRegistro<TObjeto>(TObjeto registro, ItemDicionario item)
		{
			return item.Propriedade.GetValue(registro, null);
		}

		private void ExecutarValidacoesGerais(object registro)
		{
			foreach (var validador in _validacoes)
				validador.Validar(registro);
		}

		public void Validar(DataRow registro)
		{
			ValidarRegistro(i => ConsultarValorDoRegistro(registro, i));
			ExecutarValidacoesGerais(registro);
		}

		private object ConsultarValorDoRegistro(DataRow registro, ItemDicionario item)
		{
			return registro[item.Nome];
		}

		public IEnumerable<string> Valido<TObjeto>(TObjeto registro)
		{
			return ConsultarValidadeDoRegistro(i => ConsultarValorDoRegistro(registro, i));
		}

		private IEnumerable<string> ConsultarValidadeDoRegistro(Func<ItemDicionario, object> consultadorValor)
		{
			return 
				from item in Dicionario.Itens 
				let valor = consultadorValor(item) 
				from mensagem in Valido(item, valor) 
				select mensagem;
		}

		private void Validar(ItemDicionario item, object valor)
		{
			ValidarObrigatorio(item, valor);
			ValidarTamanhoMaximo(item, valor);
			ValidarValidacoesPersonalizadas(item, valor);
		}

		private void ValidarObrigatorio(ItemDicionario item, object valor)
		{
			if (!item.Obrigatorio)
				return;
			var erro = (valor == null) || (valor == DBNull.Value);
			if (erro)
				throw new CampoPossuiPreenchimentoObrigatorioException(item.AliasOuNome);
		}

		private void ValidarTamanhoMaximo(ItemDicionario item, object valor)
		{
			if (item.TamanhoMaximo < 1)
				return;
			var erro = ((valor != null) && (valor != DBNull.Value) && (valor.ToString().Length > item.TamanhoMaximo));
			if (erro)
				throw new CampoPossuiTamanhoMaximoDePeenchimentoException(item.AliasOuNome, item.TamanhoMaximo);
		}

		private void ValidarValidacoesPersonalizadas(ItemDicionario item, object valor)
		{
			if (item.Validacoes == null)
				return;
			foreach (var validador in item.Validacoes)
				validador.Validar(valor);
		}

		public IEnumerable<string> Valido(ItemDicionario item, object valor)
		{
			var validadores = ConsultarValidadores(item);
			foreach (var validador in validadores)
			{
				string mensagem = null;
				try
				{
					validador(item, valor);
				}
				catch (Exception ex)
				{
					mensagem = ex.Message;
				}
				if (mensagem != null)
					yield return mensagem;
			}
		}

		private IEnumerable<Action<ItemDicionario, object>> ConsultarValidadores(ItemDicionario item)
		{
			yield return ValidarObrigatorio;
			yield return ValidarTamanhoMaximo;

			if (item.Validacoes == null) 
				yield break;

			foreach (var validador in item.Validacoes)
			{
				var val = validador;
				Action<ItemDicionario, object> validacao = (i, v) => val.Validar(v);
				yield return validacao;
			}
		}

	}

}
