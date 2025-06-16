using Microsoft.Data.SqlClient;
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
        // function database
        public void BackupDatabase(string backupFile)
        {
            try
            {
                string backupQuery = $"BACKUP DATABASE [testFeature] TO DISK = '{backupFile}' WITH FORMAT;";

                using var context = new MyContext();
                context.Database.ExecuteSqlRaw(backupQuery);

                MessageBox.Show($"Backup completed! File saved at: {backupFile}");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //// backup button
        //private void btnBackup_Click(object sender, RoutedEventArgs e)
        //{
        //    string backupPath = $"{Properties.Settings.Default.BackupFolderPath}\\{Properties.Settings.Default.BackupFileName}";

        //    if (!string.IsNullOrEmpty(backupPath))
        //    {
        //        BackupDatabase(backupPath);
        //    }
        //    else
        //    {
        //        MessageBox.Show("Backup path is not set. Please choose and save a backup folder.");
        //    }
        //}

        // browse button
        private void btnBrowseFolder_Click(object sender, RoutedEventArgs e)
        {
            DateTime date = DateTime.Now;
            string formattedDate = date.ToString("yyyy-MM-dd-HH-mm-ss"); // Ensures a clean file name
            string defaultFolder = Properties.Settings.Default.BackupFolderPath; // Retrieve previous path

            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Backup Files (*.bak)|*.bak",
                FileName = $"testFeatureDB-{formattedDate}.bak",
                InitialDirectory = !string.IsNullOrEmpty(defaultFolder) ? System.IO.Path.GetDirectoryName(defaultFolder) : Environment.GetFolderPath(Environment.SpecialFolder.Desktop) // Use previous folder or default to Desktop
            };

            if (dialog.ShowDialog() == true)
            {
                Properties.Settings.Default.BackupFolderPath = dialog.FileName;
                Properties.Settings.Default.Save(); // Save the updated path
                BackupDatabase(dialog.FileName);
            }

        }

        //// save default path to settings for next BACKUPS
        //private void btnSavePath_Click(object sender, RoutedEventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(lblBrowsedPath.Text))
        //    {
        //        Properties.Settings.Default.BackupFolderPath = System.IO.Path.GetDirectoryName(lblBrowsedPath.Text);
        //        Properties.Settings.Default.BackupFileName = System.IO.Path.GetFileName(lblBrowsedPath.Text);
        //        Properties.Settings.Default.Save();

        //        MessageBox.Show("Folder is ready to save your DATABASE");
        //    }
        //    else
        //    {
        //        MessageBox.Show("Please select a backup location first.");
        //    }
        //}

        // this is restore function
        private void RestoreDatabase(string backupFile)
        {
            try
            {
                string connectionString = "server=.; database=master; user id=sa; password=123; trustservercertificate=true;";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // Check if the database exists
                    string checkDbQuery = "IF DB_ID('testFeature') IS NOT NULL SELECT 1 ELSE SELECT 0;";
                    using (SqlCommand checkCmd = new SqlCommand(checkDbQuery, con))
                    {
                        int dbExists = (int)checkCmd.ExecuteScalar();

                        string restoreQuery;
                        if (dbExists == 1)
                        {
                            // Database exists, replace it
                            restoreQuery = @$"
                    ALTER DATABASE [testFeature] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                    RESTORE DATABASE [testFeature] FROM DISK = '{backupFile}' WITH REPLACE;
                    ALTER DATABASE [testFeature] SET MULTI_USER;";
                        }
                        else
                        {
                            // Database does not exist, restore directly
                            restoreQuery = @$"
                    RESTORE DATABASE [testFeature] FROM DISK = '{backupFile}' WITH REPLACE;";
                        }

                        using (SqlCommand restoreCmd = new SqlCommand(restoreQuery, con))
                        {
                            restoreCmd.ExecuteNonQuery();
                        }

                        MessageBox.Show($"Database restoration completed successfully! from\\ {backupFile}");
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"SQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected Error: {ex.Message}");
            }


        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new Microsoft.Win32.OpenFileDialog
                {
                    Filter = "Backup files (*.bak)|*.bak",
                };

                if (dialog.ShowDialog() == true)
                {
                    var path = dialog.FileName;
                    RestoreDatabase(path);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
