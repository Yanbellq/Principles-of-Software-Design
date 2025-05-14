using System;
using System.Collections.Generic;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;

namespace lab9
{
    public partial class MainWindow : Window
    {
        private List<int> L1 = new List<int>();
        private List<int> L2 = new List<int>();
        private List<int> L3 = new List<int>();

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

        // Завантаження списку з файлу
        private List<int> LoadListFromFile(string filePath)
        {
            List<int> list = new List<int>();
            try
            {
                string content = File.ReadAllText(filePath);
                string[] numbers = content.Split(new[] { ' ', '\t', '\n', '\r', ',' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string num in numbers)
                {
                    if (int.TryParse(num, out int n))
                    {
                        list.Add(n);
                    }
                }
                UpdateStatus($"Файл {Path.GetFileName(filePath)} успішно завантажено");
            }
            catch (Exception ex)
            {
                UpdateStatus($"Помилка при читанні файлу {Path.GetFileName(filePath)}: {ex.Message}", true);
            }
            return list;
        }

        // Збереження списку у файл (по 7 елементів у рядку)
        private void SaveListToFile(List<int> list, string filePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        writer.Write(list[i]);
                        if (i < list.Count - 1)
                        {
                            writer.Write(" ");
                            if ((i + 1) % 7 == 0) writer.WriteLine();
                        }
                    }
                }
                UpdateStatus($"Список успішно збережено у файл: {Path.GetFileName(filePath)}");
            }
            catch (Exception ex)
            {
                UpdateStatus($"Помилка при збереженні файлу: {ex.Message}", true);
            }
        }

        // Пошук підсписку у списку
        private int FindSubListIndex(List<int> mainList, List<int> subList)
        {
            if (subList.Count == 0 || subList.Count > mainList.Count)
                return -1;

            for (int i = 0; i <= mainList.Count - subList.Count; i++)
            {
                bool found = true;
                for (int j = 0; j < subList.Count; j++)
                {
                    if (mainList[i + j] != subList[j])
                    {
                        found = false;
                        break;
                    }
                }
                if (found) return i;
            }
            return -1;
        }

        // Заміна підсписку
        private void ReplaceSubList(List<int> mainList, List<int> oldSubList, List<int> newSubList)
        {
            int index = FindSubListIndex(mainList, oldSubList);
            if (index != -1)
            {
                mainList.RemoveRange(index, oldSubList.Count);
                mainList.InsertRange(index, newSubList);
                UpdateStatus("Заміна підсписку виконана успішно");
            }
            else
            {
                UpdateStatus("Підсписок L2 не знайдено в L1", true);
            }
        }

        // Оновлення інтерфейсу
        private void UpdateUI()
        {
            if (this.FindControl<ListBox>("listBoxL1") is { } listBoxL1)
                listBoxL1.ItemsSource = L1;

            if (this.FindControl<ListBox>("listBoxL2") is { } listBoxL2)
                listBoxL2.ItemsSource = L2;

            if (this.FindControl<ListBox>("listBoxL3") is { } listBoxL3)
                listBoxL3.ItemsSource = L3;

            if (this.FindControl<ListBox>("listBoxResult") is { } listBoxResult)
                listBoxResult.ItemsSource = L1;
        }

        // Оновлення статусу
        private void UpdateStatus(string message, bool isError = false)
        {
            if (this.FindControl<TextBlock>("statusText") is { } statusText)
            {
                statusText.Text = message;
                statusText.Foreground = isError ? Avalonia.Media.Brushes.Red : Avalonia.Media.Brushes.Green;
            }
        }

        // Обробка кнопки завантаження L1
        private async void LoadL1_Click(object sender, RoutedEventArgs e)
        {
            var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Виберіть файл для списку L1",
                AllowMultiple = false,
                FileTypeFilter = new[] { FilePickerFileTypes.TextPlain }
            });

            if (files.Count > 0 && files[0].TryGetLocalPath() is string filePath)
            {
                L1 = LoadListFromFile(filePath);
                if (this.FindControl<TextBox>("txtFileL1") is { } txtFileL1)
                    txtFileL1.Text = filePath;
                UpdateUI();
            }
        }

        // Обробка кнопки завантаження L2
        private async void LoadL2_Click(object sender, RoutedEventArgs e)
        {
            var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Виберіть файл для списку L2",
                AllowMultiple = false,
                FileTypeFilter = new[] { FilePickerFileTypes.TextPlain }
            });

            if (files.Count > 0 && files[0].TryGetLocalPath() is string filePath)
            {
                L2 = LoadListFromFile(filePath);
                if (this.FindControl<TextBox>("txtFileL2") is { } txtFileL2)
                    txtFileL2.Text = filePath;
                UpdateUI();
            }
        }

        // Обробка кнопки завантаження L3
        private async void LoadL3_Click(object sender, RoutedEventArgs e)
        {
            var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Виберіть файл для списку L3",
                AllowMultiple = false,
                FileTypeFilter = new[] { FilePickerFileTypes.TextPlain }
            });

            if (files.Count > 0 && files[0].TryGetLocalPath() is string filePath)
            {
                L3 = LoadListFromFile(filePath);
                if (this.FindControl<TextBox>("txtFileL3") is { } txtFileL3)
                    txtFileL3.Text = filePath;
                UpdateUI();
            }
        }

        // Обробка кнопки виконання
        private void Process_Click(object sender, RoutedEventArgs e)
        {
            if (L1.Count == 0 || L2.Count == 0 || L3.Count == 0)
            {
                UpdateStatus("Будь ласка, завантажте всі три списки", true);
                return;
            }

            ReplaceSubList(L1, L2, L3);
            UpdateUI();
        }

        // Обробка кнопки збереження
        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (L1.Count == 0)
            {
                UpdateStatus("Немає даних для збереження", true);
                return;
            }

            var file = await StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = "Зберегти результат",
                FileTypeChoices = new[] { FilePickerFileTypes.TextPlain }
            });

            if (file != null && file.TryGetLocalPath() is string filePath)
            {
                SaveListToFile(L1, filePath);
            }
        }
    }
}