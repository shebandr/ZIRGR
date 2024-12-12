using GraphSharp.Algorithms.Layout.Simple.FDP;
using GraphSharp.Controls;
using Microsoft.Win32;
using QuickGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ZIRGR
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		client Client = null;
		string GraphFilePath = "";
		string ColorFilePath = "";
		server Server = new server();
		public MainWindow()
		{
			InitializeComponent();
		}

		private void ReadGraphButton(object sender, RoutedEventArgs e)
		{
			

			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.InitialDirectory = "c:\\";
			openFileDialog.Filter = "txt files (*.txt)|*.txt";
			openFileDialog.RestoreDirectory = true;
			openFileDialog.ShowDialog();
			GraphFilePath = openFileDialog.FileName;
			if (GraphFilePath == "")
			{
				return;
			}
			Console.WriteLine(GraphFilePath);
		}

		private void ReadColorsButton(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.InitialDirectory = "c:\\";
			openFileDialog.Filter = "txt files (*.txt)|*.txt";
			openFileDialog.RestoreDirectory = true;
			openFileDialog.ShowDialog();
			ColorFilePath = openFileDialog.FileName;
			if (ColorFilePath == "")
			{
				return;
			}
			Console.WriteLine(ColorFilePath);

		}

		private void SetDataButton(object sender, RoutedEventArgs e)
		{
			if (ColorFilePath != "" && GraphFilePath != "")
			{
				ReadFileErrorLabel.Content = Server.SetGraph(GraphFilePath, ColorFilePath);
			}
			Server.CalculateData();
			Client = new client(Server.Vertices, Server.N, Server.d, Server.Z);


			// Создаём локальный список для хранения результата
			List<List<string>> localVertices = new List<List<string>>();

			// Преобразуем данные
			foreach (var edge in Server.Vertices)
			{
				// Получаем название и цвет первой вершины
				string firstVertexName = edge[0];
				string firstVertexColor = Server.Colors.FirstOrDefault(v => v[0] == firstVertexName)?[1] ?? "Unknown";

				// Получаем название и цвет второй вершины
				string secondVertexName = edge[1];
				string secondVertexColor = Server.Colors.FirstOrDefault(v => v[0] == secondVertexName)?[1] ?? "Unknown";


				// Добавляем результат в локальный список
				localVertices.Add(new List<string> { $"{firstVertexName}({firstVertexColor})" , $"{secondVertexName}({secondVertexColor})"});
			}
			string labelOutput = "";
			int q = 0;
			foreach(var i in localVertices)
			{
				labelOutput += q + ") " + i[0] + " - " + i[1] + "\n";
				q++;
			}
			GraphLabel.Content = labelOutput;
			// Вывод результата (для проверки)
			foreach (var edge in localVertices)
			{
				Console.WriteLine(edge[0]);
			}

			// Создание графа
			var graph = new BidirectionalGraph<object, IEdge<object>>();

			// Добавление вершин в граф
			foreach (var vertex in Server.Colors)
			{
				string vertexName = vertex[0];
				string vertexColor = vertex[1];
				string formattedVertex = $"{vertexName}({vertexColor})";
				graph.AddVertex(formattedVertex);
			}

			// Добавление рёбер в граф
			foreach (var edge in Server.Vertices)
			{
				string sourceVertex = $"{edge[0]}({Server.Colors.FirstOrDefault(v => v[0] == edge[0])?[1] ?? "Unknown"})";
				string targetVertex = $"{edge[1]}({Server.Colors.FirstOrDefault(v => v[0] == edge[1])?[1] ?? "Unknown"})";
				graph.AddEdge(new Edge<object>(sourceVertex, targetVertex));
				graph.AddEdge(new Edge<object>(targetVertex, sourceVertex)); // Для неориентированного графа
			}

			// Привязка графа к элементу управления GraphLayout
			graphLayout.Graph = graph;
			graphLayout.LayoutAlgorithmType = "KK";
		}
		private void CalcClientButton(object sender, RoutedEventArgs e)
		{
			if(Client == null)
			{
				OutputLabel.Content = "рано";
				return ;
			}
			int index;
			if (int.TryParse(inputTextBox.Text, out index) && index < Server.Vertices.Count)
			{
				OutputLabel.Content = Client.CheckVertex(Server.GetC(index), index);
			}
			else
			{
				OutputLabel.Content = "это не число";
			}
			
		}
	}
}
