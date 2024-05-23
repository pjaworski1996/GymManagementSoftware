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
    /// Interaction logic for AddGymWindow.xaml
    /// </summary>
    public partial class AddGymWindow : Window
    {
        public AddGymWindow()
        {
            InitializeComponent();
        }

        private void AddGymBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddGymAddButton_Click(object sender, RoutedEventArgs e)
        {
            // Sprawdź, czy wszystkie obowiązkowe pola są wypełnione
            if (string.IsNullOrWhiteSpace(AddGymAddressTextBox.Text))
            {
                MessageBox.Show("Proszę wypełnić obowiązkowe pola oznaczone gwiazdką (*)!", "Błąd dodawania diety",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Pobierz wartości wprowadzone przez użytkownika
            string gymAddress = AddGymAddressTextBox.Text;

            var newGym = new Silownie
            {
                adres = gymAddress,

            };

            // Dodaj nową dietę do bazy danych za pomocą kontekstu
            using (var dbContext = new GymManagementEntities())
            {
                dbContext.Silownie.Add(newGym);
                dbContext.SaveChanges();
            }

            MessageBox.Show("Dodano nową siłownię!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);

            // Wyczyść pola po udanym dodaniu diety
            ClearFields();

        }

        private void ClearFields()
        {
            // Wyczyść zawartość pól tekstowych
            AddGymAddressTextBox.Text = "";
        }
    }
}
