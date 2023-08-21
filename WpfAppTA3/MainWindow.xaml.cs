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

namespace WpfAppTA3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Student> students;

        public MainWindow()
        {
            InitializeComponent();

            // Initialize the list of students
            students = new List<Student>();
        }
        public class Student
        {
            public string Name { get; set; }
            public int Height { get; set; }
        }

        private void BtnGenerateList_Click(object sender, RoutedEventArgs e)
        {
            // Создание списка студентов наоснове введенных данных
            string input_stud = txtInput.Text.Trim();
            string[] inputArray = input_stud.Split(new char[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            List<Student> newStudents = new List<Student>();
            for (int i = 0; i < inputArray.Length; i += 2)
            {
                int height = 0;
                if (int.TryParse(inputArray[i + 1], out height))
                {
                    string name = inputArray[i];
                    Student students = new Student() { Name = name, Height = height };
                    newStudents.Add(students);
                }
            }

            // Сортировка списка в порядке убывания
            newStudents = newStudents.OrderByDescending(s => s.Height).ToList();

            // Обновление ListBox со списком студентов
            lstStudents.ItemsSource = newStudents;
            students = newStudents;
        }

        private void BtnAddStudent_Click(object sender, RoutedEventArgs e)
        {
            // Добавить нового студента в список
            int height = 0;
            if (int.TryParse(txtAddStudentHeight.Text.Trim(), out height))
            {
                string name = txtAddStudentName.Text.Trim();
                if (string.IsNullOrWhiteSpace(name))
                {
                    name = "Student " + (students.Count + 1);
                }

                Student student = new Student() { Name = name, Height = height };
                students.Add(student);
                lstStudents.ItemsSource = null;
                lstStudents.ItemsSource = students.OrderByDescending(s => s.Height).ToList();
                txtAddStudentName.Clear();
                txtAddStudentHeight.Clear();
            }
            else
            {
                MessageBox.Show("Invalid input.");
            }
        }

        private void BtnDeleteStudent_Click(object sender, RoutedEventArgs e)
        {
            // Удалить студента из списка
            Button btn = sender as Button;
            Student student = btn.Tag as Student;
            students.Remove(student);
            lstStudents.ItemsSource = null;
            lstStudents.ItemsSource = students.OrderByDescending(s => s.Height).ToList();

        }

        private void BtnSwapStudents_Click(object sender, RoutedEventArgs e)
        {
            // Поменять местами двух студентов с одинаковым ростом
            if (lstStudents.SelectedItems.Count == 2)
            {
                int index1 = students.IndexOf(lstStudents.SelectedItems[0] as Student);
                int index2 = students.IndexOf(lstStudents.SelectedItems[1] as Student);
                if (index1 != -1 && index2 != -1 && index1 != index2 && students[index1].Height == students[index2].Height)
                {
                    Student temp = students[index1];
                    students[index1] = students[index2];
                    students[index2] = temp;
                    lstStudents.ItemsSource = null;
                    lstStudents.ItemsSource = students.OrderByDescending(s => s.Height).ToList();
                    lstStudents.SelectedIndex = -1;
                }
            }
        }

        private void BtnFindStudent_Click(object sender, RoutedEventArgs e)
        {
            string search = txtFindStudent.Text.Trim().ToLower();

            List<Student> matchingStudents = new List<Student>();
            foreach (Student student in lstStudents.Items)
            {
                if (student.Name.ToLower().Contains(search) || student.Height.ToString().Contains(search))
                {
                    matchingStudents.Add(student);
                }
            }

            if (matchingStudents.Count > 0)
            {
                lstStudents.SelectedItems.Clear();
                foreach (Student student in matchingStudents)
                {
                    lstStudents.SelectedItems.Add(student);
                }
            }
            else
            {
                MessageBox.Show("No results found.");
            }
        }
    }
}
