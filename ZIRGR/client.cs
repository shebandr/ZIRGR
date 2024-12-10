using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Remoting;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ZIRGR
{
	internal class client
	{


		private List<List<string>> Vertices = new List<List<string>>();


		private List<BigInteger> N = new List<BigInteger>();
		private List<BigInteger> c = new List<BigInteger>();
		private List<BigInteger> d = new List<BigInteger>();
		private List<BigInteger> Z = new List<BigInteger>();

		public client(List<List<string>> Vpar, List<BigInteger> Npar, List<BigInteger> dpar, List<BigInteger> Zpar)
		{
			Vertices = Vpar;
			N = Npar;
			d = dpar;
			Z = Zpar;
/*			foreach (var item in Zpar)
			{
				Console.WriteLine(item);
			}
			foreach (var item in Npar)
			{
				Console.WriteLine(item);
			}
			foreach (var item in dpar)
			{
				Console.WriteLine(item);
			}
			foreach (var item in Vertices)
			{
				Console.WriteLine(item[0] + " " + item[1]);
			}*/
		}

		public string CheckVertex(List<BigInteger> CList, int index)
		{

			BigInteger AVertex = CryptographicLib.PowModule(Z[Int32.Parse(Vertices[index][0])], CList[0], N[Int32.Parse(Vertices[index][0])]);
			BigInteger BVertex = CryptographicLib.PowModule(Z[Int32.Parse(Vertices[index][1])], CList[1], N[Int32.Parse(Vertices[index][1])]);

		/*	Console.WriteLine("Проверка индексов: " + Int32.Parse(Vertices[index][0]) + " " + Int32.Parse(Vertices[index][1]));
			Console.WriteLine("Проверка всех значений под индексы: " + Z[Int32.Parse(Vertices[index][0])] + " " + CList[0] + " " + N[Int32.Parse(Vertices[index][0])]);
			Console.WriteLine("Проверка всех значений под индексы: " + Z[Int32.Parse(Vertices[index][1])] + " " + CList[1] + " " + N[Int32.Parse(Vertices[index][1])]);
		*/	string lastTwoBitsNumber1 = Convert.ToString((int)(AVertex & 3), 2).PadLeft(2, '0');
			string lastTwoBitsNumber2 = Convert.ToString((int)(BVertex & 3), 2).PadLeft(2, '0');
/*			Console.WriteLine(Convert.ToString((long)AVertex, 2).PadLeft(64, '0'));
			Console.WriteLine(Convert.ToString((long)BVertex, 2).PadLeft(64, '0'));
			// Вывод последних двух битов*/
			Console.WriteLine($"Индекс: {index} Вершины: {Vertices[index][0]} {Vertices[index][1]} Последние два бита числа 1: {lastTwoBitsNumber1}, числа 2: {lastTwoBitsNumber2}");
			bool bitsMatch = ((AVertex ^ BVertex) & 3) == 0;
			if(bitsMatch)
			{
				return "Сервер лжет";
			}
            else
            {

				return "Цвета разные";
				
            }
		}
		
	}
}
