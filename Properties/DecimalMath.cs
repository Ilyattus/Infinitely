/*
    BruteMind Framework for .Net
    Copyright (C) 2016  Valeriy Garnaga

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
*/

using System;
using System.Threading.Tasks;

namespace Newton
{

    public static class DecimalMath
    {
        // константы для точности рядов
        public const decimal PRECISION = 1E-28m;
        public const decimal SMALL_PRECISION = 1E-14m;

        // первые 100 цифр после запятой
        public const decimal E = 2.7182818284590452353602874713m;   //526624977572470936999595749669676277240766303535475945713821785251664274m;                            
        public const decimal PI = 3.14159265358979323846264338327m; //95028841971693993751058209749445923078164062862089986280348253421170679m;

     

        public static decimal[] Fact = new decimal[] 
        {
            1.0m, //0!
            1.0m, //1!
            1.0m * 2, 
            1.0m * 2 * 3,
            1.0m * 2 * 3 * 4,
            1.0m * 2 * 3 * 4 * 5,
            1.0m * 2 * 3 * 4 * 5 * 6,
            1.0m * 2 * 3 * 4 * 5 * 6 * 7,
            1.0m * 2 * 3 * 4 * 5 * 6 * 7 * 8,
            1.0m * 2 * 3 * 4 * 5 * 6 * 7 * 8 * 9,
            1.0m * 2 * 3 * 4 * 5 * 6 * 7 * 8 * 9 * 10,
            1.0m * 2 * 3 * 4 * 5 * 6 * 7 * 8 * 9 * 10 * 11,
            1.0m * 2 * 3 * 4 * 5 * 6 * 7 * 8 * 9 * 10 * 11 * 12,
            1.0m * 2 * 3 * 4 * 5 * 6 * 7 * 8 * 9 * 10 * 11 * 12 * 13,
            1.0m * 2 * 3 * 4 * 5 * 6 * 7 * 8 * 9 * 10 * 11 * 12 * 13 * 14,
            1.0m * 2 * 3 * 4 * 5 * 6 * 7 * 8 * 9 * 10 * 11 * 12 * 13 * 14 * 15,
            1.0m * 2 * 3 * 4 * 5 * 6 * 7 * 8 * 9 * 10 * 11 * 12 * 13 * 14 * 15 * 16,
            1.0m * 2 * 3 * 4 * 5 * 6 * 7 * 8 * 9 * 10 * 11 * 12 * 13 * 14 * 15 * 16 * 17,
            1.0m * 2 * 3 * 4 * 5 * 6 * 7 * 8 * 9 * 10 * 11 * 12 * 13 * 14 * 15 * 16 * 17 * 18,
            1.0m * 2 * 3 * 4 * 5 * 6 * 7 * 8 * 9 * 10 * 11 * 12 * 13 * 14 * 15 * 16 * 17 * 18 * 19,
            1.0m * 2 * 3 * 4 * 5 * 6 * 7 * 8 * 9 * 10 * 11 * 12 * 13 * 14 * 15 * 16 * 17 * 18 * 19 * 20,
            1.0m * 2 * 3 * 4 * 5 * 6 * 7 * 8 * 9 * 10 * 11 * 12 * 13 * 14 * 15 * 16 * 17 * 18 * 19 * 20 * 21,
            1.0m * 2 * 3 * 4 * 5 * 6 * 7 * 8 * 9 * 10 * 11 * 12 * 13 * 14 * 15 * 16 * 17 * 18 * 19 * 20 * 21 * 22,
            1.0m * 2 * 3 * 4 * 5 * 6 * 7 * 8 * 9 * 10 * 11 * 12 * 13 * 14 * 15 * 16 * 17 * 18 * 19 * 20 * 21 * 22 * 23,
            1.0m * 2 * 3 * 4 * 5 * 6 * 7 * 8 * 9 * 10 * 11 * 12 * 13 * 14 * 15 * 16 * 17 * 18 * 19 * 20 * 21 * 22 * 23 * 24,
            1.0m * 2 * 3 * 4 * 5 * 6 * 7 * 8 * 9 * 10 * 11 * 12 * 13 * 14 * 15 * 16 * 17 * 18 * 19 * 20 * 21 * 22 * 23 * 24 * 25,
            1.0m * 2 * 3 * 4 * 5 * 6 * 7 * 8 * 9 * 10 * 11 * 12 * 13 * 14 * 15 * 16 * 17 * 18 * 19 * 20 * 21 * 22 * 23 * 24 * 25 * 26,
            1.0m * 2 * 3 * 4 * 5 * 6 * 7 * 8 * 9 * 10 * 11 * 12 * 13 * 14 * 15 * 16 * 17 * 18 * 19 * 20 * 21 * 22 * 23 * 24 * 25 * 26 * 27
            // 28! - 30-значное чило, тип decimal имеет максимум в 29 знаков 
        };

