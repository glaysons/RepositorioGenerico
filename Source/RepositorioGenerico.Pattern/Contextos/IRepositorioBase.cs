using System.Collections.Generic;

namespace RepositorioGenerico.Pattern.Contextos
{
	public interface IRepositorioBase<in TLista, TItem>
	{

		int Quantidade { get; }

		void AtivarValidacoes();

		void DesativarValidacoes();

		IEnumerable<TItem> Itens();

		TItem Consultar(params object[] chave);

		TItem Criar();

		void Validar(TItem registro);

		IEnumerable<string> Valido(TItem registro);

		void Inserir(TLista registros);

		void Inserir(TItem registro);

		void Atualizar(TLista registros);

		void Atualizar(TItem registro);

		void Excluir(TItem registro);

	}
}
