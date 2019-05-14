using ObservableCollectionLesson.DataAcces;
using ObservableCollectionLesson.Models;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Windows;
using System.Windows.Documents;

namespace ObservableCollectionLesson
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Item> items;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Setloading(true);

            using (var context = new ShopContext())
            {
                items = new ObservableCollection<Item> ( await context.Items.ToListAsync());
                itemsDataGrid.ItemsSource = items;
            }
       
            Setloading(false);
        }

        private void Setloading(bool isLoading)
        {
            if (isLoading)
            {
                progressBar.Visibility = Visibility.Visible;
                statusTextBlock.Text = "Происходит загрузка. Пожалуйста подождите ....";
            }
            else
            {
                progressBar.Visibility = Visibility.Collapsed;
                statusTextBlock.Text = "Готово";
            }
        }

        private async void AddButtonClick(object sender, RoutedEventArgs e)
        {
            Setloading(true);
            var item = new Item
            {
                Name = nameTextBox.Text,
                Price = int.Parse(priceTextBox.Text),
                Description = new TextRange(descriptionRichTextBox.Document.ContentStart, descriptionRichTextBox.Document.ContentEnd).Text
            };

            using (var context = new ShopContext())
            {
                context.Items.Add(item);
                await context.SaveChangesAsync();

                items.Add(item);
            }

            Setloading(false);
        }

        private void ChangeFirstButtonClick(object sender, RoutedEventArgs e)
        {
            var item = items[0];
            item.Name = "Супер товар";
        }
    }
}
