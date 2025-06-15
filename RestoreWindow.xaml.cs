using Microsoft.EntityFrameworkCore;
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
using System.Windows.Shapes;
using WpfEfAuthen.Services;

namespace WpfEfAuthen
{
    /// <summary>
    /// Interaction logic for RestoreWindow.xaml
    /// </summary>
    public partial class RestoreWindow : Window
    {
        public RestoreWindow()
        {
            InitializeComponent();
        }

        private void RestoreButton_Click(object sender, RoutedEventArgs e)
        {
            // THIS FOR When you need custom custom path
            //var dialog = new Microsoft.Win32.OpenFileDialog
            //{
            //    Filter = "Backup Files (*.bak)|*.bak"
            //};

            //if (dialog.ShowDialog() == true)
            //{
            //    RestoreDatabase(path);
            //}

            // this is static folder for restoring database
            string path = "F:\\Data Development\\testFeature.bak";
            RestoreDatabase(path);
        }

        private void RestoreDatabase(string backupFile)
        {
            try
            {
                string restoreQuery = $@"
            USE master;
           
            RESTORE DATABASE [testFeature] FROM DISK = '{backupFile}' WITH REPLACE;
            ALTER DATABASE [testFeature] SET MULTI_USER;";

                using var context = new MyContext();
                context.Database.ExecuteSqlRaw(restoreQuery);

                MessageBox.Show("Database restored successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
