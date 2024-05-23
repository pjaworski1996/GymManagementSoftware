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
    /// Interaction logic for ChangeProductWindow.xaml
    /// </summary>
    public partial class ChangeProductWindow : Window
    {
        public ChangeProductWindow(int idProduktu)
        {
            InitializeComponent();
            idProdukt = idProduktu;
            Loaded += ChangeProductWindow_Loaded;
        }
        public int idProdukt { get; set; }

        private void ChangeProductBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ChangeProductWindow_Loaded(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new GymManagementEntities())
            {
                var existingProduct = dbContext.Produkty.FirstOrDefault(t => t.idProdukt == idProdukt);

                if (existingProduct != null)
                {
                    ChangeProductNameTextBox.Text = existingProduct.nazwa;
                    ChangeProductPriceTextBox.Text = existingProduct.cena.ToString();
                    ChangeProductQuantityTextBox.Text = existingProduct.ilosc.ToString();
                }
            }
        }

        private void ChangeProductChangeButton_Click(object sender, RoutedEventArgs e)
        {
            // Sprawdź, czy wszystkie obowiązkowe pola są wypełnione
            if (string.IsNullOrWhiteSpace(ChangeProductNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(ChangeProductQuantityTextBox.Text))
            {
                MessageBox.Show("Proszę wypełnić wszystkie obowiązkowe pola oznaczone gwiazdką (*)!", "Błąd modyfikowania produktu",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Pobierz wartości wprowadzone przez użytkownika
            string productName = ChangeProductNameTextBox.Text;
            string productQuantityStr = ChangeProductQuantityTextBox.Text;
            string productPriceStr = ChangeProductPriceTextBox.Text;

            int? productQuantity = null; // Użyj typu nullable int
            int? productPrice = null; // Użyj typu nullable int

            // Sprawdź, czy ilość produktu jest w formacie poprawnym
            if (!string.IsNullOrWhiteSpace(productQuantityStr))
            {
                if (!int.TryParse(productQuantityStr, out int parsedProductQuantity) || parsedProductQuantity < 0)
                {
                    MessageBox.Show("Nieprawidłowy format ilości produktu. Podaj liczbę całkowitą nieujemną!", "Błąd modyfikowania produktu",
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
                    MessageBox.Show("Nieprawidłowy format ceny produktu. Podaj liczbę całkowitą większą od zera!", "Błąd modyfikowania produktu",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                productPrice = parsedProductPrice; // Przypisz wartość tylko jeśli cena jest poprawna
            }

            // Znajdź produkt do zmodyfikowania w bazie danych
            using (var dbContext = new GymManagementEntities())
            {
                var existingProduct = dbContext.Produkty.FirstOrDefault(p => p.idProdukt == idProdukt);

                // Zaktualizuj dane produktu
                existingProduct.nazwa = productName;
                existingProduct.ilosc = productQuantity ?? 0;
                existingProduct.cena = productPrice;

                // Zapisz zmiany w bazie danych
                dbContext.SaveChanges();
            }

            MessageBox.Show("Zmodyfikowano produkt!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
