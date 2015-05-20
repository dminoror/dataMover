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
                try
                {
                    DirectoryInfo folder = new DirectoryInfo(path.ToString());
                    FileSystemInfo[] files = folder.GetFileSystemInfos();
                    foreach (var file in files)
                    {
                        int i = file.FullName.Length - 1;
                        for (; i >= 0; i--)
                        {
                            if (file.FullName[i] == '\\')
                            {
                                i--;
                                for (; i >= 0; i--)
                                { if (file.FullName[i] == '\\') { break; } }
                                break;
                            }
                        }
                        //string temp = file.FullName.Substring(0, i + 1) + file.Name;
                        try
                        {
                            Directory.Move(file.FullName, file.FullName.Substring(0, i + 1) + file.Name);
                        }
                        catch(Exception ex)
                        {
                            if (ex is System.IO.PathTooLongException)
                                sb.AppendLine(file.Name + "發生例外:" + ex);
                        }
                    }
                }
                finally {  }
            }
            if (sb.Length > 0) { MessageBox.Show(sb.ToString()); }
        }
    }
}
