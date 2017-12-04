using System;

namespace RepositorioGenerico.Entities.Anotacoes
{

	[AttributeUsage(AttributeTargets.Property)]
	public class ColunaAttribute : Attribute
	{

		public string Nome { get; set; }

		public int Ordem { get; set; }

		public string NomeDoTipo { get; set; }

		public ColunaAttribute()
		{
		}

		public ColunaAttribute(string nome)
		{
			Nome = nome;
		}

	}

}
