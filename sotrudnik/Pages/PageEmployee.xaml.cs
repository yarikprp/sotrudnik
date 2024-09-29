using sotrudnik.Commands;
using sotrudnik.Model;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace sotrudnik.Pages
{
    /// <summary>  
    /// Логика взаимодействия для PageEmployee.xaml  
    /// </summary>  
    public partial class PageEmployee : Page
    {
        public static WorkersContext? DataEntitiesEmployee { get; set; }
        ObservableCollection<Employee> listEmployee;
        private bool isDirty = true;

        public PageEmployee()
        {
            DataEntitiesEmployee = new WorkersContext();
            InitializeComponent();
            listEmployee = new ObservableCollection<Employee>();

            CommandBindings.Add(new CommandBinding(DataCommands.Edit, EditCommandBinding_Executed, CanExecuteEditCommand));
            CommandBindings.Add(new CommandBinding(DataCommands.Delete, DeleteCommandBinding_Executed, CanExecuteDeleteCommand));
            CommandBindings.Add(new CommandBinding(DataCommands.New, AddCommandBinding_Executed, CanExecuteAddCommand));
            CommandBindings.Add(new CommandBinding(DataCommands.Save, SaveCommandBinding_Executed, CanExecuteSaveCommand));
            CommandBindings.Add(new CommandBinding(DataCommands.Find, FindCommandBinding_Executed, CanExecuteFindCommand));
        }

        private void GetEmployees()
        {
            listEmployee.Clear();
            var employees = DataEntitiesEmployee.Employees.ToList();
            foreach (Employee emp in employees.OrderBy(e => e.Surname))
            {
                listEmployee.Add(emp);
            }
            DataGridEmployee.ItemsSource = listEmployee;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GetEmployees();
            DataGridEmployee.SelectedIndex = 0;
        }

        private void UndoCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            RewriteEmployee();
            DataGridEmployee.IsReadOnly = true;
            isDirty = true;
        }

        private void EditCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataGridEmployee.IsReadOnly = false;
            DataGridEmployee.BeginEdit();
            isDirty = false;
        }

        private void SaveCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataEntitiesEmployee.SaveChanges();
            isDirty = true;
            DataGridEmployee.IsReadOnly = true;
        }

        private void AddCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Employee employee = Employee.CreateEmployee("не задано", "не задано", "не задано", 0);
            try
            {
                DataEntitiesEmployee.Employees.Add(employee);
                listEmployee.Add(employee);
                DataGridEmployee.ScrollIntoView(employee);
                DataGridEmployee.SelectedIndex = DataGridEmployee.Items.Count - 1;
                DataGridEmployee.Focus();
                DataGridEmployee.IsReadOnly = false;
                isDirty = false;

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Ошибка добавления нового сотрудника в контекст данных" + ex.ToString());
            }
        }

        private void DeleteCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Employee? emp = DataGridEmployee.SelectedItem as Employee;
            if (emp != null)
            {
                MessageBoxResult result = MessageBox.Show($"Удалить сотрудника: {emp.Surname.Trim()} {emp.Name.Trim()} {emp.Patronymic.Trim()}?",
                                                           "Предупреждение", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    DataEntitiesEmployee.Employees.Remove(emp);
                    listEmployee.Remove(emp);
                    DataEntitiesEmployee.SaveChanges();
                }
            }
            else
            {
                MessageBox.Show("Выберите строку для удаления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FindCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Поиск");
            isDirty = true;
        }

        private void RewriteEmployee()
        {
            DataEntitiesEmployee = new WorkersContext();
            listEmployee.Clear();
            GetEmployees();
        }

        private void CanExecuteEditCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = isDirty;
        }

        private void CanExecuteSaveCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !isDirty;
        }

        private void CanExecuteAddCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CanExecuteDeleteCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true; 
        }

        private void CanExecuteFindCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;  
        }

        private void CanRefreshFindCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true; 
        }

        /*private void TextBoxSurname_TextChanged(object sender, TextChangedEventArgs e)
        {
            ButtonFindSurname.IsEnabled = true;
            ButtonFindTitle.IsEnabled = false;
            ComboBoxTitle.SelectedIndex = -1;
        }

        private void ButtonFindSurname_Click(object sender, RoutedEventArgs e)
        {
            string surname = TextBoxSurname.Text;
            DataEntitiesEmployee = new WorkersContext();
            listEmployee.Clear();
            var employees = DataEntitiesEmployee.Employees;
            var queryEmployee = from employee in employees where employee.Surname == surname select employee;
            foreach (Employee emp in queryEmployee)
            {
                listEmployee.Add(emp);
            }
            if(listEmployee.Count > 0)
            {
                DataGridEmployee.ItemsSource = listEmployee;
                ButtonFindSurname.IsEnabled = true;
                ButtonFindTitle.IsEnabled = false;
            }
            else
            {
                MessageBox.Show("Сотрудник с фамилией \n" + surname + "\n не найден", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ComboBoxTitle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ButtonFindTitle.IsEnabled = true;
            ButtonFindSurname.IsEnabled = false;
            TextBoxSurname.Text = "";
        }

        private void ButtonFindTitle_Click(object sender, RoutedEventArgs e)
        {
            DataEntitiesEmployee = new WorkersContext();
            listEmployee.Clear();

            Title? title = ComboBoxTitle.SelectedItem as Title;

            var employees = DataEntitiesEmployee.Employees;
            var queryEmployee = from employee in employees where employee.TitleId == title.TitleId orderby employee.Surname select employee;
            foreach (Employee emp in queryEmployee)
            {
                listEmployee.Add(emp);
            }
            DataGridEmployee.ItemsSource = listEmployee;
        }*/
    }
}