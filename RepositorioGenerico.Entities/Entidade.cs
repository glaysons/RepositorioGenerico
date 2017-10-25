using System;
using RepositorioGenerico.Entities.Anotacoes;

namespace RepositorioGenerico.Entities
{

	public class Entidade : IEntidade, ICloneable
	{

		[NaoMapeado]
		public EstadosEntidade EstadoEntidade { get; set; }

		public Entidade()
		{
			EstadoEntidade = EstadosEntidade.Novo;
		}

		public object Clone()
		{
			var clone = MemberwiseClone();
			((IEntidade)clone).EstadoEntidade = EstadosEntidade.Novo;
			return clone;
		}

	}

}
