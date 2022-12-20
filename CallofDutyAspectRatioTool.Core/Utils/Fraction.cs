using System;
using System.Runtime.InteropServices;

namespace CallofDutyAspectRatioTool.Core.Utils
{
    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct Fraction
    {
        public long Numerator { get; set; }
        public long Denominator { get; set; }

        public double Decimal
        {
            get
            {
                return (double)Numerator / (double)Denominator;
            }
            set
            {
                // c# does not handle repeating floats to double, so i had to manually make a bad solution
                if (((float)value).IsRepeating())
                    value = ((float)value).RepeatingFloatToDouble();

                long fractionNumerator = (long)value;
                double fractionDenominator = 1;
                double previousDenominator = 0;
                double remainingDigits = value;
                int maxIterations = 594;
                while (remainingDigits != Math.Floor(remainingDigits) && Math.Abs(value - (fractionNumerator / fractionDenominator)) > double.Epsilon)
                {
                    remainingDigits = 1.0 / (remainingDigits - Math.Floor(remainingDigits));
                    double scratch = fractionDenominator;
                    fractionDenominator = (Math.Floor(remainingDigits) * fractionDenominator) + previousDenominator;
                    fractionNumerator = (long)(value * fractionDenominator + 0.5);
                    previousDenominator = scratch;
                    if (maxIterations-- < 0)
                        break;
                }
                Numerator = fractionNumerator * Math.Sign(value);
                Denominator = (long)fractionDenominator;
            }
        }

        public static Fraction NaN => new Fraction()
        {
            Numerator = 0,
            Denominator = 0
        };

        public static Fraction PositiveInfinity => new Fraction()
        {
            Numerator = 1,
            Denominator = 0
        };

        public static Fraction NegativeInfinity => new Fraction()
        {
            Numerator = -1,
            Denominator = 0
        };

        public static Fraction Zero => new Fraction()
        {
            Numerator = 0,
            Denominator = 1
        };

        public static Fraction Epsilon => new Fraction()
        {
            Numerator = 1,
            Denominator = long.MaxValue
        };

        public static Fraction MaxValue => new Fraction()
        {
            Numerator = long.MaxValue,
            Denominator = 1
        };

        public static Fraction MinValue => new Fraction()
        {
            Numerator = long.MinValue,
            Denominator = 1
        };

        public override string ToString() => Numerator.ToString() + "/" + Denominator.ToString();

        public bool IsNaN => Denominator == 0 && Numerator == 0;
        public bool IsInfinity => Denominator == 0 && Numerator != 0;
        public bool IsPositiveInfinity => Denominator == 0 && Numerator > 0;
        public bool IsNegativeInfinity => Denominator == 0 && Numerator < 0;

        public static Fraction operator -(Fraction left)
        {
            return new Fraction()
            {
                Numerator = -left.Numerator,
                Denominator = left.Denominator,
            };
        }

        public static Fraction operator +(Fraction left, Fraction right)
        {
            if (left.IsNaN || right.IsNaN)
                return NaN;
            long gcd = GCD(left.Denominator, right.Denominator);
            long leftDenominator = left.Denominator / gcd;
            long rightDenominator = right.Denominator / gcd;
            checked
            {
                long numerator = left.Numerator * rightDenominator + right.Numerator * leftDenominator;
                long denominator = leftDenominator * rightDenominator * gcd;

                return new Fraction()
                {
                    Numerator = numerator,
                    Denominator = denominator
                };
            }
        }

        public static Fraction operator -(Fraction left, Fraction right)
        {
            return left + -right;
        }

        public static Fraction operator *(Fraction left, Fraction right)
        {
            if (left.IsNaN || right.IsNaN)
                return NaN;
            return new Fraction()
            {
                Decimal = left.Decimal * right.Decimal
            };
        }

        public static Fraction operator /(Fraction left, Fraction right)
        {
            return left * right.Inverse();
        }

        public static Fraction operator %(Fraction left, Fraction right)
        {
            if (left.IsNaN || right.IsNaN)
                return NaN;
            checked
            {
                Int64 quotient = (Int64)(left / right);
                Fraction whole = new Fraction()
                {
                    Numerator = quotient * right.Numerator,
                    Denominator = right.Denominator
                };
                return left - whole;
            }
        }

        public static bool operator ==(Fraction left, Fraction right)
        {
            return left.Decimal == right.Decimal;
        }

        public static bool operator !=(Fraction left, Fraction right)
        {
            return !(left == right);
        }

        public static bool operator <(Fraction left, Fraction right)
        {
            return left.Decimal < right.Decimal;
        }

        public static bool operator >(Fraction left, Fraction right)
        {
            return left.Decimal > right.Decimal;
        }

        public static bool operator <=(Fraction left, Fraction right)
        {
            return left.Decimal <= right.Decimal;
        }

        public static bool operator >=(Fraction left, Fraction right)
        {
            return left.Decimal >= right.Decimal;
        }

        public static implicit operator Fraction(double value)
        {
            return new Fraction()
            {
                Decimal = value
            };
        }

        public static explicit operator double(Fraction frac)
        {
            return frac.Decimal;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Fraction))
                return false;
            return this == (Fraction)obj;
        }

        public override int GetHashCode()
        {
            Fraction frac = Reduce();
            int numeratorHash = frac.Numerator.GetHashCode();
            int denominatorHash = frac.Denominator.GetHashCode();
            return (numeratorHash ^ denominatorHash);
        }

        public Fraction Reduce()
        {
            long iGCD = GCD(Numerator, Denominator);
            return new Fraction()
            {
                Numerator = this.Numerator / iGCD,
                Denominator = this.Denominator / iGCD
            };
        }

        public Fraction Inverse()
        {
            return new Fraction()
            {
                Numerator = this.Denominator,
                Denominator = this.Numerator
            };
        }

        private static long GCD(long left, long right)
        {
            if (left < 0)
                left = -left;
            if (right < 0)
                right = -right;
            if (left < 2 || right < 2)
                return 1;
            do
            {
                if (left < right)
                {
                    long temp = left;
                    left = right;
                    right = temp;
                }
                left %= right;
            } while (left != 0);

            return right;
        }
    }
}