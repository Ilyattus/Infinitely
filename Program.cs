using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Newton;

namespace UseInfConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = new Infinitely(10, 16);
            var b = new Infinitely(10, 16);
            Infinitely.toInfinitely(2.0m, a);
            Infinitely.toInfinitely(8.0m, b);
            Console.WriteLine(a != b);

            /*Infinitely x = new Infinitely(10, 17);
            Infinitely y = new Infinitely(10, 13);
            Infinitely.toInfinitely(123456789123456789.111m, x);
            Infinitely.toInfinitely(2m, y);
            //Infinitely.ShowAll(x/y);
            //Infinitely.Show(y);
            //Infinitely.Show(y);
            //Console.WriteLine();
            Infinitely.Show(x);
            //Console.WriteLine(x != y);
            Console.ReadLine();*/


            /*int n = 10;
            int[] a = new int[n], b = new int[n], c = new int[10];
            a[4] = 7; a[3] = 7; a[2] = 7; a[1] = 7; a[0] = 7;
            b[1] = 3;//int m = 4, q = 4, mc = q - m +1;
            int m = 3, q = 7, mc = q - m; bool inc = false;
            Console.Write("A = ");for (int i = 9; i >= 0; i--){Console.Write("{0}_", a[i]);}
            Console.Write("B = ");for (int i = 9; i >= 0; i--){Console.Write("{0}_", b[i]);}
            Console.Write("C = ");for (int i = 9; i >= 0; i--){Console.Write("{0}_", c[i]);}
            //for (int i = q - 1; i >= 0; i--)
            for (int i = q; i >= 0; i--)
            {
                int res = 0; bool run = true;
                while (run && (mc >= 0))
                {
                    int j = i, jm = m; //jm = m-1;
                    bool g = true, l = true;
                    if (!inc)
                    {
                        while (g && l && (jm >= 0) && (j >= 0))
                        {
                            if (a[j] < b[jm]) g = false;
                            if (a[j] > b[jm]) l = false;//Console.Write("\n {0} > {1} = {2}", a[j], b[jm], g);
                            --j; --jm;
                        }
                    }
                    if (g)
                    {   // Console.Write("\n g \n");
                        inc = false; run = true;
                        // for (int u = 9; u >= 0; u--) { Console.Write("{0}_", a[u]); } Console.ReadLine();                      
                        //Console.Write("\n g \n");
                        //int p = i - m + 1;
                        int p = i - m,  mod = 0, k = 0;
                        while (p + k < 0) { k++;}
                        //for (; k < m; k++)
                        while ((k <= m) || (mod > 0))
                        {//Console.WriteLine("P+K = {0}", (p+k)); //Console.Write("{0} - {1} - {2} = ", a[p+k], b[k], mod);
                            int t = a[p + k] - b[k] - mod;
                            if (t < 0) {mod = 1; t += 10;}
                            else {mod = 0;}//Console.WriteLine("{0}", t);
                            a[p + k] = t;
                            k++;
                        }
                        ++res;
                    }
                    else
                    {//Console.Write("\n l \n");
                        if (a[i] > 0) inc = true;
                        run = false;
                        c[mc--] = res;//--mc;
                    }
                }
            }
            Console.Write("A = ");for (int i = 9; i >= 0; i--){Console.Write("{0}_", a[i]);}
            Console.Write("B = ");for (int i = 9; i >= 0; i--){Console.Write("{0}_", b[i]);}
            Console.Write("C = ");for (int i = 9; i >= 0; i--){Console.Write("{0}_", c[i]);}
            Console.ReadLine();*/

            /*
            //-0.0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000007
            //2.7182818284590452353602874710000000000000000000000000000000000000000000000000000000000000000000000000

            /*DateTime S = DateTime.Now;
            Infinitely g = a * b;
            DateTime D = DateTime.Now;
            TimeSpan T1 = D - S;
            Console.WriteLine(" Async = {0}", T1);
            //Infinitely.Show(a);

            decimal A = 12345.12345m, B = 2m;
            DateTime S1 = DateTime.Now;
            decimal G = A * B;
            DateTime D1 = DateTime.Now;
            TimeSpan T = D1 - S1;
            Console.WriteLine(" NOTAsync = {0}", T);*/

            /*Console.Write("\nA = \n");
            Infinitely.Show(a);
            Console.WriteLine("\nB = ");
            Infinitely.Show(b);

            Console.WriteLine("\n Асинхронное умножение A * B  = ");
            Infinitely d = Infinitely.AsyncMull(a, b);
            Infinitely.Show(d);

            Console.WriteLine("\n Асинхронное умножение A * B  = ");
             d = Infinitely.tAsyncMull(a, b);
            Infinitely.Show(d);*/
            //Console.ReadLine();
        }
    }
}