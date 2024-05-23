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
    /// Interaction logic for ClientPanel.xaml
    /// </summary>
    public partial class ClientPanel : Window
    {
        public string username { get; set; }

        public ClientPanel(string Username)
        {
            InitializeComponent();
            username = Username;
            Loaded += Window_Loaded;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ClientImieTextBox.Text = GetUserRole(username)+','+' '+GetUserLastName(username)+' '+ GetUserFirstName(username);
        }

        private string GetUserFirstName(string username)
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

        private string GetUserLastName(string username)
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

        private string GetUserRole(string username)
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

        private void ClientShowTrainersButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new GymManagementEntities())
            {
                // Wykonaj zapytanie do bazy danych, aby pobrać imiona i nazwiska trenerów
                var trainersList = dbContext.Trenerzy
                                         .Select(t => new { Nazwisko_Trenera = t.nazwisko, Imię_Trenera = t.imie, Telefon = t.telefon })
                                         .ToList();

                // Przypisz listę trenerów jako źródło danych dla DataGrid
                ClientPanelDataGrid.ItemsSource = trainersList;
            }
        }

        private void ClientShowDieticiansButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new GymManagementEntities())
            {
                // Wykonaj zapytanie do bazy danych, aby pobrać imiona i nazwiska trenerów
                var dieticiansList = dbContext.Dietetycy
                                         .Select(t => new { Nazwisko_Dietetyka = t.nazwisko, Imię_Dietetyka = t.imie, Telefon = t.telefon })
                                         .ToList();

                // Przypisz listę trenerów jako źródło danych dla DataGrid
                ClientPanelDataGrid.ItemsSource = dieticiansList;
            }
        }

        private void ClientShowGymsButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new GymManagementEntities())
            {
                // Wykonaj zapytanie do bazy danych, aby pobrać imiona i nazwiska trenerów
                var gymsList = dbContext.Silownie
                                         .Select(t => new { Adres_Siłowni = t.adres})
                                         .ToList();

                // Przypisz listę trenerów jako źródło danych dla DataGrid
                ClientPanelDataGrid.ItemsSource = gymsList;
            }
        }

        private void ClientShowTrainingsButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new GymManagementEntities())
            {
                // Pobierz dzisiejszą datę
                var today = DateTime.Today;

                var trainingsList = dbContext.Treningi
                    .Where(trening => trening.dataTreningu >= today)
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
                        Wolne_Miejsca = trening.LimitMiejsc - trening.ZarezerwowaneMiejsca
                    })
                    .ToList();

                ClientPanelDataGrid.ItemsSource = trainingsList;
            }
        }

        private void ClientShowMyTrainingsButton_Click(object sender, RoutedEventArgs e)
        {
            // Pobierz imię i nazwisko klienta
            string firstName = GetUserFirstName(username);
            string lastName = GetUserLastName(username);

            using (var dbContext = new GymManagementEntities())
            {
                // Znajdź identyfikator klienta na podstawie imienia i nazwiska
                var clientId = dbContext.Klienci
                    .Where(klient => klient.imie == firstName && klient.nazwisko == lastName)
                    .Select(klient => klient.idKlient)
                    .FirstOrDefault();

                // Pobierz dzisiejszą datę
                var today = DateTime.Today;

                var clientBookings = dbContext.Treningi
                    .Where(trening => trening.ZarezerwowaneTreningi.Any(rezerwacja => rezerwacja.idKlient == clientId) && trening.dataTreningu >= today)
                    .Select(trening => new
                    {
                        TreningId = trening.idTrening,
                        TreningNazwa = trening.nazwa,
                        TreningData = trening.dataTreningu,
                        TreningAdres = trening.Silownie.adres,
                        TreningTrener = trening.Trenerzy.nazwisko + " " + trening.Trenerzy.imie,
                        ZarezerwowaneMiejsca = trening.ZarezerwowaneTreningi.Count(),
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
                        Wolne_Miejsca = trening.LimitMiejsc - trening.ZarezerwowaneMiejsca
                    })
                    .ToList();

                ClientPanelDataGrid.ItemsSource = clientBookings;
            }
        }



        private void ClientShowDietsButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new GymManagementEntities())
            {
                // Wykonaj zapytanie do bazy danych, aby pobrać imiona i nazwiska trenerów
                var dietsList = dbContext.Diety
                                         .Select(t => new { Nazwa_Diety = t.nazwa, Rodzaj_Diety = t.rodzaj, Cena = t.cena })
                                         .ToList();

                // Przypisz listę trenerów jako źródło danych dla DataGrid
                ClientPanelDataGrid.ItemsSource = dietsList;
            }
        }

        private void ClientBookTrainingButton_Click(object sender, RoutedEventArgs e)
        {
            if (ClientPanelDataGrid.SelectedItem != null)
            {
                // Zaznaczony trening
                dynamic selectedTraining = ClientPanelDataGrid.SelectedItem;

                // Pobranie idTrening z zaznaczonego treningu
                int idTrening = selectedTraining.Numer_Treningu;

                // Pobranie imienia i nazwiska
                string imie = GetUserFirstName(username);
                string nazwisko = GetUserLastName(username);

                // Sprawdzenie, czy imię i nazwisko zostały wprowadzone
                if (string.IsNullOrWhiteSpace(imie) || string.IsNullOrWhiteSpace(nazwisko))
                {
                    MessageBox.Show("Wprowadź imię i nazwisko.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Połączenie z bazą danych
                using (var dbContext = new GymManagementEntities())
                {
                    // Sprawdzenie, czy istnieje już rezerwacja tego samego treningu dla tej samej osoby
                    var existingReservation = dbContext.ZarezerwowaneTreningi
                        .FirstOrDefault(r => r.idTrening == idTrening &&
                                              r.Klienci.imie == imie &&
                                              r.Klienci.nazwisko == nazwisko);

                    if (existingReservation != null)
                    {
                        MessageBox.Show("Ta osoba już ma zarezerwowany ten trening.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Sprawdzenie, czy maksymalna liczba rezerwacji na ten trening nie została przekroczona
                    int reservationsCount = dbContext.ZarezerwowaneTreningi
                        .Count(r => r.idTrening == idTrening);

                    if (reservationsCount >= 20)
                    {
                        MessageBox.Show("Maksymalna liczba rezerwacji na ten trening została osiągnięta.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Wyszukanie idKlient na podstawie imienia i nazwiska
                    var klient = dbContext.Klienci.FirstOrDefault(k => k.imie == imie && k.nazwisko == nazwisko);
                    if (klient == null)
                    {
                        MessageBox.Show("Nie znaleziono klienta o podanym imieniu i nazwisku.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Dodanie nowego wpisu do tabeli ZarezerwowaneTreningi
                    ZarezerwowaneTreningi nowaRezerwacja = new ZarezerwowaneTreningi
                    {
                        idTrening = idTrening,
                        idKlient = klient.idKlient
                    };

                    dbContext.ZarezerwowaneTreningi.Add(nowaRezerwacja);
                    dbContext.SaveChanges();

                    MessageBox.Show("Trening został pomyślnie zarezerwowany.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Wybierz trening do zarezerwowania.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClientRemoveBookedTrainingButton_Click(object sender, RoutedEventArgs e)
        {
            if (ClientPanelDataGrid.SelectedItem != null)
            {
                // Zaznaczona rezerwacja
                dynamic selectedReservation = ClientPanelDataGrid.SelectedItem;

                // Pobranie idTrening z zaznaczonej rezerwacji
                int idTrening = selectedReservation.Numer_Treningu;

                // Pobranie imienia i nazwiska
                string imie = GetUserFirstName(username);
                string nazwisko = GetUserLastName(username);

                // Sprawdzenie, czy imię i nazwisko zostały wprowadzone
                if (string.IsNullOrWhiteSpace(imie) || string.IsNullOrWhiteSpace(nazwisko))
                {
                    MessageBox.Show("Wprowadź imię i nazwisko.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Połączenie z bazą danych
                using (var dbContext = new GymManagementEntities())
                {
                    // Wyszukanie rezerwacji na podstawie idTrening oraz imienia i nazwiska klienta
                    var reservationToRemove = dbContext.ZarezerwowaneTreningi
                        .FirstOrDefault(r => r.idTrening == idTrening &&
                                              r.Klienci.imie == imie &&
                                              r.Klienci.nazwisko == nazwisko);

                    if (reservationToRemove != null)
                    {
                        // Usunięcie rezerwacji z bazy danych
                        dbContext.ZarezerwowaneTreningi.Remove(reservationToRemove);
                        dbContext.SaveChanges();

                        MessageBox.Show("Rezerwacja treningu została pomyślnie usunięta.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);

                        LoadMyTrainings();
                    }
                    else
                    {
                        MessageBox.Show("Nie znaleziono rezerwacji dla podanych danych.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Wybierz rezerwację do usunięcia.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadMyTrainings()
        {
            string firstName = GetUserFirstName(username);
            string lastName = GetUserLastName(username);

            using (var dbContext = new GymManagementEntities())
            {
                // Znajdź identyfikator klienta na podstawie imienia i nazwiska
                var clientId = dbContext.Klienci
                    .Where(klient => klient.imie == firstName && klient.nazwisko == lastName)
                    .Select(klient => klient.idKlient)
                    .FirstOrDefault();

                // Pobierz dzisiejszą datę
                var today = DateTime.Today;

                var clientBookings = dbContext.Treningi
                    .Where(trening => trening.ZarezerwowaneTreningi.Any(rezerwacja => rezerwacja.idKlient == clientId) && trening.dataTreningu >= today)
                    .Select(trening => new
                    {
                        TreningId = trening.idTrening,
                        TreningNazwa = trening.nazwa,
                        TreningData = trening.dataTreningu,
                        TreningAdres = trening.Silownie.adres,
                        TreningTrener = trening.Trenerzy.nazwisko + " " + trening.Trenerzy.imie,
                        ZarezerwowaneMiejsca = trening.ZarezerwowaneTreningi.Count(),
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
                        Wolne_Miejsca = trening.LimitMiejsc - trening.ZarezerwowaneMiejsca
                    })
                    .ToList();

                ClientPanelDataGrid.ItemsSource = clientBookings;
            }
        }

        private void ClientPanelDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Sprawdzamy, czy coś jest zaznaczone w DataGrid
            if (ClientPanelDataGrid.SelectedItem != null)
            {
                // Jeśli zaznaczono trening, aktywuj przyciski BookTrainingButton i RemoveBookedTrainingButton
                dynamic selectedItem = ClientPanelDataGrid.SelectedItem;
                if (selectedItem.GetType().GetProperty("Numer_Treningu") != null)
                {
                    ClientBookTrainingButton.IsEnabled = true;
                    ClientRemoveBookedTrainingButton.IsEnabled = true;
                }
                else
                {
                    ClientBookTrainingButton.IsEnabled = false;
                    ClientRemoveBookedTrainingButton.IsEnabled = false;
                }
            }
            else
            {
                // Jeśli nic nie jest zaznaczone, dezaktywuj przyciski BookTrainingButton i RemoveBookedTrainingButton
                ClientBookTrainingButton.IsEnabled = false;
                ClientRemoveBookedTrainingButton.IsEnabled = false;
            }
        }

        private void ClientLogoffButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow NewLogowanie = new MainWindow();
            NewLogowanie.Show();
            Close();
        }

    }
}
