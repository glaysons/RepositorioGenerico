using System.Collections.Generic;

namespace RepositorioGenerico.Pattern.Contextos
{
	public interface IRepositorioObject
	{

		int Quantidade { get; }

		void AtivarValidacoes();

		void DesativarValidacoes();

		IEnumerable<object> Itens();

		object Criar();

		void Validar(object registro);

		IEnumerable<string> Valido(object registro);

		void Inserir(object registro);

		void Atualizar(object registro);

		void Excluir(object registro);

	}
}
