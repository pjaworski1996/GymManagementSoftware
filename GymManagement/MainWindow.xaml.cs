using System.Linq;
using System.Windows;

namespace GymManagement
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            // Wywołujemy metodę AuthenticateUser, aby sprawdzić poprawność danych logowania
            bool isAuthenticated = AuthenticateUser(username, password);

            if (isAuthenticated)
            {
                string userRole = GetUserRole(username);
                string message;

                switch (userRole)
                {
                    case "Klient":
                        message = "Witaj Kliencie! Zostałeś pomyślnie zalogowany.";
                        ClientPanel NewClientWindow = new ClientPanel(username);
                        NewClientWindow.Show();
                        Close();
                        break;
                    case "Trener":
                        message = "Witaj Trenerze! Zostałeś pomyślnie zalogowany.";
                        TrainerPanel NewTrainerWindow = new TrainerPanel(username);
                        NewTrainerWindow.Show();
                        Close();
                        break;
                    case "Dietetyk":
                        message = "Witaj Dietetyku! Zostałeś pomyślnie zalogowany.";
                        DieticianPanel NewDieticianWindow = new DieticianPanel(username);
                        NewDieticianWindow.Show();
                        Close();
                        break;
                    case "Kasjer":
                        message = "Witaj Kasjerze! Zostałeś pomyślnie zalogowany.";
                        CashierPanel NewCashierWindow = new CashierPanel(username);
                        NewCashierWindow.Show();
                        Close();
                        break;
                    case "Administrator":
                        message = "Witaj Administratorze! Zostałeś pomyślnie zalogowany.";
                        AdminPanel NewAdminWindow = new AdminPanel(username);
                        NewAdminWindow.Show();
                        Close();
                        break;
                    default:
                        message = "Nie masz wystarczających uprawnień do zalogowania.";
                        break;
                }

                MessageBox.Show(message, "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Błędny login, hasło lub konto jest nieaktywne. Spróbuj ponownie.", "Błąd logowania", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool AuthenticateUser(string login, string password)
        {
            using (var dbContext = new GymManagementEntities())
            {
                // Sprawdzamy, czy istnieje użytkownik o podanym loginie i haśle oraz czy jego status jest aktywny
                var user = dbContext.Uzytkownicy.FirstOrDefault(u => u.login == login && u.haslo == password && u.status == "Aktywny");

                // Jeśli użytkownik został znaleziony, zwracamy true, w przeciwnym razie false
                return user != null;
            }
        }

        public string GetUserRole(string login)
        {
            using (var dbContext = new GymManagementEntities())
            {
                // Pobieramy rolę użytkownika na podstawie loginu
                var user = dbContext.Uzytkownicy.First(u => u.login == login);

                // Jeśli użytkownik został znaleziony, zwracamy jego rolę, w przeciwnym razie null
                return user.uprawnienia;
            }
        }



        private void Register_Click(object sender, RoutedEventArgs e)
        {
            RegisterPanel NewRegisterWindow = new RegisterPanel();
            NewRegisterWindow.Show();
            Close();
        }

        private void MainWindowExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}