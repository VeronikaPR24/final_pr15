using final_pr15.Models;
using final_pr15.Service;
using Microsoft.EntityFrameworkCore;
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
    /// Логика взаимодействия для ProductsPage.xaml
    /// </summary>
    public partial class ProductsPage : Page
    {
        public ProductsPage()
        {
            InitializeComponent();
            if (AuthService.IsAdmin)
            {
                AdminPanel.Visibility = Visibility.Visible;
            }
            else
            {
                AdminPanel.Visibility = Visibility.Collapsed;
            }

            LoadFilters();
            UpdateList();
        }

        private void LoadFilters()
        {
            CategoryFilter.ItemsSource = DBService.Instance.Context.Categories.ToList();
            BrandFilter.ItemsSource = DBService.Instance.Context.Brands.ToList();
        }

        private void UpdateList()
        {
            var db = DBService.Instance.Context;
            var query = db.Products.Include(p => p.Category).AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchBox.Text))
                query = query.Where(p => p.Name.Contains(SearchBox.Text));

            if (CategoryFilter.SelectedItem is Category cat)
                query = query.Where(p => p.CategoryId == cat.Id);

            if (BrandFilter.SelectedItem is Brand brand)
                query = query.Where(p => p.BrandId == brand.Id);

            if (double.TryParse(PriceFrom.Text, out double min))
                query = query.Where(p => p.Price >= min);

            if (double.TryParse(PriceTo.Text, out double max))
                query = query.Where(p => p.Price <= max);

            
            bool hasMinPrice = double.TryParse(PriceFrom.Text, out min);
            if (hasMinPrice && min < 0)
            {
                MessageBox.Show("Цена 'от' не может быть отрицательной", "Ошибка", MessageBoxButton.OK);
                PriceFrom.Text = "";
                return;
            }
            if (hasMinPrice)
                query = query.Where(p => p.Price >= min);
            
            bool hasMaxPrice = double.TryParse(PriceTo.Text, out max);
            if (hasMaxPrice && max < 0)
            {
                MessageBox.Show("Цена 'до' не может быть отрицательной", "Ошибка", MessageBoxButton.OK);
                PriceTo.Text = "";
                return;
            }
            if (hasMaxPrice)
                query = query.Where(p => p.Price <= max);

            if (hasMinPrice && hasMaxPrice && min > max)
            {
                MessageBox.Show("Цена 'от' не может быть больше цены 'до'", "Ошибка", MessageBoxButton.OK);
                PriceFrom.Text = "";
                PriceTo.Text = "";
                return;
            }

            var sortTag = (SortComboBox.SelectedItem as ComboBoxItem)?.Tag?.ToString();
            if (sortTag == "PriceAsc") query = query.OrderBy(p => p.Price);
            else if (sortTag == "PriceDesc") query = query.OrderByDescending(p => p.Price);
            else if (sortTag == "Name") query = query.OrderBy(p => p.Name);

            var result = query.ToList();
            ProductsList.ItemsSource = result;
            TotalText.Text = $"Всего: {db.Products.Count()}";
            FilteredText.Text = $"Показано: {result.Count}";
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateList();
        }
        private void CategoryFilter_Changed(object sender, SelectionChangedEventArgs e)
        {
            UpdateList();
        }
        private void BrandFilter_Changed(object sender, SelectionChangedEventArgs e)
        {
            UpdateList();
        }
        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateList();
        }
        private void ApplyPriceFilter_Click(object sender, RoutedEventArgs e)
        {
            UpdateList();
        }

        private void ResetFilters_Click(object sender, RoutedEventArgs e)
        {
            SearchBox.Text = "";
            CategoryFilter.SelectedItem = null;
            BrandFilter.SelectedItem = null;
            PriceFrom.Text = "";
            PriceTo.Text = "";
            UpdateList();
        }

        private void ProductsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool isSelected = ProductsList.SelectedItem != null;
            EditProductButton.IsEnabled = isSelected;
            DeleteProductButton.IsEnabled = isSelected;
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e) =>
            NavigationService.Navigate(new ProductEditPage(null));

        private void EditSelectedProduct_Click(object sender, RoutedEventArgs e) =>
            NavigationService.Navigate(new ProductEditPage(ProductsList.SelectedItem as Product));

        private void DeleteSelectedProduct_Click(object sender, RoutedEventArgs e)
        {
            var item = ProductsList.SelectedItem as Product;
            if (item != null && MessageBox.Show("Удалить товар?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                DBService.Instance.Context.Products.Remove(item);
                DBService.Instance.Context.SaveChanges();
                UpdateList();
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void ManageCategories_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ManagerPage("categories"));
        }

        private void ManageBrands_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ManagerPage("brands"));
        }

        private void ManageTags_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ManagerPage("tags"));
        }
    }
}