        public class Pw
        {
            public Infinitely x;
            public uint y;
            public Pw(Infinitely x, uint y)
            {
                this.x = x;
                this.y = y;
            }
        }

        public static decimal Sqrt(decimal x)
        {
            decimal current = (decimal)Math.Sqrt((double)x); // calculate starting point
            decimal previous;
            checked
            {
                try
                {
                    do
                    {
                        previous = current;
                        current = (previous + x / previous) / 2.0m;
                    }
                    while (Math.Abs(current - previous) > PRECISION);
                }
                catch (DivideByZeroException) { Console.WriteLine("Cannot divide by zero"); }
                catch (OverflowException) { Console.WriteLine("Overflow"); }
            }
            return current;
        }

        public static Infinitely P(Pw zz)
        {

            Infinitely result = new Infinitely(zz.x.allDigitsNumber, zz.x.fractionDigitsNumber);
            Infinitely.toInfinitely(1, result);
            for (uint i = 0; i < zz.y; i++)
            {
                result = result * zz.x;
            }
            return result;
        }

        public static Infinitely APow(Infinitely xx, uint z)
        {
            Pw x1, x2;
            Task<Infinitely> t1, t2;

            if (z % 2 == 0)
            {
                x1 = new Pw(xx, z/2);
                x2 = new Pw(xx, z/2);
            }
            else
            {
                x1 = new Pw(xx, z/2);
                x2 = new Pw(xx, z/2 + 1);
            }

            t1 = new Task<Infinitely>(x => P((Pw)x), x1);
            t2 = new Task<Infinitely>(x => P((Pw)x), x2);
            t1.Start();
            t2.Start();
            return t1.Result * t2.Result;
        }

        /*
        ------------------------------------------------------------------------------------------------------
        Асинхронное вычисление экспоненты.
        Сумма разбивается на две -- сумма четных членов(n = 0..2..4..2n) и сумма нечетных (n = 1..3..5..2n+1).
        Exp1 и Exp2 считают четную и нечетную суммы соответственно.
        Exp вызывает одновременно оба метода.
        Происходит выделение потока под каждый метод.
        Затем возвращается значение, равное сумме результатов вычислений метода.
        ------------------------------------------------------------------------------------------------------
        */

        /*    
        public static void Exp1(object y)
        {
            decimal x = decimal.Parse(y.ToString());
            if (x < -30)
            {
                x = 0;
            }

            decimal result = 1.0m;
            decimal old = 1.0m;
            decimal pow = 1.0m;
            decimal i = 1.0m, j = 2.0m;
            decimal tpow = Pow(x, 2);

            checked
            {
                try
                {
                        for (int i3 = 0; i3 < 28; i3++)
                        {
                            old = result;
                            pow *= tpow / (i * j);
                            result += pow;
                            i += 2;
                            j += 2;
                        }
                }

                catch (DivideByZeroException) { }
                catch (OverflowException) { }
            }
            e1 = result;
        }
        public static void Exp2(object y)
        {
            decimal x = decimal.Parse(y.ToString());

            if (x < -28)
            {
                y = 0;
            }

            decimal result = x;
            decimal old = x;
            decimal pow = x;
            decimal i = 2.0m, j = 3.0m;
            checked
            {
                try
                {
                    for (int i3 = 0; i3 < 28; i3++)
                    {
                        old = result;
                        pow *= (x * x) / (i * j);
                        result += pow;
                        i += 2;
                        j += 2;
                    }
                }
                catch (DivideByZeroException) { }
                catch (OverflowException) { }
            }
            e2 = result;
        }
        */

