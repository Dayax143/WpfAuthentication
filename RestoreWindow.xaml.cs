using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
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

        // this is restore function
        private void RestoreDatabase(string backupFile)
        {
            try
            {
                using (SqlConnection con = new SqlConnection("server=.; database=master; user id=sa; password=123; trustservercertificate=true;"))
                {
                    string restoreQuery = "USE master; RESTORE DATABASE [testFeature] " +
                        "FROM DISK = '{backupFile}' WITH REPLACE;" +
                        "ALTER DATABASE [testFeature] SET MULTI_USER;";

                    SqlCommand cmd = new SqlCommand(restoreQuery, con);
                    con.Open();
                    cmd.ExecuteNonQuery();

                    //MessageBox.Show("Database restored successfully!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // This is for browsing database file for restore, and call the RestoreDatabase function
        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new Microsoft.Win32.OpenFileDialog
                {
                    Filter = "Backup files (*.bak)|*.bak"
                };

                if (dialog.ShowDialog() == true)
                {
                    var path = dialog.FileName;
                    RestoreDatabase(path);
                    MessageBox.Show($"Sucessfully restored from {path}");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
