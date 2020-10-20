using System;
using System.Globalization;

namespace Lab3
{
    class Program
    {
        static void Main(string[] args)
        {
            var infelicity = ReadNumber("Введите точность:");
            Func<double, double> rounder = x => Math.Round(x, (int)Math.Log10(Math.Round(1 / infelicity)));
            var n = ReadNumber("Введите кол-во уравнений:");
            var a = new double[(int) n, (int) n];
            var y = new double[(int) n];
            for (var i = 0; i < n; i++)
            for (var j = 0; j < n; j++)
                a[i, j] = ReadNumber($"a[{i}][{j}]=");
            for (var i = 0; i < n; i++)
                y[i] = ReadNumber($"y[{i}]=");
            var result = Gauss(a, y, (int)n);
            if(result != null)
                for (var i = 0; i < n; i++) 
                    Console.WriteLine($"x[{i}]={rounder(result[i])}");
        }

        private static double ReadNumber(string s)
        {
            Console.WriteLine($"{s} ");
            double.TryParse(Console.ReadLine()?.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out var c);
            return c;
        }

        private static double[]? Gauss(double[,] a, double[] y, int n)
        {
            var k = 0;
            const double eps = 1e-10;
            var result = new double[n];
            while (k < n)
            {
                var max = Math.Abs(a[k, k]);
                var index = k;
                for (var i = k + 1; i < n; i++)
                    if (Math.Abs(a[i, k]) > max)
                    {
                        max = Math.Abs(a[i, k]);
                        index = i;
                    }

                if (max < eps)
                {
                    Console.WriteLine($"Решение получить невозможно из-за нулевого столбца {index} матрицы А");
                    return null;
                }

                for (var j = 0; j < n; j++)
                {
                    var temp = a[k, j];
                    a[k, j] = a[index, j];
                    a[index, j] = temp;
                }

                var tempC = y[k];
                y[k] = y[index];
                y[index] = tempC;

                for (var i = k; i < n; i++)
                {
                    var temp = a[i, k];
                    if (Math.Abs(temp) < eps) continue;
                    for (var j = 0; j < n; j++)
                        a[i, j] = a[i, j] / temp;
                    y[i] = y[i] / temp;
                    if(i == k) continue;
                    for (var j = 0; j < n; j++)
                        a[i, j] = a[i, j] - a[k, j];
                    y[i] = y[i] - y[k];
                }

                k++;
            }
            for (k = n - 1; k >= 0; k--)
            {
                result[k] = y[k];
                for (var i = 0; i < k; i++)
                    y[i] = y[i] - a[i, k] * result[k];
            }

            return result;
        }
    }
}
