using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;//Для работы с БД
using System.Data.SqlClient;//Для работы с БД
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Faker;//Словарь имен и фамилий

namespace Laba_2
{
    public partial class Form1 : Form
    {
        SqlConnection sqlConnection;
        int string_count = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=USER-ПК;Initial Catalog=Trolleybus_traffic;Integrated Security=True";

            sqlConnection = new SqlConnection(connectionString);

            await sqlConnection.OpenAsync();
        }

        //Очищаем таблицу
        private async void button1_Click(object sender, EventArgs e)
        {
            //Обьявляем ридера
            SqlDataReader sqlReader = null;
            SqlCommand command = new SqlCommand("DELETE FROM Employee_people", sqlConnection);
            try
            {
                sqlReader = await command.ExecuteReaderAsync();
                sqlReader.Close();
                MessageBox.Show("Таблиця очищена!", "Статус", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null)
                {
                    sqlReader.Close();
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void создатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        //Наповнення таблиці
        private void button2_Click(object sender, EventArgs e)
        {
            //Отримання кількості рядків
            try
            {
                string_count = int.Parse(textBox1.Text);
                System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
                timer.Start();//Початок відліку таймеру
                Random rand = new Random();
                SqlDataReader sqlReader = null;
                for (int i = 0; i < string_count; i++)
                {
                    SqlCommand command = 
                        new SqlCommand("INSERT INTO [Employee_people](Surname, Name, Phone, Department_id)VALUES(@Surname, @Name, @Phone, @Department_id)", sqlConnection);
                    command.Parameters.AddWithValue("@Surname", Faker.NameFaker.LastName());
                    command.Parameters.AddWithValue("@Name", Faker.NameFaker.FirstName());
                    command.Parameters.AddWithValue("@Phone", Faker.PhoneFaker.Phone());
                    command.Parameters.AddWithValue("@Department_id", rand.Next(1, 10));

                    sqlReader = command.ExecuteReader();
                    sqlReader.Close();
                }
                timer.Stop();//Кінець відліку таймеру
                textBox2.Text = timer.ElapsedMilliseconds.ToString() + "ms";
                textBox3.Text = Convert.ToString(string_count);
            }
            catch (FormatException)
            {
                MessageBox.Show("You don`t enter count of rows or data not numerical!", "Error", MessageBoxButtons.OK);
            }
            MessageBox.Show("Вставка успішна!", "Справка", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            SqlCommand command =
                new SqlCommand("SELECT e.Employee_id, e.Surname, e.Name, e.Phone, d.Department_id, d.Salary FROM [Employee_people] AS e JOIN [Departments] AS d ON e.Department_id=d.Department_id WHERE e.Surname LIKE 'D%'", sqlConnection);
            SqlDataReader sqlReader = null;
            sqlReader = command.ExecuteReader();
            sqlReader.Close();
            timer.Stop();
            textBox4.Text = timer.ElapsedMilliseconds.ToString() + "ms";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SqlCommand command = 
                new SqlCommand("CREATE INDEX indexSurname ON Employee_people(Surname)", sqlConnection);
            SqlDataReader sqlReader = null;
            sqlReader = command.ExecuteReader();
            sqlReader.Close();
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();

            SqlCommand command_2 =
                new SqlCommand("SELECT e.Employee_id, e.Surname, e.Name, e.Phone, d.Department_id, d.Salary FROM [Employee_people] AS e JOIN [Departments] AS d ON e.Department_id=d.Department_id WHERE e.Surname LIKE 'D%'", sqlConnection);
            SqlDataReader sqlReader_2 = null;
            sqlReader_2 = command_2.ExecuteReader();
            sqlReader_2.Close();
            timer.Stop();

            SqlCommand command_3 =
                new SqlCommand("DROP INDEX indexSurname ON Employee_people", sqlConnection);
            SqlDataReader sqlReader_3 = null;
            sqlReader_3 = command_3.ExecuteReader();
            sqlReader_3.Close();

            textBox6.Text = timer.ElapsedMilliseconds.ToString() + "ms";

        }

        private void вихідToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sqlConnection != null && sqlConnection.State != ConnectionState.Closed)
            {
                sqlConnection.Close();
                Application.Exit();
            }
        }

    }
}
