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
using System.IO;

namespace dataMover
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            var paths = ((System.Array)e.Data.GetData(DataFormats.FileDrop));
            listFolder.Items.Clear();
            foreach (var path in paths)
            {
                listFolder.Items.Add(path);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string path in listFolder.Items)
            {
                if (Directory.Exists(path))
                {
                    DirectoryInfo folder = new DirectoryInfo(path);
                    FileInfo[] files = folder.GetFiles();
                    foreach (FileInfo file in files)
                    {
                        string newPath = folder.Parent.FullName + "\\" + file.Name;
                        if (!File.Exists(newPath))
                        {
                            file.MoveTo(newPath);
                        }
                        else
                        {
                            newPath = folder.Parent.FullName + "\\" + System.IO.Path.GetFileNameWithoutExtension(file.Name);
                            for (int i = 1; ; i++)
                            {
                                string newPathTemp = string.Format("{0} ({1}){2}", newPath, i, file.Extension);
                                if (!File.Exists(newPathTemp))
                                {
                                    file.MoveTo(newPathTemp);
                                    break;
                                }
                            }
                        }
                    }
                    DirectoryInfo[] directoryies = folder.GetDirectories();
                    foreach (DirectoryInfo directory in directoryies)
                    {
                        string newPath = folder.Parent.FullName + "\\" + directory.Name;
                        if (!Directory.Exists(newPath))
                        {
                            directory.MoveTo(newPath);
                        }
                        else
                        {
                            for (int i = 1; ; i++)
                            {
                                string newPathTemp = string.Format("{0} ({1})", newPath, i);
                                if (!Directory.Exists(newPathTemp))
                                {
                                    directory.MoveTo(newPathTemp);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            if (sb.Length > 0) { MessageBox.Show(sb.ToString()); }
        }
    }
}
