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
    /// Interaction logic for DieticianPanel.xaml
    /// </summary>
    public partial class DieticianPanel : Window
    {
        public DieticianPanel(string Username)
        {
            InitializeComponent();
            username = Username;
            Loaded += Window_Loaded;
        }

        public string username { get; set; }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DieticianImieTextBox.Text = GetDieticianRole(username) + ',' + ' ' + GetDieticianLastName(username) + ' ' + GetDieticianFirstName(username);
        }

        private string GetDieticianFirstName(string username)
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

        private string GetDieticianLastName(string username)
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

        private string GetDieticianRole(string username)
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

        private void DieticianShowTrainersButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new GymManagementEntities())
            {
                // Wykonaj zapytanie do bazy danych, aby pobrać imiona i nazwiska trenerów
                var trainersList = dbContext.Trenerzy
                                         .Select(t => new { Nazwisko_Trenera = t.nazwisko, Imię_Trenera = t.imie, Telefon = t.telefon })
                                         .ToList();

                // Przypisz listę trenerów jako źródło danych dla DataGrid
                DieticianPanelDataGrid.ItemsSource = trainersList;
            }
        }

        private void DieticianShowDieticiansButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new GymManagementEntities())
            {
                // Wykonaj zapytanie do bazy danych, aby pobrać imiona i nazwiska trenerów
                var dieticiansList = dbContext.Dietetycy
                                         .Select(t => new { Nazwisko_Dietetyka = t.nazwisko, Imię_Dietetyka = t.imie, Telefon = t.telefon })
                                         .ToList();

                // Przypisz listę trenerów jako źródło danych dla DataGrid
                DieticianPanelDataGrid.ItemsSource = dieticiansList;
            }
        }

        private void DieticianShowGymsButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new GymManagementEntities())
            {
                // Wykonaj zapytanie do bazy danych, aby pobrać imiona i nazwiska trenerów
                var gymsList = dbContext.Silownie
                                         .Select(t => new { Adres_Siłowni = t.adres })
                                         .ToList();

                // Przypisz listę trenerów jako źródło danych dla DataGrid
                DieticianPanelDataGrid.ItemsSource = gymsList;
            }
        }

        private void DieticianShowTrainingsButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new GymManagementEntities())
            {
                // Pobierz dzisiejszą datę
                var today = DateTime.Today;

                var trainingsList = dbContext.Treningi
                    .Where(trening => trening.dataTreningu >= today)
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

                DieticianPanelDataGrid.ItemsSource = trainingsList;
            }
        }

        private void DieticianShowDietsButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new GymManagementEntities())
            {
                // Wykonaj zapytanie do bazy danych, aby pobrać imiona i nazwiska trenerów
                var dietsList = dbContext.Diety
                                         .Select(t => new
                                         { 
                                             Numer_Diety = t.idDieta,
                                             Nazwa_Diety = t.nazwa, 
                                             Rodzaj_Diety = t.rodzaj, 
                                             Cena = t.cena })
                                         .ToList();

                // Przypisz listę trenerów jako źródło danych dla DataGrid
                DieticianPanelDataGrid.ItemsSource = dietsList;
            }
        }

        private void DieticianPanelDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Sprawdzamy, czy coś jest zaznaczone w DataGrid
            if (DieticianPanelDataGrid.SelectedItem != null)
            {
                // Jeśli zaznaczono trening, aktywuj przyciski BookTrainingButton i RemoveBookedTrainingButton
                dynamic selectedItem = DieticianPanelDataGrid.SelectedItem;
                if (selectedItem.GetType().GetProperty("Numer_Diety") != null)
                {
                    DieticianRemoveDietButton.IsEnabled = true;
                    DieticianChangeDietButton.IsEnabled = true;
                    DieticianAssignDietButton.IsEnabled = true;
                }
                else
                {
                    DieticianRemoveDietButton.IsEnabled = false;
                    DieticianChangeDietButton.IsEnabled = false;
                    DieticianAssignDietButton.IsEnabled = false;
                }
            }
            else
            {
                DieticianRemoveDietButton.IsEnabled = false;
                DieticianChangeDietButton.IsEnabled = false;
                DieticianAssignDietButton.IsEnabled = false;
            }
        }

        private void DieticianLogoffButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow NewLogowanie = new MainWindow();
            NewLogowanie.Show();
            Close();
        }

        private void DieticianRemoveDietButton_Click(object sender, RoutedEventArgs e)
        {
            if (DieticianPanelDataGrid.SelectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz usunąć tę dietę i wszystkie jej przypisania?", "Potwierdzenie usunięcia diety", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Zaznaczony trening
                    dynamic selectedDiet = DieticianPanelDataGrid.SelectedItem;

                    // Pobranie idTrening z zaznaczonego treningu
                    int idDieta = selectedDiet.Numer_Diety;

                    // Połączenie z bazą danych
                    using (var dbContext = new GymManagementEntities())
                    {
                        // Sprawdzenie, czy trening istnieje
                        var existingDiet = dbContext.Diety.FirstOrDefault(t => t.idDieta == idDieta);
                        if (existingDiet != null)
                        {
                            // Usunięcie treningu z bazy danych
                            dbContext.Diety.Remove(existingDiet);

                            // Usunięcie wszystkich rezerwacji związanych z tym treningiem
                            var reservationsToRemove = dbContext.PrzypisaneDiety.Where(r => r.idDieta == idDieta);
                            dbContext.PrzypisaneDiety.RemoveRange(reservationsToRemove);

                            dbContext.SaveChanges();

                            MessageBox.Show("Dieta oraz wszystkie jej przypisania zostały pomyślnie usunięte.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);

                            LoadDiets();

                        }
                        else
                        {
                            MessageBox.Show("Nie znaleziono diety o podanym identyfikatorze.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Wybierz dietę do usunięcia.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DieticianAddDietButton_Click(object sender, RoutedEventArgs e)
        {
            AddDietWindow NewAddDietWindow = new AddDietWindow();
            NewAddDietWindow.Owner = this;
            NewAddDietWindow.ShowDialog();
        }

        public void LoadDiets()
        {
            using (var dbContext = new GymManagementEntities())
            {
                // Wykonaj zapytanie do bazy danych, aby pobrać imiona i nazwiska trenerów
                var dietsList = dbContext.Diety
                                         .Select(t => new
                                         {
                                             Numer_Diety = t.idDieta,
                                             Nazwa = t.nazwa,
                                             Rodzaj = t.rodzaj,
                                             Cena = t.cena
                                         })
                                         .ToList();

                // Przypisz listę trenerów jako źródło danych dla DataGrid
                DieticianPanelDataGrid.ItemsSource = dietsList;
            }
        }

        private void DieticianChangeDietButton_Click(object sender, RoutedEventArgs e)
        {
            if (DieticianPanelDataGrid.SelectedItem != null)
            {
                dynamic selectedDiet = DieticianPanelDataGrid.SelectedItem;

                // Pobranie idTrening z zaznaczonej rezerwacji
                int idDieta = selectedDiet.Numer_Diety;
                ChangeDietWindow NewChangeDietWindow = new ChangeDietWindow(idDieta);
                NewChangeDietWindow.Owner = this;
                NewChangeDietWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Wybierz dietę do modyfikacji.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DieticianAssignDietButton_Click(object sender, RoutedEventArgs e)
        {
            if (DieticianPanelDataGrid.SelectedItem != null)
            {
                // Zaznaczony trening
                dynamic selectedDiet = DieticianPanelDataGrid.SelectedItem;

                // Pobranie idTrening z zaznaczonego treningu
                int idDieta = selectedDiet.Numer_Diety;

                // Pobranie imienia i nazwiska
                string imie = DieticianPanelClientImieTextBox.Text;
                string nazwisko = DieticianPanelClientNazwiskoTextBox.Text;

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
                    var existingReservation = dbContext.PrzypisaneDiety
                        .FirstOrDefault(r => r.idDieta == idDieta &&
                                              r.Klienci.imie == imie &&
                                              r.Klienci.nazwisko == nazwisko);

                    if (existingReservation != null)
                    {
                        MessageBox.Show("Ta osoba już ma przypisaną tę dietę.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Wyszukanie idKlient na podstawie imienia i nazwiska
                    var klient = dbContext.Klienci.FirstOrDefault(k => k.imie == imie && k.nazwisko == nazwisko);
                    if (klient == null)
                    {
                        MessageBox.Show("Nie znaleziono klienta o podanym imieniu i nazwisku.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    PrzypisaneDiety nowaRezerwacja = new PrzypisaneDiety
                    {
                        idDieta = idDieta,
                        idKlient = klient.idKlient
                    };

                    dbContext.PrzypisaneDiety.Add(nowaRezerwacja);
                    dbContext.SaveChanges();

                    MessageBox.Show("Dieta została pomyślnie przypisana.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Wybierz dietę do przypisania.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DieticianShowAssignedDietsButton_Click(object sender, RoutedEventArgs e)
        {
            {
                using (var dbContext = new GymManagementEntities())
                {
                    var dietsList = dbContext.PrzypisaneDiety
                        .Select(d => new
                        {
                            Klient = d.Klienci.nazwisko + " " + d.Klienci.imie,
                            Nazwa_Diety = d.Diety.nazwa,
                            Rodzaj_Diety = d.Diety.rodzaj,
                            Cena = d.Diety.cena
                        })
                        .ToList();

                    DieticianPanelDataGrid.ItemsSource = dietsList;
                }
            }
        }
    }
}
