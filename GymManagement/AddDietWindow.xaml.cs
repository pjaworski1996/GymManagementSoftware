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
    /// Interaction logic for AddDietWindow.xaml
    /// </summary>
    public partial class AddDietWindow : Window
    {
        public AddDietWindow()
        {
            InitializeComponent();
        }

        private void AddDietBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddDietAddButton_Click(object sender, RoutedEventArgs e)
        {
            // Sprawdź, czy wszystkie obowiązkowe pola są wypełnione
            if (string.IsNullOrWhiteSpace(AddDietNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(AddDietTypeTextBox.Text))
            {
                MessageBox.Show("Proszę wypełnić wszystkie obowiązkowe pola oznaczone gwiazdką (*)!", "Błąd dodawania diety",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Pobierz wartości wprowadzone przez użytkownika
            string dietName = AddDietNameTextBox.Text;
            string dietType = AddDietTypeTextBox.Text;
            string dietPriceStr = AddDietPriceTextBox.Text;

            int? dietPrice = null; // Użyj typu nullable int

            // Sprawdź, czy cena diety jest w formacie poprawnym
            if (!string.IsNullOrWhiteSpace(dietPriceStr))
            {
                if (!int.TryParse(dietPriceStr, out int parsedDietPrice) || parsedDietPrice < 0)
                {
                    MessageBox.Show("Nieprawidłowy format ceny diety. Podaj liczbę całkowitą większą od zera!", "Błąd dodawania diety",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                dietPrice = parsedDietPrice; // Przypisz wartość tylko jeśli cena jest poprawna
            }

            // Utwórz nowy obiekt Dieta na podstawie danych wprowadzonych przez użytkownika
            var newDiet = new Diety
            {
                nazwa = dietName,
                rodzaj = dietType,
                cena = dietPrice
            };

            // Dodaj nową dietę do bazy danych za pomocą kontekstu
            using (var dbContext = new GymManagementEntities())
            {
                dbContext.Diety.Add(newDiet);
                dbContext.SaveChanges();
            }

            MessageBox.Show("Dodano nową dietę!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);

            // Wyczyść pola po udanym dodaniu diety
            ClearFields();

        }

        private void ClearFields()
        {
            // Wyczyść zawartość pól tekstowych
            AddDietNameTextBox.Text = "";
            AddDietTypeTextBox.Text = "";
            AddDietPriceTextBox.Text = "";
        }

    }
}
