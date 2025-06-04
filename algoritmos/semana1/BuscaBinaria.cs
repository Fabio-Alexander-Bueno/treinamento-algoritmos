using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algoritmos.semana1
{
    public class BuscaBinaria
    {
        /// <summary>
        /// Procura <paramref name="target"/> na coleção **ordenada** <paramref name="data"/>.
        /// Retorna o índice ou -1 se não encontrar.
        /// Complexidade: O(log n).
        /// </summary>
        public static int Executar<T>(IReadOnlyList<T> data, T target)
            where T : IComparable<T>
        {
            int low = 0;
            int high = data.Count - 1;

            while (low <= high)
            {
                int mid = (low + high) / 2;
                int cmp = target.CompareTo(data[mid]);

                if (cmp == 0)
                    return mid;                 // achou
                if (cmp < 0)
                    high = mid - 1;             // busca 1ª metade
                else
                    low = mid + 1;             // busca 2ª metade
            }
            return -1;                          // não encontrado
        }

        /// <summary>
        /// Mede o tempo (ms) e bytes alocados para executar a busca binária
        /// em uma lista de <paramref name="quantidadeItens"/> inteiros já ordenados.
        /// </summary>
        public static string MedirTempoExecucao(int quantidadeItens, int valorBuscado = 1000)
        {
            // Dados de teste fora do cronômetro — isolamos apenas o custo da busca
            var lista = Enumerable.Range(0, quantidadeItens).ToList();

            long before = GC.GetTotalMemory(true);
            var sw = Stopwatch.StartNew();

            BuscaBinaria.Executar(lista, valorBuscado);

            sw.Stop();
            long after = GC.GetTotalMemory(false);

            return $"Busca Binária com {quantidadeItens:N0} " +
                   $"buscando o valor {valorBuscado:N0} " +
                   $"levou {sw.ElapsedMilliseconds} ms | {after - before,8} B";
        }

        /// <summary>
        /// Facilitador assíncrono para rodar a medição em um ponto do programa.
        /// </summary>
        public static async Task Executar(int quantidade)
        {
            Console.WriteLine(BuscaBinaria.MedirTempoExecucao(quantidade));
            await Task.Delay(500);
        }
    }
}
