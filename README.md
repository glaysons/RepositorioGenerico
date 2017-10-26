# RepositorioGenerico

RepositorioGenerico é um Framework ORM OpenSource escrito em C# para utilização em projetos com qualquer quantidade de tabelas.

Este ORM foi padronizado totalmente em português e já é utilizado em ambiente com mais de 1000 tabelas.

### Porque RepositorioGenerico? ###

 - Utiliza o padrão de projeto **Repository Pattern**
 - Suporte a **Testes Automatizados**
 - Integrado com injetor de dependências **AutoFac**
 - Mapeamento da estrutura dos objetos é feito sobre demanda
 - Utilização de anotações para definição dos padrões
 - Possibilidade de utilização das anotações no padrão **DataAnnotations** em inglês
 - Possibilidade de desenvolvimento de validadores na forma de anotações
 - Preparado para utilização com tabelas com múltiplos campos na chave
 - Permite utilização de contextos simples ou transacionais
 - Sincronização de relacionamentos *filhos* é feita automaticamente
 - Suporte apenas ao **Sql Server**, por enquanto!
 - Suporte a execução de **procedures**

## Como Utilizar ##

### Definir os Objetos ###

Todas as tabelas que serão utilizadas no contexto devem ser definidas e devem herdar o objeto **RepositorioGenerico.Entities.Entidade**

O nome da tabela e dos campos é obtida a partir do nome dos objetos e suas propriedades, assim, basta definir as regras estruturais do banco, como obrigatoriedade, tamanho e tipo de dado.
Se o nome das tabelas for diferente do nome dos objetos, basta configurar o nome utilizado no banco das anotações.

  
```
  [ValidadorPersonalizadoDeClasse]
  [ValidadorPersonalizadoDeClasse2]
  [Tabela("NomeDaTabelaNoBanco")]
  public class ObjetoDeTestes : Entidade
  {

    [Chave, Obrigatorio]
    [AutoIncremento(Incremento.Identity)]
    [Coluna(Ordem = 0, Nome = "CampoCodigo", NomeDoTipo = "int")]
    public int Codigo { get; set; }

    [Coluna(Ordem = 1, Nome = "CampoCodigoNulo", NomeDoTipo = "int")]
    public int? CodigoNulo { get; set; }

    [Obrigatorio, TamanhoMaximo(50)]
    [Coluna(Ordem = 2, Nome = "CampoNome", NomeDoTipo = "varchar")]
    [ValidadorPersonalizadoDePropriedade]
    [ValidadorPersonalizadoDePropriedade2]
    public string Nome { get; set; }

    [Obrigatorio, ValorPadrao(123.45)]
    [Coluna(Ordem = 3, Nome = "CampoDuplo", NomeDoTipo = "float")]
    public double Duplo { get; set; }

    [Coluna(Ordem = 4, Nome = "CampoDuploNulo", NomeDoTipo = "float")]
    public double? DuploNulo { get; set; }

    [Obrigatorio]
    [Coluna(Ordem = 5, Nome = "CampoDecimal", NomeDoTipo = "decimal")]
    public decimal Decimal { get; set; }

    [Coluna(Ordem = 6, Nome = "CampoDecimalNulo", NomeDoTipo = "decimal")]
    public decimal? DecimalNulo { get; set; }

    [Obrigatorio, ValorPadrao(true)]
    [Coluna(Ordem = 7, Nome = "CampoLogico", NomeDoTipo = "bit")]
    public bool Logico { get; set; }

    [Obrigatorio]
    [Coluna(Ordem = 8, Nome = "CampoDataHora", NomeDoTipo = "datetime")]
    public DateTime DataHora { get; set; }

    [Coluna(Ordem = 9, Nome = "CampoDataHoraNulo", NomeDoTipo = "datetime")]
    public DateTime? DataHoraNulo { get; set; }

    [PropriedadeDeLigacaoEstrangeira("Pai")]
    public virtual ICollection<FilhoDoObjetoDeTestes> Filhos { get; set; }

  }

  [Tabela("NomeDaTabelaFilhaDoBanco")]
  public class FilhoDoObjetoDeTestes : Entidade
  {

    [Chave, Obrigatorio, AutoIncremento(Incremento.Identity)]
    [Coluna(Ordem = 0, Nome = "CampoCodigoFilho", NomeDoTipo = "int")]
    public int CodigoFilho { get; set; }

    [Obrigatorio, TamanhoMaximo(50)]
    [Coluna(Ordem = 1, Nome = "CampoNomeFilho", NomeDoTipo = "varchar")]
    public string NomeFilho { get; set; }

    [Coluna(Ordem = 2, Nome = "CampoCodigoPai", NomeDoTipo = "int")]
    public int CodigoPai { get; set; }

    [ChaveEstrangeira("CodigoPai")]
    public virtual ObjetoDeTestes Pai { get; set; }

  }
```

### Criar um Contexto ###

```
  var cs = ConfigurationManager.ConnectionStrings["Conexao"].ConnectionString;
  IContexto contexto = RepositorioGenerico.SqlClient.Fabrica.CriarContexto(cs);
```

### Adicionar Registros ###

