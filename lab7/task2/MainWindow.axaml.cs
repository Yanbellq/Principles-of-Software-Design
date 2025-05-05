using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;

namespace lab7
{
    public partial class MainWindow : Window
    {
        public List<SKLAD> SHOP { get; set; } = new List<SKLAD>();

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

        private void Button1_Click(object? sender, RoutedEventArgs e)
        {
            var textBox1 = this.FindControl<TextBox>("textBox1");
            var textBox2 = this.FindControl<TextBox>("textBox2");
            var textBox3 = this.FindControl<TextBox>("textBox3");
            var textBox4 = this.FindControl<TextBox>("textBox4");
            var errorText = this.FindControl<TextBlock>("errorText");

            if (textBox1 == null || textBox2 == null || textBox3 == null || textBox4 == null || errorText == null)
                return;

            try
            {
                SKLAD item = new SKLAD
                {
                    NAME = textBox1.Text ?? string.Empty,
                    TYPE = textBox2.Text ?? string.Empty,
                    QUANTITY = int.Parse(textBox3.Text ?? "0"),
                    COST = decimal.Parse(textBox4.Text ?? "0")
                };

                if (string.IsNullOrWhiteSpace(item.NAME))
                {
                    errorText.Text = "Назва товару не може бути порожньою";
                    return;
                }

                SHOP.Add(item);
                UpdateList();
                ClearInputs();
                errorText.Text = string.Empty;
            }
            catch (FormatException)
            {
                errorText.Text = "Невірний формат числа для кількості або ціни";
            }
            catch
            {
                errorText.Text = "Невірні вхідні дані!";
            }
        }

        private void UpdateList()
        {
            var listBox1 = this.FindControl<ListBox>("listBox1");
            if (listBox1 != null)
            {
                listBox1.Items.Clear();
                foreach (var item in SHOP.Select(s => $"{s.NAME} ({s.TYPE}) - {s.QUANTITY} * {s.COST:C}"))
                {
                    listBox1.Items.Add(item);
                }
            }
        }

        private void ClearInputs()
        {
            var textBox1 = this.FindControl<TextBox>("textBox1");
            var textBox2 = this.FindControl<TextBox>("textBox2");
            var textBox3 = this.FindControl<TextBox>("textBox3");
            var textBox4 = this.FindControl<TextBox>("textBox4");

            textBox1!.Text = string.Empty;
            textBox2!.Text = string.Empty;
            textBox3!.Text = string.Empty;
            textBox4!.Text = string.Empty;
        }

        private void Button2_Click(object? sender, RoutedEventArgs e)
        {
            SHOP = SHOP.OrderBy(s => s.NAME).ToList();
            UpdateList();
        }

        private void Button3_Click(object? sender, RoutedEventArgs e)
        {
            var textBox5 = this.FindControl<TextBox>("textBox5");
            var label1 = this.FindControl<TextBlock>("label1");
            var errorText = this.FindControl<TextBlock>("errorText");

            if (textBox5 == null || label1 == null || errorText == null)
                return;

            string searchName = textBox5.Text ?? string.Empty;
            var item = SHOP.FirstOrDefault(s => s.NAME.Equals(searchName, StringComparison.OrdinalIgnoreCase));

            if (item != null)
            {
                label1.Text = $"Товар: {item.NAME}, К-сть: {item.QUANTITY}, Ціна: {item.COST:C}, Сума: {item.TOTAL:C}";
                errorText.Text = string.Empty;
            }
            else
            {
                label1.Text = "Товар не знайдено.";
                errorText.Text = "Товар не знайдено.";
            }
        }
    }

    public class SKLAD
    {
        public string NAME { get; set; } = string.Empty;
        public string TYPE { get; set; } = string.Empty;
        public int QUANTITY { get; set; }
        public decimal COST { get; set; }
        public decimal TOTAL => QUANTITY * COST;
    }
}