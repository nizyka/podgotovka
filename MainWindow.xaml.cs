using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace LibraryIs2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged 
    {
        private int _currentCountBook = 0;
        private int _allCountBook = 0;
        public int CurrentCountBook
        {
            get => _currentCountBook;
            set
            {
                _currentCountBook = value;
                PropertyChange("CurrentCountBook");
            }
        }
        public int AllCountBook
        {
            get => _allCountBook;
            set
            {
                _allCountBook = value;
                PropertyChange("AllCountBook");
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            UpdateCatalogBook();
            List<PM05Section> pM05Sections = new List<PM05Section>();
            pM05Sections.Add(new PM05Section() { Name = "Все жанры" });
            pM05Sections.AddRange(DataBase.GetContext().PM05Section.ToList());
            filterComboBox.ItemsSource = pM05Sections;
            DataContext = this;

        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void PropertyChange(string property) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        public void UpdateCatalogBook()
        {
            List<PM05Book> pM05Books = DataBase.GetContext().PM05Book.ToList();
            AllCountBook = pM05Books.Count();
            if(!String.IsNullOrWhiteSpace(searchTextBox.Text))
            {
                pM05Books = pM05Books.Where(p => p.Name.Contains(searchTextBox.Text) || p.Description.Contains(searchTextBox.Text)).ToList();
            }
            if(filterComboBox.SelectedIndex >0)
            {
                pM05Books = pM05Books.Where(p => p.PM05Section == (filterComboBox.SelectedItem as PM05Section)).ToList();
            }
            switch(sortComboBox.SelectedIndex)
            {
                case 0:
                    pM05Books = pM05Books.OrderBy(p => p.Name).ToList();
                    break;
                case 1:
                    pM05Books = pM05Books.OrderByDescending(p => p.Name).ToList();
                    break;
                case 2:
                    pM05Books = pM05Books.OrderBy(p => p.YearPub).ToList();
                    break;
                case 3:
                    pM05Books = pM05Books.OrderByDescending(p => p.YearPub).ToList();
                    break;
            }
            CurrentCountBook = pM05Books.Count();
            listBoxBooks.ItemsSource = pM05Books;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateCatalogBook();
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateCatalogBook();
        }
    }
}
