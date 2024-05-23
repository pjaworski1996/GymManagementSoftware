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
    /// Interaction logic for ChangeEquipmentWindow.xaml
    /// </summary>
    public partial class ChangeEquipmentWindow : Window
    {
        public ChangeEquipmentWindow(int idSprzetu)
        {
            InitializeComponent();
            idSprzet = idSprzetu;
            Loaded += ChangeEquipmentWindow_Loaded;
        }
        public int idSprzet { get; set; }

        private void ChangeEquipmentBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ChangeEquipmentWindow_Loaded(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new GymManagementEntities())
            {
                var existingEquipment = dbContext.Sprzety.FirstOrDefault(t => t.idSprzet == idSprzet);

                if (existingEquipment != null)
                {
                    ChangeEquipmentNameTextBox.Text = existingEquipment.nazwa;
                    ChangeEquipmentQuantityTextBox.Text = existingEquipment.ilosc.ToString();
                }
            }
        }

        private void ChangeEquipmentChangeButton_Click(object sender, RoutedEventArgs e)
        {
            // Sprawdź, czy wszystkie obowiązkowe pola są wypełnione
            if (string.IsNullOrWhiteSpace(ChangeEquipmentNameTextBox.Text) || string.IsNullOrWhiteSpace(ChangeEquipmentQuantityTextBox.Text))
            {
                MessageBox.Show("Proszę wypełnić wszystkie obowiązkowe pola oznaczone gwiazdką (*)!", "Błąd modyfikowania sprzętu",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Pobierz wartości wprowadzone przez użytkownika
            string equipmentName = ChangeEquipmentNameTextBox.Text;
            string equipmentQuantityStr = ChangeEquipmentQuantityTextBox.Text;

            int? equipmentQuantity = null; // Użyj typu nullable int

            // Sprawdź, czy ilość sprzętu jest w formacie poprawnym
            if (!string.IsNullOrWhiteSpace(equipmentQuantityStr))
            {
                if (!int.TryParse(equipmentQuantityStr, out int parsedEquipmentQuantity) || parsedEquipmentQuantity < 0)
                {
                    MessageBox.Show("Nieprawidłowy format ilości sprzętu. Podaj liczbę całkowitą nieujemną!", "Błąd modyfikowania sprzętu",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                equipmentQuantity = parsedEquipmentQuantity; // Przypisz wartość tylko jeśli ilość jest poprawna
            }

            // Znajdź sprzęt do zmodyfikowania w bazie danych
            using (var dbContext = new GymManagementEntities())
            {
                var existingEquipment = dbContext.Sprzety.FirstOrDefault(s => s.idSprzet == idSprzet);

                // Zaktualizuj dane sprzętu
                existingEquipment.nazwa = equipmentName;
                existingEquipment.ilosc = equipmentQuantity ?? 0;

                // Zapisz zmiany w bazie danych
                dbContext.SaveChanges();
            }

            MessageBox.Show("Zmodyfikowano sprzęt!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
