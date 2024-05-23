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
    /// Interaction logic for AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : Window
    {
        public AdminPanel(string Username)
        {
            InitializeComponent();
            username = Username;
            Loaded += Window_Loaded;
        }

        public string username { get; set; }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AdminImieTextBox.Text = GetAdminRole(username) + ',' + ' ' + GetAdminLastName(username) + ' ' + GetAdminFirstName(username);
        }

        private string GetAdminFirstName(string username)
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

        private string GetAdminLastName(string username)
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

        private string GetAdminRole(string username)
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

        private void AdminLogoffButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow NewLogowanie = new MainWindow();
            NewLogowanie.Show();
            Close();
        }

        private void AdminShowTrainersButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new GymManagementEntities())
            {
                // Wykonaj zapytanie do bazy danych, aby pobrać imiona i nazwiska trenerów
                var trainersList = dbContext.Trenerzy
                                         .Select(t => new { Nazwisko_Trenera = t.nazwisko, Imię_Trenera = t.imie, Telefon = t.telefon })
                                         .ToList();

                // Przypisz listę trenerów jako źródło danych dla DataGrid
                AdminPanelDataGrid.ItemsSource = trainersList;
            }
        }

        private void AdminShowDieticiansButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new GymManagementEntities())
            {
                // Wykonaj zapytanie do bazy danych, aby pobrać imiona i nazwiska trenerów
                var dieticiansList = dbContext.Dietetycy
                                         .Select(t => new { Nazwisko_Dietetyka = t.nazwisko, Imię_Dietetyka = t.imie, Telefon = t.telefon })
                                         .ToList();

                // Przypisz listę trenerów jako źródło danych dla DataGrid
                AdminPanelDataGrid.ItemsSource = dieticiansList;
            }
        }

        private void AdminShowGymsButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new GymManagementEntities())
            {
                // Wykonaj zapytanie do bazy danych, aby pobrać imiona i nazwiska trenerów
                var gymsList = dbContext.Silownie
                                         .Select(t => new { Numer_Siłowni = t.idSilownia, Adres_Siłowni = t.adres })
                                         .ToList();

                // Przypisz listę trenerów jako źródło danych dla DataGrid
                AdminPanelDataGrid.ItemsSource = gymsList;
            }
        }

        private void AdminShowTrainingsButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new GymManagementEntities())
            {
                var trainingsList = dbContext.Treningi
                    .Select(trening => new
                    {
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
                        Nazwa = trening.TreningNazwa,
                        Data = trening.TreningData.ToString("yyyy-MM-dd"),
                        Adres = trening.TreningAdres,
                        Trener = trening.TreningTrener,
                        Wolne_Miejsca = trening.LimitMiejsc - trening.ZarezerwowaneMiejsca
                    })
                    .ToList();

                AdminPanelDataGrid.ItemsSource = trainingsList;
            }
        }

        private void AdminShowDietsButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new GymManagementEntities())
            {
                // Wykonaj zapytanie do bazy danych, aby pobrać imiona i nazwiska trenerów
                var dietsList = dbContext.Diety
                                         .Select(t => new
                                         {
                                             Nazwa_Diety = t.nazwa,
                                             Rodzaj_Diety = t.rodzaj,
                                             Cena = t.cena
                                         })
                                         .ToList();

                // Przypisz listę trenerów jako źródło danych dla DataGrid
                AdminPanelDataGrid.ItemsSource = dietsList;
            }
        }

        private void AdminShowAdminsDietsButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new GymManagementEntities())
            {
                // Wykonaj zapytanie do bazy danych, aby pobrać imiona i nazwiska trenerów
                var adminsList = dbContext.Administratorzy
                                         .Select(t => new
                                         {
                                             Nazwisko_Administratora = t.nazwisko,
                                             Imie_Administratora = t.imie,
                                             Telefon = t.telefon
                                         })
                                         .ToList();

                // Przypisz listę trenerów jako źródło danych dla DataGrid
                AdminPanelDataGrid.ItemsSource = adminsList;
            }
        }

        private void AdminShowCashiersButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new GymManagementEntities())
            {
                // Wykonaj zapytanie do bazy danych, aby pobrać imiona i nazwiska trenerów
                var cashiersList = dbContext.PracownicyKasowi
                                         .Select(t => new
                                         {
                                             Nazwisko_Kasjera= t.nazwisko,
                                             Imie_Kasjera = t.imie,
                                             Telefon = t.telefon
                                         })
                                         .ToList();

                // Przypisz listę trenerów jako źródło danych dla DataGrid
                AdminPanelDataGrid.ItemsSource = cashiersList;
            }
        }

        private void AdminShowClientsButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new GymManagementEntities())
            {
                // Wykonaj zapytanie do bazy danych, aby pobrać imiona i nazwiska trenerów
                var clientsList = dbContext.Klienci
                                         .Select(t => new
                                         {
                                             Nazwisko_Klienta = t.nazwisko,
                                             Imie_Klienta = t.imie,
                                             Telefon = t.telefon
                                         })
                                         .ToList();

                // Przypisz listę trenerów jako źródło danych dla DataGrid
                AdminPanelDataGrid.ItemsSource = clientsList;
            }
        }

        private void AdminShowUsersClientsButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new GymManagementEntities())
            {
                var usersList = dbContext.Uzytkownicy
                                         .Select(t => new
                                         {
                                             Numer_Użytkownika = t.idUzytkownika,
                                             Login = t.login,
                                             Uprawnienia = t.uprawnienia,
                                             Status = t.status,
                                             Nazwisko = t.nazwisko,
                                             Imie = t.imie
                                         })
                                         .ToList();

                AdminPanelDataGrid.ItemsSource = usersList;
            }
        }

        private void AdminPanelDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AdminPanelDataGrid.SelectedItem != null)
            {
                dynamic selectedItem = AdminPanelDataGrid.SelectedItem;

                if (selectedItem.GetType().GetProperty("Numer_Siłowni") != null)
                {
                    // Obsługa zdarzenia dla produktu
                    AdminChangeGymButton.IsEnabled = true;
                    AdminDeleteGymButton.IsEnabled = true;

                    // Wyłącz obsługę zdarzenia dla sprzętu
                    AdminChangeUserButton.IsEnabled = false;
                }
                else if (selectedItem.GetType().GetProperty("Numer_Użytkownika") != null)
                {
                    // Obsługa zdarzenia dla sprzętu
                    AdminChangeUserButton.IsEnabled = true;

                    // Wyłącz obsługę zdarzenia dla produktu
                    AdminChangeGymButton.IsEnabled = false;
                    AdminDeleteGymButton.IsEnabled = false;
                }
            }
            else
            {
                // Wyłącz obsługę zdarzenia dla produktu
                AdminChangeGymButton.IsEnabled = false;
                AdminDeleteGymButton.IsEnabled = false;

                // Wyłącz obsługę zdarzenia dla sprzętu
                AdminChangeUserButton.IsEnabled = false;
            }
        }

        private void AdminChangeUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (AdminPanelDataGrid.SelectedItem != null)
            {
                dynamic selectedUser = AdminPanelDataGrid.SelectedItem;

                // Pobranie idTrening z zaznaczonej rezerwacji
                int idUzytkownik = selectedUser.Numer_Użytkownika;
                ChangeUserWindow NewChangeUserWindow = new ChangeUserWindow(idUzytkownik);
                NewChangeUserWindow.Owner = this;
                NewChangeUserWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Wybierz użytkownika do modyfikacji.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AdminAddGymButton_Click(object sender, RoutedEventArgs e)
        {
            AddGymWindow NewAddGymWindow = new AddGymWindow();
            NewAddGymWindow.Owner = this;
            NewAddGymWindow.ShowDialog();
        }

        private void AdminChangeGymButton_Click(object sender, RoutedEventArgs e)
        {
            if (AdminPanelDataGrid.SelectedItem != null)
            {
                dynamic selectedGym = AdminPanelDataGrid.SelectedItem;

                // Pobranie idTrening z zaznaczonej rezerwacji
                int idSilownia = selectedGym.Numer_Siłowni;
                ChangeGymWindow NewChangeGymWindow = new ChangeGymWindow(idSilownia);
                NewChangeGymWindow.Owner = this;
                NewChangeGymWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Wybierz użytkownika do modyfikacji.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AdminDeleteGymButton_Click(object sender, RoutedEventArgs e)
        {
            if (AdminPanelDataGrid.SelectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz usunąć tę siłownie i treningi z nią związane?", "Potwierdzenie usunięcia siłowni", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Zaznaczony trening
                    dynamic selectedGym = AdminPanelDataGrid.SelectedItem;

                    // Pobranie idTrening z zaznaczonego treningu
                    int idSilownia = selectedGym.Numer_Siłowni;

                    // Połączenie z bazą danych
                    using (var dbContext = new GymManagementEntities())
                    {
                        // Sprawdzenie, czy trening istnieje
                        var existingGym = dbContext.Silownie.FirstOrDefault(t => t.idSilownia == idSilownia);
                        if (existingGym != null)
                        {
                            // Usunięcie treningu z bazy danych
                            dbContext.Silownie.Remove(existingGym);

                            // Usunięcie wszystkich rezerwacji związanych z tym treningiem
                            var trainingsToRemove = dbContext.Treningi.Where(r => r.idSilownia == idSilownia);
                            dbContext.Treningi.RemoveRange(trainingsToRemove);

                            var bookedTrainingsToRemove = dbContext.ZarezerwowaneTreningi.Where(r => trainingsToRemove.Any(t => t.idTrening == r.idTrening));
                            dbContext.ZarezerwowaneTreningi.RemoveRange(bookedTrainingsToRemove);

                            dbContext.SaveChanges();

                            MessageBox.Show("Siłownia oraz wszystkie treningi z nią związane zostały usunięte.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);

                            LoadGyms();

                        }
                        else
                        {
                            MessageBox.Show("Nie znaleziono siłowni o podanym identyfikatorze.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Wybierz siłownię do usunięcia.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void LoadGyms()
        {
            using (var dbContext = new GymManagementEntities())
            {
                // Wykonaj zapytanie do bazy danych, aby pobrać imiona i nazwiska trenerów
                var gymsList = dbContext.Silownie
                                         .Select(t => new { Numer_Siłowni = t.idSilownia, Adres_Siłowni = t.adres })
                                         .ToList();

                // Przypisz listę trenerów jako źródło danych dla DataGrid
                AdminPanelDataGrid.ItemsSource = gymsList;
            }
        }
    }
}
