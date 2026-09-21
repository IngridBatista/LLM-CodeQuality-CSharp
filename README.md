# GeneratedCodeByAI

Repositório de dados brutos de um Trabalho de Conclusão de Curso (TCC) que avalia a qualidade do código C# gerado por diferentes LLMs. Ele armazena o **código-fonte gerado automaticamente** por quatro modelos de linguagem (**GPT**, **Claude**, **Gemini** e **DeepSeek**) a partir de prompts de programação submetidos por participantes reais, produzido pela API [AIConnection](https://github.com/IngridBatista/AIConnection).

## O que este repositório contém

Cada solução de programação gerada por um modelo, para um determinado exercício e um determinado participante, fica isolada em seu próprio projeto .NET (`.csproj`), permitindo compilar, analisar e testar cada saída de forma independente, sem interferência entre os experimentos.

A raiz do repositório reúne:

- **84 projetos** (pastas), um para cada combinação de **modelo × questão × participante**
- Um arquivo de solução `GeneratedCodeByAI.slnx`, que referencia todos os 84 projetos e permite abrir o conjunto completo de uma vez no Visual Studio
- 104 arquivos `.cs` no total, onde a maioria dos projetos contém uma única classe de solução, mas alguns modelos geraram classes auxiliares adicionais (ex.: exceções customizadas)

## Convenção de nomes

Cada pasta de projeto segue o padrão:

```
{MODELO}.{QUESTAO}.{SENIORIDADE}.PARTICIPANT_{N}
```

Exemplo: `CLAUDE.ARRAY_DIFFERENCE.SENIOR.PARTICIPANT_1` → solução gerada pelo Claude, para o exercício "ArrayDifference", a partir do prompt de um participante sênior (Participant 1).

O namespace, e o nome da classe dentro do `.cs` em situações em que o código gerado não criou o nome da classe, seguem a mesma convenção do diretório, garantindo rastreabilidade total entre pasta, projeto e código.

### Modelos

| Modelo | Prefixo da pasta |
|---|---|
| GPT | `GPT.*` |
| Claude | `CLAUDE.*` |
| Gemini | `GEMINI.*` |
| DeepSeek | `DEEPSEEK.*` |

### Questões (exercícios de programação)

- **ArrayDifference** — obtenção dos elementos exclusivos entre dois arrays
- **SequenceComparison** — comparação de sequências
- **StringArrayEncoding** — codificação de arrays de strings (matriz de strings)

### Participantes e senioridade

7 participantes, cujos prompts variam em senioridade: 5 sêniores (Participant 1 a 5), 1 pleno (Participant 6) e 1 júnior (Participant 7).

## Estrutura de um projeto individual

```
CLAUDE.ARRAY_DIFFERENCE.SENIOR.PARTICIPANT_1/
├── CLAUDE.ARRAY_DIFFERENCE.SENIOR.PARTICIPANT_1.csproj   # net10.0, Nullable/ImplicitUsings habilitados
└── ClaudeArrayDifferenceSeniorParticipant1.cs            # classe com a solução gerada pelo modelo
```

Todos os projetos usam o mesmo `TargetFramework` (`net10.0`) e as mesmas configurações (`Nullable`/`ImplicitUsings` habilitados), o que padroniza a compilação e viabiliza a comparação justa entre as soluções.

## Como este repositório é usado no TCC

O código aqui armazenado é a matéria-prima da fase de avaliação do estudo, que aplica:

