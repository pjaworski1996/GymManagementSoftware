using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GymManagement
{
    /// <summary>
    /// Interaction logic for CashierPanel.xaml
    /// </summary>
    public partial class CashierPanel : Window
    {
        public CashierPanel(string Username)
        {
            InitializeComponent();
            username = Username;
            Loaded += Window_Loaded;
        }

        public string username { get; set; }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CashierImieTextBox.Text = GetCashierRole(username) + ',' + ' ' + GetCashierLastName(username) + ' ' + GetCashierFirstName(username);
        }

        private string GetCashierFirstName(string username)
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

        private string GetCashierLastName(string username)
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

        private string GetCashierRole(string username)
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

        private void CashierShowTrainersButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new GymManagementEntities())
            {
                // Wykonaj zapytanie do bazy danych, aby pobrać imiona i nazwiska trenerów
                var trainersList = dbContext.Trenerzy
                                         .Select(t => new { Nazwisko_Trenera = t.nazwisko, Imię_Trenera = t.imie, Telefon = t.telefon })
                                         .ToList();

                // Przypisz listę trenerów jako źródło danych dla DataGrid
                CashierPanelDataGrid.ItemsSource = trainersList;
            }
        }

        private void CashierShowDieticiansButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new GymManagementEntities())
            {
                // Wykonaj zapytanie do bazy danych, aby pobrać imiona i nazwiska trenerów
                var dieticiansList = dbContext.Dietetycy
                                         .Select(t => new { Nazwisko_Dietetyka = t.nazwisko, Imię_Dietetyka = t.imie, Telefon = t.telefon })
                                         .ToList();

                // Przypisz listę trenerów jako źródło danych dla DataGrid
                CashierPanelDataGrid.ItemsSource = dieticiansList;
            }
        }

        private void CashierShowGymsButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new GymManagementEntities())
            {
                // Wykonaj zapytanie do bazy danych, aby pobrać imiona i nazwiska trenerów
                var gymsList = dbContext.Silownie
                                         .Select(t => new { Adres_Siłowni = t.adres })
                                         .ToList();

                // Przypisz listę trenerów jako źródło danych dla DataGrid
                CashierPanelDataGrid.ItemsSource = gymsList;
            }
        }

        private void CashierShowTrainingsButton_Click(object sender, RoutedEventArgs e)
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

                CashierPanelDataGrid.ItemsSource = trainingsList;
            }
        }

        private void CashierShowDietsButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new GymManagementEntities())
            {
                // Wykonaj zapytanie do bazy danych, aby pobrać imiona i nazwiska trenerów
                var dietsList = dbContext.Diety
                                         .Select(t => new { Nazwa_Diety = t.nazwa, Rodzaj_Diety = t.rodzaj, Cena = t.cena })
                                         .ToList();

                // Przypisz listę trenerów jako źródło danych dla DataGrid
                CashierPanelDataGrid.ItemsSource = dietsList;
            }
        }
        private void CashierShowProductsButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new GymManagementEntities())
            {
                // Wykonaj zapytanie do bazy danych, aby pobrać imiona i nazwiska trenerów
                var productsList = dbContext.Produkty
                                         .Select(t => new { Numer_Produktu = t.idProdukt, Nazwa_Produktu = t.nazwa, Cena_Produktu = t.cena, Ilość = t.ilosc })
                                         .ToList();

                // Przypisz listę trenerów jako źródło danych dla DataGrid
                CashierPanelDataGrid.ItemsSource = productsList;
            }
        }

        private void CashierShowEquipmentButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new GymManagementEntities())
            {
                // Wykonaj zapytanie do bazy danych, aby pobrać imiona i nazwiska trenerów
                var equipmentList = dbContext.Sprzety
                                         .Select(t => new { Numer_Sprzętu = t.idSprzet, Nazwa_Sprzętu = t.nazwa, Ilość = t.ilosc })
                                         .ToList();

                // Przypisz listę trenerów jako źródło danych dla DataGrid
                CashierPanelDataGrid.ItemsSource = equipmentList;
            }
        }

        private void CashierShowMembershipsButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new GymManagementEntities())
            {
                // Wykonaj zapytanie do bazy danych, aby pobrać karnety z datą ważności dzisiaj lub w przyszłości
                var membershipsList = dbContext.Karnety
                    .Where(t => t.dataWaznosci >= DateTime.Today) // Warunek: data ważności dzisiaj lub w przyszłości
                    .Select(t => new
                    {
                        Klient = t.Klienci.nazwisko + " " + t.Klienci.imie,
                        Data_Ważności = t.dataWaznosci,
                        Cena = t.cena
                    })
                    .ToList();

                var formattedMembershipsList = membershipsList.Select(membership => new
                {
                    Klient = membership.Klient,
                    Data_Ważności = membership.Data_Ważności.ToString("yyyy-MM-dd"),
                    Cena = membership.Cena
                }).ToList();

                // Przypisz listę karnetów jako źródło danych dla DataGrid
                CashierPanelDataGrid.ItemsSource = formattedMembershipsList;
            }
        }


        private void CashierAddProductButton_Click(object sender, RoutedEventArgs e)
        {
            AddProductWindow NewAddProductWindow = new AddProductWindow();
            NewAddProductWindow.Owner = this;
            NewAddProductWindow.ShowDialog();
        }

        private void CashierChangeProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (CashierPanelDataGrid.SelectedItem != null)
            {
                dynamic selectedProduct = CashierPanelDataGrid.SelectedItem;

                // Pobranie idTrening z zaznaczonej rezerwacji
                int idProdukt = selectedProduct.Numer_Produktu;
                ChangeProductWindow NewChangeProductWindow = new ChangeProductWindow(idProdukt);
                NewChangeProductWindow.Owner = this;
                NewChangeProductWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Wybierz produkt do modyfikacji.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CashierRemoveProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (CashierPanelDataGrid.SelectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz usunąć ten produkt i wszystkie zwiazanie z nim transakcje?", "Potwierdzenie usunięcia produktu", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Zaznaczony produkt
                    dynamic selectedProduct = CashierPanelDataGrid.SelectedItem;

                    // Pobranie idProduktu z zaznaczonego produktu
                    int idProdukt = selectedProduct.Numer_Produktu;

                    // Połączenie z bazą danych
                    using (var dbContext = new GymManagementEntities())
                    {
                        // Sprawdzenie, czy produkt istnieje
                        var existingProduct = dbContext.Produkty.FirstOrDefault(p => p.idProdukt == idProdukt);
                        if (existingProduct != null)
                        {
                            // Usunięcie produktu z bazy danych
                            dbContext.Produkty.Remove(existingProduct);

                            var productsToRemove = dbContext.Transakcje.Where(r => r.idProdukt == idProdukt);
                            dbContext.Transakcje.RemoveRange(productsToRemove);

                            dbContext.SaveChanges();

                            MessageBox.Show("Produkt i wszystkie związane z nim transakcje zostały pomyślnie usunięte.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);

                            // Ponowne załadowanie danych w DataGrid
                            LoadProducts();
                        }
                        else
                        {
                            MessageBox.Show("Nie znaleziono produktu o podanym identyfikatorze.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Wybierz produkt do usunięcia.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CashierAddEquipmentButton_Click(object sender, RoutedEventArgs e)
        {
            AddEquipmentWindow NewAddEquipmentWindow = new AddEquipmentWindow();
            NewAddEquipmentWindow.Owner = this;
            NewAddEquipmentWindow.ShowDialog();
        }

        private void CashierChangeEquipmentButton_Click(object sender, RoutedEventArgs e)
        {
            if (CashierPanelDataGrid.SelectedItem != null)
            {
                dynamic selectedProduct = CashierPanelDataGrid.SelectedItem;

                int idSprzet = selectedProduct.Numer_Sprzętu;
                ChangeEquipmentWindow NewChangeEquipmentWindow = new ChangeEquipmentWindow(idSprzet);
                NewChangeEquipmentWindow.Owner = this;
                NewChangeEquipmentWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Wybierz Sprzęt do modyfikacji.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CashierRemoveEquipmentButton_Click(object sender, RoutedEventArgs e)
        {
            if (CashierPanelDataGrid.SelectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz usunąć ten sprzęt i wszystkie związane z nim wypożyczenia?", "Potwierdzenie usunięcia sprzętu", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Zaznaczony sprzęt
                    dynamic selectedEquipment = CashierPanelDataGrid.SelectedItem;

                    // Pobranie idSprzetu z zaznaczonego sprzętu
                    int idSprzet = selectedEquipment.Numer_Sprzętu;

                    // Połączenie z bazą danych
                    using (var dbContext = new GymManagementEntities())
                    {
                        // Sprawdzenie, czy sprzęt istnieje
                        var existingEquipment = dbContext.Sprzety.FirstOrDefault(s => s.idSprzet == idSprzet);
                        if (existingEquipment != null)
                        {
                            // Usunięcie sprzętu z bazy danych
                            dbContext.Sprzety.Remove(existingEquipment);

                            var equipmentToRemove = dbContext.WypozyczeniaSprzetu.Where(r => r.idSprzet == idSprzet);
                            dbContext.WypozyczeniaSprzetu.RemoveRange(equipmentToRemove);

                            dbContext.SaveChanges();

                            MessageBox.Show("Sprzęt i wszystkie związane z nim wypożyczenia zostały pomyślnie usunięte.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);

                            // Ponowne załadowanie danych w DataGrid
                            LoadEquipment();
                        }
                        else
                        {
                            MessageBox.Show("Nie znaleziono sprzętu o podanym identyfikatorze.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Wybierz sprzęt do usunięcia.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CashierLogoffButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow NewLogowanie = new MainWindow();
            NewLogowanie.Show();
            Close();
        }

        private void LoadEquipment()
        {
            using (var dbContext = new GymManagementEntities())
            {
                // Wykonaj zapytanie do bazy danych, aby pobrać imiona i nazwiska trenerów
                var equipmentList = dbContext.Sprzety
                                         .Select(t => new { Numer_Sprzętu = t.idSprzet, Nazwa_Sprzętu = t.nazwa, Ilość = t.ilosc })
                                         .ToList();

                // Przypisz listę trenerów jako źródło danych dla DataGrid
                CashierPanelDataGrid.ItemsSource = equipmentList;
            }
        }

        private void LoadProducts()
        {
            using (var dbContext = new GymManagementEntities())
            {
                // Wykonaj zapytanie do bazy danych, aby pobrać imiona i nazwiska trenerów
                var productsList = dbContext.Produkty
                                         .Select(t => new { Numer_Produktu = t.idProdukt, Nazwa_Produktu = t.nazwa, Cena_Produktu = t.cena, Ilość = t.ilosc })
                                         .ToList();

                // Przypisz listę trenerów jako źródło danych dla DataGrid
                CashierPanelDataGrid.ItemsSource = productsList;
            }
        }

        private void LoadMembership()
        {
            using (var dbContext = new GymManagementEntities())
            {
                // Wykonaj zapytanie do bazy danych, aby pobrać karnety z datą ważności dzisiaj lub w przyszłości
                var membershipsList = dbContext.Karnety
                    .Where(t => t.dataWaznosci >= DateTime.Today) // Warunek: data ważności dzisiaj lub w przyszłości
                    .Select(t => new
                    {
                        Klient = t.Klienci.imie + " " + t.Klienci.nazwisko,
                        Data_Ważności = t.dataWaznosci,
                        Cena = t.cena
                    })
                    .ToList();

                var formattedMembershipsList = membershipsList.Select(membership => new
                {
                    Klient = membership.Klient,
                    Data_Ważności = membership.Data_Ważności.ToString("yyyy-MM-dd"),
                    Cena = membership.Cena
                }).ToList();

                // Przypisz listę karnetów jako źródło danych dla DataGrid
                CashierPanelDataGrid.ItemsSource = formattedMembershipsList;
            }
        }

        private void CashierPanelDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CashierPanelDataGrid.SelectedItem != null)
            {
                dynamic selectedItem = CashierPanelDataGrid.SelectedItem;

                if (selectedItem.GetType().GetProperty("Numer_Produktu") != null)
                {
                    // Obsługa zdarzenia dla produktu
                    CashierChangeProductButton.IsEnabled = true;
                    CashierRemoveProductButton.IsEnabled = true;
                    CashierSellProductButton.IsEnabled = true;

                    // Wyłącz obsługę zdarzenia dla sprzętu
                    CashierChangeEquipmentButton.IsEnabled = false;
                    CashierRemoveEquipmentButton.IsEnabled = false;
                }
                else if (selectedItem.GetType().GetProperty("Numer_Sprzętu") != null)
                {
                    // Obsługa zdarzenia dla sprzętu
                    CashierChangeEquipmentButton.IsEnabled = true;
                    CashierRemoveEquipmentButton.IsEnabled = true;

                    // Wyłącz obsługę zdarzenia dla produktu
                    CashierChangeProductButton.IsEnabled = false;
                    CashierRemoveProductButton.IsEnabled = false;
                    CashierSellProductButton.IsEnabled = false;
                }
            }
            else
            {
                // Wyłącz obsługę zdarzenia dla produktu
                CashierChangeProductButton.IsEnabled = false;
                CashierRemoveProductButton.IsEnabled = false;
                CashierSellProductButton.IsEnabled = false;

                // Wyłącz obsługę zdarzenia dla sprzętu
                CashierChangeEquipmentButton.IsEnabled = false;
                CashierRemoveEquipmentButton.IsEnabled = false;
            }
        }


        private void CashierSellProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (CashierPanelDataGrid.SelectedItem != null)
            {
                // Zaznaczony produkt
                dynamic selectedProduct = CashierPanelDataGrid.SelectedItem;

                // Pobranie idProduktu z zaznaczonego produktu
                int idProdukt = selectedProduct.Numer_Produktu;
                string productName = selectedProduct.Nazwa_Produktu;

                // Pobranie imienia i nazwiska klienta
                string imie = CashierPanelClientImieTextBox.Text;
                string nazwisko = CashierPanelClientNazwiskoTextBox.Text;
                string QuantityStr = CashierPanelIloscTextBox.Text;

                // Sprawdzenie, czy ilość została wprowadzona i jest większa od zera
                if (!int.TryParse(QuantityStr, out int parsedQuantityStr) || parsedQuantityStr <= 0)
                {
                    MessageBox.Show("Wprowadź poprawną ilość produktu (większą od zera).", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Sprawdzenie, czy imię, nazwisko i ilość zostały wprowadzone
                if (string.IsNullOrWhiteSpace(imie) || string.IsNullOrWhiteSpace(nazwisko))
                {
                    MessageBox.Show("Wprowadź poprawne dane (imię, nazwisko).", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Połączenie z bazą danych
                using (var dbContext = new GymManagementEntities())
                {
                    // Wyszukanie produktu w tabeli Produkty
                    var produkt = dbContext.Produkty.FirstOrDefault(p => p.idProdukt == idProdukt);
                    if (produkt == null)
                    {
                        MessageBox.Show("Nie znaleziono produktu o podanym identyfikatorze.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Wyszukanie klienta na podstawie imienia i nazwiska
                    var klient = dbContext.Klienci.FirstOrDefault(k => k.imie == imie && k.nazwisko == nazwisko);
                    if (klient == null)
                    {
                        MessageBox.Show("Nie znaleziono klienta o podanym imieniu i nazwisku.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    int iloscDostepna = produkt.ilosc;
                    if (parsedQuantityStr > iloscDostepna)
                    {
                        MessageBox.Show($"Ilość wpisana ({parsedQuantityStr}) jest większa niż dostępna ilość produktu ({iloscDostepna}).", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Pobranie ceny produktu
                    int cena = 0;
                    int idProduktu = selectedProduct.Numer_Produktu;
                    var produktPrice = dbContext.Produkty.FirstOrDefault(p => p.idProdukt == idProduktu);
                    if (produktPrice != null)
                    {
                        cena = produktPrice.cena ?? 0; // użyj domyślnej wartości 0, jeśli cena jest null
                    }


                    // Obliczenie ceny na podstawie ceny produktu i ilości
                    int totalCena = 0;
                    if (cena != 0 && parsedQuantityStr > 0) // Jeśli cena produktu i ilość są większe od zera
                    {
                        totalCena = cena * parsedQuantityStr;
                    }

                    string dateString = DateTime.Now.ToString("yyyy-MM-dd");
                    DateTime parsedDate = DateTime.ParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                    // Dodanie transakcji do bazy danych
                    Transakcje nowaTransakcja = new Transakcje
                    {
                        idProdukt = idProdukt,
                        idKlient = klient.idKlient,
                        data = parsedDate,
                        cena = totalCena,
                    };

                    dbContext.Transakcje.Add(nowaTransakcja);
                    produkt.ilosc -= parsedQuantityStr;
                    dbContext.SaveChanges();

                    MessageBox.Show($"Pomyślnie sprzedano {parsedQuantityStr} sztuk produktu {productName} o wartości {totalCena} zł.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadProducts();
                }
            }
            else
            {
                MessageBox.Show("Wybierz produkt do sprzedaży.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CashierRentEquipmentButton_Click(object sender, RoutedEventArgs e)
        {
            if (CashierPanelDataGrid.SelectedItem != null)
            {
                // Zaznaczony produkt
                dynamic selectedProduct = CashierPanelDataGrid.SelectedItem;

                // Pobranie idProduktu z zaznaczonego produktu
                int idSprzet = selectedProduct.Numer_Sprzętu;
                string equipmentName = selectedProduct.Nazwa_Sprzętu;
                string QuantityStr = CashierPanelIloscTextBox.Text;

                // Sprawdzenie, czy ilość została wprowadzona i jest większa od zera
                if (!int.TryParse(QuantityStr, out int parsedQuantityStr) || parsedQuantityStr != 1)
                {
                    MessageBox.Show("Można wypożyczyć tylko jedną sztukę sprzętu na wypożyczenie!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Pobranie imienia i nazwiska klienta
                string imie = CashierPanelClientImieTextBox.Text;
                string nazwisko = CashierPanelClientNazwiskoTextBox.Text;

                // Sprawdzenie, czy imię, nazwisko i ilość zostały wprowadzone
                if (string.IsNullOrWhiteSpace(imie) || string.IsNullOrWhiteSpace(nazwisko))
                {
                    MessageBox.Show("Wprowadź poprawne dane (imię, nazwisko).", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Połączenie z bazą danych
                using (var dbContext = new GymManagementEntities())
                {
                    // Wyszukanie produktu w tabeli Produkty
                    var sprzet = dbContext.Sprzety.FirstOrDefault(p => p.idSprzet == idSprzet);
                    if (sprzet == null)
                    {
                        MessageBox.Show("Nie znaleziono sprzętu o podanym identyfikatorze.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Wyszukanie klienta na podstawie imienia i nazwiska
                    var klient = dbContext.Klienci.FirstOrDefault(k => k.imie == imie && k.nazwisko == nazwisko);
                    if (klient == null)
                    {
                        MessageBox.Show("Nie znaleziono klienta o podanym imieniu i nazwisku.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    int iloscDostepna = sprzet.ilosc;
                    if (parsedQuantityStr > iloscDostepna)
                    {
                        MessageBox.Show($"Ilość wpisana ({parsedQuantityStr}) jest większa niż dostępna ilość sprzętu ({iloscDostepna}).", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    string dateString = DateTime.Now.ToString("yyyy-MM-dd");
                    DateTime parsedDate = DateTime.ParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                    // Dodanie transakcji do bazy danych
                    WypozyczeniaSprzetu noweWypozyczenie = new WypozyczeniaSprzetu
                    {
                        idSprzet = idSprzet,
                        idKlient = klient.idKlient,
                        data = parsedDate,
                    };

                    dbContext.WypozyczeniaSprzetu.Add(noweWypozyczenie);
                    sprzet.ilosc -= parsedQuantityStr;
                    dbContext.SaveChanges();

                    MessageBox.Show($"Pomyślnie wypożyczono {parsedQuantityStr} sztuk sprzętu {equipmentName}.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadEquipment();
                }
            }
            else
            {
                MessageBox.Show("Wybierz sprzęt do wypożyczenia", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CashierSellMembershipButton_Click(object sender, RoutedEventArgs e)
        {
                string QuantityStr = CashierPanelIloscTextBox.Text;

                // Sprawdzenie, czy ilość została wprowadzona i jest większa od zera
                if (!int.TryParse(QuantityStr, out int parsedQuantityStr) || parsedQuantityStr <= 0)
                {
                    MessageBox.Show("Wpisz ilość dni większą od zera!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Pobranie imienia i nazwiska klienta
                string imie = CashierPanelClientImieTextBox.Text;
                string nazwisko = CashierPanelClientNazwiskoTextBox.Text;

                // Sprawdzenie, czy imię, nazwisko i ilość zostały wprowadzone
                if (string.IsNullOrWhiteSpace(imie) || string.IsNullOrWhiteSpace(nazwisko))
                {
                    MessageBox.Show("Wprowadź poprawne dane (imię, nazwisko).", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Połączenie z bazą danych
                using (var dbContext = new GymManagementEntities())
                {
                    // Wyszukanie klienta na podstawie imienia i nazwiska
                    var klient = dbContext.Klienci.FirstOrDefault(k => k.imie == imie && k.nazwisko == nazwisko);
                    if (klient == null)
                    {
                        MessageBox.Show("Nie znaleziono klienta o podanym imieniu i nazwisku.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    string CostPerDay = CashierPanelMembershipCostPerDayTextBox.Text;

                    if (!int.TryParse(CostPerDay, out int parsedCostPerDay) || parsedCostPerDay <= 0)
                    {
                        MessageBox.Show("Wprowadź poprawną cenę za dzień większą od zera!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    int daysQuantity = int.Parse(QuantityStr);

                    string dateString = DateTime.Now.ToString("yyyy-MM-dd");
                    DateTime parsedDate = DateTime.ParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    DateTime dataWaznosci = parsedDate.AddDays(daysQuantity);
                    int totalCost = daysQuantity * parsedCostPerDay;

                    // Dodanie transakcji do bazy danych
                    Karnety nowyKarnet = new Karnety
                    {
                        IdKlient = klient.idKlient,
                        dataWaznosci = dataWaznosci,
                        cena = totalCost
                    };

                    dbContext.Karnety.Add(nowyKarnet);
                    dbContext.SaveChanges();

                    MessageBox.Show($"Pomyślnie sprzedano karnet na {parsedQuantityStr} dni w cenie {totalCost} zł.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadMembership();
                }
            
        }

        private void CashierShowTransactionsButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new GymManagementEntities())
            {
                // Wykonaj zapytanie do bazy danych, aby pobrać transakcje z dzisiaj
                var transactionsList = dbContext.Transakcje
                    .Where(t => t.data == DateTime.Today) // Warunek: data transakcji dzisiaj
                    .Select(t => new
                    {
                        Produkt = t.Produkty.nazwa,
                        Klient = t.Klienci.nazwisko + " " + t.Klienci.imie,
                        Data_Transakcji = t.data,
                        Cena = t.cena
                    })
                    .ToList();

                var formattedTransactionsList = transactionsList.Select(transaction => new
                {
                    Produkt = transaction.Produkt,
                    Klient = transaction.Klient,
                    Data_Transakcji = transaction.Data_Transakcji.ToString("yyyy-MM-dd"),
                    Cena = transaction.Cena
                }).ToList();

                // Przypisz listę transakcji jako źródło danych dla DataGrid
                CashierPanelDataGrid.ItemsSource = formattedTransactionsList;
            }
        }


        private void CashierShowRentedEquipmentButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new GymManagementEntities())
            {
                // Wykonaj zapytanie do bazy danych, aby pobrać dzisiejsze wypożyczenia sprzętu
                var rentsList = dbContext.WypozyczeniaSprzetu
                    .Where(t => t.data == DateTime.Today) // Warunek: data wypożyczenia dzisiaj
                    .Select(t => new
                    {
                        Sprzet = t.Sprzety.nazwa,
                        Klient = t.Klienci.nazwisko + " " + t.Klienci.imie,
                        Data_Wypozyczenia = t.data,
                    })
                    .ToList();

                var formattedRentsList = rentsList.Select(rent => new
                {
                    Sprzet = rent.Sprzet,
                    Klient = rent.Klient,
                    Data_Wypozyczenia = rent.Data_Wypozyczenia.ToString("yyyy-MM-dd"),
                }).ToList();

                // Przypisz listę dzisiejszych wypożyczeń jako źródło danych dla DataGrid
                CashierPanelDataGrid.ItemsSource = formattedRentsList;
            }
        }

        private void CashierShowOldTransactionsButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new GymManagementEntities())
            {
                // Wykonaj zapytanie do bazy danych, aby pobrać transakcje z dzisiaj
                var transactionsList = dbContext.Transakcje
                    .Where(t => t.data < DateTime.Today)
                    .Select(t => new
                    {
                        Produkt = t.Produkty.nazwa,
                        Klient = t.Klienci.nazwisko + " " + t.Klienci.imie,
                        Data_Transakcji = t.data,
                        Cena = t.cena
                    })
                    .ToList();

                var formattedTransactionsList = transactionsList.Select(transaction => new
                {
                    Produkt = transaction.Produkt,
                    Klient = transaction.Klient,
                    Data_Transakcji = transaction.Data_Transakcji.ToString("yyyy-MM-dd"),
                    Cena = transaction.Cena
                }).ToList();

                // Przypisz listę transakcji jako źródło danych dla DataGrid
                CashierPanelDataGrid.ItemsSource = formattedTransactionsList;
            }
        }


        private void CashierShowOldRentedEquipmentButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new GymManagementEntities())
            {
                // Wykonaj zapytanie do bazy danych, aby pobrać dzisiejsze wypożyczenia sprzętu
                var rentsList = dbContext.WypozyczeniaSprzetu
                    .Where(t => t.data < DateTime.Today) 
                    .Select(t => new
                    {
                        Sprzet = t.Sprzety.nazwa,
                        Klient = t.Klienci.nazwisko + " " + t.Klienci.imie,
                        Data_Wypozyczenia = t.data,
                    })
                    .ToList();

                var formattedRentsList = rentsList.Select(rent => new
                {
                    Sprzet = rent.Sprzet,
                    Klient = rent.Klient,
                    Data_Wypozyczenia = rent.Data_Wypozyczenia.ToString("yyyy-MM-dd"),
                }).ToList();

                // Przypisz listę dzisiejszych wypożyczeń jako źródło danych dla DataGrid
                CashierPanelDataGrid.ItemsSource = formattedRentsList;
            }
        }

        private void CashierShowOldMembershipsButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new GymManagementEntities())
            {
                // Wykonaj zapytanie do bazy danych, aby pobrać karnety z datą ważności dzisiaj lub w przyszłości
                var membershipsList = dbContext.Karnety
                    .Where(t => t.dataWaznosci < DateTime.Today) // Warunek: data ważności dzisiaj lub w przyszłości
                    .Select(t => new
                    {
                        Klient = t.Klienci.nazwisko + " " + t.Klienci.imie,
                        Data_Ważności = t.dataWaznosci,
                        Cena = t.cena
                    })
                    .ToList();

                var formattedMembershipsList = membershipsList.Select(membership => new
                {
                    Klient = membership.Klient,
                    Data_Ważności = membership.Data_Ważności.ToString("yyyy-MM-dd"),
                    Cena = membership.Cena
                }).ToList();

                // Przypisz listę karnetów jako źródło danych dla DataGrid
                CashierPanelDataGrid.ItemsSource = formattedMembershipsList;
            }
        }

    }
}
