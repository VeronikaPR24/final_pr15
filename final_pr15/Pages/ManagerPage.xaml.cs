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
    /// Логика взаимодействия для ManagerPage.xaml
    /// </summary>
    public partial class ManagerPage : Page
    {
        private string _currentType;

        public ManagerPage()
        {
            InitializeComponent();
            _currentType = "categories";
            LoadData();
        }

        public ManagerPage(string type)
        {
            InitializeComponent();
            _currentType = type;
            LoadData();
        }

        private void LoadData()
        {
            var db = DBService.Instance.Context;
            
            switch (_currentType)
            {
                case "categories":
                    ItemsList.ItemsSource = db.Categories.ToList();
                    break;
                    
                case "brands":
                    ItemsList.ItemsSource = db.Brands.ToList();
                    break;
                    
                case "tags":
                    ItemsList.ItemsSource = db.Tags.ToList();
                    break;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddPanel.Visibility = Visibility.Visible;
        }

        private void CancelAdd_Click(object sender, RoutedEventArgs e)
        {
            AddPanel.Visibility = Visibility.Collapsed;
        }

        private void SaveAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(NewNameBox.Text))
            {
                var db = DBService.Instance.Context;
                
                switch (_currentType)
                {
                    case "categories":
                        db.Categories.Add(new Category { Name = NewNameBox.Text });
                        break;
                        
                    case "brands":
                        db.Brands.Add(new Brand { Name = NewNameBox.Text });
                        break;
                        
                    case "tags":
                        db.Tags.Add(new Tag { Name = NewNameBox.Text });
                        break;
                }
                
                db.SaveChanges();
                NewNameBox.Text = "";
                AddPanel.Visibility = Visibility.Collapsed;
                LoadData();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = ItemsList.SelectedItem;
            string itemType = "";
            
            switch (_currentType)
            {
                case "categories":
                    itemType = "категорию";
                    break;
                case "brands":
                    itemType = "бренд";
                    break;
                case "tags":
                    itemType = "тег";
                    break;
            }
            
            if (selectedItem != null && MessageBox.Show($"Удалить {itemType}?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var db = DBService.Instance.Context;
                
                switch (_currentType)
                {
                    case "categories":
                        db.Categories.Remove(selectedItem as Category);
                        break;
                        
                    case "brands":
                        db.Brands.Remove(selectedItem as Brand);
                        break;
                        
                    case "tags":
                        db.Tags.Remove(selectedItem as Tag);
                        break;
                }
                
                db.SaveChanges();
                LoadData();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = ItemsList.SelectedItem;
            if (selectedItem != null)
            {
                EditPanel.Visibility = Visibility.Visible;
                EditNameBox.Text = GetSelectedItemName(selectedItem);
            }
        }

        private void SaveEdit_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(EditNameBox.Text))
            {
                var selectedItem = ItemsList.SelectedItem;
                if (selectedItem != null)
                {
                    var db = DBService.Instance.Context;
                    
                    switch (_currentType)
                    {
                        case "categories":
                            var category = selectedItem as Category;
                            if (category != null) category.Name = EditNameBox.Text;
                            break;
                            
                        case "brands":
                            var brand = selectedItem as Brand;
                            if (brand != null) brand.Name = EditNameBox.Text;
                            break;
                            
                        case "tags":
                            var tag = selectedItem as Tag;
                            if (tag != null) tag.Name = EditNameBox.Text;
                            break;
                    }
                    
                    db.SaveChanges();
                    EditNameBox.Text = "";
                    EditPanel.Visibility = Visibility.Collapsed;
                    LoadData();
                }
            }
        }

        private void CancelEdit_Click(object sender, RoutedEventArgs e)
        {
            EditPanel.Visibility = Visibility.Collapsed;
            EditNameBox.Text = "";
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void ItemsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DeleteButton.IsEnabled = EditButton.IsEnabled = ItemsList.SelectedItem != null;
        }

        private string GetSelectedItemName(object item)
        {
            switch (_currentType)
            {
                case "categories":
                    return (item as Category)?.Name ?? "";
                case "brands":
                    return (item as Brand)?.Name ?? "";
                case "tags":
                    return (item as Tag)?.Name ?? "";
                default:
                    return "";
            }
        }
    }
}
