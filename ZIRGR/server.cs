using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ZIRGR
{
	internal class server
	{

		public List<List<string>> Vertices = new List<List<string>>();
		public List<List<string>> Colors = new List<List<string>>();
		private List<List<string>> ColorsShuffle = new List<List<string>> { new List<string> { "0", "0" }, new List<string> { "1", "1" }, new List<string> { "2", "2" } };
		private List<BigInteger> V = new List<BigInteger>();
		private List<BigInteger> P = new List<BigInteger>();
		private List<BigInteger> Q = new List<BigInteger>();
		public List<BigInteger> N = new List<BigInteger>();
		private List<BigInteger> c = new List<BigInteger>();
		public List<BigInteger> d = new List<BigInteger>();
		public List<BigInteger> Z = new List<BigInteger>();
		public string SetGraph(string verticesPath, string colorsPath)
		{
			Vertices = new List<List<string>>();
			Colors = new List<List<string>>();
			ColorsShuffle = new List<List<string>> { new List<string> { "0", "0" }, new List<string> { "1", "1" }, new List<string> { "2", "2" } };
			V = new List<BigInteger>();
			P = new List<BigInteger>();
			Q = new List<BigInteger>();
			N = new List<BigInteger>();
			c = new List<BigInteger>();
			d = new List<BigInteger>();
			Z = new List<BigInteger>();
			List<string> verticesTemp = ReadConfig(verticesPath);
			foreach (var vertex in verticesTemp)
			{
				Vertices.Add(vertex.Split(' ').ToList());
			}


			List<string> colorsTemp = ReadConfig(colorsPath);
			foreach (var color in colorsTemp)
			{
				Colors.Add(color.Split(' ').ToList());
			}

			return CheckColors();
		
			
		}

		public void CalculateData()
		{
			ShuffleSecondElements();

			Console.WriteLine("цвета:");

			foreach (var list in ColorsShuffle)
			{
				Console.WriteLine($"[{list[0]}, {list[1]}]");
			}
			Console.WriteLine("цвета до перемешивания:");
			for (int i = 0; i < Colors.Count; i++)
			{
				Console.WriteLine(Colors[i][0] + " " + Colors[i][1]);
			}
			for (int i = 0; i < Colors.Count; i++)

			{
				foreach (var c in ColorsShuffle)
				{
					if (c[0] == Colors[i][1])
					{
						Colors[i][1] = c[1].ToString();
						break;
					}

				}
			}
			Console.WriteLine("цвета после перемешивания:");
			for (int i = 0; i < Colors.Count; i++)
			{
				Console.WriteLine(Colors[i][0] + " " + Colors[i][1]);
			}

			// СПИСКОМ ВЕРШИН ЯВЛЯЕТСЯ СПИСОК ИХ ОКРАСОК, НАЧИНАЯ С 0 И ПО ПОРЯДКУ, ЕСЛИ ИНАЧЕ - так нельзя делать

			for (int i = 0; i < Colors.Count; i++)
			{
				BigInteger tempV = CryptographicLib.GenerateRandomBigInteger(CryptographicLib.PowBigInteger(2, 63), CryptographicLib.PowBigInteger(2, 64) - 1);
				tempV = tempV & ~3;
				
				switch (Colors[i][1])
				{
					case "0":
						tempV = tempV | 00;
						break;
					case "1":
						tempV = tempV | 01;
						break;
					case "2":
						tempV = tempV | 10;
						break;
				}
				V.Add(tempV);
/*				Console.WriteLine(Convert.ToString((long)tempV, 2).PadLeft(128, '0'));*/

				BigInteger tempP = CryptographicLib.GenerateRandomBigInteger(CryptographicLib.PowBigInteger(2, 63), CryptographicLib.PowBigInteger(2, 64) - 1);
				while (CryptographicLib.CheckPrime(tempP) != true || tempP == 0 || tempP == 1)
				{
					tempP = CryptographicLib.GenerateRandomBigInteger(CryptographicLib.PowBigInteger(2, 63), CryptographicLib.PowBigInteger(2, 64) - 1);
				}
				BigInteger tempQ = CryptographicLib.GenerateRandomBigInteger(CryptographicLib.PowBigInteger(2, 63), CryptographicLib.PowBigInteger(2, 64) - 1);
				while (CryptographicLib.CheckPrime(tempQ) != true || tempQ == 0 || tempQ == 1)
				{
					tempQ = CryptographicLib.GenerateRandomBigInteger(CryptographicLib.PowBigInteger(2, 63), CryptographicLib.PowBigInteger(2, 64) - 1);
				}
				BigInteger tempN = tempP * tempQ;
				BigInteger Phi = (tempP-1)*(tempQ-1);
				BigInteger tempD = CryptographicLib.GenerateCoprime(Phi);
				BigInteger tempC = CryptographicLib.ModInverse(tempD, Phi);

				BigInteger tempZ = CryptographicLib.PowModule(tempV, tempD, tempN);


				P.Add(tempP);
				Q.Add(tempQ);
				N.Add(tempN);
				c.Add(tempC);
				d.Add(tempD);
				Z.Add(tempZ);
				Console.WriteLine(tempP + " " + tempQ + " " + tempN + " " + tempC + " " + tempD + " " + tempZ + " " + Phi);
			}
		}

		public List<BigInteger> GetC(int index)
		{
			return new List<BigInteger> { c[Int32.Parse(Vertices[index][0])], c[Int32.Parse(Vertices[index][1])] };
		}

		private void ShuffleSecondElements()
		{
			List<string> availableValues = new List<string> { "0", "1", "2" };
			Random random = new Random();
			List<string> shuffledValues = availableValues.OrderBy(x => random.Next()).ToList();
			for (int i = 0; i < ColorsShuffle.Count; i++)
			{
				ColorsShuffle[i][1] = shuffledValues[i];
			}
		}

		public string CheckColors()
		{
			foreach (List<string> v in Vertices)
			{

				string colorA = "";
				string colorB = "";

				foreach (List<string> c in Colors)
				{
					if (c[0] == v[0])
					{
						colorA = c[1];
						break;
					}
				}
				foreach (List<string> c in Colors)
				{
					if (c[0] == v[1])
					{
						colorB = c[1];
						break;
					}
				}
				if(colorA == colorB)
				{
					return "у " + v[0] + " и " + v[1] + " одинаковые цвета";
				}

			}
			return "ok";
		}



		public List<string> ReadConfig(string filePath)
		{
			var config = new List<string>();

			string[] lines;
			try
			{
				lines = File.ReadAllLines(filePath);
				foreach (string line in lines)
				{
					Console.WriteLine(line);
					config.Add(line);
				}
			}
			catch (IOException e)
			{
				Console.WriteLine("Ошибка при чтении файла:");
				Console.WriteLine(e.Message);
				return new List<string> { "Ошибка при чтении файла " + e.Message };
			}
			
			return config;
		}


	} 
}
