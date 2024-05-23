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
    /// Interaction logic for AddEquipmentWindow.xaml
    /// </summary>
    public partial class AddEquipmentWindow : Window
    {
        public AddEquipmentWindow()
        {
            InitializeComponent();
        }

        private void AddEquipmentBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddEquipmentAddButton_Click(object sender, RoutedEventArgs e)
        {
            // Sprawdź, czy wszystkie obowiązkowe pola są wypełnione
            if (string.IsNullOrWhiteSpace(AddEquipmentNameTextBox.Text) || string.IsNullOrWhiteSpace(AddEquipmentQuantityTextBox.Text))
            {
                MessageBox.Show("Proszę wypełnić wszystkie obowiązkowe pola oznaczone gwiazdką (*)!", "Błąd dodawania sprzętu",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Pobierz wartości wprowadzone przez użytkownika
            string equipmentName = AddEquipmentNameTextBox.Text;
            string equipmentQuantityStr = AddEquipmentQuantityTextBox.Text;

            int? equipmentQuantity = null; // Użyj typu nullable int

            // Sprawdź, czy ilość sprzętu jest w formacie poprawnym
            if (!string.IsNullOrWhiteSpace(equipmentQuantityStr))
            {
                if (!int.TryParse(equipmentQuantityStr, out int parsedEquipmentQuantity) || parsedEquipmentQuantity < 0)
                {
                    MessageBox.Show("Nieprawidłowy format ilości sprzętu. Podaj liczbę całkowitą nieujemną!", "Błąd dodawania sprzętu",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                equipmentQuantity = parsedEquipmentQuantity; // Przypisz wartość tylko jeśli ilość jest poprawna
            }

            // Utwórz nowy obiekt Sprzęt na podstawie danych wprowadzonych przez użytkownika
            var newEquipment = new Sprzety
            {
                nazwa = equipmentName,
                ilosc = equipmentQuantity ?? 0
            };

            // Dodaj nowy sprzęt do bazy danych za pomocą kontekstu
            using (var dbContext = new GymManagementEntities())
            {
                dbContext.Sprzety.Add(newEquipment);
                dbContext.SaveChanges();
            }

            MessageBox.Show("Dodano nowy sprzęt!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);

            // Wyczyść pola po udanym dodaniu sprzętu
            ClearFields();
        }

        private void ClearFields()
        {
            // Wyczyść zawartość pól tekstowych
            AddEquipmentNameTextBox.Text = "";
            AddEquipmentQuantityTextBox.Text = "";
        }

    }
}
