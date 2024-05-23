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

namespace GymManagement
{
    /// <summary>
    /// Interaction logic for ChangeGymWindow.xaml
    /// </summary>
    public partial class ChangeGymWindow : Window
    {
        public ChangeGymWindow(int idSilowni)
        {
            InitializeComponent();
            idSilownia = idSilowni;
            Loaded += ChangeGymWindow_Loaded;
        }

        public int idSilownia { get; set; }

        private void ChangeGymBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ChangeGymWindow_Loaded(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new GymManagementEntities())
            {
                var existingGym = dbContext.Silownie.FirstOrDefault(t => t.idSilownia == idSilownia);

                if (existingGym != null)
                {
                    ChangeGymAddressTextBox.Text = existingGym.adres;
                }
            }
        }

        private void ChangeGymChangeButton_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrWhiteSpace(ChangeGymAddressTextBox.Text))
            {
                MessageBox.Show("Proszę wypełnić obowiązkowe pola oznaczone gwiazdką (*)!", "Błąd dodawania diety",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string address = ChangeGymAddressTextBox.Text;

            using (var dbContext = new GymManagementEntities())
            {
                var existingGym = dbContext.Silownie.FirstOrDefault(t => t.idSilownia == idSilownia);

                if (existingGym != null)
                {
                    existingGym.adres = address;

                    dbContext.SaveChanges();

                    MessageBox.Show("Siłownia została pomyślnie zaktualizowana!", "Sukces",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Nie można odnaleźć siłowni.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
