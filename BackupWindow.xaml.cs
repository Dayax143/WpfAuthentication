using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
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
            lblDefaultPath.Text = Properties.Settings.Default.BackupFolderPath;
        }

        // This validates the license is expired
        //public void checkLicense()
        //{
        //    // Ensure correct parsing of the stored installation date
        //    DateTime installed_date = Properties.Settings.Default.Installed_date;
        //    DateTime current_date = DateTime.Now;

        //    int daysDifference = (current_date - installed_date).Days;

        //    if (daysDifference <= 0) // Assuming expiration after 10 day
        //    {
        //        MessageBox.Show("License Expired");
        //        App.Current.Shutdown();
        //        Close();
        //    }
        //    else
        //    {
        //        MessageBox.Show($"You are a legitimate user {daysDifference} days left "); // Fixed grammar
        //    }
        //}

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
            string formattedDate = date.ToString("yyyy_MM_dd_HH_mm_ss"); // Ensures a clean file name
            string defaultFolder = Properties.Settings.Default.BackupFolderPath; // Retrieve previous path

            // Ask the user whether to save in the default folder or choose a new path
            MessageBoxResult result = MessageBox.Show(
                $"Do you want to save the backup in the default folder?\n(Default: {defaultFolder})",
                "Backup Confirmation",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Question
            );

            string backupFilePath;

            if (result == MessageBoxResult.Yes)
            {
                if (!string.IsNullOrEmpty(defaultFolder) && Directory.Exists(defaultFolder))
                {
                    // Save backup automatically in default folder
                    backupFilePath = Path.Combine(defaultFolder, $"testFeatureDB_{formattedDate}.bak");
                }
                else
                {
                    MessageBox.Show("The default backup folder is not set or does not exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else if (result == MessageBoxResult.No)
            {
                var dialog = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "Backup Files (*.bak)|*.bak",
                    FileName = $"testFeatureDB_{formattedDate}.bak"
                };

                if (dialog.ShowDialog() == true)
                {
                    backupFilePath = dialog.FileName;
                    Properties.Settings.Default.BackupFolderPath = Path.GetDirectoryName(dialog.FileName);
                    Properties.Settings.Default.Save(); // Save the updated path

                    lblDefaultPath.Text = backupFilePath;
                }
                else
                {
                    return; // Exit if the user cancels the dialog
                }
            }
            else
            {
                return; // Exit if the user cancels the confirmation dialog
            }

            // Proceed with the backup
            BackupDatabase(backupFilePath);

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

        private void btnSavePath_Click(object sender, RoutedEventArgs e)
        {
            BackupDatabase(lblDefaultPath.Text + "testBAckup");
        }
    }
}
