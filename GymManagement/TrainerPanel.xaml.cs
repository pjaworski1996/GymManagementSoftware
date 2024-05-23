using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
    /// Interaction logic for TrainerPanel.xaml
    /// </summary>
    public partial class TrainerPanel : Window
    {
        public TrainerPanel(string Username)
        {
            InitializeComponent();
            username = Username;
            Loaded += Window_Loaded;
        }

        public string username { get; set; }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TrainerImieTextBox.Text = GetTrainerRole(username) + ',' + ' ' + GetTrainerLastName(username) + ' ' + GetTrainerFirstName(username);
        }

        private string GetTrainerFirstName(string username)
        {
            using (var dbContext = new GymManagementEntities())
            {
                var user = dbContext.Uzytkownicy.FirstOrDefault(u => u.login == username);

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

        private string GetTrainerLastName(string username)
        {
            using (var dbContext = new GymManagementEntities())
            {
                var user = dbContext.Uzytkownicy.FirstOrDefault(u => u.login == username);

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

        private string GetTrainerRole(string username)
        {
            using (var dbContext = new GymManagementEntities())
            {
                var user = dbContext.Uzytkownicy.FirstOrDefault(u => u.login == username);

                if (user != null)
                {
                    return user.uprawnienia;
                }
                else
                {
                    // Obsługa przypadku, gdy użytkownik o podanym loginie nie został znaleziony
                    return "Nieznane";
                }
            }
        }

        private void TrainerShowTrainersButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new GymManagementEntities())
            {
                // Wykonaj zapytanie do bazy danych, aby pobrać imiona i nazwiska trenerów
                var trainersList = dbContext.Trenerzy
                                         .Select(t => new { Nazwisko_Trenera = t.nazwisko, Imię_Trenera = t.imie, Telefon = t.telefon })
                                         .ToList();

                // Przypisz listę trenerów jako źródło danych dla DataGrid
                TrainerPanelDataGrid.ItemsSource = trainersList;
            }
        }

        private void TrainerShowDieticiansButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new GymManagementEntities())
            {
                // Wykonaj zapytanie do bazy danych, aby pobrać imiona i nazwiska trenerów
                var dieticiansList = dbContext.Dietetycy
                                         .Select(t => new { Nazwisko_Dietetyka = t.nazwisko, Imię_Dietetyka = t.imie, Telefon = t.telefon })
                                         .ToList();

                // Przypisz listę trenerów jako źródło danych dla DataGrid
                TrainerPanelDataGrid.ItemsSource = dieticiansList;
            }
        }

        private void TrainerShowGymsButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new GymManagementEntities())
            {
                // Wykonaj zapytanie do bazy danych, aby pobrać imiona i nazwiska trenerów
                var gymsList = dbContext.Silownie
                                         .Select(t => new { Adres_Siłowni = t.adres })
                                         .ToList();

                // Przypisz listę trenerów jako źródło danych dla DataGrid
                TrainerPanelDataGrid.ItemsSource = gymsList;
            }
        }

        private void TrainerShowTrainingsButton_Click(object sender, RoutedEventArgs e)
        {
            // Pobierz imię i nazwisko trenera
            string firstName = GetTrainerFirstName(username);
            string lastName = GetTrainerLastName(username);

            using (var dbContext = new GymManagementEntities())
            {
                // Znajdź identyfikator trenera na podstawie imienia i nazwiska
                var trainerId = dbContext.Trenerzy
                    .Where(trener => trener.imie == firstName && trener.nazwisko == lastName)
                    .Select(trener => trener.idTrener)
                    .FirstOrDefault();

                // Pobierz dzisiejszą datę
                var today = DateTime.Today;

                var trainerTrainings = dbContext.Treningi
                    .Where(trening => trening.idTrener == trainerId && trening.dataTreningu >= today)
                    .Select(trening => new
                    {
                        TreningId = trening.idTrening,
                        TreningNazwa = trening.nazwa,
                        TreningData = trening.dataTreningu,
                        TreningAdres = trening.Silownie.adres,
                        TreningTrener = trening.Trenerzy.nazwisko + " " + trening.Trenerzy.imie,
                        ZarezerwowaneMiejsca = dbContext.ZarezerwowaneTreningi
                                                       .Count(rezerwacja => rezerwacja.idTrening == trening.idTrening),
                        LimitMiejsc = 20
                    })
                    .ToList()
                    .Select(trening => new
                    {
                        Numer_Treningu = trening.TreningId,
                        Nazwa = trening.TreningNazwa,
                        Data = trening.TreningData.ToString("yyyy-MM-dd"),
                        Adres = trening.TreningAdres,
                        Trener = trening.TreningTrener,
                        WolneMiejsca = trening.LimitMiejsc - trening.ZarezerwowaneMiejsca
                    })
                    .ToList();

                TrainerPanelDataGrid.ItemsSource = trainerTrainings;
            }
        }



        private void TrainerShowDietsButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new GymManagementEntities())
            {
                // Wykonaj zapytanie do bazy danych, aby pobrać imiona i nazwiska trenerów
                var dietsList = dbContext.Diety
                                         .Select(t => new { Nazwa_Diety = t.nazwa, Rodzaj_Diety = t.rodzaj, Cena = t.cena })
                                         .ToList();

                // Przypisz listę trenerów jako źródło danych dla DataGrid
                TrainerPanelDataGrid.ItemsSource = dietsList;
            }
        }

        private void TrainerPanelDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Sprawdzamy, czy coś jest zaznaczone w DataGrid
            if (TrainerPanelDataGrid.SelectedItem != null)
            {
                // Jeśli zaznaczono trening, aktywuj przyciski BookTrainingButton i RemoveBookedTrainingButton
                dynamic selectedItem = TrainerPanelDataGrid.SelectedItem;
                if (selectedItem.GetType().GetProperty("Numer_Treningu") != null)
                {
                    TrainerRemoveTrainingButton.IsEnabled = true;
                    TrainerChangeTrainingButton.IsEnabled = true;
                }
                else
                {
                    TrainerRemoveTrainingButton.IsEnabled = false;
                    TrainerChangeTrainingButton.IsEnabled = false;
                }
            }
            else
            {
                TrainerRemoveTrainingButton.IsEnabled = false;
                TrainerChangeTrainingButton.IsEnabled = false;
            }
        }

        private void TrainerLogoffButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow NewLogowanie = new MainWindow();
            NewLogowanie.Show();
            Close();
        }

        private void TrainerRemoveTrainingButton_Click(object sender, RoutedEventArgs e)
        {
            if (TrainerPanelDataGrid.SelectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz usunąć ten trening i wszystkie związane z nim rezerwacje?", "Potwierdzenie usunięcia treningu", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Zaznaczony trening
                    dynamic selectedTraining = TrainerPanelDataGrid.SelectedItem;

                    // Pobranie idTrening z zaznaczonego treningu
                    int idTrening = selectedTraining.Numer_Treningu;

                    // Połączenie z bazą danych
                    using (var dbContext = new GymManagementEntities())
                    {
                        // Sprawdzenie, czy trening istnieje
                        var existingTraining = dbContext.Treningi.FirstOrDefault(t => t.idTrening == idTrening);
                        if (existingTraining != null)
                        {
                            // Usunięcie treningu z bazy danych
                            dbContext.Treningi.Remove(existingTraining);

                            // Usunięcie wszystkich rezerwacji związanych z tym treningiem
                            var reservationsToRemove = dbContext.ZarezerwowaneTreningi.Where(r => r.idTrening == idTrening);
                            dbContext.ZarezerwowaneTreningi.RemoveRange(reservationsToRemove);

                            dbContext.SaveChanges();

                            MessageBox.Show("Trening oraz wszystkie związane z nim rezerwacje zostały pomyślnie usunięte.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);

                            LoadTrainings();

                        }
                        else
                        {
                            MessageBox.Show("Nie znaleziono treningu o podanym identyfikatorze.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Wybierz trening do usunięcia.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TrainerAddTrainingButton_Click(object sender, RoutedEventArgs e)
        {
            AddTrainingWindow NewAddTrainingWindow = new AddTrainingWindow(GetTrainerFirstName(username), GetTrainerLastName(username));
            NewAddTrainingWindow.Owner = this;
            NewAddTrainingWindow.ShowDialog();
        }

        private void TrainerChangeTrainingButton_Click(object sender, RoutedEventArgs e)
        {
            if (TrainerPanelDataGrid.SelectedItem != null)
            {
                dynamic selectedTraining = TrainerPanelDataGrid.SelectedItem;

                // Pobranie idTrening z zaznaczonej rezerwacji
                int idTrening = selectedTraining.Numer_Treningu;
                ChangeTrainingWindow NewChangeTrainingWindow = new ChangeTrainingWindow(idTrening);
                NewChangeTrainingWindow.Owner = this;
                NewChangeTrainingWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Wybierz trening do modyfikacji.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void LoadTrainings()
        {
            // Pobierz imię i nazwisko trenera
            string firstName = GetTrainerFirstName(username);
            string lastName = GetTrainerLastName(username);

            using (var dbContext = new GymManagementEntities())
            {
                // Znajdź identyfikator trenera na podstawie imienia i nazwiska
                var trainerId = dbContext.Trenerzy
                    .Where(trener => trener.imie == firstName && trener.nazwisko == lastName)
                    .Select(trener => trener.idTrener)
                    .FirstOrDefault();

                // Pobierz dzisiejszą datę
                var today = DateTime.Today;

                var trainerTrainings = dbContext.Treningi
                    .Where(trening => trening.idTrener == trainerId && trening.dataTreningu >= today)
                    .Select(trening => new
                    {
                        TreningId = trening.idTrening,
                        TreningNazwa = trening.nazwa,
                        TreningData = trening.dataTreningu,
                        TreningAdres = trening.Silownie.adres,
                        TreningTrener = trening.Trenerzy.nazwisko + " " + trening.Trenerzy.imie,
                        ZarezerwowaneMiejsca = dbContext.ZarezerwowaneTreningi
                                                       .Count(rezerwacja => rezerwacja.idTrening == trening.idTrening),
                        LimitMiejsc = 20
                    })
                    .ToList()
                    .Select(trening => new
                    {
                        Numer_Treningu = trening.TreningId,
                        Nazwa = trening.TreningNazwa,
                        Data = trening.TreningData.ToString("yyyy-MM-dd"),
                        Adres = trening.TreningAdres,
                        Trener = trening.TreningTrener,
                        WolneMiejsca = trening.LimitMiejsc - trening.ZarezerwowaneMiejsca
                    })
                    .ToList();

                TrainerPanelDataGrid.ItemsSource = trainerTrainings;
            }
        }

        private void TrainerShowOldTrainingsButton_Click(object sender, RoutedEventArgs e)
        {
            // Pobierz imię i nazwisko trenera
            string firstName = GetTrainerFirstName(username);
            string lastName = GetTrainerLastName(username);

            using (var dbContext = new GymManagementEntities())
            {
                // Znajdź identyfikator trenera na podstawie imienia i nazwiska
                var trainerId = dbContext.Trenerzy
                    .Where(trener => trener.imie == firstName && trener.nazwisko == lastName)
                    .Select(trener => trener.idTrener)
                    .FirstOrDefault();

                // Pobierz dzisiejszą datę
                var today = DateTime.Today;

                var trainerTrainings = dbContext.Treningi
                    .Where(trening => trening.idTrener == trainerId && trening.dataTreningu < today)
                    .Select(trening => new
                    {
                        TreningId = trening.idTrening,
                        TreningNazwa = trening.nazwa,
                        TreningData = trening.dataTreningu,
                        TreningAdres = trening.Silownie.adres,
                        TreningTrener = trening.Trenerzy.nazwisko + " " + trening.Trenerzy.imie,
                        ZarezerwowaneMiejsca = dbContext.ZarezerwowaneTreningi
                                                       .Count(rezerwacja => rezerwacja.idTrening == trening.idTrening),
                        LimitMiejsc = 20
                    })
                    .ToList()
                    .Select(trening => new
                    {
                        Numer = trening.TreningId,
                        Nazwa = trening.TreningNazwa,
                        Data = trening.TreningData.ToString("yyyy-MM-dd"),
                        Adres = trening.TreningAdres,
                        Trener = trening.TreningTrener,
                        WolneMiejsca = trening.LimitMiejsc - trening.ZarezerwowaneMiejsca
                    })
                    .ToList();

                TrainerPanelDataGrid.ItemsSource = trainerTrainings;
            }
        }

    }
}
