﻿using RepositorioGenerico.Fake.Contextos;

namespace RepositorioGenerico.Fake
{
	public static class DbFactoryFake
	{
		public static IContextoFake CriarContexto()
		{
			return new ContextoFake();
		}

		public static Contextos.Tables.IContextoFake CriarContextoLegado()
		{
			return new Contextos.Tables.ContextoFake();
		}

	}
}
