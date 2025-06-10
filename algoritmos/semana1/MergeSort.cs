using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algoritmos.semana1
{
    public class MergeSort
    {
        private static IList<T> MergeSortRecursao<T>(IList<T> data) where T : IComparable<T>
        {
            if (data.Count <= 1) return data;

            int mid = data.Count / 2;
            var left = MergeSortRecursao(data.Take(mid).ToList());
            var right = MergeSortRecursao(data.Skip(mid).ToList());

            return Merge(left, right);
        }

        private static IList<T> Merge<T>(IList<T> left, IList<T> right) where T : IComparable<T>
        {
            var result = new List<T>(left.Count + right.Count);
            int i = 0, j = 0;

            while (i < left.Count && j < right.Count)
            {
                if (left[i].CompareTo(right[j]) <= 0)
                    result.Add(left[i++]);
                else
                    result.Add(right[j++]);
            }

            while (i < left.Count) result.Add(left[i++]);
            while (j < right.Count) result.Add(right[j++]);

            return result;
        }


        public static string MedirTempoExecucao(int quantidadeItens)
        {
            var rand = new Random(42);
            var lista = Enumerable.Range(0, quantidadeItens)
                                  .Select(_ => rand.Next())
                                  .ToList();

            long before = GC.GetTotalMemory(true);
            var sw = Stopwatch.StartNew();

            MergeSortRecursao(lista.ToList());

            sw.Stop();
            long after = GC.GetTotalMemory(false);

            return $"MergeSort com {quantidadeItens} itens levou {sw.ElapsedMilliseconds} ms | {after - before,8} B";
        }


        public static void Executar(int quantidade)
        {
            Console.WriteLine(MedirTempoExecucao(quantidade));
        }
    }
}
