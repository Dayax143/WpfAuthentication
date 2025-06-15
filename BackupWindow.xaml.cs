using Microsoft.EntityFrameworkCore;
using System.Windows;
using WpfEfAuthen.Services;

namespace WpfEfAuthen
{
    /// <summary>
    /// Interaction logic for BackupWindow.xaml
    /// </summary>
    public partial class BackupWindow : Window
    {
        public BackupWindow()
        {
            InitializeComponent();
        }

        private void BackupButton_Click(object sender, RoutedEventArgs e)
        {
            //This for custom restoring from a folder
            //var dialog = new Microsoft.Win32.SaveFileDialog
            //{
            //    Filter = "Backup Files (*.bak)|*.bak",
            //    FileName = "testFeatureDatabaseBackup.bak"
            //};


            //if (dialog.ShowDialog() == true)
            //{
            //    BackupDatabase(path);
            //}


            string path = "F:\\Data Development\\testFeature.bak";
            BackupDatabase(path);
        }

        private void BackupDatabase(string backupPath)
        {
            try
            {
                string backupQuery = $"BACKUP DATABASE [testFeature] TO DISK = '{backupPath}' with format;";
                //string backupQuery = $"BACKUP DATABASE [testFeature] TO DISK = '{backupPath}\\GGtestFeatureDatabaseBackup.bak' with format;";
                using var context = new MyContext();
                context.Database.ExecuteSqlRaw(backupQuery);

                MessageBox.Show("Backup completed successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
