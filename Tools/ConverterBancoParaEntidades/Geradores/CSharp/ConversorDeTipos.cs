using ConverterBancoParaEntidades.Constantes;

namespace ConverterBancoParaEntidades.Geradores.CSharp
{
	public static class ConversorDeTipos
	{

		public static string ConsultarTipoDoCampo(TipoCampo tipo)
		{
			switch (tipo)
			{
				case TipoCampo.String: return "string";
				case TipoCampo.Inteiro: return "int";
				case TipoCampo.Boolean: return "bool";
				case TipoCampo.Decimal: return "decimal";
				case TipoCampo.DateTime: return "System.DateTime";
				case TipoCampo.Double: return "double";
				case TipoCampo.Guid: return "System.Guid";
				case TipoCampo.Imagem: return "System.Drawing.Image";
				default:
					return "object";
			}
		}

	}
}
