using Microsoft.Win32;
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
			if(ColorFilePath != "" && GraphFilePath != "")
			{
				ReadFileErrorLabel.Content = Server.SetGraph(GraphFilePath, ColorFilePath);
			}
			Server.CalculateData();
			Client = new client(Server.Vertices, Server.N, Server.d, Server.Z);

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