```
  var registro = new ObjetoDeTestes()
  {
    Nome = "Teste(" + Guid.NewGuid().ToString() + ")",
    DataHora = DateTime.Now,
    Decimal = 123.45M,
    Duplo = 234.56
  };

  IRepositorio<ObjetoDeTestes> repositorio = contexto.Repositorio<ObjetoDeTestes>();

  repositorio.Inserir(registro);

  contexto.Salvar();
```

### Consultar Registros ###

Este ORM ainda não possui uma estrutura de conversão de expressões Linq para Sql, assim, foi desenvolvido um formato fluente de desenvolvimento das consultas.

```
  var consultarUm = repositorio.Buscar.CriarQuery() // Ou .CriarProcedure("nomeDaProcedure")
    .AdicionarCondicao(c => c.Codigo).Igual(123);

  var registroSimples = repositorio.Buscar.Um(consultarUm);

  if (registroSimples != null)
    ToDo(registroSimples);

  var itensAtivosComMargem = repositorio.Buscar.CriarQuery()
    .AdicionarCondicao(c => c.Logico).Igual(true)
    .AdicionarCondicao(c => c.Codigo).Seja(Operadores.MenorOuIgual, 100)
    .AdicionarCondicao(c => c.Duplo).Entre(50.5, 100.0)
    .AdicionarOrdemDescendente(c => c.DataHora);

  foreach (var registroConsulta in repositorio.Buscar.Varios(itensAtivosComMargem))
    ToDo(registroConsulta);
```

Outra forma de realizar consultas no banco de dados é utilizar o método **Consultar()**. Este método exige que seja informado apenas os valores da chave da tabela, na ordem que é definida pelas propriedades do objeto.

```
  var registroDireto = repositorio.Consultar(123);
```

### Alterar Registros ###

```
  var registroParaAlteracao = repositorio.Buscar.Um(consultarUm);
  registroParaAlteracao.Nome = "Novo nome!!!";

  repositorio.Atualizar(registroParaAlteracao);

  contexto.Salvar();
```

### Excluir Registros ###

```
  var registroParaExclusao = repositorio.Buscar.Um(consultarUm);

  repositorio.Excluir(registroParaExclusao);
  contexto.Salvar();
```

### Validar Registros ###

Todo processo de inclusão/alteração gera uma validação automática nos objetos envolvidos, porém, estes sempre geram exceções.
Caso seja necessário fazer uma leitura dos erros encontrados, basta executar o método **Valido** do repositório.

```
  var registroSemNome = new ObjetoDeTestes()
  {
    DataHora = DateTime.Now,
    Decimal = 123.45M,
    Duplo = 234.56
  };

  var errosEncontrados = repositorio.Valido(registroSemNome);
  foreach (var erro in errosEncontrados)
    ToDo(erro);
```

É possível desativar as validações ao inserir/alterar itens, basta utilizar o método **DesativarValidacoes()** para desativar ou **AtivarValidacoes()** para ativar.

### Registros em Memória ###

Sempre que o contexto é salvo, os registros utilizados no processo de inclusão/alteração/exclusão ficam na memória, assim, caso seja necessário realizar a leitura dos mesmos, basta utilizar o método **Itens()**.

```
  foreach (var itemEmMemoria in repositorio.Itens())
    ToDo(itemEmMemoria);
```

### Criando novos Objetos ###

Sempre que precisar gerar novos objetos, utilize o método **Criar**, pois, o objeto é preenchido automaticamente com base em seus valores padrões.

```
  var novoRegistro = repositorio.Criar();
```

### Criando Testes ###

Recomendamos que desenvolva todas as classes de negócio dependendo apenas de **IRepositorio<*SeuObjeto*>***, assim, suas regras estarão menos dependente da estrutura de banco ou da transação. O objetivo é que estas regras não saibam se o repositório está utilizando o Sql Server ou Oracle, muito menos se é um banco de dados falso (***Fake***).

Para montar um contexto de testes, basta executar o método **RepositorioGenerico.Fake.FabricaFake.CriarContexto()** e em seguida adicionar todos os registros que deverão ser encontrados no contexto.

```
  var contextoFalso = RepositorioGenerico.Fake.FabricaFake.CriarContexto();
  contextoFalso.AdicionarRegistro(new ObjetoDeTestes() { Nome = "A", DataHora = DateTime.Now });
  contextoFalso.AdicionarRegistro(new ObjetoDeTestes() { Nome = "B", DataHora = DateTime.Now });
  contextoFalso.AdicionarRegistro(new ObjetoDeTestes() { Nome = "C", DataHora = DateTime.Now });
```

Vamos considerar a regra de negócios abaixo:

```
  public ObjetoDeTestes ConsultarNomesContendoB(IRepositorio<ObjetoDeTestes> tabelaDeObjetos)
  {
    var nomeContendoB = tabelaDeObjetos.Buscar.CriarQuery()
      .AdicionarCondicao(e => e.Nome).Contenha("B");
    return tabelaDeObjetos.Buscar.Um(nomeContendoB);
  }
```

Ao escrever o teste com o cenário descrito acima, basta validar se o método encontrou um objeto válido!

```
  var objeto = ConsultarNomesContendoB(contextoFalso.Repositorio<ObjetoDeTestes>());
  Assert.IsNotNull(objeto);
```
