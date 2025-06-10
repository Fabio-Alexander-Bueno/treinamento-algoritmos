using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algoritmos.semana2
{
    internal class Tipos
    {
        public void Executar()
        {
            // 1. ARRAY  —  bloco fixo, acesso ultra-rápido
            int[] numbers = { 10, 20, 30 };
            Console.WriteLine($"Array[1] = {numbers[1]}");   // 20
            Array.Resize(ref numbers, 5);                  // realoca; agora length = 5
            Console.WriteLine($"Array length after resize = {numbers.Length}\n");
            Console.WriteLine();
            Console.ReadLine();

            // 2. LIST<T>  —  crescimento dinâmico, API rica
            var fruits = new List<string> { "Apple", "Banana" };
            fruits.Add("Cherry");                          // O(1) amortizado
            fruits.Insert(1, "Blueberry");                 // O(n) — desloca elementos
            Console.WriteLine("List:");
            fruits.ForEach(f => Console.WriteLine($" • {f}"));
            Console.WriteLine();
            Console.ReadLine();

            // 3. LINKEDLIST<T>  —  inserções/remoções locais baratas
            var queue = new LinkedList<string>();
            queue.AddLast("First");
            queue.AddLast("Second");
            queue.AddFirst("Zero");
            queue.Remove("Second");                        // O(1) se o nó é conhecido
            Console.WriteLine("LinkedList:");
            foreach (var item in queue) Console.WriteLine($" • {item}");
            Console.WriteLine();
            Console.ReadLine();

            // 4. DICTIONARY<TKey,TValue>  —  lookup por chave
            var ages = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
            {
                ["Alice"] = 30,
                ["Bob"] = 28
            };
            ages["Charlie"] = 25;                          // Add/update em O(1) esperado
            Console.WriteLine($"Idade da Bob: {ages["bob"]}\n");
            Console.WriteLine();
            Console.ReadLine();

            // 5. HASHSET<T>  —  unicidade garantida
            var visited = new HashSet<int>();
            visited.Add(1);
            visited.Add(2);
            visited.Add(2);                                // ignorado (duplicado)
            Console.WriteLine($"HashSet count (unique) = {visited.Count}\n");
            Console.WriteLine();
            Console.ReadLine();

            // 6. SPAN<T>  —  fatia sem alocação
            Span<int> slice = numbers.AsSpan(0, 3);        // pega os 3 primeiros ints
            slice[0] = 99;                                 // reflete no array subjacente
            Console.WriteLine("Span over Array:");
            foreach (var n in numbers) Console.Write($"{n} ");  // 99 20 30 0 0
            Console.WriteLine();
            Console.ReadLine();
        }
    }
}
