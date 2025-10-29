# Avaliação Técnica – Fundamentos Backend (.NET 8)

## Ambiente Necessário
- .NET SDK 8.0+
- Visual Studio 2022 ou Visual Studio Code
- xUnit, FluentAssertions e Moq (restaurados via `dotnet restore`)
- EF Core InMemory e Polly para persistência e políticas de retry

---

## Estrutura do Projeto
```
src/
 └── Core/
      ├── Algorithms/
      ├── Concurrency/
      ├── Design/
      ├── Integration/
      ├── Observability/
      └── Persistence/

tests/
 └── Core.Tests/
      ├── Algorithms/
      ├── Concurrency/
      ├── Design/
      ├── Integration/
      ├── Observability/
      └── Persistence/
```

---

## Comandos Principais
```bash
# Restaurar dependências
dotnet restore

# Compilar a solução
dotnet build

# Executar todos os testes automatizados
dotnet test
```

---

## Decisões Técnicas Relevantes

| Tema | Decisão |
|------|----------|
| **Framework** | Projeto direcionado ao .NET 8.0, conforme solicitado no enunciado |
| **Estrutura** | Separação entre `src/` e `tests/` para isolar código de produção e testes |
| **Global Usings** | Definidos em `Core` e `Core.Tests` para evitar duplicações de `using` e manter consistência |
| **Padrão de Testes** | Aplicado o padrão AAA (Arrange, Act, Assert) em todos os testes |
| **Concorrência** | Utilização de `SemaphoreSlim` para controle de paralelismo e evitar race conditions |
| **Cancelamento** | Implementado `CancellationToken` para interrupção ordenada |
| **Tratamento de Erros** | Serviço orquestrador captura exceções individuais e agrega resultados válidos |
| **Persistência** | Implementado `EF Core InMemory` para simular operações CRUD |
| **Integração Externa** | Uso de `HttpClientFactory` com política de retry exponencial (Polly) |
| **Observabilidade** | Logs estruturados com `operationId` e métricas de execução e erro |
| **Testes e Mocking** | Moq utilizado para simular dependências e validar logs e métricas |

---

## Justificativas e Trade-offs

1. **Class Library ao invés de Console Application**  
   Optou-se por uma biblioteca de classes (`classlib`) para manter o código desacoplado de interface, facilitando testes e reuso futuro (ex: API, console, worker).

2. **Uso de `IEnumerable<T>` em algoritmos**  
   Essa escolha oferece flexibilidade para receber qualquer tipo de coleção sem impor dependência direta a listas, mantendo baixo acoplamento e boa coesão.

3. **Estrutura modular (Clean Architecture inspired)**  
   Separar domínios por pastas (`Algorithms`, `Design`, `Persistence`, etc.) facilita evolução, manutenibilidade e testes unitários.

4. **Complexidade de algoritmos**  
   - **FindFirstRepeated** → O(n) tempo, O(n) espaço.  
   - **LargestIncreasingSequenceFinder** → O(n) tempo, O(1) espaço adicional.

5. **Trade-offs técnicos**  
   - *GlobalUsings*: menos repetição, porém reduz visibilidade de dependências por arquivo.  
   - *EF Core InMemory*: rápido e simples, mas não replica fielmente restrições de banco real.  
   - *Polly*: adiciona leve complexidade, mas melhora resiliência nas chamadas externas.  
   - *Moq*: excelente para isolamento de testes, com pequeno custo de curva de aprendizado.

---

## Cobertura de Testes
Testes unitários e de integração cobrindo:

- Algoritmos: duplicados e subsequências crescentes  
- Concorrência: execução paralela e cancelamento  
- Design: orquestração com exceções individuais  
- Persistência: CRUD e repositórios em memória  
- Integração: simulações de sucesso, timeout e falha após retries  
- Observabilidade: logs estruturados e métricas instrumentadas  

---

## Boas Práticas Aplicadas
- Código autoexplicativo e sem comentários redundantes  
- Estrutura clara e versionável, adequada a CI/CD  
- Padrões SOLID e baixo acoplamento entre módulos  
- Testes determinísticos e independentes  
- Logs e métricas estruturados, prontos para exportação futura  

---

Esse README cobre **ambiente**, **comandos**, **decisões**, **complexidade** e **trade-offs**, atendendo integralmente aos requisitos do documento de avaliação técnica.
