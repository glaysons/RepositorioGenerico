using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RepositorioGenerico.Dictionary.Itens;
using RepositorioGenerico.Dictionary.Relacionamentos;
using RepositorioGenerico.Entities;
using RepositorioGenerico.Framework.Helpers;
using RepositorioGenerico.Pattern.Contextos;
using RepositorioGenerico.Search;

namespace RepositorioGenerico.SqlClient.Contextos
{
	internal class FilhosRepositorio<TObjeto> where TObjeto : class, IEntidade
	{

		private readonly Contexto _contexto;
		private readonly IEnumerable<ItemDicionario> _camposFilho;

		public FilhosRepositorio(IContexto contexto, IEnumerable<ItemDicionario> camposFilho)
		{
			_contexto = contexto as Contexto;
			_camposFilho = camposFilho;
		}

		public void InserirFilhos(TObjeto model, object[] chaveDoModel)
		{
			foreach (var campo in _camposFilho)
			{
				var itens = (ICollection)campo.Propriedade.GetValue(model, null);
				if (itens == null)
					continue;
				var repositorio = _contexto.Repositorio(campo.Ligacao.Dicionario.Tipo);
				foreach (var item in itens)
				{
					campo.Ligacao.AplicarChaveAscendente(chaveDoModel, item);
					repositorio.Inserir(item);
				}
			}
		}

		public void AtualizarFilhos(TObjeto model, object[] chaveDoModel)
		{
			foreach (var campo in _camposFilho)
			{
				var filhosAtuais = (ICollection)campo.Propriedade.GetValue(model, null);
				if (filhosAtuais == null)
					continue;
				var repositorio = _contexto.Repositorio(campo.Ligacao.Dicionario.Tipo);
				var filhosAntigos = ConsultarFilhosAtuais(model, campo);
				if (filhosAntigos != null)
					foreach (var filho in filhosAntigos)
					{
						var chaveAntiga = campo.Ligacao.Dicionario.ConsultarValoresDaChave(filho);
						if (ChaveAtualFoiExcluida(campo.Ligacao, filhosAtuais, chaveAntiga))
							repositorio.Excluir(filho);
					}
				foreach (var item in filhosAtuais)
				{
					var filho = (IEntidade)item;
					if (campo.Ligacao.PossuiChaveAscendente(chaveDoModel, filho))
						repositorio.Atualizar(filho);
					else
					{
						campo.Ligacao.AplicarChaveAscendente(chaveDoModel, filho);
						repositorio.Inserir(filho);
					}
				}
			}
		}

		private ICollection<object> ConsultarFilhosAtuais(TObjeto model, ItemDicionario campo)
		{
			var expressao = ExpressionHelper.CriarExpressaoDeConsultaDaPropriedade<TObjeto>(campo.Propriedade);
			var buscador = (Buscador<TObjeto>) _contexto.Repositorio<TObjeto>().Buscar;
			return buscador.ConsultarPropriedade(model, expressao)?.Cast<object>().ToList();
		}

		private bool ChaveAtualFoiExcluida(Relacionamento ligacao, ICollection itens, object[] chaveDoModel)
		{
			foreach (var item in itens)
			{
				var chave = ligacao.Dicionario.ConsultarValoresDaChave(item);
				if (chaveDoModel.SequenceEqual(chave))
					return false;
			}
			return true;
		}

		public void ExcluirFilhos(TObjeto model)
		{
			foreach (var campo in _camposFilho)
			{
				var repositorio = _contexto.Repositorio(campo.Ligacao.Dicionario.Tipo);
				var itens = (ICollection)campo.Propriedade.GetValue(model, null);
				if (itens == null)
					continue;
				foreach (var item in itens)
					repositorio.Excluir(item);
			}
		}

	}
}