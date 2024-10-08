using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using WPF_User.Models;

namespace WPF_User
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PRN2xx_UserContext _context;
        public MainWindow()
        {
            InitializeComponent();
            _context = new PRN2xx_UserContext();
            LoadPersons();
            LoadHobbies();
        }
        private void LoadPersons()
        {
            var persons = _context.Persons
                .Include(p => p.Address)
                .Include(p => p.Hobbies)
                .ToList();
            ComboBoxPersons.ItemsSource = persons;
            ComboBoxPersons.DisplayMemberPath = "FirstName";
            ComboBoxPersons.SelectedValuePath = "PersonId";

            if (persons.Any())
            {
                ComboBoxPersons.SelectedIndex = 0; // Chọn người đầu tiên
            }
        }
        private void LoadHobbies()
        {
            var hobbies = _context.Hobbies.ToList();
            ItemsControlHobbies.ItemsSource = hobbies;
        }

        private void ComboBoxPersons_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ComboBoxPersons.SelectedValue != null)
            {
                int personID = (int)ComboBoxPersons.SelectedValue;
                LoadPersonDetails(personID);
                LoadOrders(personID);
            }
        }

        private void LoadPersonDetails(int personID)
        {
            var person = _context.Persons.Include(p => p.Address).Include(p => p.Hobbies).FirstOrDefault(p => p.PersonId == personID);
            if (person != null)
            {
                TextBoxFirstName.Text = person.FirstName;
                TextBoxLastName.Text = person.LastName;
                DatePickerDOB.SelectedDate = person.DateOfBirth;

                if (person.Gender == "Nam")
                {
                    RadioButtonMale.IsChecked = true;
                }
                else if (person.Gender == "Nữ")
                {
                    RadioButtonFemale.IsChecked = true;
                }

                if (person.Address != null)
                {
                    TextBoxStreet.Text = person.Address.Street;
                    TextBoxCity.Text = person.Address.City;
                    TextBoxZipCode.Text = person.Address.ZipCode;
                }
                else
                {
                    TextBoxStreet.Text = string.Empty;
                    TextBoxCity.Text = string.Empty;
                    TextBoxZipCode.Text = string.Empty;
                }

                // Hiển thị sở thích
                foreach (var hobby in _context.Hobbies)
                {
                    hobby.IsSelected = person.Hobbies.Contains(hobby);
                }
                ItemsControlHobbies.ItemsSource = _context.Hobbies.ToList();
            }
        }

        private void LoadOrders(int? personID)
        {
            if (personID.HasValue)
            {
                var orders = _context.Orders
                    .Include(o => o.Person)
                    .ThenInclude(p => p.Address)
                    .Where(o => o.PersonId == personID.Value)
                    .ToList();
                DataGridOrders.ItemsSource = orders;
            }
            else
            {
                DataGridOrders.ItemsSource = null;
            }
        }

        private void DataGridOrders_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (DataGridOrders.SelectedItem != null)
            {
                var selectedOrder = DataGridOrders.SelectedItem as Order;
                if (selectedOrder != null)
                {
                    LoadOrderDetails(selectedOrder.OrderId);
                    LoadProducts(selectedOrder.OrderId);
                }
            }
        }

        private void LoadOrderDetails(int orderID)
        {
            var order = _context.Orders.Include(o => o.Person).ThenInclude(p => p.Address).FirstOrDefault(o => o.OrderId == orderID);
            if (order != null)
            {
                TextBoxOrderID.Text = order.OrderId.ToString();
                DatePickerOrderDate.SelectedDate = order.OrderDate;

                // Hiển thị thông tin người mua
                var person = order.Person;
                if (person != null)
                {
                    TextBoxCustomerFirstName.Text = person.FirstName;
                    TextBoxCustomerLastName.Text = person.LastName;

                    if (person.Address != null)
                    {
                        TextBoxCustomerStreet.Text = person.Address.Street;
                        TextBoxCustomerCity.Text = person.Address.City;
                        TextBoxCustomerZipCode.Text = person.Address.ZipCode;
                    }
                    else
                    {
                        TextBoxCustomerStreet.Text = string.Empty;
                        TextBoxCustomerCity.Text = string.Empty;
                        TextBoxCustomerZipCode.Text = string.Empty;
                    }
                }
            }
        }

        private void ButtonEditOrderDetails_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridOrders.SelectedItem != null)
            {
                var selectedOrder = DataGridOrders.SelectedItem as Order;
                if (selectedOrder != null)
                {
                    selectedOrder.OrderDate = DatePickerOrderDate.SelectedDate.HasValue ? DatePickerOrderDate.SelectedDate.Value : DateTime.Now;

                    _context.SaveChanges();
                    LoadOrders(selectedOrder.PersonId);
                }
            }
        }

        private void LoadProducts(int orderID)
        {
            var products = _context.Orders
                .Where(o => o.OrderId == orderID)
                .SelectMany(o => o.Products) // Truy vấn sản phẩm liên kết với đơn hàng
                .ToList();
            DataGridProducts.ItemsSource = products;
        }




        private void ButtonAddPerson_Click(object sender, RoutedEventArgs e)
        {
            var person = new Person
            {
                FirstName = TextBoxFirstName.Text,
                LastName = TextBoxLastName.Text,
                DateOfBirth = DatePickerDOB.SelectedDate.HasValue ? DatePickerDOB.SelectedDate.Value : (DateTime?)null,
                Gender = RadioButtonMale.IsChecked == true ? "Nam" : "Nữ",
                Address = new Address
                {
                    Street = TextBoxStreet.Text,
                    City = TextBoxCity.Text,
                    ZipCode = TextBoxZipCode.Text
                }
            };

            foreach (var hobby in _context.Hobbies)
            {
                if (hobby.IsSelected)
                {
                    person.Hobbies.Add(hobby);
                }
            }

            _context.Persons.Add(person);
            _context.SaveChanges();
            LoadPersons();
        }

        private void ButtonEditPerson_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBoxPersons.SelectedValue != null)
            {
                int personID = (int)ComboBoxPersons.SelectedValue;
                var person = _context.Persons.Include(p => p.Hobbies).FirstOrDefault(p => p.PersonId == personID);
                if (person != null)
                {
                    person.FirstName = TextBoxFirstName.Text;
                    person.LastName = TextBoxLastName.Text;
                    person.DateOfBirth = DatePickerDOB.SelectedDate.HasValue ? DatePickerDOB.SelectedDate.Value : (DateTime?)null;
                    person.Gender = RadioButtonMale.IsChecked == true ? "Nam" : "Nữ";
                    person.Address.Street = TextBoxStreet.Text;
                    person.Address.City = TextBoxCity.Text;
                    person.Address.ZipCode = TextBoxZipCode.Text;

                    person.Hobbies.Clear();
                    foreach (var hobby in _context.Hobbies)
                    {
                        if (hobby.IsSelected)
                        {
                            person.Hobbies.Add(hobby);
                        }
                    }

                    _context.SaveChanges();
                    LoadPersons();
                }
            }
        }


    }
}
