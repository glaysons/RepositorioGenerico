using System;

namespace RepositorioGenerico.Exceptions
{
	public class NaoFoiPossivelConverterConsultaParaTipoException : Exception
	{
		public NaoFoiPossivelConverterConsultaParaTipoException(string tipo)
			: base(string.Concat("Não foi possível converter a consulta para o tipo [", tipo, "]! Verifique se as colunas estão na mesma ordem do objeto e com os mesmos tipos de dados."))
		{
		}
	}
}
