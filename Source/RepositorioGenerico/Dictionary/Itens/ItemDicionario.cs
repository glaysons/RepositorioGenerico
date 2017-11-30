using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using RepositorioGenerico.Dictionary.Relacionamentos;
using RepositorioGenerico.Entities.Anotacoes;
using RepositorioGenerico.Entities.Anotacoes.Validadores;

namespace RepositorioGenerico.Dictionary.Itens
{
	public class ItemDicionario
	{

		public int Id { get; private set; }

		public string Alias { get; private set; }

		public string Nome { get; private set; }

		public string AliasOuNome { get { return Alias ?? Nome; } }

		public DbType TipoBanco { get; private set; }

		public Type TipoLocal { get; private set; }

		public bool Chave { get; private set; }

		public bool Obrigatorio { get; private set; }

		public int TamanhoMinimo { get; private set; }

		public int TamanhoMaximo { get; private set; }

		public int Ordem { get; private set; }

		public Incremento OpcaoGeracao { get; private set; }

		public object ValorPadrao { get; private set; }

		public bool Mapeado { get; private set; }

		public PropertyInfo Propriedade { get; private set; }

		public IList<IValidadorPropriedadeAttribute> Validacoes { get; private set; }

		public Relacionamento Ligacao { get; private set; }

		public ItemDicionario(int id, string alias, string nome, DbType tipoBanco, Type tipoLocal, bool chave, bool obrigatorio, 
			int tamanhoMinimo, int tamanhoMaximo, int ordem, Incremento opcaoGeracao, object valorPadrao, bool mapeado, PropertyInfo propriedade,
			IList<IValidadorPropriedadeAttribute> validacoes, Relacionamento ligacao)
		{
			Id = id;
			Alias = (string.IsNullOrEmpty(alias))
				? null
				: alias;
			Nome = nome;
			TipoBanco = tipoBanco;
			TipoLocal = tipoLocal;
			Chave = chave;
			Obrigatorio = obrigatorio || chave;
			TamanhoMinimo = tamanhoMinimo;
			TamanhoMaximo = tamanhoMaximo;
			Ordem = ordem;
			OpcaoGeracao = opcaoGeracao;
			ValorPadrao = valorPadrao;
			Mapeado = mapeado;
			Propriedade = propriedade;
			Validacoes = validacoes;
			Ligacao = ligacao;
		}

	}
}
