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
    /// Interaction logic for ChangeUserWindow.xaml
    /// </summary>
    public partial class ChangeUserWindow : Window
    {
        public ChangeUserWindow(int idUzytkownika)
        {
            InitializeComponent();
            idUzytkownik = idUzytkownika;
            Loaded += ChangeUserWindow_Loaded; // Dodanie obsługi zdarzenia Loaded
        }

        // Metoda obsługująca zdarzenie Loaded
        private void ChangeUserWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Pobierz dane użytkownika na podstawie jego identyfikatora
            using (var dbContext = new GymManagementEntities())
            {
                var existingUser = dbContext.Uzytkownicy.FirstOrDefault(t => t.idUzytkownika == idUzytkownik);

                // Jeśli użytkownik istnieje, ustaw odpowiednie wartości w ComboBoxach
                if (existingUser != null)
                {
                    ChangeUserRoleComboBox.Text = existingUser.uprawnienia;
                    ChangeUserStatusComboBox.Text = existingUser.status;
                }
            }
        }

        public int idUzytkownik { get; set; }

        private void ChangeUserBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ChangeUserChangeButton_Click(object sender, RoutedEventArgs e)
        {
            string role = ChangeUserRoleComboBox.Text;
            string status = ChangeUserStatusComboBox.Text;

            using (var dbContext = new GymManagementEntities())
            {
                var existingUser = dbContext.Uzytkownicy.FirstOrDefault(t => t.idUzytkownika == idUzytkownik);

                if (existingUser != null)
                {
                    existingUser.uprawnienia = role;
                    existingUser.status = status;

                    dbContext.SaveChanges();

                    MessageBox.Show("Użytkownik został pomyślnie zaktualizowany!", "Sukces",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Nie można odnaleźć użytkownika.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