        public static decimal Exp1(decimal x)
        {
            decimal result = 1.0m;
            decimal old = 1.0m;
            decimal pow = 1.0m;
            decimal i = 1.0m, j = 2.0m;
            decimal tpow = x * x;

            checked
            {
                try
                {
                    for (int ii = 0; ii < 28; ii++)
                    {
                        old = result;
                        pow *= tpow / (i * j);
                        result += pow;
                        i += 2;
                        j += 2;
                    }
                }
                catch (DivideByZeroException) { Console.WriteLine("zero division"); }
                catch (OverflowException) { Console.WriteLine("overflow"); }
            }

            return result;
        }
        public static decimal Exp2(decimal x)
        {
            decimal result = x;
            decimal old = x;
            decimal pow = x;
            decimal i = 2.0m, j = 3.0m;

            checked
            {
                try
                {
                    for (int ii = 0; ii < 28; ii++)
                    {
                        old = result;
                        pow *= (x * x) / (i * j);
                        result += pow;
                        i += 2;
                        j += 2;
                    }
                }
                catch (DivideByZeroException) { Console.WriteLine("zero division"); }
                catch (OverflowException) { Console.WriteLine("overflow"); }
            }

            return result;
        }
        public static decimal Exp(decimal z)
        {
            
            Task<decimal> t1, t2;
            t1 = new Task<decimal>(x => Exp1((decimal)x), z);
            t2 = new Task<decimal>(x => Exp2((decimal)x), z);
            t1.Start();
            t2.Start();
            return t1.Result + t2.Result;

            /*
            ThreadPool.QueueUserWorkItem(Exp1, x);
            ThreadPool.QueueUserWorkItem(Exp2, x);
            return e1 + e2;
            */
        }

        /*
        public static decimal Expold(decimal y)
        {
            if (y < -30)
            {
                return 0;
            }

            decimal result = 1.0m;
            decimal old;
            decimal pow = 1.0m;
            decimal i = 0;

            do
            {
                old = result;
                pow *= y / (++i);
                result += pow;
            } while (Math.Abs(result - old) > PRECISION);

            return result;
        }
        */

        public static decimal NthRoot(decimal power, decimal root)
        {
            /* (C) John Gabriel */
            decimal t, l, r;
            decimal a = power;
            decimal n = root;
            decimal s = 1.0m;

            do
            {
                t = s;
                l = (a / Pow(s, (n - 1.0m)));
                r = (n - 1.0m) * s;
                s = (l + r) / n;
            } while (Math.Abs(l - s) > SMALL_PRECISION);

            return s;
        }
        public static decimal Ln(decimal power)
        {
            decimal old;
            decimal p = power;
            decimal result = 0;

            while (p >= E)
            {
                p /= E;
                result++;
            }
            result += (p / E);
            p = power;

            checked
            {
                try
                {
                    do
                    {
                        old = result;
                        result = ((p / (Exp(result - 1.0m))) / E) + (result - 1.0m);

                    } while (Math.Abs(result - old) > PRECISION);
                }
                catch (OverflowException) { }
                catch (DivideByZeroException) { }
            }

            return result;
        }
        public static decimal Pow(decimal x, decimal y)
        {
            if (x < 0)
            {
                return 1.0m / Exp(y * Ln(-x));
            }
            else
            {
                return Exp(y * Ln(x));
            }
        }
        public static decimal Log(decimal a, decimal b)
        {
            return Ln(a) / Ln(b);
        }
        public static decimal Log2(decimal a)
        {
            return Ln(a) / Ln(2); //0.693147180559945309417232121458m;
        }
        public static decimal Log10(decimal a)
        {
            return Ln(a) / Ln(10); //2.30258509299404568401799145468m;
        }
        public static decimal ALog(decimal power, decimal exponent)
        {
            return Exp(Ln(power) / exponent);
        }
    }
}
