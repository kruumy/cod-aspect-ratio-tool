﻿using System;
using System.IO;
using System.Reflection;

namespace CallofDutyAspectRatioTool.Core.Utils
{
    public struct Fraction
    {
        public int Numerator { get; set; }
        public int Denominator { get; set; }
        public float Decimal
        {
            get
            {
                return (float)Numerator / (float)Denominator;
            }
            set
            {
                if(!float.IsNaN(value) && !float.IsInfinity(value))
                {
                    int i = 1;
                    while (true)
                    {
                        float res = value * (float)i;
                        System.Threading.Thread.Sleep(1); // idk if im crazy but without sleeping this does not work on release build
                        if (((int)res) == res)
                        {
                            Numerator = (int)res;
                            Denominator = i;
                            break;
                        }
                        i++;
                    }
                }
                else
                {
                    Numerator = 0;
                    Denominator = 0;
                }
            }
        }

        public Fraction Simplified
        {
            get
            {
                int gcd = GetGCD(Numerator, Denominator);
                return new Fraction()
                {
                    Numerator = Numerator / gcd,
                    Denominator = Denominator / gcd
                };
            }
        }

        private int GetGCD(int a, int b)
        {
            if (a == 0 || b == 0)
                return a + b;
            return GetGCD(b, a % b);
        }

        public override string ToString()
        {
            return $"{Numerator}/{Denominator}";
        }
        public static implicit operator float(Fraction f)
        {
            return f.Decimal;
        }
        public static implicit operator Fraction(float f)
        {
            return new Fraction
            {
                Decimal = f
            };
        }
        public static implicit operator double(Fraction f)
        {
            return f.Decimal;
        }
        public static implicit operator Fraction(double d)
        {
            return new Fraction
            {
                Decimal = (float)d
            };
        }

    }
}
