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
        public static int Executar<T>(IReadOnlyList<T> data, T target)
            where T : IComparable<T>
        {
            int menor = 0;
            int maior = data.Count - 1;

            while (menor <= maior)
            {
                int meio = (menor + maior) / 2;
                int cmp = target.CompareTo(data[meio]);

                if (cmp == 0)
                    return meio;                 // achou
                if (cmp < 0)
                    maior = meio - 1;             // busca 1ª metade
                else
                    menor = meio + 1;             // busca 2ª metade
            }
            return -1;                          // não encontrado
        }

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
        public static  void Executar(int quantidade)
        {
            Console.WriteLine(BuscaBinaria.MedirTempoExecucao(quantidade));
        }
    }
}
