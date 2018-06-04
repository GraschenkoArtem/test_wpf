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
using System.IO;
using System.Xml;
using System.Data;
using System.Data.SqlClient;

namespace test_wpf
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public struct User
        {
            public string Name;
            public string Number;
            public string Email;

        }

        public User new_User;
        List<User> User_List = new List<User>();
        SqlConnection sqlConnection;

        public MainWindow()
        {
            InitializeComponent();

            User_List.Clear();
            List1.Items.Clear();

            //для работы с БД раскомментить строки 47, 48, 52-109, 256
            //DB_Connect();
            //BD_Read();
            XML_Read();
        }

        /* void DB_Connect()
         {
             //для подключения к бд надо заменить путь "C:\Users\Артём\Source\Repos\test_wpf\test_wpf\Database.mdf" на новый путь
             string connectionstring = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Артём\Source\Repos\test_wpf\test_wpf\Database.mdf;Integrated Security=True";

             sqlConnection = new SqlConnection(connectionstring);

             sqlConnection.Open();

         }

         void BD_Read()
         {
             SqlDataReader sqlReader = null;

             SqlCommand command = new SqlCommand("SELECT * FROM [User_DB]", sqlConnection);

             try
             {
                 sqlReader = command.ExecuteReader();

                 while( sqlReader.Read())
                 {
                     new_User.Name = Convert.ToString(sqlReader["Name_t"]);
                     new_User.Number = Convert.ToString(sqlReader["Number_t"]);
                     new_User.Email = Convert.ToString(sqlReader["Email_t"]);
                     User_List.Add(new_User);
                     //List1.Items.Add("Имя: " + new_User.Name + "; Номер: " + new_User.Number + "; Почта: " + new_User.Email);
                     List1.Items.Add("Фамилия: " + new_User.Name);
                 }
             }
             catch (Exception ex)
             {
                 MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
             }
             finally
             {
                 if (sqlReader != null)
                     sqlReader.Close();
             }
         }

         void BD_Save()
         {
             SqlCommand command0 = new SqlCommand("DELETE FROM [User_DB]", sqlConnection);

             command0.ExecuteNonQuery();

             foreach (User a in User_List)
             {
                 SqlCommand command = new SqlCommand("INSERT INTO [User_DB] (Name_t, Number_t, Email_t) VALUES(@_Name, @_Number, @_Email)", sqlConnection);

                 command.Parameters.AddWithValue("_Name", a.Name);
                 command.Parameters.AddWithValue("_Number", a.Number);
                 command.Parameters.AddWithValue("_Email", a.Email);

                 command.ExecuteNonQuery();
             }
         }*/

        void XML_Read()
        {
            try
            {
                XmlTextReader reader = new XmlTextReader("User_Data.xml");

                while (reader.Read())
                {
                    if (reader.Name == "Name")
                    {
                        new_User.Name = reader.GetAttribute("name");
                    }
                    else
                    {

                        if (reader.Name == "Number")
                        {
                            new_User.Number = reader.GetAttribute("name");
                        }
                        else
                        {
                            if (reader.Name == "Email")
                            {
                                new_User.Email = reader.GetAttribute("name");
                                User_List.Add(new_User);
                                //List1.Items.Add("Имя: " + new_User.Name + "; Номер: " + new_User.Number + "; Почта: " + new_User.Email);
                                List1.Items.Add(new_User.Name);
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            }
            



        }

        void XML_Save()
        {
            try
            {
                XmlTextWriter writer = new XmlTextWriter("User_Data.xml", Encoding.UTF8);

                writer.WriteStartElement("Users_Table");
                writer.WriteString("\n\t");
                foreach (User a in User_List)
                {
                    writer.WriteStartElement("User");

                    writer.WriteString("\n\t");

                    writer.WriteStartElement("Name");
                    writer.WriteAttributeString("name", a.Name);
                    writer.WriteEndElement();

                    writer.WriteString("\n\t");

                    writer.WriteStartElement("Number");
                    writer.WriteAttributeString("name", a.Number);
                    writer.WriteEndElement();

                    writer.WriteString("\n\t");

                    writer.WriteStartElement("Email");
                    writer.WriteAttributeString("name", a.Email);
                    writer.WriteEndElement();

                    writer.WriteString("\n\t");

                    writer.WriteEndElement();
                    writer.WriteString("\n\t");
                }
                writer.WriteString("\n\t");
                writer.WriteEndElement();

                writer.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            Add_Wpf New_Window = new Add_Wpf();
            New_Window.Owner = this;

            if (New_Window.ShowDialog() == true)
            {
                new_User.Name = New_Window.Name_TextBox.Text;
                new_User.Number = New_Window.Number_TextBox.Text;
                new_User.Email = New_Window.Email_TextBox.Text;

                User_List.Add(new_User);
                //List1.Items.Add("Имя: " + new_User.Name + "; Номер: " + new_User.Number + "; Почта: " + new_User.Email);
                List1.Items.Add(new_User.Name);
            }
            else
                MessageBox.Show("Ошибка!");

        }

        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                User_List.RemoveAt(List1.SelectedIndex);
                List1.Items.RemoveAt(List1.SelectedIndex);
            }
            catch
            {
                MessageBox.Show("Выберите элемент в списке для удаления");
            }
            
        }

        private void Find_Button_Click(object sender, RoutedEventArgs e)
        {
            if (Find_TextBox.Text != "")
            {
                foreach (User a in User_List)
                {
                    if (Find_TextBox.Text == a.Name || Find_TextBox.Text == a.Number || Find_TextBox.Text == a.Email)
                        MessageBox.Show("Фамилия: " + a.Name + "; Номер: " + a.Number + "; Почта: " + a.Email + ";", "Результат поиска");
                }

            }
            else
                MessageBox.Show("Нечего искать!");
            Find_TextBox.Text = "";
        }

        private void Exit_Button_Click(object sender, RoutedEventArgs e)
        {            
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //для записи данных в БД раскомментить следующую строку
            //BD_Save();
            XML_Save();
            if (sqlConnection != null && sqlConnection.State != ConnectionState.Closed)
                sqlConnection.Close();
        }

    }
}
