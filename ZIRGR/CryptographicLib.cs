using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Numerics;
using System.Windows.Navigation;

namespace ZIRGR
{
	internal class CryptographicLib
	{

		public static BigInteger GenerateRandomBigInteger(BigInteger min, BigInteger max, int size = 8) // 
		{
			Random rnd = new Random(Guid.NewGuid().GetHashCode());
			if (min >= max)
			{
				throw new ArgumentException("Минимальное значение должно быть меньше максимального." + min + " > " + max);
			}

			BigInteger range = max - min;
			byte[] buffer = new byte[size];
			rnd.NextBytes(buffer);
			BigInteger randomValue = new BigInteger(buffer);

			return (randomValue % range) + min;
		}

		public static BigInteger PowModule(BigInteger bigA, BigInteger bigX, BigInteger bigP)
		{

			BigInteger result = BigInteger.One;
			bigA = bigA % bigP;

			if (bigA == 0)
			{
				return 0;
			}

			while (bigX > 0)
			{
				if ((bigX & 1) == 1)
				{
					result = (result * bigA) % bigP;
				}

				bigA = (bigA * bigA) % bigP;
				bigX >>= 1;
			}

			return (BigInteger)result;
		}

		public static List<BigInteger> GCDMod(BigInteger a, BigInteger b)
		{
			List<BigInteger> U = new List<BigInteger> { a, 1, 0 };
			List<BigInteger> V = new List<BigInteger> { b, 0, 1 };
			while (V[0] != 0)
			{
				BigInteger q = U[0] / V[0];
				List<BigInteger> T = new List<BigInteger> { U[0] % V[0], U[1] - q * V[1], U[2] - q * V[2] };
				U = V;
				V = T;
			}

			return U;
		}
		public static bool CheckPrime(BigInteger p)
		{
			Random rnd = new Random();

			if (p <= 1) return false;
			else if (p == 2) return true;
			BigInteger a = GenerateRandomBigInteger(2, p - 1);
			if (PowModule(a, (p - 1), p) != 1 || gcd(p, a) > 1)
			{
				return false;
			}
			return true;
		}

		public static BigInteger gcd(BigInteger a, BigInteger b)
		{
			while (b != 0)
			{
				BigInteger r = a % b;
				a = b;
				b = r;
			}
			return a;
		}

		public static BigInteger GeneratePrime(BigInteger left, BigInteger right)
		{
			while (true)
			{
				BigInteger p = GenerateRandomBigInteger(left, right);
				if (CheckPrime(p)) return p;
			}
		}


		public static BigInteger GenerateCoprime(BigInteger p)
		{

			Random rnd = new Random();

			BigInteger result = GenerateRandomBigInteger(2, p);

			while (gcd(p, result) != 1)
			{
				result = GenerateRandomBigInteger(2, p);
			}

			return result;
		}


		public static BigInteger ModInverse(BigInteger a, BigInteger m)
		{
			BigInteger m0 = m;
			BigInteger y = 0, x = 1;

			if (m == 1)
				return 0;

			while (a > 1)
			{
				BigInteger q = a / m;
				BigInteger t = m;
				m = a % m;
				a = t;
				t = y;
				y = x - q * y;
				x = t;
			}
			if (x < 0)
				x += m0;

			return x;
		}

		public static BigInteger PowBigInteger(BigInteger a, BigInteger p)
		{
			BigInteger result = a;
			for (BigInteger i = 1; i < p; i++)
			{
				result = result * a;
			}
			return result;
		}


	}
}
