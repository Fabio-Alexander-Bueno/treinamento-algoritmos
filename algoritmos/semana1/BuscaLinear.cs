
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace algoritmos.semana1
{
    public class BuscaLinear
    {
        public static int Executar<T>(IReadOnlyList<T> data, T target)
        {
            var index = -1;
            for (int i = 0; i < data.Count; i++)
            {
                if (EqualityComparer<T>.Default.Equals(data[i], target))
                    index =  i;
            }
            return index;
        }

        public static string MedirTempoExecucao(int quantidadeItens, int valorBuscado = 1000)
        {
            long before = GC.GetTotalMemory(true);
            var sw = Stopwatch.StartNew();
            BuscaLinear.Executar(Enumerable.Range(0, quantidadeItens).ToList(), valorBuscado);
            sw.Stop();
            long after = GC.GetTotalMemory(false);

            return ($"Busca Linear com {quantidadeItens} buscando o valor {valorBuscado} levou {sw.ElapsedMilliseconds} ms | {after - before,8} B");

        }

        public static async Task Executar(int quantidade)
        {
            Console.WriteLine(BuscaLinear.MedirTempoExecucao(quantidade));

        }
    }
}
