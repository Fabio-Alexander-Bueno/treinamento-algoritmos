1. T[] — Array
| Aspecto                | Detalhes                                                                                                                                                          |
| ---------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Estrutura interna**  | Bloco contíguo na heap (ou na stack, se `stackalloc`). O CLR armazena metadados (tipo, comprimento) num cabeçalho escondido antes do primeiro elemento.           |
| **Acesso**             | Indexação direta (`arr[i]`) compila para IL `ldelema` + `ldelem.*`, custo **O(1)**.                                                                               |
| **Inserção / remoção** | Precisa copiar tudo que vem depois do índice ⇒ **O(n)**. O tamanho é imutável após a alocação.                                                                    |
| **Vantagens**          | • Menor overhead por elemento (apenas o valor bruto).<br/>• Excelente localidade de cache.<br/>• Pode ser *stack-allocated*, evitando GC.                         |
| **Desvantagens**       | • Crescimento exige realocar todo o bloco.<br/>• APIs de alto nível (LINQ, Add, Remove) inexistem.                                                                |
| **Use quando**         | Você conhece o tamanho final de antemão **OU** a coleção é “grande mas estável” e será varrida sequencialmente (ex.: LUTs, buffers de áudio, matrizes numéricas). |

2. List<T>
| Aspecto                | Detalhes                                                                                                                                         |
| ---------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------ |
| **Estrutura**          | *Wrapper* sobre um array interno `_items`. A capacidade dobra (≈ ×2) quando o `Count` atinge o limite.                                           |
| **Acesso**             | Índice → **O(1)** (como no array).                                                                                                               |
| **Inserção / remoção** | Na cauda: **O(1)** *amortizado*.<br/>No meio: copia elementos remanescentes ⇒ **O(n)**.                                                          |
| **Crescimento**        | `EnsureCapacity()` realoca e copia o array; o dobro de espaço evita realocações frequentes.                                                      |
| **Vantagens**          | • API rica: `AddRange`, `Sort`, `BinarySearch`, `ConvertAll`.<br/>• Integração LINQ excelente.                                                   |
| **Desvantagens**       | • Overhead de crescimento (picos de CPU + GC).<br/>• Inserções frequentes no início/meio custam caro.                                            |
| **Use quando**         | Tamanho varia, mas você **acessa principalmente por índice** e adiciona no fim (ex.: coleções de DTOs carregadas para exibição, buffers de log). |

3. LinkedList<T> (duplamente encadeada)
| Aspecto                | Detalhes                                                                                                                                    |
| ---------------------- | ------------------------------------------------------------------------------------------------------------------------------------------- |
| **Estrutura**          | Cada nó (`LinkedListNode<T>`) contém `Value`, `Next`, `Previous`. Nó é um objeto separado ⇒ 3 cabeçalhos (um por objeto).                   |
| **Acesso**             | Precisa percorrer de `Head` até `i` ⇒ **O(n)**.                                                                                             |
| **Inserção / remoção** | Conhecendo o nó: **O(1)**; caso contrário, primeiro precisa localizar (**O(n)**).                                                           |
| **Memória**            | Grande overhead (pelo menos 24 B por nó em 64 bit).                                                                                         |
| **Vantagens**          | • Inserções/remoções locais baratas.<br/>• Serve de base para implementar filas (com `AddLast` + `RemoveFirst`) sem cópia.                  |
| **Desvantagens**       | • Cache-unfriendly (nós “soltos” pela heap).<br/>• Não suporta indexador; LINQ recorre a enumerador lento.                                  |
| **Use quando**         | Você precisa **mover, concatenar ou cortar nós** sem copiar os dados (ex.: playlist que se reordena, LRU cache, back/forward de navegador). |

