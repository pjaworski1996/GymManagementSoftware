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
    /// Interaction logic for ChangeTrainingWindow.xaml
    /// </summary>
    public partial class ChangeTrainingWindow : Window
    {
        public ChangeTrainingWindow(int idTreningu)
        {
            InitializeComponent();
            idTrening = idTreningu;
            Loaded += ChangeTrainingWindow_Loaded;
        }

        public int idTrening { get; set; }

        private void ChangeTrainingBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ChangeTrainingWindow_Loaded(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new GymManagementEntities())
            {
                    // Wykonaj zapytanie do bazy danych, aby pobrać adresy siłowni
                    var gymsList = dbContext.Silownie
                                             .Select(silownia => silownia.adres)
                                             .OrderBy(adres => adres)
                                             .ToList();

                    // Przypisz listę adresów siłowni jako źródło danych dla ComboBox
                    ChangeTrainingGymAddressComboBox.ItemsSource = gymsList;

                var existingTraining = dbContext.Treningi.FirstOrDefault(t => t.idTrening == idTrening);

                if (existingTraining != null)
                {
                    ChangeTrainingTrainingNameTextBox.Text = existingTraining.nazwa;
                    ChangeTrainingDateDatePicker.Text = existingTraining.dataTreningu.ToString("yyyy -MM-dd");
                    ChangeTrainingTrainerNameTextBox.Text = existingTraining.Trenerzy.imie + " " + existingTraining.Trenerzy.nazwisko;
                    ChangeTrainingGymAddressComboBox.Text = existingTraining.Silownie.adres;
                }
            }
        }

        private void ChangeTrainingChangeButton_Click(object sender, RoutedEventArgs e)
        {
            // Sprawdź, czy wszystkie obowiązkowe pola są wypełnione
            if (string.IsNullOrWhiteSpace(ChangeTrainingTrainingNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(ChangeTrainingDateDatePicker.Text) ||
                string.IsNullOrWhiteSpace(ChangeTrainingTrainerNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(ChangeTrainingGymAddressComboBox.Text))
            {
                MessageBox.Show("Proszę wypełnić wszystkie obowiązkowe pola oznaczone gwiazdką (*)!", "Błąd dodawania treningu",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Pobierz wartości wprowadzone przez użytkownika
            string trainingName = ChangeTrainingTrainingNameTextBox.Text;
            string trainingDateStr = ChangeTrainingDateDatePicker.Text;
            string trainerFullName = ChangeTrainingTrainerNameTextBox.Text;
            string gymAddress = ChangeTrainingGymAddressComboBox.Text;

            // Sprawdź, czy data treningu jest w formacie poprawnym
            if (!DateTime.TryParse(trainingDateStr, out DateTime trainingDate))
            {
                MessageBox.Show("Nieprawidłowy format daty treningu. Poprawny format to RRRR-MM-DD!", "Błąd dodawania treningu",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Sprawdź, czy data treningu nie jest datą przeszłą
            if (trainingDate <= DateTime.Today)
            {
                MessageBox.Show("Data treningu musi być datą przyszłą!", "Błąd dodawania treningu",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Pobierz imię i nazwisko trenera
            string[] trainerNameParts = trainerFullName.Split(' ');
            if (trainerNameParts.Length != 2)
            {
                MessageBox.Show("Wpisz imię i nazwisko trenera oddzielone spacją!", "Błąd dodawania treningu",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string trainerFirstName = trainerNameParts[0];
            string trainerLastName = trainerNameParts[1];

            // Sprawdź, czy istnieje trener o podanym imieniu i nazwisku
            using (var dbContext = new GymManagementEntities())
            {
                var existingTrainer = dbContext.Trenerzy.FirstOrDefault(t => t.imie == trainerFirstName && t.nazwisko == trainerLastName);
                if (existingTrainer == null)
                {
                    MessageBox.Show("Nie istnieje trener o podanym imieniu i nazwisku!", "Błąd dodawania treningu",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Sprawdź, czy istnieje siłownia o podanym adresie
                var existingGym = dbContext.Silownie.FirstOrDefault(s => s.adres == gymAddress);
                if (existingGym == null)
                {
                    MessageBox.Show("Nie istnieje siłownia o podanym adresie!", "Błąd dodawania treningu",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Znajdź istniejący trening na podstawie idTreningu
                var existingTraining = dbContext.Treningi.FirstOrDefault(t => t.idTrening == idTrening);

                // Aktualizuj dane treningu zgodnie z danymi wprowadzonymi przez użytkownika
                existingTraining.nazwa = trainingName;
                existingTraining.dataTreningu = trainingDate;
                existingTraining.idTrener = existingTrainer.idTrener;
                existingTraining.idSilownia = existingGym.idSilownia;

                // Zapisz zmiany w bazie danych
                dbContext.SaveChanges();

                MessageBox.Show("Trening został pomyślnie zaktualizowany!", "Sukces",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }

            // Wyczyść pola po udanym dodaniu treningu
        }
    }
}
