using RepositorioGenerico.Entities.Mapas;

namespace RepositorioGenerico.Test
{
	public class ObjetoMapeadoComChaveDuplaMap : MapaEntidade<ObjetoMapeadoComChaveDupla>
	{
		public override void Referenciar(IMapa<ObjetoMapeadoComChaveDupla> builder)
		{
			builder.Tabela<ObjetoComChaveDupla>()
				.Campo(c => c.ChaveBase).Propriedade(p => p.MapeadoComChaveBase)
				.Campo(c => c.ChaveAutoIncremento).Propriedade(p => p.MapeadoComChaveAutoIncremento)
				.Campo(c => c.Nome).Propriedade(p => p.MapeadoComNome);
		}
	}
}
