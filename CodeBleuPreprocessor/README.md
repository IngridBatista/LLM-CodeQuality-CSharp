# CodeBleuPreprocessor

API em **ASP.NET Core (.NET 10)** que prepara arquivos de código C# para o cálculo de **CodeBLEU**, normalizando-os em um único dataset de texto plano e o script Python que executa o cálculo final. É a etapa de pré-processamento e cálculo de métricas de um Trabalho de Conclusão de Curso (TCC) que avalia a qualidade do código C# gerado por LLMs, consumindo as soluções produzidas pelo [AIConnection](https://github.com/IngridBatista/AIConnection) e armazenadas no [GeneratedCodeByAI](https://github.com/IngridBatista/GeneratedCodeByAI).

## O que o repositório contém

1. **API de normalização** (`CodeBleuPreprocessor/`) — projeto ASP.NET Core que varre pastas de código `.cs` e gera datasets normalizados (sem comentários, sem `using`s, sem indentação), prontos para comparação via CodeBLEU.
2. **Datasets normalizados** (`NormalizedGeneratedCode/` e `NormalizedReferenceCode/`) — a saída já gerada pela API acima, versionada no repositório.
3. **Script de cálculo** (`run_codebleu.py`) — script Python que lê um arquivo do código gerado e um arquivo de referência e calcula as métricas de CodeBLEU entre eles.
4. **Resultados consolidados** (`results/codebleu_results.csv`) — uma linha por execução do CodeBLEU (4 modelos × 3 questões × 7 participantes = 84 combinações).

## API de normalização

A API expõe um único endpoint que:

1. Varre recursivamente uma pasta informada em busca de todos os arquivos `.cs`, ignorando artefatos de build/gerados automaticamente (`*.Designer.cs`, `*.g.cs`, `*.generated.cs`, `AssemblyInfo.cs`).
2. Ordena os arquivos encontrados de forma determinística (por caminho).
3. Para cada arquivo, faz o parsing do código com o **Roslyn** (`Microsoft.CodeAnalysis.CSharp`) e aplica as seguintes transformações:
   - Remove todos os comentários (linha única, múltiplas linhas e comentários de documentação `///`/`/** */`).
   - Remove todas as diretivas `using`.
   - "Desembrulha" o conteúdo de dentro de um `namespace` (tradicional ou file-scoped), deixando apenas os membros da classe quando o arquivo tem um único namespace de nível superior.
   - Normaliza todo o espaçamento e as quebras de linha, produzindo o código em uma única linha contínua, sem indentação.
4. Grava cada resultado como uma linha em um arquivo de saída (`outputFile`).

### Endpoint

```
POST /CodeBleuPreprocessor/geracao-dataset?sourceFolder={caminho}&outputFile={caminho}
```

| Parâmetro | Descrição |
|---|---|
| `sourceFolder` | Pasta raiz onde os arquivos `.cs` serão buscados recursivamente |
| `outputFile` | Caminho do arquivo de texto de saída, um "código normalizado" por linha, em UTF-8 sem BOM |

Retorna `200 OK` com uma mensagem de confirmação em caso de sucesso, ou `500` com os detalhes do erro em caso de falha.

### Exemplo de transformação

Entrada (`ClaudeArrayDifferenceSeniorParticipant1.cs`):

```csharp
namespace CLAUDE.ARRAY_DIFFERENCE.SENIOR.PARTICIPANT_1
{
    public class ClaudeArrayDifferenceSeniorParticipant1
    {
        public static int[] ObterElementosExclusivos(int[] array1, int[] array2)
        {
            // remove comentário
            return array1.Except(array2).ToArray();
        }
    }
}
```

Saída (uma linha no dataset, sem comentários, sem `namespace`, sem `using`, sem indentação):

```
public class ClaudeArrayDifferenceSeniorParticipant1 { public static int[] ObterElementosExclusivos(int[] array1, int[] array2) { return array1.Except(array2).ToArray(); } }
```

## Datasets normalizados

Os datasets abaixo já foram gerados pela API e estão versionados no repositório:

```
NormalizedGeneratedCode/
├── CLAUDE/
│   ├── ArrayDifference/Participant_1/GeneratedCodeClaudeArrayDifferenceParticipant1.txt
│   ├── ...Participant_2 ... Participant_7
│   ├── SequenceComparison/Participant_.../...
│   └── StringArrayEncoding/Participant_.../...
├── DEEPSEEK/  (mesma estrutura)
├── GEMINI/    (mesma estrutura)
└── GPT/       (mesma estrutura)

NormalizedReferenceCode/
├── ArrayDifference/ReferenceCodeArrayDifference.txt
├── SequenceComparison/ReferenceCodeSequenceComparison.txt
└── StringArrayEncoding/ReferenceCodeStringArrayEncoding.txt
```

