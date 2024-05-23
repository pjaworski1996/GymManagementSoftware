using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Interaction logic for ChangeDietWindow.xaml
    /// </summary>
    public partial class ChangeDietWindow : Window
    {
        public ChangeDietWindow(int idDiety)
        {
            InitializeComponent();
            idDieta = idDiety;
            Loaded += ChangeDietWindow_Loaded;
        }

        public int idDieta { get; set; }

        private void ChangeDietBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ChangeDietWindow_Loaded(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new GymManagementEntities())
            {
                var existingDiet = dbContext.Diety.FirstOrDefault(t => t.idDieta == idDieta);

                if (existingDiet != null)
                {
                    ChangeDietNameTextBox.Text = existingDiet.nazwa;
                    ChangeDietTypeTextBox.Text = existingDiet.rodzaj;
                    ChangeDietPriceTextBox.Text = existingDiet.cena.ToString();
                }
            }
        }

        private void ChangeDietChangeButton_Click(object sender, RoutedEventArgs e)
        {
            // Sprawdź, czy wszystkie obowiązkowe pola są wypełnione
            if (string.IsNullOrWhiteSpace(ChangeDietNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(ChangeDietTypeTextBox.Text))
            {
                MessageBox.Show("Proszę wypełnić wszystkie obowiązkowe pola oznaczone gwiazdką (*)!", "Błąd dodawania diety",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Pobierz wartości wprowadzone przez użytkownika
            string dietName = ChangeDietNameTextBox.Text;
            string dietType = ChangeDietTypeTextBox.Text;
            string dietPriceStr = ChangeDietPriceTextBox.Text;

            int? dietPrice = null; // Użyj typu nullable int

            // Sprawdź, czy cena diety jest w formacie poprawnym
            if (!string.IsNullOrWhiteSpace(dietPriceStr))
            {
                if (!int.TryParse(dietPriceStr, out int parsedDietPrice))
                {
                    MessageBox.Show("Nieprawidłowy format ceny diety. Podaj liczbę całkowitą!", "Błąd dodawania diety",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                dietPrice = parsedDietPrice; // Przypisz wartość tylko jeśli cena jest poprawna
            }

            using (var dbContext = new GymManagementEntities())
            {
                // Znajdź istniejący trening na podstawie idTreningu
                var existingDiet = dbContext.Diety.FirstOrDefault(t => t.idDieta == idDieta);

                // Aktualizuj dane treningu zgodnie z danymi wprowadzonymi przez użytkownika
                existingDiet.nazwa = dietName;
                existingDiet.rodzaj = dietType;
                existingDiet.cena = dietPrice;

                // Zapisz zmiany w bazie danych
                dbContext.SaveChanges();

                MessageBox.Show("Dieta została pomyślnie zaktualizowana!", "Sukces",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
