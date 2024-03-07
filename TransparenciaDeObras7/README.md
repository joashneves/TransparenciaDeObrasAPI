
#  Back End Transparencia De Obras

### Descrição

Uma API para armazenar e exibir obras que estão sendo realizadas pela prefeitura.


## Documentação da API

A Api possui politica de Cors

# Obras

#### Retorna todas as obras

```http
  GET / Obra
```

| Parâmetro   | Tipo       | Descrição                           |
| :---------- | :--------- | :---------------------------------- |
| `id` | `json` | Retorna lista de **Todas** as obras no banco de dados |

#### Retorna todos as obras publicos

```http
  GET /Obra/public/
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `id`      | `json` | Retorna todas as obras que estão listada com o valor PublicadoDetalhe == True |


#### Retorna todas as obras publicas com paginação

```http
  GET /Obra/Pag/
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `pageNumber&pageQuantity`      | `json` | Retorna obras com paginação |


#### Adiciona a obras no banco de dados

```http
  POST / Obra
```

| Parâmetro   | Tipo       | Descrição                           |
| :---------- | :--------- | :---------------------------------- |
| `id` | `long` |  Valor requerido da obra para enviar para o banco de dados |
| `NomeDetalhe` | `string` |  Valor requerido da obra para enviar para o banco de dados |
| `NumeroDetalhe` | `int` |  Valor requerido da obra para enviar para o banco de dados |
| `SituacaoDetalhe` | `string` |  Valor requerido da obra para enviar para o banco de dados |
| `PublicadoDetalhe` | `bool` |  Valor requerido da obra para enviar para o banco de dados |
| `PublicacaoData` | `DateTime` |  Valor requerido da obra para enviar para o banco de dados|
| `OrgaoPublicoDetalhe` | `string` |  Valor requerido da obra para enviar para o banco de dados |
| `TipoObraDetalhe` | `string` |  Valor requerido da obra para enviar para o banco de dados |
| `NomeContratadaDetalhe` | `string` |  Valor requerido da obra para enviar para o banco de dados |
| `PrazoInicial` | `int` |  Valor requerido da obra para enviar para o banco de dados |
| `PrazoFinal` | `int` |  Valor requerido da obra para enviar para o banco de dados |
| `ValorEmpenhado` | `double` |  Valor requerido da obra para enviar para o banco de dados |
| `ValorLiquidado` | `double` |  Valor requerido da obra para enviar para o banco de dados |
| `CnpjContratadaObraDetalhe` | `string` |  Valor requerido da obra para enviar para o banco de dados|
| `AnoDetalhe` | `int` |  Valor requerido da obra para enviar para o banco de dados|
| `Licitacao` | `string` |  Valor requerido da obra para enviar para o banco de dados|
| `Contrato` | `string` |  Valor requerido da obra para enviar para o banco de dados|
| `LocalizacaoobraDetalhe` | `string` |  Valor requerido da obra para enviar para o banco de dados |

#### Cria obras

#### Atualiza obra

```http
  PUT /Obra/{id}
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `id`      | `json` | Atualiza o Json da obra |

### Medicao