- `NormalizedGeneratedCode/{MODELO}/{QUESTAO}/Participant_{N}/` — código normalizado gerado por cada LLM, para cada questão e participante (saída do [GeneratedCodeByAI](https://github.com/IngridBatista/GeneratedCodeByAI) já processada por esta API).
- `NormalizedReferenceCode/{QUESTAO}/` — código normalizado da solução de referência (especialista) de cada uma das 3 questões, usado como base de comparação para todos os modelos e participante ([ReferenceCode](https://github.com/IngridBatista/LLM-CodeQuality-CSharp/tree/main/ReferenceCode)).

> `CLAUDE/SequenceComparison` e `DEEPSEEK/SequenceComparison` não possuem a pasta `Participant_3`: a geração de código para esse par não produziu uma solução compilável, por isso não há CodeBLEU calculado para essas duas combinações (ver `results/codebleu_results.csv`, onde as métricas ficam em branco para essas duas linhas).

## Script de cálculo (`run_codebleu.py`)

O script usa a biblioteca [`codebleu`](https://pypi.org/project/codebleu/) para calcular as métricas entre um arquivo de referência e um arquivo de código gerado.

### Pré-requisitos

- Python 3.8 ou superior
- `pip`

### Passo a passo de instalação

1. Clone o repositório e entre na pasta:
   ```bash
   git clone https://github.com/IngridBatista/CodeBleuPreprocessor.git
   cd CodeBleuPreprocessor
   ```
2. (Recomendado) Crie e ative um ambiente virtual:
   ```bash
   python -m venv venv
   # Windows
   venv\Scripts\activate
   # Linux/macOS
   source venv/bin/activate
   ```
3. Instale a biblioteca `codebleu`:
   ```bash
   pip install codebleu
   ```

### Passo a passo de execução

O script **não recebe parâmetros por linha de comando**, os caminhos do arquivo de referência (`ref_path`) e do arquivo gerado a ser comparado (`hyp_path`) estão fixos diretamente no código-fonte. Isso significa que, para cada execução, é necessário editar manualmente essas duas linhas antes de rodar o script:

```python
ref_path = base_dir / "NormalizedReferenceCode" / "ArrayDifference" / "ReferenceCodeArrayDifference.txt"
hyp_path = base_dir / "NormalizedGeneratedCode" / "GEMINI" / "ArrayDifference" / "Participant_4" / "GeneratedCodeGeminiArrayDifferenceParticipant4.txt"
```

1. Abra `run_codebleu.py` em um editor de texto.
2. Ajuste `ref_path` para a questão que deseja avaliar (`ArrayDifference`, `SequenceComparison` ou `StringArrayEncoding`).
3. Ajuste `hyp_path` para o modelo, a questão e o participante desejados (`{MODELO}/{QUESTAO}/Participant_{N}/...`).
4. Salve o arquivo e execute:
   ```bash
   python run_codebleu.py
   ```
5. O script imprime no terminal as 5 métricas da combinação avaliada:
   ```
   CodeBLEU: 0.xxxxxxxx
   N-gram: 0.xxxxxxxx
   Weighted N-gram: 0.xxxxxxxx
   AST Match: 0.xxxxxxxx
   Dataflow Match: 0.xxxxxxxx
   ```
6. Repita os passos 2 a 5 para cada uma das 84 combinações de modelo/questão/participante, anotando o resultado impresso a cada execução.

Foi assim que `results/codebleu_results.csv` foi construído: 84 execuções manuais do script, uma por combinação, com os valores impressos no terminal copiados para a planilha de resultados.

## Resultados do CodeBLEU

Os resultados completos das 84 execuções (uma por combinação de modelo × questão × participante) estão em [`results/codebleu_results.csv`](results/codebleu_results.csv), com uma linha por combinação e as colunas:

| Coluna | Descrição |
|---|---|
| `llm` | Modelo avaliado (`CLAUDE`, `DEEPSEEK`, `GEMINI`, `GPT`) |
| `question` | Exercício (`ArrayDifference`, `SequenceComparison`, `StringArrayEncoding`) |
| `participant` | Número do participante (1 a 7) |
| `codebleu` | Score final de CodeBLEU (0–1) |
| `ngram_match` | N-gram Match |
| `weighted_ngram_match` | Weighted N-gram Match |
| `ast_match` | AST Match |
| `dataflow_match` | Dataflow Match |

As duas combinações sem código gerado válido (`CLAUDE`/`SequenceComparison`/participante 3 e `DEEPSEEK`/`SequenceComparison`/participante 3) aparecem com as colunas de métrica em branco.

**Médias de CodeBLEU por modelo** (considerando apenas as combinações com resultado válido):

| Modelo | CodeBLEU médio |
|---|---|
| DeepSeek | 0.2629 |
| Claude | 0.2399 |
| GPT | 0.2226 |
| Gemini | 0.2206 |

## Stack técnica

- **.NET 10** / ASP.NET Core Web API — normalização do código-fonte
- `Microsoft.CodeAnalysis` (Roslyn) — parsing
- Swagger / OpenAPI habilitado para exploração e teste do endpoint
- **Python** + [`codebleu`](https://pypi.org/project/codebleu/) — cálculo das métricas CodeBLEU

## Estrutura do projeto

```
CodeBleuPreprocessor/
├── CodeBleuPreprocessor/                 # API .NET de normalização
│   ├── Controllers/
│   │   └── CodeBleuPreprocessorController.cs   # Endpoint POST /geracao-dataset
│   ├── Service/
│   │   └── CodeBleuPreprocessorService.cs      # Lógica de normalização via Roslyn
│   ├── CodeBleuPreprocessor.http               # Requisições de exemplo
│   └── Program.cs                              # Configuração do pipeline HTTP / Swagger
├── NormalizedGeneratedCode/               # Datasets normalizados por modelo/questão/participante
├── NormalizedReferenceCode/               # Datasets normalizados das soluções de referência
├── results/
│   └── codebleu_results.csv               # Resultados das 84 execuções do CodeBLEU
└── run_codebleu.py                        # Script de cálculo do CodeBLEU (caminhos manuais)
```

## Contexto

Este repositório é a etapa final do pipeline de avaliação por similaridade do TCC: normaliza o código bruto gerado pelos LLMs (armazenado no [GeneratedCodeByAI](https://github.com/IngridBatista/GeneratedCodeByAI)) e calcula, para cada combinação de modelo, questão e participante, as métricas de CodeBLEU (AST Match, Dataflow Match, N-gram Match e Weighted N-gram Match) contra as soluções de referência do especialista.
