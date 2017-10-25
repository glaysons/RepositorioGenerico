using RepositorioGenerico.Entities.Mapas;

namespace RepositorioGenerico.Test
{
	public class FilhoMapeadoDoObjetoMapeadoDeTestesMap : MapaEntidade<FilhoMapeadoDoObjetoMapeadoDeTestes>
	{
		public override void Referenciar(IMapa<FilhoMapeadoDoObjetoMapeadoDeTestes> builder)
		{
			builder.Tabela<FilhoDoObjetoDeTestes>()
				.Campo(c => c.CodigoFilho).Propriedade(p => p.MapeadoComCodigoFilho)
				.Campo(c => c.NomeFilho).Propriedade(p => p.MapeadoComNomeFilho)
				.Campo(c => c.CodigoPai).Propriedade(p => p.MapeadoComCodigoPai)
				.Campo(c => c.Pai).Propriedade(p => p.MapeadoComPai);
		}
	}
}
