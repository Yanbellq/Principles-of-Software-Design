using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;

namespace lab8
{
    public partial class MainWindow : Window
    {
        private ProductManager manager = new ProductManager();

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            string name = this.FindControl<TextBox>("textBox1")?.Text ?? string.Empty;
            decimal price = this.FindControl<NumericUpDown>("numericUpDown1")?.Value ?? 0;
            string category = this.FindControl<TextBox>("textBox2")?.Text ?? string.Empty;
            bool inStock = this.FindControl<CheckBox>("checkBox1")?.IsChecked ?? false;

            manager.AddProduct(name, price, category, inStock);

            // Show message in ListBox instead of MessageBox
            var listBox = this.FindControl<ListBox>("listBox1");
            listBox?.Items.Add("Продукт додано!");

            ClearInputs();
        }

        private void ClearInputs()
        {
            this.FindControl<TextBox>("textBox1")!.Text = string.Empty;
            this.FindControl<TextBox>("textBox2")!.Text = string.Empty;
            this.FindControl<NumericUpDown>("numericUpDown1")!.Value = 0;
            this.FindControl<CheckBox>("checkBox1")!.IsChecked = false;
        }

        private void ShowAllProducts_Click(object sender, RoutedEventArgs e)
        {
            var listBox = this.FindControl<ListBox>("listBox1");
            if (listBox != null)
            {
                listBox.Items.Clear();
                foreach (var p in manager.GetAllProducts())
                {
                    string InStockText = p.Item4 ? "Так" : "Ні";
                    listBox.Items.Add($"Назва: {p.Item1}, Ціна: {p.Item2}, Категорія: {p.Item3}, В наявності: {InStockText}");
                }
            }
        }

        private void ShowStats_Click(object sender, RoutedEventArgs e)
        {
            var listBox = this.FindControl<ListBox>("listBox1");
            if (listBox != null)
            {
                listBox.Items.Clear();
                decimal avg = manager.GetAveragePrice();
                listBox.Items.Add($"Середня ціна: {avg:F2}");

                var socials = manager.GetSocialProducts();
                foreach (var p in socials)
                {
                    listBox.Items.Add($"Назва: {p.Item1}, Ціна: {p.Item2}");
                }

                listBox.Items.Add($"Кількість соціальних продуктів: {socials.Count}");
            }
        }
    }

    public class ProductManager
    {
        private List<(string Name, decimal Price, string Category, bool InStock)> products
           = new List<(string Name, decimal Price, string Category, bool InStock)>();

        public void AddProduct(string name, decimal price, string category, bool inStock)
        {
            products.Add((name, price, category, inStock));
        }

        public List<(string, decimal, string, bool)> GetAllProducts()
        {
            return products;
        }

        public (string, decimal, string, bool)? GetProduct(int index)
        {
            if (index >= 0 && index < products.Count)
                return products[index];
            return null;
        }

        public decimal GetAveragePrice()
        {
            if (products.Count == 0) return 0;
            return products.Average(p => p.Price);
        }

        public List<(string, decimal, string, bool)> GetSocialProducts()
        {
            decimal avg = GetAveragePrice();
            return products.Where(p => p.Price < avg).ToList();
        }
    }
}