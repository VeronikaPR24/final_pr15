using final_pr15.Models;
using final_pr15.Service;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace final_pr15.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProductEditPage.xaml
    /// </summary>
    public partial class ProductEditPage : Page
    {
        private Product _item;

        public ProductEditPage(Product selected)
        {
            InitializeComponent();
            CategoryBox.ItemsSource = DBService.Instance.Context.Categories.ToList();
            BrandBox.ItemsSource = DBService.Instance.Context.Brands.ToList();
            TagsBox.ItemsSource = DBService.Instance.Context.Tags.ToList();
            _item = selected ?? new Product();
            DataContext = _item;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameBox.Text))
            {
                MessageBox.Show("Укажите название товара", "Ошибка", MessageBoxButton.OK);
                NameBox.Focus();
                return;
            }
            _item.Name = NameBox.Text;

            if (string.IsNullOrWhiteSpace(DescriptionBox.Text))
            {
                MessageBox.Show("Укажите описание товара", "Ошибка", MessageBoxButton.OK);
                DescriptionBox.Focus();
                return;
            }
            _item.Description = DescriptionBox.Text;

            if (string.IsNullOrWhiteSpace(PriceBox.Text))
            {
                MessageBox.Show("Укажите цену товара", "Ошибка", MessageBoxButton.OK);
                PriceBox.Focus();
                return;
            }
            if (!double.TryParse(PriceBox.Text, out double price) || price <= 0)
            {
                MessageBox.Show("Цена должна быть положительным числом", "Ошибка", MessageBoxButton.OK);
                PriceBox.Focus();
                return;
            }
            _item.Price = price;

            if (string.IsNullOrWhiteSpace(StockBox.Text))
            {
                MessageBox.Show("Укажите количество товара на складе", "Ошибка", MessageBoxButton.OK);
                StockBox.Focus();
                return;
            }
            if (!int.TryParse(StockBox.Text, out int stock) || stock < 0)
            {
                MessageBox.Show("Количество должно быть целым неотрицательным числом", "Ошибка", MessageBoxButton.OK);
                StockBox.Focus();
                return;
            }
            _item.Stock = stock;

            if (CategoryBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите категорию товара", "Ошибка", MessageBoxButton.OK);
                CategoryBox.Focus();
                return;
            }
            _item.Category = CategoryBox.SelectedItem as Category;
            _item.CategoryId = (_item.Category?.Id) ?? 0;

            if (BrandBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите бренд товара", "Ошибка", MessageBoxButton.OK);
                BrandBox.Focus();
                return;
            }
            _item.Brand = BrandBox.SelectedItem as Brand;
            _item.BrandId = (_item.Brand?.Id) ?? 0;

            if (string.IsNullOrWhiteSpace(RatingBox.Text))
            {
                MessageBox.Show("Укажите рейтинг товара", "Ошибка", MessageBoxButton.OK);
                RatingBox.Focus();
                return;
            }
            if (!double.TryParse(RatingBox.Text, out double rating) || rating < 0 || rating > 5)
            {
                MessageBox.Show("Рейтинг должен быть числом от 0 до 5", "Ошибка", MessageBoxButton.OK);
                RatingBox.Focus();
                return;
            }
            _item.Rating = rating;

            if (TagsBox.SelectedItem != null)
            {
                var selectedTag = TagsBox.SelectedItem as Tag;
                if (selectedTag != null)
                {
                    var existingProductTag = DBService.Instance.Context.ProductTags
                        .FirstOrDefault(pt => pt.ProductId == _item.Id && pt.TagId == selectedTag.Id);

                    if (existingProductTag == null && _item.Id != 0)
                    {
                        DBService.Instance.Context.ProductTags.Add(new ProductTag
                        {
                            ProductId = _item.Id,
                            TagId = selectedTag.Id
                        });
                    }
                }
            }

            var db = DBService.Instance.Context;
            if (_item.Id == 0)
                db.Products.Add(_item);

            db.SaveChanges();
            MessageBox.Show("Товар успешно сохранен!", "Успех", MessageBoxButton.OK);
            NavigationService.GoBack();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
