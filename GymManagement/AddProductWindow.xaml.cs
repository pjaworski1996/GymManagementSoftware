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
    /// Interaction logic for AddProductWindow.xaml
    /// </summary>
    public partial class AddProductWindow : Window
    {
        public AddProductWindow()
        {
            InitializeComponent();
        }

        private void AddProductBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddProductAddButton_Click(object sender, RoutedEventArgs e)
        {
            // Sprawdź, czy wszystkie obowiązkowe pola są wypełnione
            if (string.IsNullOrWhiteSpace(AddProductNameTextBox.Text) || string.IsNullOrWhiteSpace(AddProductQuantityTextBox.Text))
            {
                MessageBox.Show("Proszę wypełnić wszystkie obowiązkowe pola oznaczone gwiazdką (*)!", "Błąd dodawania produktu",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Pobierz wartości wprowadzone przez użytkownika
            string productName = AddProductNameTextBox.Text;
            string productQuantityStr = AddProductQuantityTextBox.Text;
            string productPriceStr = AddProductPriceTextBox.Text;

            int? productQuantity = null; // Użyj typu nullable int
            int? productPrice = null; // Użyj typu nullable int

            // Sprawdź, czy ilość produktu jest w formacie poprawnym
            if (!string.IsNullOrWhiteSpace(productQuantityStr))
            {
                if (!int.TryParse(productQuantityStr, out int parsedProductQuantity) || parsedProductQuantity < 0)
                {
                    MessageBox.Show("Nieprawidłowy format ilości produktu. Podaj liczbę całkowitą nieujemną!", "Błąd dodawania produktu",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                productQuantity = parsedProductQuantity; // Przypisz wartość tylko jeśli ilość jest poprawna
            }

            // Sprawdź, czy cena produktu jest w formacie poprawnym
            if (!string.IsNullOrWhiteSpace(productPriceStr))
            {
                if (!int.TryParse(productPriceStr, out int parsedProductPrice) || parsedProductPrice <= 0)
                {
                    MessageBox.Show("Nieprawidłowy format ceny produktu. Podaj liczbę całkowitą większą od zera!", "Błąd dodawania produktu",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                productPrice = parsedProductPrice; // Przypisz wartość tylko jeśli cena jest poprawna
            }

            // Utwórz nowy obiekt Produkt na podstawie danych wprowadzonych przez użytkownika
            var newProduct = new Produkty
            {
                nazwa = productName,
                ilosc = productQuantity ?? 0,
                cena = productPrice
            };

            // Dodaj nowy produkt do bazy danych za pomocą kontekstu
            using (var dbContext = new GymManagementEntities())
            {
                dbContext.Produkty.Add(newProduct);
                dbContext.SaveChanges();
            }

            MessageBox.Show("Dodano nowy produkt!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);

            // Wyczyść pola po udanym dodaniu produktu
            ClearFields();
        }

        private void ClearFields()
        {
            // Wyczyść zawartość pól tekstowych
            AddProductNameTextBox.Text = "";
            AddProductPriceTextBox.Text = "";
            AddProductQuantityTextBox.Text = "";
        }
    }
}
