using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
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
    public partial class RegisterPanel : Window
    {
        public RegisterPanel()
        {
            InitializeComponent();
        }

        private void AllowOnlyNumbers(object sender, TextCompositionEventArgs e)
        {
            foreach (var ch in e.Text)
            {
                if (!char.IsDigit(ch))
                {
                    e.Handled = true; // Jeśli znak nie jest cyfrą, odrzuć go
                    return;
                }
            }
        }


        private void SecretTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SecretTextBox.Text == "123456")
            {
                RoleComboBox.SelectedIndex = 1;
            }
            else if (SecretTextBox.Text == "234567")
            {
                RoleComboBox.SelectedIndex = 2;
            }
            else if (SecretTextBox.Text == "345678")
            {
                RoleComboBox.SelectedIndex = 3;
            }
            else
            {
                RoleComboBox.SelectedIndex = 0;
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            // Sprawdź, czy wszystkie obowiązkowe pola są wypełnione
            if (string.IsNullOrWhiteSpace(UsernameTextBox.Text) ||
                string.IsNullOrWhiteSpace(PasswordTextBox.Password) ||
                string.IsNullOrWhiteSpace(LastNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(TelephoneTextBox.Text) ||
                RoleComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Proszę wypełnić wszystkie obowiązkowe pola oznaczone gwiazdką (*)!", "Błąd rejestracji",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Pobierz wartości wprowadzone przez użytkownika
            string login = UsernameTextBox.Text;
            string haslo = PasswordTextBox.Password;
            string imieTabela = FirstNameTextBox.Text; // Może być puste
            string nazwiskoTabela = LastNameTextBox.Text;
            string imie = FirstNameTextBox.Text; // Może być puste
            string nazwisko = LastNameTextBox.Text;
            string telefon = TelephoneTextBox.Text;
            string uprawnienia = ((ComboBoxItem)RoleComboBox.SelectedItem).Content.ToString();
            string status = "Aktywny"; // Domyślnie ustawiamy status na "Aktywny"
            string kodDostepu = SecretTextBox.Text; // Może być puste

            // Sprawdź, czy numer telefonu ma dokładnie 9 cyfr
            if (TelephoneTextBox.Text.Length != 9)
            {
                MessageBox.Show("Numer telefonu musi składać się z dokładnie 9 cyfr!", "Błąd rejestracji",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Zweryfikuj, czy login jest unikalny
            if (!IsLoginUnique(login))
            {
                MessageBox.Show("Podany login jest już zajęty, proszę wybrać inny login!", "Błąd rejestracji", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Utwórz nowy obiekt Uzytkownik na podstawie danych wprowadzonych przez użytkownika
            var nowyUzytkownik = new Uzytkownicy
            {
                login = login,
                haslo = haslo,
                uprawnienia = uprawnienia,
                status = status,
                imie = imie,
                nazwisko = nazwisko
            };

            // Dodaj nowego użytkownika do bazy danych za pomocą kontekstu
            using (var dbContext = new GymManagementEntities())
            {
                dbContext.Uzytkownicy.Add(nowyUzytkownik);
                dbContext.SaveChanges();
            }

            // Dodaj rekord do odpowiedniej tabeli w zależności od wybranej roli
            switch (uprawnienia)
            {
                case "Klient":
                    DodajKlienta(login, imieTabela, nazwiskoTabela, telefon);
                    break;
                case "Trener":
                    DodajTrenera(login, imieTabela, nazwiskoTabela, telefon);
                    break;
                case "Dietetyk":
                    DodajDietetyka(login, imieTabela, nazwiskoTabela, telefon);
                    break;
                case "Kasjer":
                    DodajKasjera(login, imieTabela, nazwiskoTabela, telefon);
                    break;
            }

            MessageBox.Show("Rejestracja zakończona pomyślnie!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);

            // Wyczyść pola po udanej rejestracji
            ClearFields();
        }

        // Metoda sprawdzająca, czy login jest unikalny
        private bool IsLoginUnique(string login)
        {
            // Utwórz instancję kontekstu bazy danych
            using (var dbContext = new GymManagementEntities())
            {
                // Sprawdź, czy istnieje użytkownik o podanym loginie
                return !dbContext.Uzytkownicy.Any(u => u.login == login);
            }
        }

        // Metoda do dodawania klienta do tabeli Klienci
        private void DodajKlienta(string login, string imie, string nazwisko, string telefon)
        {
            // Tworzymy nowego klienta
            var klient = new Klienci
            {
                imie = imie,
                nazwisko = nazwisko,
                telefon = telefon
            };

            // Dodajemy klienta do kontekstu i zapisujemy zmiany
            using (var dbContext = new GymManagementEntities())
            {
                dbContext.Klienci.Add(klient);
                dbContext.SaveChanges();
            }
        }

        // Metoda do dodawania trenera do tabeli Trenerzy
        private void DodajTrenera(string login, string imie, string nazwisko, string telefon)
        {
            // Tworzymy nowego trenera
            var trener = new Trenerzy
            {
                imie = imie,
                nazwisko = nazwisko,
                telefon = telefon
            };

            // Dodajemy trenera do kontekstu i zapisujemy zmiany
            using (var dbContext = new GymManagementEntities())
            {
                dbContext.Trenerzy.Add(trener);
                dbContext.SaveChanges();
            }
        }

        // Metoda do dodawania dietetyka do tabeli Dietetycy
        private void DodajDietetyka(string login, string imie, string nazwisko, string telefon)
        {
            // Tworzymy nowego dietetyka
            var dietetyk = new Dietetycy
            {
                imie = imie,
                nazwisko = nazwisko,
                telefon = telefon
            };

            // Dodajemy dietetyka do kontekstu i zapisujemy zmiany
            using (var dbContext = new GymManagementEntities())
            {
                dbContext.Dietetycy.Add(dietetyk);
                dbContext.SaveChanges();
            }
        }

        // Metoda do dodawania kasjera do tabeli PracownicyKasowi
        private void DodajKasjera(string login, string imie, string nazwisko, string telefon)
        {
            // Tworzymy nowego kasjera
            var kasjer = new PracownicyKasowi
            {
                imie = imie,
                nazwisko = nazwisko,
                telefon = telefon
            };

            // Dodajemy kasjera do kontekstu i zapisujemy zmiany
            using (var dbContext = new GymManagementEntities())
            {
                dbContext.PracownicyKasowi.Add(kasjer);
                dbContext.SaveChanges();
            }
        }

        // Metoda do wyczyszczenia pól po udanej rejestracji
        private void ClearFields()
        {
            UsernameTextBox.Text = "";
            PasswordTextBox.Password = "";
            FirstNameTextBox.Text = "";
            LastNameTextBox.Text = "";
            TelephoneTextBox.Text = "";
            SecretTextBox.Text = "";
            RoleComboBox.SelectedIndex = -1;
        }



    }
}