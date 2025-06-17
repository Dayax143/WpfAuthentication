using System.Windows;
using WpfEfAuthen.Properties;
using System.IO;

namespace WpfEfAuthen
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            try
            {
                int maxUsageDays = 10; // Standard license period
                int gracePeriodDays = 5; // Extra days after expiration

                // Ensure correct parsing of the stored installation date
                DateTime installed_date =Settings.Default.Installed_date;

                DateTime current_date = DateTime.Now;
                int daysDifference = (current_date - installed_date).Days;

                if (daysDifference > maxUsageDays + gracePeriodDays)
                {
                    MessageBox.Show("License Fully Expired. Application will now close.");
                    Environment.Exit(0); // Immediate shutdown
                }
                else if (daysDifference > maxUsageDays)
                {
                    MessageBox.Show($"License Expired. You are in a {gracePeriodDays}-day grace period.");
                    OfferLicenseRenewal();
                }
                else
                {
                    MessageBox.Show($"You are a legitimate user. {maxUsageDays - daysDifference}");
                } }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}");
                Current.Shutdown(); // Ensuring shutdown on failure
            }
        }

        public void OfferLicenseRenewal()
        {
            try
            {
                MessageBoxResult result = MessageBox.Show(
                "Your license is about to expire. Would you like to renew?",
                "License Renewal",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    Settings.Default.Installed_date = DateTime.Now;
                    Settings.Default.Save();
                    MessageBox.Show("Your license has been renewed!");
                }
                else
                {
                    MessageBox.Show("License Expired. Application will now close.");
                    Environment.Exit(0);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            base.OnStartup(e);
            
        }
    }
}