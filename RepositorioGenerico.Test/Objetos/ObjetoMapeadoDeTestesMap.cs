using RepositorioGenerico.Entities.Mapas;

namespace RepositorioGenerico.Test.Objetos
{

	public class ObjetoMapeadoDeTestesMap : MapaEntidade<ObjetoMapeadoDeTestes>
	{
		public override void Referenciar(IMapa<ObjetoMapeadoDeTestes> builder)
		{
			builder.Tabela<ObjetoDeTestes>()
				.Campo(c => c.Codigo).Propriedade(p => p.MapeadoComCodigo)
				.Campo(c => c.CodigoNulo).Propriedade(p => p.MapeadoComCodigoNulo)
				.Campo(c => c.Nome).Propriedade(p => p.MapeadoComNome)
				.Campo(c => c.Duplo).Propriedade(p => p.MapeadoComDuplo)
				.Campo(c => c.DuploNulo).Propriedade(p => p.MapeadoComDuploNulo)
				.Campo(c => c.Decimal).Propriedade(p => p.MapeadoComDecimal)
				.Campo(c => c.DecimalNulo).Propriedade(p => p.MapeadoComDecimalNulo)
				.Campo(c => c.Logico).Propriedade(p => p.MapeadoComLogico)
				.Campo(c => c.DataHora).Propriedade(p => p.MapeadoComDataHora)
				.Campo(c => c.DataHoraNulo).Propriedade(p => p.MapeadoComDataHoraNulo)
				.Campo(c => c.Filhos).Propriedade(p => p.MapeadoComFilhos);
		}
	}

}
