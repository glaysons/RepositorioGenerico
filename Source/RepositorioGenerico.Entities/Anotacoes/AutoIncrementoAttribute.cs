using System;

namespace RepositorioGenerico.Entities.Anotacoes
{

	[AttributeUsage(AttributeTargets.Property)]
	public class AutoIncrementoAttribute : Attribute
	{

		public Incremento Incremento { get; set; }

		/// <summary>
		/// incremento - Nenhum: Você deverá fornecer o ID a ser gravado;
		/// incremento - Identity: O SGBD calculará automaticamente o próximo ID;
		/// incremento - Calculado: O Repositório Genérico calculará automaticamente o próximo ID;
		/// </summary>
		/// <param name="incremento"></param>
		public AutoIncrementoAttribute(Incremento incremento)
		{
			Incremento = incremento;
		}

	}

}