- **CodeBLEU** — similaridade estrutural/sintática entre cada solução gerada e a solução de referência (especialista), calculada pelo [CodeBleuPreprocessor](https://github.com/IngridBatista/CodeBleuPreprocessor)
- **SonarQube** — análise estática de qualidade e code smells, executada diretamente sobre este repositório e, com a mesma configuração, sobre o código de referência do especialista ([code-metrics](https://github.com/reginaldomota/code-metrics)), para fins de comparação

## Análise estática com SonarQube

A análise de qualidade estática é feita com o **SonarQube Community Edition**, rodando localmente via Docker, contra a solução `GeneratedCodeByAI.slnx` completa (os 84 projetos).

### Pré-requisitos

- Docker
- .NET SDK (mesma versão do `TargetFramework` dos projetos, `net10.0`)
- Java (exigido pelo `dotnet-sonarscanner` para se comunicar com o servidor SonarQube)

### 1. Instalação do SonarQube Community via Docker

Suba o servidor SonarQube localmente com um único contêiner:

```bash
docker run -d --name sonarqube -p 9000:9000 sonarqube:community
```

Aguarde a inicialização do contêiner e acesse `http://localhost:9000` no navegador. Faça login com as credenciais padrão (`admin` / `admin`) e defina uma nova senha quando solicitado.

> Para manter os dados entre reinicializações do contêiner, monte volumes para `/opt/sonarqube/data`, `/opt/sonarqube/logs` e `/opt/sonarqube/extensions`.

### 2. Criação do projeto

No painel do SonarQube:

1. Clique em **Create Project** → **Manually**.
2. Informe a **Project display name** e a **Project key** (ex.: `GeneratedCodeByAI`).
3. Escolha o método de análise **Locally**.
4. Gere um **token** de autenticação para o projeto (usado no comando `sonar.login` da análise) e guarde-o com segurança.

### 3. Configuração da linguagem C#

O SonarQube detecta a linguagem automaticamente a partir dos arquivos analisados, mas é necessário garantir que o **Quality Profile** ativo para o projeto seja o de **C#**:

1. Acesse **Quality Profiles** no menu superior.
2. Confirme (ou crie) um profile para a linguagem **C#**.
3. Em **Project Settings → Quality Profile**, associe esse profile de C# ao projeto `GeneratedCodeByAI`.

### 4. Configuração das regras do projeto

O Quality Profile de C# usado neste projeto foi customizado para ativar exclusivamente as regras abaixo (em **Quality Profiles → [profile C#] → Activate More Rules**, busque cada código e ative individualmente):

| Regra | Descrição |
|---|---|
| S100 | Os métodos e propriedades devem ser nomeados em PascalCase |
| S101 | Os tipos devem ser nomeados em PascalCase |
| S103 | As linhas não devem ter comprimento excessivo |
| S104 | Os arquivos não devem ter muitas linhas de código |
| S107 | Os métodos não devem ter muitos parâmetros |
| S125 | Trechos de código não devem ser comentados |
| S134 | Instruções de controle de fluxo não devem ser aninhadas em excesso |
| S138 | As funções não devem ter muitas linhas de código |
| S1066 | Instruções "if" mescláveis devem ser combinadas |
| S1104 | Os campos não devem ser acessíveis ao público |
| S1118 | Classes utilitárias não devem ter construtores públicos |
| S1125 | Os literais booleanos não devem ser redundantes |
| S1128 | O uso desnecessário de "using" deve ser removido |
| S1144 | Tipos ou membros privados não utilizados devem ser removidos |
| S1172 | Os parâmetros de método não utilizados devem ser removidos |
| S1186 | Os métodos não devem estar vazios |
| S1200 | As classes não devem ser agrupadas em excesso com outras classes |
| S1210 | Operadores de comparação devem ser sobrescritos ao implementar IComparable |
| S1301 | Instruções "switch" devem ter pelo menos 3 cláusulas "case" |
| S1312 | Os campos do logger devem ser "private static readonly" |
| S1450 | Campos privados usados apenas localmente devem se tornar variáveis locais |
| S1481 | Variáveis locais não utilizadas devem ser removidas |
| S1541 | Os métodos e propriedades não devem ser muito complexos |
| S1854 | Atribuições não utilizadas devem ser removidas |
| S2187 | As classes de teste devem conter pelo menos um caso de teste |
| S2234 | Argumentos devem ser passados na mesma ordem que os parâmetros |
| S2326 | Os parâmetros de tipo não utilizados devem ser removidos |
| S2344 | Nomes de enumeração não devem ter sufixos "Flags" ou "Enum" |
| S2699 | Os testes devem incluir asserções |
| S2955 | Parâmetros genéricos não restritos não devem ser comparados a "null" |
| S3242 | Os parâmetros do método devem ser declarados com tipos base |
| S3459 | Os membros não atribuídos devem ser removidos |
| S3776 | A complexidade cognitiva dos métodos não deve ser muito alta |
| S3875 | O operador "==" não deve ser sobrecarregado em tipos de referência |
| S4201 | Operadores de "is" não devem ser usados de forma redundante |

### 5. Executando a análise

Instale o `dotnet-sonarscanner` como ferramenta global do .NET (apenas na primeira vez):

```bash
dotnet tool install --global dotnet-sonarscanner
```

Na raiz do repositório (onde está `GeneratedCodeByAI.slnx`), rode os três comandos em sequência:

```bash
dotnet sonarscanner begin /k:"NomeProjeto" /n:"NomeProjeto" /d:sonar.host.url="UrlProjeto" /d:sonar.login="Login"

dotnet build

dotnet sonarscanner end /d:sonar.login="Login"
```

| Placeholder | Descrição |
|---|---|
| `NomeProjeto` | Project key/nome cadastrado no passo 2 (ex.: `GeneratedCodeByAI`) |
| `UrlProjeto` | URL do servidor SonarQube local (ex.: `http://localhost:9000`) |
| `Login` | Token de autenticação gerado no passo 2 |

Ao final da execução, os resultados ficam disponíveis no dashboard do projeto em `http://localhost:9000`, com o detalhamento de cada regra violada por arquivo/classe.

## Análise comparativa com o código de referência (especialista)

Para que os resultados do SonarQube sejam comparáveis, a mesma configuração descrita acima também é aplicada ao repositório [code-metrics](https://github.com/reginaldomota/code-metrics), que contém as soluções de referência escritas manualmente pelo especialista (sem uso de IA) para os três exercícios do estudo, diferença entre arrays (`ArrayDifference`), comparação de sequências (`SequenceComparison`) e manipulação de matrizes de strings (`StringArrayEncoding`). Isso gera um baseline de qualidade contra o qual o código dos quatro LLMs é comparado.

Diferente deste repositório, o `code-metrics` é um único projeto .NET console (`code-metrics.sln` / `code-metrics.csproj`, .NET 8.0) que reúne as três soluções em uma mesma base de código, então a análise cobre o projeto inteiro de uma só vez.

1. **Instalação do SonarQube**: reutiliza a mesma instância local já criada via Docker (`docker run ... sonarqube:community`), não é necessário subir um segundo servidor.
2. **Criação do projeto**: crie um novo projeto no SonarQube exclusivo para o código de referência (ex.: project key `CodeMetrics` ou `code-metrics-specialist`), seguindo os mesmos passos de **Create Project → Manually** e gerando um novo token para esse projeto.
3. **Configuração da linguagem C#**: associe o **mesmo Quality Profile de C#** configurado para o `GeneratedCodeByAI` a este novo projeto, em **Project Settings → Quality Profile**, garantindo que exatamente as mesmas 35 regras listadas acima sejam avaliadas.
4. **Configuração das regras**: nenhuma regra adicional é necessária, por reutilizar o mesmo Quality Profile do passo anterior, o conjunto de regras já fica idêntico ao aplicado no código gerado por IA.
5. **Execução da análise**: clone o repositório `code-metrics` e, na sua raiz (onde está `code-metrics.sln`), rode os mesmos três comandos, trocando apenas o `NomeProjeto` (e, se necessário, o `Login`, caso use um token diferente por projeto):

   ```bash
   dotnet sonarscanner begin /k:"CodeMetrics" /n:"CodeMetrics" /d:sonar.host.url="UrlProjeto" /d:sonar.login="Login"

   dotnet build

   dotnet sonarscanner end /d:sonar.login="Login"
   ```

Com os dois projetos (`GeneratedCodeByAI` e `CodeMetrics`) analisados sob o mesmo Quality Profile, no mesmo servidor SonarQube, os resultados de cada regra passam a ser diretamente comparáveis entre o código gerado por cada LLM e o código de referência do especialista.

## Extração dos resultados via API (Postman)

Além do dashboard web, os resultados de cada análise podem ser extraídos diretamente pela **Web API REST do SonarQube**, usando o Postman, e exportados para planilha para avaliação posterior, o mesmo processo se aplica tanto ao projeto `GeneratedCodeByAI` quanto ao `CodeMetrics`, bastando trocar o `componentKeys`.

1. No Postman, crie uma requisição `GET` para o endpoint de busca de ocorrências (issues):
   ```
   GET http://localhost:9000/api/issues/search?componentKeys={NomeProjeto}&ps=500
   ```
   - `componentKeys` — project key do projeto analisado (ex.: `GeneratedCodeByAI` ou `CodeMetrics`)
   - `ps` — quantidade de resultados por página (máximo 500; use o parâmetro `p` para paginar caso o projeto tenha mais ocorrências que isso)
2. Configure a autenticação da requisição na aba **Authorization** do Postman como **Basic Auth**, usando o token gerado na criação do projeto como *Username* e deixando o campo *Password* em branco.
3. Envie a requisição. A resposta em JSON traz, para cada ocorrência encontrada, campos como `rule` (código da regra, ex.: `csharpsquid:S1481`), `component` (arquivo/classe onde ocorreu), `severity` e `message`.
4. Exporte a resposta do Postman (botão **Save Response → Save as Example** ou copiando o JSON) e converta os dados para planilha (Excel/Google Sheets), organizando por modelo, questão e participante, mesma granularidade usada na tabela de resultados do CodeBLEU.
5. Repita o processo trocando o `componentKeys` para `CodeMetrics`, obtendo assim os dados do código de referência do especialista na mesma planilha, lado a lado com os dados de cada LLM, para a avaliação comparativa.

> Para obter contagens agregadas por regra em vez da lista completa de ocorrências, o endpoint `GET /api/measures/component?component={NomeProjeto}&metricKeys=code_smells,violations,ncloc` também pode ser usado no Postman, retornando métricas resumidas do projeto.

## Origem dos dados

O código deste repositório não foi escrito manualmente: cada arquivo `.cs` é a extração direta da resposta de um LLM, obtida via API do projeto [AIConnection](https://github.com/IngridBatista/AIConnection) e salva sem alterações de lógica (apenas formatação/indentação via Roslyn).
