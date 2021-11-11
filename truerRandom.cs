using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
namespace truerRandom
{
// trying to create a truer random using the randomness of thread pools, however the class still can't really handle a range bigger than 1000
    class TruerRandom
    {
        private List<int> numbers = new List<int>();
        public TruerRandom() {; }
        async Task FillNumbers(IProgress<int> progress, int minValue, int maxValue)
        {
            for (int i = minValue; i < maxValue; i++)
            {
                progress.Report(i);
            }
        }
        public int Next(int minValue, int maxValue)
        {

            Task<int> task = Task.Run(async () =>
            {
            await FillNumbers(new Progress<int>(i => {numbers.Add(i);}),
                minValue, maxValue) ;
                while (numbers.Count != maxValue - minValue) {; }
                return numbers[new Random().Next(0, numbers.Count)];
            });
            while (!task.IsCompleted) {; }
            return task.Result;
        }    
    }
    class Program
    {
        static void Main(string[] args)
        {
            TruerRandom rnd = new TruerRandom();
            int result = rnd.Next(1, 1000);
            // for some reason if I try to run it on a number 
            // like 10000 the program takes forever
            // I guess it has something to do with exponential complexity
            // but I have no idea how to optimize it or what it's about
            Console.WriteLine(result);
            Console.ReadKey();
        }
    }
}
