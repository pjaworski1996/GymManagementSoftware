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
    /// Interaction logic for AddTrainingWindow.xaml
    /// </summary>
    public partial class AddTrainingWindow : Window
    {
        public AddTrainingWindow(string Imie, string Nazwisko)
        {
            InitializeComponent();
            imie = Imie;
            nazwisko = Nazwisko;
            Loaded += AddTrainingWindow_Loaded;
        }

        public string imie { get; set; }
        public string nazwisko { get; set; }

        private void AddTrainingWindow_Loaded(object sender, RoutedEventArgs e)
        {
            AddTrainingTrainerNameTextBox.Text = GetTrainerFirstName(imie) + " " + GetTrainerLastName(nazwisko);

            using (var dbContext = new GymManagementEntities())
            {
                // Wykonaj zapytanie do bazy danych, aby pobrać adresy siłowni
                var gymsList = dbContext.Silownie
                                         .Select(silownia => silownia.adres)
                                         .OrderBy(adres => adres)
                                         .ToList();

                // Przypisz listę adresów siłowni jako źródło danych dla ComboBox
                AddTrainingGymAddressComboBox.ItemsSource = gymsList;
            }
        }

        private string GetTrainerFirstName(string imie)
        {
            using (var dbContext = new GymManagementEntities())
            {
                var user = dbContext.Uzytkownicy.FirstOrDefault(u => u.imie == imie);

                if (user != null)
                {
                    return user.imie;
                }
                else
                {
                    // Obsługa przypadku, gdy użytkownik o podanym loginie nie został znaleziony
                    return "Nieznane";
                }
            }
        }

        private string GetTrainerLastName(string nazwisko)
        {
            using (var dbContext = new GymManagementEntities())
            {
                var user = dbContext.Uzytkownicy.FirstOrDefault(u => u.nazwisko == nazwisko);

                if (user != null)
                {
                    return user.nazwisko;
                }
                else
                {
                    // Obsługa przypadku, gdy użytkownik o podanym loginie nie został znaleziony
                    return "Nieznane";
                }
            }
        }

        private void AddTrainingAddButton_Click(object sender, RoutedEventArgs e)
        {
            // Sprawdź, czy wszystkie obowiązkowe pola są wypełnione
            if (string.IsNullOrWhiteSpace(AddTrainingTrainingNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(AddTrainingDateDatePicker.Text) ||
                string.IsNullOrWhiteSpace(AddTrainingTrainerNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(AddTrainingGymAddressComboBox.Text))
            {
                MessageBox.Show("Proszę wypełnić wszystkie obowiązkowe pola oznaczone gwiazdką (*)!", "Błąd dodawania treningu",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Pobierz wartości wprowadzone przez użytkownika
            string trainingName = AddTrainingTrainingNameTextBox.Text;
            string trainingDateStr = AddTrainingDateDatePicker.Text;
            string trainerFullName = AddTrainingTrainerNameTextBox.Text;
            string gymAddress = AddTrainingGymAddressComboBox.Text;

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
                MessageBox.Show("Data treningu nmusi być przyszłą datą!", "Błąd dodawania treningu",
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

                // Utwórz nowy obiekt Trening na podstawie danych wprowadzonych przez użytkownika
                var newTraining = new Treningi
                {
                    nazwa = trainingName,
                    dataTreningu = trainingDate,
                    idTrener = existingTrainer.idTrener,
                    idSilownia = existingGym.idSilownia
                };

                // Dodaj nowy trening do bazy danych za pomocą kontekstu
                dbContext.Treningi.Add(newTraining);
                dbContext.SaveChanges();
            }

            MessageBox.Show("Dodano nowy trening!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);

            // Wyczyść pola po udanym dodaniu treningu
            ClearFields();
        }

        private void ClearFields()
        {
            // Wyczyść zawartość pól tekstowych
            AddTrainingTrainingNameTextBox.Text = "";
            AddTrainingDateDatePicker.Text = "";
            AddTrainingGymAddressComboBox.Text = "";
        }

        private void AddTrainingBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
