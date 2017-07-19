/*
    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Numerics;///

namespace Newton
{

    public class InfinitelyException : Exception
    {
        public InfinitelyException(string Message) :base(Message)
        {

        }
    }

    // класс для передачи параметров в ассинхронные методы
    public class Point
    {
        public int i, j, ii, jj;
        public Infinitely a, b, c;

        public Point(int i, int j, int ii, int jj, Infinitely a, Infinitely b, Infinitely c)
        {
            this.i = i;
            this.j = j;
            this.ii = ii;
            this.jj = jj;
            this.a = a;
            this.b = b;
            this.c = c;
        }
    }
  

    public class Infinitely
    {
        private const int ZERO = 0;
        private const int NEGATIVE = -1;
        private const int POSITIVE = 1;
        private const int POSITIVE_INFINITY = Int32.MaxValue;
        private const int NEGATIVE_INFINITY = Int32.MinValue;
        private int attribute;
        private bool Zero = false;
        // long –9,223,372,036,854,775,808 to 9,223,372,036,854,775,807
        // 64 bit
        // 4 bit per digit, 16 digits per long
        private long[] number;
        public long allDigitsNumber;
        public long fractionDigitsNumber;


        //метод для ассинхронного умножения
        private static void Mull(Object state)
        {
            Point g = (Point)state;

            long t = (g.a.number[g.i] >> 4 * g.j & 0x000000000000000F);
            int m = g.j - g.jj, p = g.i - g.ii;
            if (m < -16)
            {
                p--;
                m += 15;
            }
            long n = 0, mod = 0;
            if (p >= 0)
            {
                for (int l = 0; l < m; l++)
                {
                    n |= (g.c.number[p] >> 4 * l & 0x000000000000000F) << 4 * l;
                }
            }
            for (int k = 0; k < g.b.number.Length; k++)
            {
                for (int h = 0; (h < 16) && (p < g.c.number.Length); h++)
                {
                    if (p >= 0)
                    {
                        long f = (g.b.number[k] >> 4 * h & 0x000000000000000F) * t + mod;
                        if (p < g.c.number.Length) f += (g.c.number[p] >> 4 * m & 0x000000000000000F);
                        if (f > 9)
                        {
                            mod = f / 10;
                            f %= 10;
                        }
                        else
                        {
                            mod = 0;
                        }

                        n |= f << 4 * m;
                        if (++m > 15)
                        {
                            g.c.number[p] = n;
                            n = 0;
                            m = 0;
                            p++;
                        }
                    }
                    else
                    {
                        if (++m > 15)
                        {
                            m = 0;
                            p++;
                        }
                    }

                }
            }

        }

        public Infinitely(long allDigitsNumber, long fractionDigitsNumber)
        {
            this.allDigitsNumber = allDigitsNumber;
            this.fractionDigitsNumber = fractionDigitsNumber;
            this.number = new long[1 + allDigitsNumber / 2];
        }


        public static void toInfinitely(decimal d, Infinitely a)
        {
            toInfinitely(d.ToString(), a);
            /*if (d != 0)
            {
                int i = 0, j = 0;
                decimal g;
                if (d > 0)
                {
                    a.attribute = 1;
                    g = d;
                }
                else
                {
                    a.attribute = -1;
                    g = -d;
                }
                FindFrac(a, ref i, ref j);
                while (g >= 1)
                {
                    g /= 10;
                    if (++j > 15)
                    {
                        ++i;
                        j = 0;
                    }
                }
                decimal m = d;
                long n = 0;
                for (; (m >= 10)&&(i >= 0); i--)
                {
                    n = 0;
                    for (; (m >= 10)&&(j >= 0); j--)
                    {
                        long t = (long)g;
                        if (t > g) --t;
                        n |= t << 4 * j;
                        g = g * 10 - t * 10;
                        m -= t*10;
                    }
                    j = 15;
                    a.number[i] = n;
                }
                while(i>=0)
                {
                    for (; j >= 0; j--)
                    {
                        long t = (long)m;
                        if (t > m) --t;
                        n |= t << 4 * j;
                        m = m * 10 - t * 10;
                    }

                    n = 0;
                    j = 15;
                    a.number[i] = n;
                    i--;
                }
            }
            else
            {
                a.attribute = 0;
                for (int i = 0; i < a.number.Length; i++)
                {
                    a.number[i] = 0;
                }
            }*/
        }

        public static void toInfinitely(string d, Infinitely a)
        {
            if (d !="0")
            {
                int k = 0, h = d.Length - 1;
                if (d[k] == '-')
                {
                    a.attribute = -1;
                    ++k;
                }
                else
                {
                    a.attribute = 1;
                }
                long n = 0, i = 0, f = a.fractionDigitsNumber - 1;
                int j = 0;
            
                if (d.IndexOf(',') > -1)
                {
                    f -= d.Length - d.IndexOf(',') - 1;
                }

                for (; f>=0; f--)
                {
                    if (++j > 15)
                    {
                        j = 0;
                        i++;
                    }
                }

                if (d.IndexOf(',') > -1)
                {
                    for (; h > d.IndexOf(','); h--)
                    {
                        long t = Convert.ToInt64(d[h]) - 48;
                        n |= t << 4 * j;
                        if (++j > 15)
                        {
                            a.number[i] = n;
                            n = 0;
                            j = 0;
                            i++;
                        }
                    }
                    --h;
                }

                for (; h >= k; h--)
                {
                    long t = Convert.ToInt64(d[h]) - 48;
                    //Console.Write(t); Console.ReadLine(); ////////////////////delete this
                    n |= t << 4 * j;
                    if (++j > 15)
                    {
                        a.number[i] = n;
                        n = 0;
                        j = 0;
                        i++;
                    }
                }
                a.number[i] = n;
                
            }
            else
            {
                a.attribute = 0;
                a.Zero = true;
                for (int i = 0; i < a.number.Length; i++)
                {
                    a.number[i] = 0;
                }
            }
        }
       
        /*
        public static decimal toDecimal(Infinitely a)
        {
            decimal r = 0;
            long i = a.allDigitsNumber-1;
            while ((a.number[i] == 0)&&(i>=0)) --i;
            return r;
        }
        */

        // возвращает копию числа а
        public static Infinitely Copy(Infinitely a)
        {
            Infinitely b = new Infinitely(a.allDigitsNumber, a.fractionDigitsNumber);
            b.attribute = a.attribute;
            for (long i = 0; i < a.number.Length; ++i)
            {
                b.number[i] = a.number[i];
            }
            return b;
        }

        public static bool isZero(Infinitely a)
        {
            if (a.attribute == ZERO) return true;
            for (long i = 0; i<a.number.Length; ++i)
            {
                if (a.number[i] != 0) return false;
            }
            return true;
        }

        // поиск первой не нулевой цифры слева
        public static void FindBegin(Infinitely a, ref int i, ref int j)
        {
            if (a.Zero) { i = 0; j = 0; }
            else
            {
                if (a.attribute != 0)
                {
                    bool f = true;
                    i = a.number.Length - 1;
                    while (f && (i >= 0))
                    {
                        if (a.number[i] != 0) f = false;
                        else --i;
                    }
                    f = true;
                    j = 15;
                    while (f && (j >= 0))
                    {
                        if (((a.number[i] >> 4 * j) & 0x000000000000000F) > 0) f = false;
                        else --j;
                    }
                }
            }
        }

        // |a| > |b|
        public static bool AbsGreater(Infinitely a, Infinitely b)
        {
           // long n = a.allDigitsNumber - a.fractionDigitsNumber;
            int i=0, j=0, p=0, q=0;
            FindBegin(a, ref i, ref j);
            FindBegin(b, ref p, ref q);

            if (i > p) return true;
            if (i < p) return false;
            if (j > q) return true;
            if (j < q) return false;

            while ((i >= 0) && (p >= 0))
            {
                if ((a.number[i] >> 4 * j & 0x000000000000000F) > (b.number[p] >> 4 * q & 0x000000000000000F)) return true;
                if ((a.number[i] >> 4 * j & 0x000000000000000F) < (b.number[p] >> 4 * q & 0x000000000000000F)) return false;
                if (--j < 0)
                {
                    j = 15;
                    i--;
                }
                if (--q < 0)
                {
                    q = 15;
                    p--;
                }
            }
            /*while (i >= 0)
            {
                if ((a.number[i] >> 4 * j & 0x000000000000000F) > 0) return true;
                if (--j < 0)
                {
                    j = 15;
                    i--;
                }
            }
            while (p >= 0)
            {
                if ((b.number[p] >> 4 * q & 0x000000000000000F) > 0) return false;
                if (--q < 0)
                {
                    q = 15;
                    p--;
                }
            }*/
            return false;
        }

        public static Infinitely Abs(Infinitely a)
        {
            if (a.attribute == 0) return a;
            a.attribute = 1;
            return a;   
        }

        public static Infinitely Trunc(Infinitely a)
        {
            Infinitely b = Copy(a);
            long w = b.fractionDigitsNumber;
            long i = 0, f = 0;
            int j = 0;
            while (w > 0)
            {
                //b.number[i] &= 0xFFFF << 4 * j;
                if (++j > 15)
                {
                    b.number[i++] = f;
                    j = 0;
                }
                --w;
            }
            for (; j<16; j++)
            {
                f |= b.number[i] << 4 * j & 0x000000000000000F;
            }
            b.number[i] = f;
            return b;
        }

        public static bool operator ==(Infinitely a, Infinitely b)
        {
            return !(a!=b);
        }

        public static bool operator ==(Infinitely a, decimal y)
        {
            Infinitely b = new Infinitely(a.allDigitsNumber, a.fractionDigitsNumber);
            toInfinitely(y, b);
            return !(a != b);
        }

        public static bool operator ==(decimal x, Infinitely b)
        {
            Infinitely a = new Infinitely(b.allDigitsNumber, b.fractionDigitsNumber);
            toInfinitely(x, a);
            return !(a != b);
        }

        public static bool operator !=(Infinitely a, Infinitely b)
        {
            if (a.attribute != b.attribute) return true;
            long n = a.allDigitsNumber - a.fractionDigitsNumber;
            int i = 0, j = 0, p = 0, q = 0;
            FindBegin(a, ref i, ref j);
            FindBegin(b, ref p, ref q);

            if ((i != p) || (j != q)) return true;

            while (i >= 0)
            {
                if ((a.number[i] >> 4 * j & 0x000000000000000F) != (b.number[i] >> 4 * j & 0x000000000000000F)) return true;
                if (--j < 0)
                {
                    j = 15;
                    i--;
                }
            }     
            return false;
        }

        public static bool operator !=(Infinitely a, decimal y)
        {
            Infinitely b = new Infinitely(a.allDigitsNumber, a.fractionDigitsNumber);
            toInfinitely(y, b);

            if (a.attribute != b.attribute) return true;
            long n = a.allDigitsNumber - a.fractionDigitsNumber;
            int i = 0, j = 0, p = 0, q = 0;
            FindBegin(a, ref i, ref j);
            FindBegin(b, ref p, ref q);

            if ((i != p) || (j != q)) return true;

            while (i >= 0)
            {
                if ((a.number[i] >> 4 * j & 0x000000000000000F) != (b.number[i] >> 4 * j & 0x000000000000000F)) return true;
                if (--j < 0)
                {
                    j = 15;
                    i--;
                }
            }
            return false;
        }

        public static bool operator !=(decimal x, Infinitely b)
        {
            Infinitely a = new Infinitely(b.allDigitsNumber, b.fractionDigitsNumber);
            toInfinitely(x, a);

            if (a.attribute != b.attribute) return true;
            long n = a.allDigitsNumber - a.fractionDigitsNumber;
            int i = 0, j = 0, p = 0, q = 0;
            FindBegin(a, ref i, ref j);
            FindBegin(b, ref p, ref q);

            if ((i != p) || (j != q)) return true;

            while (i >= 0)
            {
                if ((a.number[i] >> 4 * j & 0x000000000000000F) != (b.number[i] >> 4 * j & 0x000000000000000F)) return true;
                if (--j < 0)
                {
                    j = 15;
                    i--;
                }
            }
            return false;
        }

        public static bool operator <(Infinitely a, Infinitely b)
        {
            if (a.attribute > b.attribute) return false;
            if (a.attribute < b.attribute) return true;

            if (a == b) return false;
            else
            {
                if (a.attribute == 1)
                {
                    return !AbsGreater(a, b);
                }
                if (a.attribute == -1)
                {
                    return AbsGreater(a, b);
                }
            }
            return false;
        }

        public static bool operator <(Infinitely a, decimal y)
        {
            Infinitely b = new Infinitely(a.allDigitsNumber, a.fractionDigitsNumber);
            toInfinitely(y, b);

            if (a.attribute > b.attribute) return false;
            if (a.attribute < b.attribute) return true;
            if (a == b) return false;
            if (a.attribute == 1)
            {
                return !AbsGreater(a, b);
            }
            if (a.attribute == -1)
            {
                return AbsGreater(a, b);
            }
            return false;
        }

        public static bool operator <(decimal x, Infinitely b)
        {
            Infinitely a = new Infinitely(b.allDigitsNumber, b.fractionDigitsNumber);
            toInfinitely(x, a);

            if (a.attribute > b.attribute) return false;
            if (a.attribute < b.attribute) return true;
            if (a == b) return false;
            if (a.attribute == 1)
            {
                return !AbsGreater(a, b);
            }
            if (a.attribute == -1)
            {
                return AbsGreater(a, b);
            }
            return false;
        }

        public static bool operator >(Infinitely a, Infinitely b)
        {
            if (a.attribute > b.attribute) return true;
            if (a.attribute < b.attribute) return false;
            if (a.attribute == 0) return false;
            if (a == b) return false;
            if (a.attribute == 1)
            {
                return AbsGreater(a, b);
            }
            if (a.attribute == -1)
            {
                return !(AbsGreater(a, b));
            }
            return false;
        }

        public static bool operator >(Infinitely a, decimal y)
        {
            Infinitely b = new Infinitely(a.allDigitsNumber, a.fractionDigitsNumber);
            toInfinitely(y, b);

            if (a.attribute > b.attribute) return true;
            if (a.attribute < b.attribute) return false;
            if (a.attribute == 0) return false;
            if (a == b) return false;
            if (a.attribute == 1)
            {
                return AbsGreater(a, b);
            }
            if (a.attribute == -1)
            {
                return !(AbsGreater(a, b));
            }
            return false;
        }

        public static bool operator >(decimal x, Infinitely b)
        {
            Infinitely a = new Infinitely(b.allDigitsNumber, b.fractionDigitsNumber);
            toInfinitely(x, a);

            if (a.attribute > b.attribute) return true;
            if (a.attribute < b.attribute) return false;
            if (a.attribute == 0) return false;
            if (a == b) return false;
            if (a.attribute == 1)
            {
                return AbsGreater(a, b);
            }
            if (a.attribute == -1)
            {
                return !(AbsGreater(a, b));
            }
            return false;
        }

        public static bool operator >=(Infinitely a, Infinitely b)
        {
            if (a.attribute > b.attribute) return true;
            if (a.attribute < b.attribute) return false;
            //if (a.attribute == 0) return true;
            if (a == b) return true;
            if (a.attribute == 1)
            {
                return AbsGreater(a, b);
            }
            if (a.attribute == -1)
            {
                return !(AbsGreater(a, b));
            }
            return false;
        }

        public static bool operator >=(Infinitely a, decimal y)
        {
            Infinitely b = new Infinitely(a.allDigitsNumber, a.fractionDigitsNumber);
            toInfinitely(y, b);

            if (a.attribute > b.attribute) return true;
            if (a.attribute < b.attribute) return false;
            //if (a.attribute == 0) return true;
            if (a == b) return true;
            if (a.attribute == 1)
            {
                return AbsGreater(a, b);
            }
            if (a.attribute == -1)
            {
                return !(AbsGreater(a, b));
            }
            return false;
        }

        public static bool operator >=(decimal x, Infinitely b)
        {
            Infinitely a = new Infinitely(b.allDigitsNumber, b.fractionDigitsNumber);
            toInfinitely(x, a);

            if (a.attribute > b.attribute) return true;
            if (a.attribute < b.attribute) return false;
            //if (a.attribute == 0) return true;
            if (a == b) return true;
            if (a.attribute == 1)
            {
                return AbsGreater(a, b);
            }
            if (a.attribute == -1)
            {
                return !(AbsGreater(a, b));
            }
            return false;
        }

        public static bool operator <=(Infinitely a, Infinitely b)
        {
            if (a.attribute > b.attribute) return false;
            if (a.attribute < b.attribute) return true;

            if (a == b) return true;
            else
            {
                if (a.attribute == 1)
                {
                    return !AbsGreater(a, b);
                }
                if (a.attribute == -1)
                {
                    return AbsGreater(a, b);
                }
            }
            return false;
        }

        public static bool operator <=(Infinitely a, decimal y)
        {
            Infinitely b = new Infinitely(a.allDigitsNumber, a.fractionDigitsNumber);
            toInfinitely(y, b);

            if (a.attribute > b.attribute) return false;
            if (a.attribute < b.attribute) return true;

            if (a == b) return true;
            else
            {
                if (a.attribute == 1)
                {
                    return !AbsGreater(a, b);
                }
                if (a.attribute == -1)
                {
                    return AbsGreater(a, b);
                }
            }
            return false;
        }

        public static bool operator <=(decimal x, Infinitely b)
        {
            Infinitely a = new Infinitely(b.allDigitsNumber, b.fractionDigitsNumber);
            toInfinitely(x, a);

            if (a.attribute > b.attribute) return false;
            if (a.attribute < b.attribute) return true;

            if (a == b) return true;
            else
            {
                if (a.attribute == 1)
                {
                    return !AbsGreater(a, b);
                }
                if (a.attribute == -1)
                {
                    return AbsGreater(a, b);
                }
            }
            return false;
        }

        //|a| + |b|
        public static Infinitely AbsPlus(Infinitely a, Infinitely b)
        {
            Infinitely c = new Infinitely(a.allDigitsNumber, a.fractionDigitsNumber);
            c.attribute = a.attribute;
            long mod = 0;
            for (long i = 0; i < a.number.Length; i++)
            {
                long n = 0;
                for (int j = 0; j < 16; j++)
                {
                    long t = (a.number[i] >> 4 * j & 0x000000000000000F) + mod;
                    if (i < b.number.Length) t += (b.number[i] >> 4 * j & 0x000000000000000F);
                    if (t > 9)
                    {
                        mod = 1;
                        t %= 10;
                    }
                    else
                    {
                        mod = 0;
                    }
                    n |= t << 4 * j;
                }
                c.number[i] = n;
            }
            return c;
        }

        //|a| - |b|
        public static Infinitely AbsMinus(Infinitely a, Infinitely b)
        {
            Infinitely c = new Infinitely(a.allDigitsNumber, a.fractionDigitsNumber);
            long mod = 0;
            if (AbsGreater(a, b))
            {
                for (long i = 0; i < a.number.Length; i++)
                {
                    long n = 0;
                    for (int j = 0; j < 16; j++)
                    {
                        long t = (a.number[i] >> 4 * j & 0x000000000000000F) - mod;
                        if (i < b.number.Length) t -= (b.number[i] >> 4 * j & 0x000000000000000F);
                        if (t < 0)
                        {
                            mod = 1;
                            t += 10;
                        }
                        else
                        {
                            mod = 0;
                        }
                        n |= t << 4 * j;
                    }
                    c.number[i] = n;
                }
            }
            else
            {
                for (int i = 0; i < b.number.Length; i++)
                {
                    long n = 0;
                    for (int j = 0; j < 16; j++)
                    {
                        long t = (b.number[i] >> 4 * j & 0x000000000000000F) - mod;
                        if (i < a.number.Length) t -= (a.number[i] >> 4 * j & 0x000000000000000F);
                        if (t < 0)
                        {
                            mod = 1;
                            t += 10;
                        }
                        else
                        {
                            mod = 0;
                        }
                        n |= t << 4 * j;
                    }
                    c.number[i] = n;
                }
            }
            return c;
        }

        public static Infinitely operator +(Infinitely a, Infinitely b)
        {
            if (a.attribute == 0 )return b;
            if (b.attribute == 0) return a;
            if (a.attribute == b.attribute)
            {
                Infinitely c = AbsPlus(a, b);
                c.attribute = a.attribute;
                return c;          
            }
            else
            {
                if (AbsGreater(a, b))
                {
                    Infinitely c = AbsMinus(a, b);
                    c.attribute = a.attribute;
                    return c;
                }
                else
                {
                    Infinitely c = AbsMinus(b, a);
                    c.attribute = b.attribute;
                    return c;
                }
            }
        }

        public static Infinitely operator +(Infinitely a, decimal y)
        {
            Infinitely b = new Infinitely(a.allDigitsNumber, a.fractionDigitsNumber);
            toInfinitely(y, b);

            if (a.attribute == 0) return b;
            if (b.attribute == 0) return a;
            if (a.attribute == b.attribute)
            {
                Infinitely c = AbsPlus(a, b);
                c.attribute = a.attribute;
                return c;
            }
            else
            {
                if (AbsGreater(a, b))
                {
                    Infinitely c = AbsMinus(a, b);
                    c.attribute = a.attribute;
                    return c;
                }
                else
                {
                    Infinitely c = AbsMinus(b, a);
                    c.attribute = b.attribute;
                    return c;
                }
            }
        }

        public static Infinitely operator +(decimal x, Infinitely b)
        {
            Infinitely a = new Infinitely(b.allDigitsNumber, b.fractionDigitsNumber);
            toInfinitely(x, a);

            if (a.attribute == 0) return b;
            if (b.attribute == 0) return a;
            if (a.attribute == b.attribute)
            {
                Infinitely c = AbsPlus(a, b);
                c.attribute = a.attribute;
                return c;
            }
            else
            {
                if (AbsGreater(a, b))
                {
                    Infinitely c = AbsMinus(a, b);
                    c.attribute = a.attribute;
                    return c;
                }
                else
                {
                    Infinitely c = AbsMinus(b, a);
                    c.attribute = b.attribute;
                    return c;
                }
            }
        }

        public static Infinitely operator -(Infinitely a, Infinitely b)
        {
            if (b.attribute == 0)
            {
                Infinitely v = new Infinitely(a.allDigitsNumber, a.fractionDigitsNumber);
                v.number = a.number;
                v.attribute = a.attribute;
                return v;
            }
            if (a.attribute == 0)
            {
                Infinitely v = new Infinitely(b.allDigitsNumber, b.fractionDigitsNumber);
                v.number = b.number;
                v.attribute = -b.attribute;
                return v;
            }
            if (a.attribute == b.attribute)
            {
                Console.WriteLine("Abs = {0}", AbsGreater(a, b));
                if (AbsGreater(a, b))
                {
                    Infinitely c = AbsMinus(a, b);
                    c.attribute = a.attribute;
                    return c;
                }
                else
                {
                    Infinitely c = AbsMinus(b, a);
                    c.attribute = - b.attribute;
                    return c;
                }
            }
            else
            {
                Infinitely c = AbsPlus(a, b);
                c.attribute = a.attribute;
                return c;
            }
        }

        public static Infinitely operator -(Infinitely a, decimal y)
        {
            Infinitely b = new Infinitely(a.allDigitsNumber, a.fractionDigitsNumber);
            toInfinitely(y, b);

            if (b.attribute == 0)
            {
                Infinitely v = new Infinitely(a.allDigitsNumber, a.fractionDigitsNumber);
                v.number = a.number;
                v.attribute = a.attribute;
                return v;
            }
            if (a.attribute == 0)
            {
                Infinitely v = new Infinitely(b.allDigitsNumber, b.fractionDigitsNumber);
                v.number = b.number;
                v.attribute = -b.attribute;
                return v;
            }
            if (a.attribute == b.attribute)
            {
                Console.WriteLine("Abs = {0}", AbsGreater(a, b));
                if (AbsGreater(a, b))
                {
                    Infinitely c = AbsMinus(a, b);
                    c.attribute = a.attribute;
                    return c;
                }
                else
                {
                    Infinitely c = AbsMinus(b, a);
                    c.attribute = -b.attribute;
                    return c;
                }
            }
            else
            {
                Infinitely c = AbsPlus(a, b);
                c.attribute = a.attribute;
                return c;
            }
        }

        public static Infinitely operator -(decimal x, Infinitely b)
        {
            Infinitely a = new Infinitely(b.allDigitsNumber, b.fractionDigitsNumber);
            toInfinitely(x, a);

            if (b.attribute == 0)
            {
                Infinitely v = new Infinitely(a.allDigitsNumber, a.fractionDigitsNumber);
                v.number = a.number;
                v.attribute = a.attribute;
                return v;
            }
            if (a.attribute == 0)
            {
                Infinitely v = new Infinitely(b.allDigitsNumber, b.fractionDigitsNumber);
                v.number = b.number;
                v.attribute = -b.attribute;
                return v;
            }
            if (a.attribute == b.attribute)
            {
                Console.WriteLine("Abs = {0}", AbsGreater(a, b));
                if (AbsGreater(a, b))
                {
                    Infinitely c = AbsMinus(a, b);
                    c.attribute = a.attribute;
                    return c;
                }
                else
                {
                    Infinitely c = AbsMinus(b, a);
                    c.attribute = -b.attribute;
                    return c;
                }
            }
            else
            {
                Infinitely c = AbsPlus(a, b);
                c.attribute = a.attribute;
                return c;
            }
        }

        // асинхронное умножение через ThreadPool.QueueUserWorkItem
        public static Infinitely AsyncMull(Infinitely a, Infinitely b)
        {
            Infinitely c = new Infinitely(a.allDigitsNumber, a.fractionDigitsNumber);
            c.attribute = a.attribute * b.attribute;
            
            if (c.attribute != ZERO)
            {
                int ii = 0, jj = 0;
                for (long fd = c.fractionDigitsNumber; fd > 0; fd--)
                {
                    if (++jj == 16)
                    {
                        jj = 0;
                        ii++;
                    }
                }
                for (int i = 0; i < a.number.Length; i++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        Point p = new Point(i, j, ii, jj, a, b, c);
                        ThreadPool.QueueUserWorkItem(Mull, p);
                    }
                }
            }
            else
            {
                for (int i = 0; i <= c.number.Length; i++)
                {
                    c.number[i] = 0;
                }
            }
            //Thread.Sleep(3);
            return c;
        }

        // асинхронное умножение через Task
        public static Infinitely tAsyncMull(Infinitely a, Infinitely b)
        {
            Infinitely c = new Infinitely(a.allDigitsNumber, a.fractionDigitsNumber);
            c.attribute = a.attribute * b.attribute;
            
            if (c.attribute != ZERO)
            {
                int ii = 0, jj = 0;
                for (long fd = c.fractionDigitsNumber; fd > 0; fd--)
                {
                    if (++jj == 16)
                    {
                        jj = 0;
                        ii++;
                    }
                }
                for (int i = 0; i < a.number.Length; i++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        Point p = new Point(i, j, ii, jj, a, b, c);
                        new Task(Mull, p).Start();
                    }
                }
            }
            else
            {
                for (int i = 0; i <= c.number.Length; i++)
                {
                    c.number[i] = 0;
                }
            }
            Task.WaitAll();

            return c;
        }

        // умножение
        public static Infinitely operator *(Infinitely a, Infinitely b)
        {
            Infinitely c = new Infinitely(a.allDigitsNumber, a.fractionDigitsNumber);
            c.attribute = a.attribute * b.attribute;
            
            if (c.attribute != ZERO)
            {
                int ii = 0, jj = 0;
                for (long fd = c.fractionDigitsNumber; fd > 0; fd--)
                {
                    if (++jj == 16)
                    {
                        jj = 0;
                        ii++;
                    }
                }
                for (int i = 0; i < a.number.Length; i++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        long t = (a.number[i] >> 4 * j & 0x000000000000000F);
                        int m = j - jj, p = i - ii;
                        if (m < -16)
                        {
                            p--;
                            m += 15;
                        }
                        long n = 0, mod = 0;
                        if (p >= 0)
                        {
                            for (int l = 0; l < m; l++)
                            {
                                n |= (c.number[p] >> 4 * l & 0x000000000000000F) << 4 * l;
                            }
                        }
                        for (int k = 0; k < b.number.Length; k++)
                        {
                            for (int h = 0; (h < 16) && (p < c.number.Length); h++)
                            {
                                if (p >= 0)
                                {
                                    long f = (b.number[k] >> 4 * h & 0x000000000000000F) * t + mod;
                                    if (p < c.number.Length) f += (c.number[p] >> 4 * m & 0x000000000000000F);
                                    if (f > 9)
                                    {
                                        mod = f / 10;
                                        f %= 10;
                                    }
                                    else
                                    {
                                        mod = 0;
                                    }

                                    n |= f << 4 * m;
                                    if (++m > 15)
                                    {
                                        c.number[p] = n;
                                        n = 0;
                                        m = 0;
                                        p++;
                                    }
                                }
                                else
                                {
                                    if (++m > 15)
                                    {
                                        m = 0;
                                        p++;
                                    }
                                }

                            }
                        }
                    }
                }
            }
            return c;
        }

        public static Infinitely operator *(Infinitely a, decimal y)
        {
            Infinitely b = new Infinitely(a.allDigitsNumber, a.fractionDigitsNumber);
            toInfinitely(y, b);
            Infinitely c = new Infinitely(a.allDigitsNumber, a.fractionDigitsNumber);
            c.attribute = a.attribute * b.attribute;

            if (c.attribute != ZERO)
            {
                int ii = 0, jj = 0;
                for (long fd = c.fractionDigitsNumber; fd > 0; fd--)
                {
                    if (++jj == 16)
                    {
                        jj = 0;
                        ii++;
                    }
                }
                for (int i = 0; i < a.number.Length; i++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        long t = (a.number[i] >> 4 * j & 0x000000000000000F);
                        int m = j - jj, p = i - ii;
                        if (m < -16)
                        {
                            p--;
                            m += 15;
                        }
                        long n = 0, mod = 0;
                        if (p >= 0)
                        {
                            for (int l = 0; l < m; l++)
                            {
                                n |= (c.number[p] >> 4 * l & 0x000000000000000F) << 4 * l;
                            }
                        }
                        for (int k = 0; k < b.number.Length; k++)
                        {
                            for (int h = 0; (h < 16) && (p < c.number.Length); h++)
                            {
                                if (p >= 0)
                                {
                                    long f = (b.number[k] >> 4 * h & 0x000000000000000F) * t + mod;
                                    if (p < c.number.Length) f += (c.number[p] >> 4 * m & 0x000000000000000F);
                                    if (f > 9)
                                    {
                                        mod = f / 10;
                                        f %= 10;
                                    }
                                    else
                                    {
                                        mod = 0;
                                    }

                                    n |= f << 4 * m;
                                    if (++m > 15)
                                    {
                                        c.number[p] = n;
                                        n = 0;
                                        m = 0;
                                        p++;
                                    }
                                }
                                else
                                {
                                    if (++m > 15)
                                    {
                                        m = 0;
                                        p++;
                                    }
                                }

                            }
                        }
                    }
                }
            }
            return c;
        }

        public static Infinitely operator *(decimal x, Infinitely b)
        {
            Infinitely a = new Infinitely(b.allDigitsNumber, b.fractionDigitsNumber);
            toInfinitely(x, a);
            Infinitely c = new Infinitely(a.allDigitsNumber, a.fractionDigitsNumber);
            c.attribute = a.attribute * b.attribute;

            if (c.attribute != ZERO)
            {
                int ii = 0, jj = 0;
                for (long fd = c.fractionDigitsNumber; fd > 0; fd--)
                {
                    if (++jj == 16)
                    {
                        jj = 0;
                        ii++;
                    }
                }
                for (int i = 0; i < a.number.Length; i++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        long t = (a.number[i] >> 4 * j & 0x000000000000000F);
                        int m = j - jj, p = i - ii;
                        if (m < -16)
                        {
                            p--;
                            m += 15;
                        }
                        long n = 0, mod = 0;
                        if (p >= 0)
                        {
                            for (int l = 0; l < m; l++)
                            {
                                n |= (c.number[p] >> 4 * l & 0x000000000000000F) << 4 * l;
                            }
                        }
                        for (int k = 0; k < b.number.Length; k++)
                        {
                            for (int h = 0; (h < 16) && (p < c.number.Length); h++)
                            {
                                if (p >= 0)
                                {
                                    long f = (b.number[k] >> 4 * h & 0x000000000000000F) * t + mod;
                                    if (p < c.number.Length) f += (c.number[p] >> 4 * m & 0x000000000000000F);
                                    if (f > 9)
                                    {
                                        mod = f / 10;
                                        f %= 10;
                                    }
                                    else
                                    {
                                        mod = 0;
                                    }

                                    n |= f << 4 * m;
                                    if (++m > 15)
                                    {
                                        c.number[p] = n;
                                        n = 0;
                                        m = 0;
                                        p++;
                                    }
                                }
                                else
                                {
                                    if (++m > 15)
                                    {
                                        m = 0;
                                        p++;
                                    }
                                }

                            }
                        }
                    }
                }
            }
            return c;
        }

        //деление
        public static Infinitely operator /(Infinitely x, Infinitely b)
        {
            Infinitely a = Copy(x);
            Infinitely c = new Infinitely(a.allDigitsNumber, a.fractionDigitsNumber);
            checked
            {
                try
                {
                    c.attribute = a.attribute / b.attribute;
                    /*  i - номер элемента в массиве делимого
                     *  j - номер цифры в i-м элементе массива делимого
                     *  p - номер элемента в массиве делителя
                     *  q - номер цифры в p-м элементе массива делителя
                     *  ci - номер элемента в массиве результата 
                     *  cj - номер цифры в ci-м элементе массива результата
                     *  inci
                     *  incj
                     *  inc               
                    */
                    int iDvdDigPos = 0, jDvdDigPos = 0, iDzrDigPos = 0, jDzrDigPos = 0, ci = 0, cj = 0, inci = 0, incj = 0;
                    bool inc = false;
                    //определяем позицию первой цифры
                    FindBegin(a, ref iDvdDigPos, ref jDvdDigPos);
                    FindBegin(b, ref iDzrDigPos, ref jDzrDigPos);
                   
                    //определяем позицию первой цифры результата деления
                    ci = iDvdDigPos - iDzrDigPos; cj = jDvdDigPos - jDzrDigPos;
                    if (cj < 0) { ci--; cj += 16; }
                    long w = 0;
                    while (w < a.fractionDigitsNumber)
                    {   if (++cj > 15) { ++ci; cj = 0; }
                        ++w;
                    }
                    for (; iDvdDigPos >= 0; iDvdDigPos--, jDvdDigPos = 15)
                    {
                        for (; jDvdDigPos >= 0; jDvdDigPos--)
                        {
                            long res = 0; bool run = true;
                            while (run && (ci >= 0))
                            {
                                int ii = iDvdDigPos, jj = jDvdDigPos, pp = iDzrDigPos, qq = jDzrDigPos;
                                bool g = true, l = true;
                                if (!inc)
                                {
                                    while (g && l && (pp >= 0) && (qq >= 0) && (ii >= 0) && (jj >= 0))
                                    {
                                        if ((a.number[ii] >> 4 * jj & 0x000000000000000F) > (b.number[pp] >> 4 * qq & 0x000000000000000F)) l = false;
                                        if ((a.number[ii] >> 4 * jj & 0x000000000000000F) < (b.number[pp] >> 4 * qq & 0x000000000000000F)) g = false;
                                        if (--jj < 0) { jj = 15; --ii; }
                                        if (--qq < 0) { qq = 15; --pp; }
                                    }
                                }
                                if (g)
                                {   inc = false; run = true;
                                    int itDvdDigPos = iDvdDigPos - iDzrDigPos;
                                    int jtDvdDigPos = jDvdDigPos - jDzrDigPos;
                                    if (jtDvdDigPos < 0) { itDvdDigPos--; jtDvdDigPos += 16; }
                                    
                                    int mod = 0, itDzrDigPos = 0, jtDzrDigPos = 0, ffi = 0, ffj = 0, bbi = 0, bbj = 0;
                                    // определяем разряд, если вышли за границы числа справа
                                    while (itDvdDigPos < 0)
                                    {   if (++jtDvdDigPos > 15) { ++itDvdDigPos; jtDvdDigPos = 0; }
                                        if (++jtDzrDigPos > 15) { ++itDzrDigPos; jtDzrDigPos = 0; }
                                    }

                                    while (((itDzrDigPos <= iDzrDigPos) || (mod > 0)) && (itDvdDigPos < a.number.Length))
                                    {
                                        long t = (a.number[itDvdDigPos] >> 4 * (jtDvdDigPos) & 0x000000000000000F) - mod;
                                        if (itDzrDigPos < b.number.Length) t -= (b.number[itDzrDigPos] >> 4 * jtDzrDigPos & 0x000000000000000F);
                                        if (t < 0) { mod = 1; t += 10; }
                                        else { mod = 0; }
                                        
                                        long f = 0;//отнимаем от делимого числа
                                        for (int u = 0; u <= 15; u++)
                                        {
                                            if (u == jtDvdDigPos) f |= t << 4 * u;
                                            else f |= (a.number[itDvdDigPos] >> 4 * u & 0x000000000000000F) << 4 * u;
                                        }//получаем делимое, из которого вычли число [fi, fj]-тых разрядов
                                        a.number[itDvdDigPos] = f;
                                        if (++jtDvdDigPos > 15) { itDvdDigPos++; jtDvdDigPos = 0; }
                                        if (++jtDzrDigPos > 15) { itDzrDigPos++; jtDzrDigPos = 0; }
                                    }
                                    ++res;
                                    if ((a.number[inci] >> 4 * incj & 0x000000000000000F) > 0) inc = true;
                                    else inc = false;
                                }
                                else
                                {
                                    if ((a.number[iDvdDigPos] >> 4 * jDvdDigPos & 0x000000000000000F) > 0)
                                    {
                                        inc = true;
                                       inci = iDvdDigPos;
                                       incj = jDvdDigPos;
                                    }
                                    run = false;
                                    if (res <= 9) { c.number[ci] |= res << 4 * cj; }
                                    else
                                    {
                                        long ri = ci;int rj = cj;
                                        c.number[ri] |= (res % 10) << 4 * rj;
                                        res /= 10;
                                        while (res > 0)
                                        {
                                            if (++rj > 15) { ++ri; rj = 0; }
                                            res += (c.number[ri] >> 4 * rj & 0x000000000000000F);// + (res % 10);
                                            c.number[ri] |= (res % 10) << 4 * rj;//res = res2;
                                            res /= 10;
                                        }
                                    }
                                    if (--cj < 0) { --ci; cj = 15; }
                                }
                            }// while (run && (ci >= 0))
                        }// for j                  
                    }// for i
                    if (isZero(c)) c.attribute = ZERO;
                    return c;
                }
                catch (DivideByZeroException) { Console.WriteLine("DIVIDE BY ZERO"); return c; }
            }
        }

        public static Infinitely operator /(Infinitely x, decimal y)
        {
            Infinitely a = Copy(x);
            Infinitely c = new Infinitely(a.allDigitsNumber, a.fractionDigitsNumber);
            Infinitely b = new Infinitely(a.allDigitsNumber, a.fractionDigitsNumber);
            toInfinitely(y, b);
            checked
            {
                try
                {
                    c.attribute = a.attribute / b.attribute;
                    /*  i - номер элемента в массиве делимого
                     *  j - номер цифры в i-м элементе массива делимого
                     *  p - номер элемента в массиве делителя
                     *  q - номер цифры в p-м элементе массива делителя
                     *  ci - номер элемента в массиве результата 
                     *  cj - номер цифры в ci-м элементе массива результата
                     *  inci
                     *  incj
                     *  inc               
                    */
                    int iDvdDigPos = 0, jDvdDigPos = 0, iDzrDigPos = 0, jDzrDigPos = 0, ci = 0, cj = 0, inci = 0, incj = 0;
                    bool inc = false;
                    //определяем позицию первой цифры
                    FindBegin(a, ref iDvdDigPos, ref jDvdDigPos);
                    FindBegin(b, ref iDzrDigPos, ref jDzrDigPos);

                    //определяем позицию первой цифры результата деления
                    ci = iDvdDigPos - iDzrDigPos; cj = jDvdDigPos - jDzrDigPos;
                    if (cj < 0) { ci--; cj += 16; }
                    long w = 0;
                    while (w < a.fractionDigitsNumber)
                    {
                        if (++cj > 15) { ++ci; cj = 0; }
                        ++w;
                    }
                    for (; iDvdDigPos >= 0; iDvdDigPos--, jDvdDigPos = 15)
                    {
                        for (; jDvdDigPos >= 0; jDvdDigPos--)
                        {
                            long res = 0; bool run = true;
                            while (run && (ci >= 0))
                            {
                                int ii = iDvdDigPos, jj = jDvdDigPos, pp = iDzrDigPos, qq = jDzrDigPos;
                                bool g = true, l = true;
                                if (!inc)
                                {
                                    while (g && l && (pp >= 0) && (qq >= 0) && (ii >= 0) && (jj >= 0))
                                    {
                                        if ((a.number[ii] >> 4 * jj & 0x000000000000000F) > (b.number[pp] >> 4 * qq & 0x000000000000000F)) l = false;
                                        if ((a.number[ii] >> 4 * jj & 0x000000000000000F) < (b.number[pp] >> 4 * qq & 0x000000000000000F)) g = false;
                                        if (--jj < 0) { jj = 15; --ii; }
                                        if (--qq < 0) { qq = 15; --pp; }
                                    }
                                }
                                if (g)
                                {
                                    inc = false; run = true;
                                    int itDvdDigPos = iDvdDigPos - iDzrDigPos;
                                    int jtDvdDigPos = jDvdDigPos - jDzrDigPos;
                                    if (jtDvdDigPos < 0) { itDvdDigPos--; jtDvdDigPos += 16; }

                                    int mod = 0, itDzrDigPos = 0, jtDzrDigPos = 0, ffi = 0, ffj = 0, bbi = 0, bbj = 0;
                                    // определяем разряд, если вышли за границы числа справа
                                    while (itDvdDigPos < 0)
                                    {
                                        if (++jtDvdDigPos > 15) { ++itDvdDigPos; jtDvdDigPos = 0; }
                                        if (++jtDzrDigPos > 15) { ++itDzrDigPos; jtDzrDigPos = 0; }
                                    }

                                    while (((itDzrDigPos <= iDzrDigPos) || (mod > 0)) && (itDvdDigPos < a.number.Length))
                                    {
                                        long t = (a.number[itDvdDigPos] >> 4 * (jtDvdDigPos) & 0x000000000000000F) - mod;
                                        if (itDzrDigPos < b.number.Length) t -= (b.number[itDzrDigPos] >> 4 * jtDzrDigPos & 0x000000000000000F);
                                        if (t < 0) { mod = 1; t += 10; }
                                        else { mod = 0; }

                                        long f = 0;//отнимаем от делимого числа
                                        for (int u = 0; u <= 15; u++)
                                        {
                                            if (u == jtDvdDigPos) f |= t << 4 * u;
                                            else f |= (a.number[itDvdDigPos] >> 4 * u & 0x000000000000000F) << 4 * u;
                                        }//получаем делимое, из которого вычли число [fi, fj]-тых разрядов
                                        a.number[itDvdDigPos] = f;
                                        if (++jtDvdDigPos > 15) { itDvdDigPos++; jtDvdDigPos = 0; }
                                        if (++jtDzrDigPos > 15) { itDzrDigPos++; jtDzrDigPos = 0; }
                                    }
                                    ++res;
                                    if ((a.number[inci] >> 4 * incj & 0x000000000000000F) > 0) inc = true;
                                    else inc = false;
                                }
                                else
                                {
                                    if ((a.number[iDvdDigPos] >> 4 * jDvdDigPos & 0x000000000000000F) > 0)
                                    {
                                        inc = true;
                                        inci = iDvdDigPos;
                                        incj = jDvdDigPos;
                                    }
                                    run = false;
                                    if (res <= 9) { c.number[ci] |= res << 4 * cj; }
                                    else
                                    {
                                        long ri = ci; int rj = cj;
                                        c.number[ri] |= (res % 10) << 4 * rj;
                                        res /= 10;
                                        while (res > 0)
                                        {
                                            if (++rj > 15) { ++ri; rj = 0; }
                                            res += (c.number[ri] >> 4 * rj & 0x000000000000000F);// + (res % 10);
                                            c.number[ri] |= (res % 10) << 4 * rj;//res = res2;
                                            res /= 10;
                                        }
                                    }
                                    if (--cj < 0) { --ci; cj = 15; }
                                }
                            }// while (run && (ci >= 0))
                        }// for j                  
                    }// for i
                    if (isZero(c)) c.attribute = ZERO;
                    return c;
                }
                catch (DivideByZeroException) { Console.WriteLine("DIVIDE BY ZERO"); return c; }
            }
        }

        public static Infinitely operator /(decimal x, Infinitely b)
        {
            Infinitely a = new Infinitely(b.allDigitsNumber, b.fractionDigitsNumber);
            toInfinitely(x, a);
            Infinitely c = new Infinitely(a.allDigitsNumber, a.fractionDigitsNumber);
            checked
            {
                try
                {
                    c.attribute = a.attribute / b.attribute;
                    /*  i - номер элемента в массиве делимого
                     *  j - номер цифры в i-м элементе массива делимого
                     *  p - номер элемента в массиве делителя
                     *  q - номер цифры в p-м элементе массива делителя
                     *  ci - номер элемента в массиве результата 
                     *  cj - номер цифры в ci-м элементе массива результата
                     *  inci
                     *  incj
                     *  inc               
                    */
                    int iDvdDigPos = 0, jDvdDigPos = 0, iDzrDigPos = 0, jDzrDigPos = 0, ci = 0, cj = 0, inci = 0, incj = 0;
                    bool inc = false;
                    //определяем позицию первой цифры
                    FindBegin(a, ref iDvdDigPos, ref jDvdDigPos);
                    FindBegin(b, ref iDzrDigPos, ref jDzrDigPos);

                    //определяем позицию первой цифры результата деления
                    ci = iDvdDigPos - iDzrDigPos; cj = jDvdDigPos - jDzrDigPos;
                    if (cj < 0) { ci--; cj += 16; }
                    long w = 0;
                    while (w < a.fractionDigitsNumber)
                    {
                        if (++cj > 15) { ++ci; cj = 0; }
                        ++w;
                    }
                    for (; iDvdDigPos >= 0; iDvdDigPos--, jDvdDigPos = 15)
                    {
                        for (; jDvdDigPos >= 0; jDvdDigPos--)
                        {
                            long res = 0; bool run = true;
                            while (run && (ci >= 0))
                            {
                                int ii = iDvdDigPos, jj = jDvdDigPos, pp = iDzrDigPos, qq = jDzrDigPos;
                                bool g = true, l = true;
                                if (!inc)
                                {
                                    while (g && l && (pp >= 0) && (qq >= 0) && (ii >= 0) && (jj >= 0))
                                    {
                                        if ((a.number[ii] >> 4 * jj & 0x000000000000000F) > (b.number[pp] >> 4 * qq & 0x000000000000000F)) l = false;
                                        if ((a.number[ii] >> 4 * jj & 0x000000000000000F) < (b.number[pp] >> 4 * qq & 0x000000000000000F)) g = false;
                                        if (--jj < 0) { jj = 15; --ii; }
                                        if (--qq < 0) { qq = 15; --pp; }
                                    }
                                }
                                if (g)
                                {
                                    inc = false; run = true;
                                    int itDvdDigPos = iDvdDigPos - iDzrDigPos;
                                    int jtDvdDigPos = jDvdDigPos - jDzrDigPos;
                                    if (jtDvdDigPos < 0) { itDvdDigPos--; jtDvdDigPos += 16; }

                                    int mod = 0, itDzrDigPos = 0, jtDzrDigPos = 0, ffi = 0, ffj = 0, bbi = 0, bbj = 0;
                                    // определяем разряд, если вышли за границы числа справа
                                    while (itDvdDigPos < 0)
                                    {
                                        if (++jtDvdDigPos > 15) { ++itDvdDigPos; jtDvdDigPos = 0; }
                                        if (++jtDzrDigPos > 15) { ++itDzrDigPos; jtDzrDigPos = 0; }
                                    }

                                    while (((itDzrDigPos <= iDzrDigPos) || (mod > 0)) && (itDvdDigPos < a.number.Length))
                                    {
                                        long t = (a.number[itDvdDigPos] >> 4 * (jtDvdDigPos) & 0x000000000000000F) - mod;
                                        if (itDzrDigPos < b.number.Length) t -= (b.number[itDzrDigPos] >> 4 * jtDzrDigPos & 0x000000000000000F);
                                        if (t < 0) { mod = 1; t += 10; }
                                        else { mod = 0; }

                                        long f = 0;//отнимаем от делимого числа
                                        for (int u = 0; u <= 15; u++)
                                        {
                                            if (u == jtDvdDigPos) f |= t << 4 * u;
                                            else f |= (a.number[itDvdDigPos] >> 4 * u & 0x000000000000000F) << 4 * u;
                                        }//получаем делимое, из которого вычли число [fi, fj]-тых разрядов
                                        a.number[itDvdDigPos] = f;
                                        if (++jtDvdDigPos > 15) { itDvdDigPos++; jtDvdDigPos = 0; }
                                        if (++jtDzrDigPos > 15) { itDzrDigPos++; jtDzrDigPos = 0; }
                                    }
                                    ++res;
                                    if ((a.number[inci] >> 4 * incj & 0x000000000000000F) > 0) inc = true;
                                    else inc = false;
                                }
                                else
                                {
                                    if ((a.number[iDvdDigPos] >> 4 * jDvdDigPos & 0x000000000000000F) > 0)
                                    {
                                        inc = true;
                                        inci = iDvdDigPos;
                                        incj = jDvdDigPos;
                                    }
                                    run = false;
                                    if (res <= 9) { c.number[ci] |= res << 4 * cj; }
                                    else
                                    {
                                        long ri = ci; int rj = cj;
                                        c.number[ri] |= (res % 10) << 4 * rj;
                                        res /= 10;
                                        while (res > 0)
                                        {
                                            if (++rj > 15) { ++ri; rj = 0; }
                                            res += (c.number[ri] >> 4 * rj & 0x000000000000000F);// + (res % 10);
                                            c.number[ri] |= (res % 10) << 4 * rj;//res = res2;
                                            res /= 10;
                                        }
                                    }
                                    if (--cj < 0) { --ci; cj = 15; }
                                }
                            }// while (run && (ci >= 0))
                        }// for j                  
                    }// for i
                    if (isZero(c)) c.attribute = ZERO;
                    return c;
                }
                catch (DivideByZeroException) { Console.WriteLine("DIVIDE BY ZERO"); return c; }
            }
        }

        //возвращает первый разряд целой части числа
        private static void FindFrac(Infinitely a, ref long i, ref int j)
        {
            long w = a.fractionDigitsNumber;
            i = 0;
            j = 0;
            while (w > 0)
            {
                if (++j > 15)
                {
                    ++i;
                    j = 0;
                }
                --w;
            }
        }


        //----------------------------------------------------------------------------------------
        // НА ИСПРАВЛЕНИЕ
        //----------------------------------------------------------------------------------------

        // вывод числа в консоль
        //// позже вместо этого перегружу ToString()
        public static void Show(Infinitely a)
        {
           if (a.attribute == 0) Console.Write(0);
           else
            {
                if (a.attribute == -1) Console.Write("-");
                int i = a.number.Length - 1, j = 15;
                long w = 0;
                int wi = 0, wj = -1;
                //поиск позиции первой цифры дробной части
                while (w < a.fractionDigitsNumber)
                {
                    if (++wj > 15)
                    {
                        wj = 0;
                        ++wi;
                    }
                    ++w;
                }
                // пропускаем незначащие нули до точки
                bool p = true;
                while (((i >= 0)&&(a.number[i] >> 4 * j & 0x000000000000000F) == 0) && p)
                {
                    if ((i == wi) && (j == wj))
                    {
                        Console.Write("0.{0}", a.number[i] >> 4 * j & 0x000000000000000F);
                        p = false;
                    }
                    if (--j < 0)
                    {
                        j = 15;
                        i--;
                    }
                }
                //если первая ненулевая цифра числа находится в дробной части, то приписываем ноль к целой части
                if (i >= 0)
                {
                    if ((i == wi) && (j == wj))
                    {
                        if (p) Console.Write("0.");
                    }
                    Console.Write("{0}", a.number[i] >> 4 * j & 0x000000000000000F);
                    if (--j < 0)
                    {
                        j = 15;
                        i--;
                    }
                }
                //вывод остальных цифр числа
                while (i >= 0)
                {
                    if ((i == wi) && (j == wj))
                    {
                        Console.Write(".");
                    }
                    Console.Write("{0}", a.number[i] >> 4 * j & 0x000000000000000F);
                    if (--j < 0)
                    {
                        j = 15;
                        i--;
                    }
                }
            }
            Console.WriteLine();
        }

        // выводт число с незначащими нулями
        public static void ShowAll(Infinitely a)
        {
            if (a.attribute == 0) Console.Write(0);
            else
            {
                int i = a.number.Length - 1, j = 15;
                int wi = 0, wj = 15;

                //вывод остальных цифр числа
                while (i >= 0)
                {
                    if ((i == wi) && (j == wj))
                    {
                        Console.Write(".");
                    }
                    Console.Write("{0}", a.number[i] >> 4 * j & 0x000000000000000F);
                    if (--j < 0)
                    {
                        j = 15;
                        i--;
                        //delete this
                        Console.Write(" ");
                    }
                }

            }
        }

        /*public static explicit operator Infinitely(int d)
       {
           if (d != 0)
           {
               int g = d;
               int h = 0;
               while (g != 0)
               {
                   g /= 10;
                   h++;
               }
               Infinitely a = new Infinitely(h, 0);
               a.allDigitsNumber = h;

               if (d > 0)
               {
                   a.attribute = 1;
                   g = d;
               }
               else
               {
                   a.attribute = -1;
                   g = -d;
               }

               h = (h - 1) / 16;
               for (int i = 0; i <= h; i++)
               {
                   long n = 0;
                   for (int j = 0; j < 16; j++)
                   {
                       long t = (g % 10);
                       n |= t << 4 * j;
                       g /= 10;
                   }
                   a.number[i] = n;
               }
               return a;
           }
           else
           {
               Infinitely a = new Infinitely(10,5);
               a.attribute = 0;
               for (int i = 0; i < a.number.Length; i++)
               {
                   a.number[i] = 0;
               }
               return a;
           }
   }*/

        public static void toInfinitely(int d, Infinitely a)
        {
            a.number = new long[1 + a.allDigitsNumber / 2];
            if (d != 0)
            {
                int g;
                if (d > 0)
                {
                    a.attribute = 1;
                    g = d;
                }
                else
                {
                    a.attribute = -1;
                    g = -d;
                }
                int i = 0, j = 0;
                long w = 0;
                while (w < a.fractionDigitsNumber)
                {
                    if (++j > 15)
                    {
                        j = 0;
                        ++i;
                    }
                    ++w;
                }

                for (; i < a.number.Length; i++)
                {
                    long n = 0;
                    for (; j < 16; j++)
                    {
                        long t = (g % 10);
                        n |= t << 4 * j;
                        g /= 10;
                    }
                    a.number[i] = n;
                }
            }
            else
            {
                a.attribute = 0;
                for (int i = 0; i < a.number.Length; i++)
                {
                    a.number[i] = 0;
                }
            }
        }

        public static void toInfinitely(long d, Infinitely a)
        {
            a.number = new long[1 + a.allDigitsNumber / 2];
            if (d != 0)
            {
                long g;
                if (d > 0)
                {
                    a.attribute = 1;
                    g = d;
                }
                else
                {
                    a.attribute = -1;
                    g = -d;
                }
                int i = 0, j = 0;
                long w = 0;
                while (w < a.fractionDigitsNumber)
                {
                    if (++j > 15)
                    {
                        j = 0;
                        ++i;
                    }
                    ++w;
                }

                for (; i < a.number.Length; i++)
                {
                    long n = 0;
                    for (; j < 16; j++)
                    {
                        long t = (g % 10);
                        n |= t << 4 * j;
                        g /= 10;
                    }
                    a.number[i] = n;
                }
            }
            else
            {
                a.attribute = 0;
                for (int i = 0; i < a.number.Length; i++)
                {
                    a.number[i] = 0;
                }
            }
        }

        /*public static void toInfinitely (decimal d,  Infinitely a)
        {
            if (d != 0)
            {
                int i = 0, j = 0;
                decimal g;
                if (d > 0)
                {
                    a.attribute = 1;
                    g = d;
                }
                else
                {
                    a.attribute = -1;
                    g = -d;
                }
                FindFrac(a, ref i, ref j);
                while (g > 1)
                {
                    g /= 10;
                    if (++j > 15)
                    {
                        ++i;
                        j = 0;
                    }
                }
                for (; i >= 0; i--)
                {
                    long n = 0;
                    for (; j >= 0; j--)
                    {
                        long t = (long)g;
                        if (t > g) --t;
                        n |= t << 4 * j;
                        g = g * 10 - t * 10;
                    }
                    j = 15;
                    a.number[i] = n;
                }
            }
            else
            {
                a.attribute = 0;
                for (int i = 0; i < a.number.Length; i++)
                {
                    a.number[i] = 0;
                }
            }
        }
        */

        /*public static void toInfinitely (float d, ref Infinitely a)
{
    if (d != 0)
    {

        int i = 0, j = 0;
        float g;
        if (d > 0)
        {
            a.attribute = 1;
            g = d;
        }
        else
        {
            a.attribute = -1;
            g = -d;
        }
        FindFrac(a, ref i, ref j);
        while (g > 1)
        {
            g /= 10;
            if (++j > 15)
            {
                ++i;
                j = 0;
            }
        }

        for (; i >= 0; i--)
        {
            long n = 0;
            for (; j >= 0; j--)
            {
                long t = (long)g;
                if ((float)t > g) --t;
                n |= t << 4 * j;
                g = g*10 - t*10;
            }
            j = 15;
            a.number[i] = n;
        }
    }
    else
    {
        a.attribute = 0;
        for (int i = 0; i < a.number.Length; i++)
        {
            a.number[i] = 0;
        }
    }
}*/

    }
}