4. Dictionary<TKey,TValue> (hash table)
| Aspecto                | Detalhes                                                                                                                                                                          |
| ---------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Estrutura**          | Dois arrays: `buckets` (int) e `entries` (`Entry{hashCode, next, key, value}`).<br/>`hashCode` é pré-computado; `next` cria a “lista de colisão” encadeada dentro do mesmo array. |
| **Acesso**             | `GetHashCode()` → índice do bucket → percorre cadeias de colisão; **O(1)** *esperado*, **O(n)** *pior caso* (muito raro se o *load factor* < 0,75).                               |
| **Inserção / remoção** | Também **O(1)** esperado. Realocação dobra capacidade e rehaxa tudo.                                                                                                              |
| **Vantagens**          | • Busca por chave muito rápida.<br/>• Operações de leitura não deslocam dados.                                                                                                    |
| **Desvantagens**       | • Ordem dos pares não é garantida (exceto em .NET Core 3.0+ onde é ‘inserção-preservada’, mas você não deve depender disso).<br/>• Chaves mutáveis são perigosas.                 |
| **Use quando**         | Lookup por chave domina (ex.: cache de objetos, mapeamento ID→entidade, contadores por categoria).                                                                                |

5. HashSet<T> (conjunto)
| Aspecto          | Detalhes                                                                                                                                      |
| ---------------- | --------------------------------------------------------------------------------------------------------------------------------------------- |
| **Estrutura**    | A mesma de `Dictionary`, mas armazena **apenas a chave** (`value` inexistente).                                                               |
| **Operações**    | `Add`, `Contains`, `Remove` ⇒ **O(1)** esperado.                                                                                              |
| **Vantagens**    | • Sem duplicatas por design.<br/>• Suporta `SetEquals`, `IntersectWith`, `ExceptWith`, etc.                                                   |
| **Desvantagens** | • Precisa de `GetHashCode()` consistente (se você muda campos, o hash “fica errado”).                                                         |
| **Use quando**   | Precisa garantir unicidade ou fazer operações de teoria de conjuntos (ex.: coleção de permissões, rastrear IPs únicos, algoritmos de grafos). |

6. Span<T> (e ReadOnlySpan<T>, Memory<T>)
| Aspecto          | Detalhes                                                                                                                                                                                        |
| ---------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Estrutura**    | *Ref-struct* stack-only que referencia (ponteiro + comprimento) **qualquer** bloco contíguo: arrays, stackalloc, `&struct`, fatias de strings/bytes via *string interning* interno.             |
| **Alocação**     | **Zero GC** — o `Span<T>` vive na stack; só aponta para dados já existentes.                                                                                                                    |
| **Acesso**       | Indexador **O(1)** (como array). Sem copiar, mas regras de segurança impedem sair do slice.                                                                                                     |
| **Limitações**   | • Não pode ser campo de classe (só stack).<br/>• Não implementa coleções ou LINQ completos (apenas `.Slice`, `.ToArray`, `.ToString`).                                                          |
| **Vantagens**    | • Ótimo para **parsing** e **serialização** (evita criar substrings).<br/>• Permite manipular memória nativa com segurança.                                                                     |
| **Desvantagens** | • Vida curta: não retorna de métodos sem usar `Memory<T>`.                                                                                                                                      |
| **Use quando**   | Você quer **fatiar dados temporariamente** — por exemplo, ler prefixos de bytes de um buffer de rede, percorrer um array sem alocar sub-arrays, ou escrever rotinas de *fast CSV/JSON parsing*. |


| Operação / Tipo        | Array | List   | LinkedList | Dictionary         | HashSet | Span |
| ---------------------- | ----- | ------ | ---------- | ------------------ | ------- | ---- |
| Acesso por índice      | O(1)  | O(1)   | —          | —                  | —       | O(1) |
| Inserir no fim         | —     | O(1)\* | O(1)       | —                  | O(1)    | —    |
| Inserir no meio        | O(n)  | O(n)   | O(1)\*     | —                  | —       | —    |
| Buscar por chave       | —     | —      | —          | O(1)               | O(1)    | —    |
| Memória extra/elemento | \~0 B | \~2 B  | ≥24 B      | \~8 B              | \~8 B   | 0 B  |
| Ordenação preservada   | Sim   | Sim    | Sim        | Não (prático: sim) | Não     | N/A  |
| Aloca no GC?           | Sim   | Sim    | Sim        | Sim                | Sim     | Não  |


“Quando usar qual?” em 15 segundos
Array — Buffer fixo, alta performance e zero overhead.

List — Coleção geral de tamanho variável com acesso random.

LinkedList — Muitas operações de cortar/colar nós conhecidos.

Dictionary — Precisou de lookup por chave? É aqui.

HashSet — Mesmo que acima, mas só quer saber “existe?” e unicidade.

Span — Fatie sem medo de coletor; parsing e cópias zero-alocação.
