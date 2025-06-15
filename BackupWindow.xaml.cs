using Microsoft.EntityFrameworkCore;
using System;
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
           lblBrowsedPath.Text = $"{Properties.Settings.Default.BackupFolderPath}\\{Properties.Settings.Default.BackupFileName}";

        }


        // function database
        public void BackupDatabase(string backupFile)
        {
            try
            {
                string backupQuery = $"BACKUP DATABASE [testFeature] TO DISK = '{backupFile}'";

                using var context = new MyContext();
                context.Database.ExecuteSqlRaw(backupQuery);

                MessageBox.Show($"Backup completed! File saved at: {backupFile}");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // backup button
        private void btnBackup_Click(object sender, RoutedEventArgs e)
        {
            string backupPath = $"{Properties.Settings.Default.BackupFolderPath}\\{Properties.Settings.Default.BackupFileName}";

            if (!string.IsNullOrEmpty(backupPath))
            {
                BackupDatabase(backupPath);
            }
            else
            {
                MessageBox.Show("Backup path is not set. Please choose and save a backup folder.");
            }
        }

        // browse button
        private void btnBrowseFolder_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Backup Files (*.bak)|*.bak",
                FileName = "testFeatureDB.bak"
            };

            if (dialog.ShowDialog() == true)
            {
                // Save selected path to label
                lblBrowsedPath.Text = dialog.FileName;
            }
        }

        // save path to settings
        private void btnSavePath_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(lblBrowsedPath.Text))
            {
                Properties.Settings.Default.BackupFolderPath = System.IO.Path.GetDirectoryName(lblBrowsedPath.Text);
                Properties.Settings.Default.BackupFileName = System.IO.Path.GetFileName(lblBrowsedPath.Text);
                Properties.Settings.Default.Save();

                MessageBox.Show("Folder is ready to save your DATABASE");
            }
            else
            {
                MessageBox.Show("Please select a backup location first.");
            }
        }
    }
}
