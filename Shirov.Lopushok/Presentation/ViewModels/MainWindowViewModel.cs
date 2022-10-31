using Microsoft.EntityFrameworkCore;
using Shirov.Lopushok.Domain.Entities;
using Shirov.Lopushok.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Shirov.Lopushok.Presentation.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private List<List<Product>> _productPages = new List<List<Product>>();
        private List<Product> _product;
        private List<Product> _productsForFilter;
        private List<Product> _currentProducts;
        private List<Product> _allProduct;
        private List<Product> _productBeforeSort;
        private List<Button> _buttonsPanel;
        private int _selectedSortIndex = 0;
        private int _selectedFilterIndex = 0;
        private int _selectedPage;

        public List<List<Product>> ProductPages { get => _productPages; set => Set(ref _productPages, value, nameof(ProductPages)); }
        public List<Product> AllProduct { get => _allProduct; set => Set(ref _allProduct , value,nameof(AllProduct)); }
        public List<Product> Product { get => _product; set => Set(ref _product, value, nameof(Product)); }
        public List<Product> CurrentProducts { get => _currentProducts; set => Set(ref _currentProducts, value, nameof(CurrentProducts)); }
        public List<Product> ProductsForFilter { get => _productsForFilter; set => Set(ref _productsForFilter, value, nameof(ProductsForFilter)); }
        public List<Product> ProductBeforeSort { get => _productBeforeSort; set => Set(ref _productBeforeSort, value, nameof(ProductBeforeSort)); }
        public List<string> ProductTypes { get; set; } = new List<string>();
        public List<Button> ButtonsPanel { get => _buttonsPanel; set => Set(ref _buttonsPanel, value, nameof(ButtonsPanel)); }
        public int SelectedSortIndex { get => _selectedSortIndex; set => Set(ref _selectedSortIndex, value, nameof(SelectedSortIndex)); }
        public int SelectedFilterIndex { get => _selectedFilterIndex; set => Set(ref _selectedFilterIndex, value, nameof(SelectedFilterIndex)); }
        public int SelectedPageIndex { get => _selectedPage; set => Set(ref _selectedPage, value, nameof(SelectedPageIndex)); }
        public MainWindowViewModel()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                Product = context.Products
                    .Include(p => p.ProductType)
                    .ToList();

                List<ProductType> productTypes = context.ProductTypes
                    .Include(pt => pt.Products)
                    .ToList();

                ProductTypes.Add("Все типы");
                foreach (ProductType productType in productTypes)
                    ProductTypes.Add(productType.Title);
            }
            AllProduct = Product;
            ProductsForFilter = Product;
            SetPages(AllProduct);
        }
        public void leftPageButtonClick(object sender, EventArgs e)
        {
            if (SelectedPageIndex != 0)
            {
                Product = ProductPages[SelectedPageIndex - 1];
                SelectedPageIndex--;
            }
        }
        public void rightPageButtonClick(object sender, EventArgs e)
        {
            if (SelectedPageIndex != ProductPages.Count() - 1)
            {
                Product = ProductPages[SelectedPageIndex + 1];
                SelectedPageIndex++;
            }
        }
        public void changePageButtonClick(object sender, EventArgs e)
        {
            Product = ProductPages[Convert.ToInt32((sender as Button).Content) - 1];
            SelectedPageIndex = Convert.ToInt32((sender as Button).Content) - 1;
        }

        public void Search(string text)
        {
            List<Product> products = (CurrentProducts == null || CurrentProducts.Count == 0) ? AllProduct : CurrentProducts;

            products = products
                .Where(p => p.Title.ToLower().Contains(text.ToLower()))
                .ToList();

            CurrentProducts = products;
            Product = CurrentProducts;
            ProductsForFilter = CurrentProducts;

            if (text == "")
            {
                CurrentProducts = null;
                SetPages(AllProduct);
                ProductsForFilter = AllProduct;
            }

            Filter(SelectedFilterIndex);
            Sort(SelectedSortIndex);
        }
        public void Filter(int index)
        {
            SelectedFilterIndex = index;
            List<Product> filterList = ProductsForFilter;

            switch (index)
            {
                case 0:
                    filterList = ProductsForFilter;
                    break;
                case 1:
                    filterList = filterList
                        .Where(p => p.ProductType.Title == "Три слоя")
                        .ToList();
                    break;
                case 2:
                    filterList = filterList
                        .Where(p => p.ProductType.Title == "Два слоя")
                        .ToList();
                    break;
                case 3:
                    filterList = filterList
                        .Where(p => p.ProductType.Title == "Детская")
                        .ToList();
                    break;
                case 4:
                    filterList = filterList
                        .Where(p => p.ProductType.Title == "Супер мягкая")
                        .ToList();
                    break;
                case 5:
                    filterList = filterList
                        .Where(p => p.ProductType.Title == "Один слой")
                        .ToList();
                    break;
            }

            CurrentProducts = filterList;
            SetPages(CurrentProducts);
            Sort(SelectedSortIndex);
        }
        public void Sort(int index)
        {
            SelectedSortIndex = index;
            List<Product> sortingList = (CurrentProducts == null) ? AllProduct : CurrentProducts;
            List<Product> listBeforeSort = (ProductBeforeSort == null) ? CurrentProducts : CurrentProducts;
            ProductBeforeSort = sortingList;

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                switch (index)
                {
                    case 0:
                        sortingList = listBeforeSort;
                        break;
                    case 1:
                        sortingList = sortingList
                            .OrderBy(p => p.Title)
                            .ToList();
                        break;
                    case 2:
                        sortingList = sortingList
                            .OrderByDescending(p => p.Title)
                            .ToList();
                        break;
                    case 3:
                        sortingList = sortingList
                            .OrderBy(p => p.ProductionWorkshopNumber)
                            .ToList();
                        break;
                    case 4:
                        sortingList = sortingList
                            .OrderByDescending(p => p.ProductionWorkshopNumber)
                            .ToList();
                        break;
                    case 5:
                        sortingList = sortingList
                            .OrderBy(p => p.Cost)
                            .ToList();
                        break;
                    case 6:
                        sortingList = sortingList
                            .OrderByDescending(p => p.Cost)
                            .ToList();
                        break;
                }

                CurrentProducts = sortingList;
                SetPages(CurrentProducts);
            }
        }
        public void SetPages(List<Product> products)
        {
            ProductPages.Clear();

            List<Product> page = new List<Product>();
            int pageCount = (products.Count % 20 == 0) ? products.Count / 20 : products.Count / 20 + 1;
            int lastPageItemsCount = products.Count % 20;

            for (int i = 0; i < pageCount; i++)
            {
                page = (products.Skip(20 * i).Count() < 20) ? products.Skip(20 * i).Take(lastPageItemsCount).ToList() : products.Skip(20 * i).Take(20).ToList();
                ProductPages.Add(page);
            }

            if (ProductPages.Count > 0)
                Product = ProductPages[0];
            else
                Product = null;

            SelectedPageIndex = 0;
            SetButtons();
        }

        public void SetButtons()
        {
            List<Button> buttons = new List<Button>();

            Button leftPageButton = new Button();
            leftPageButton.Content = "<";
            leftPageButton.Margin = new Thickness(0, 0, 2, 0);
            leftPageButton.BorderBrush = new SolidColorBrush(Colors.White);
            leftPageButton.BorderThickness = new Thickness(0, 0, 0, 0);
            leftPageButton.Background = new SolidColorBrush(Colors.White);
            leftPageButton.Click += leftPageButtonClick;
            buttons.Add(leftPageButton);

            for (int i = 0; i < ProductPages.Count(); i++)
            {
                Button changePageButton = new Button();
                changePageButton.Content = $"{i + 1}";
                changePageButton.Margin = new Thickness(0, 0, 2, 0);
                changePageButton.BorderBrush = new SolidColorBrush(Colors.White);
                changePageButton.BorderThickness = new Thickness(0, 0, 0, 0);
                changePageButton.Background = new SolidColorBrush(Colors.White);
                changePageButton.Click += changePageButtonClick;

                buttons.Add(changePageButton);
            }

            Button rightPageButton = new Button();
            rightPageButton.Content = ">";
            rightPageButton.Margin = new Thickness(0, 0, 2, 0);
            rightPageButton.BorderBrush = new SolidColorBrush(Colors.White);
            rightPageButton.BorderThickness = new Thickness(0, 0, 0, 0);
            rightPageButton.Background = new SolidColorBrush(Colors.White);
            rightPageButton.Click += rightPageButtonClick;
            buttons.Add(rightPageButton);

            ButtonsPanel = buttons;
        }
    }
}
