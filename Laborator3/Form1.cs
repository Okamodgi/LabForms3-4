using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Laborator3
{
    public partial class Form1 : Form
    {
        private TextBox txtLogin;
        private TextBox txtPassword;
        private Button btnLogin;

        private string connectionString = "Server=DESKTOP-44TQDIG;Database=LABORATS; Integrated Security=True;";

        public Form1()
        {
            InitializeComponent();
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            Label lblLogin = new Label();
            lblLogin.Text = "Логин";
            lblLogin.Location = new System.Drawing.Point(10, 10);
            lblLogin.AutoSize = true;

            txtLogin = new TextBox();
            txtLogin.Location = new System.Drawing.Point(10, 30);
            txtLogin.Width = 200;

            Label lblPassword = new Label();
            lblPassword.Text = "Пароль";
            lblPassword.Location = new System.Drawing.Point(10, 60);
            lblPassword.AutoSize = true;

            txtPassword = new TextBox();
            txtPassword.Location = new System.Drawing.Point(10, 80);
            txtPassword.Width = 200;
            txtPassword.PasswordChar = '*';

            btnLogin = new Button();
            btnLogin.Text = "Войти";
            btnLogin.Location = new System.Drawing.Point(10, 110);
            btnLogin.Click += BtnLogin_Click;

            this.Controls.Add(lblLogin);
            this.Controls.Add(txtLogin);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnLogin);
        }
        private bool IsValidPassword(string password)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d).+$");
            return regex.IsMatch(password);
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string login = txtLogin.Text;
            string password = txtPassword.Text;

            if (!IsValidPassword(password))
            {
                MessageBox.Show("Пароль должен содержать хотя бы одну заглавную букву, одну строчную букву и одну цифру.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT COUNT(1) FROM [dbo].[Users] WHERE [login] = @login AND [password] = @password";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@login", login);
                        command.Parameters.AddWithValue("@password", password);

                        int count = (int)command.ExecuteScalar();

                        if (count == 1)
                        {
                            Form2 form2 = new Form2(login);
                            form2.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Неверный логин или пароль.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void ConnectToDatabase()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM [dbo].[Users]";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32(0);
                                string login = reader.GetString(1);
                                string password = reader.GetString(2);
                                string tp = reader.IsDBNull(3) ? null : reader.GetString(3);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        private void InsertData(string login, string password, string tp)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO [dbo].[Users] (login, password, TP) VALUES (@login, @password, @tp)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@login", login);
                        command.Parameters.AddWithValue("@password", password);
                        command.Parameters.AddWithValue("@tp", tp);

                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
