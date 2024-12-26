using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Laborator3
{
    public partial class Form2 : Form
    {
        private string connectionString = "Server=DESKTOP-44TQDIG;Database=LABORATS; Integrated Security=True;";
        private string userLogin;

        public Form2(string login)
        {
            InitializeComponent();
            userLogin = login;
            InitializeCustomComponents();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
        }

        private void InitializeCustomComponents()
        {
            GroupBox groupBox = new GroupBox();
            groupBox.Text = "Выберите метод:";
            groupBox.Location = new System.Drawing.Point(10, 10);
            groupBox.Size = new System.Drawing.Size(250, 180);

            RadioButton rbLoginPassword = new RadioButton();
            rbLoginPassword.Text = "Логин/пароль";
            rbLoginPassword.Location = new System.Drawing.Point(10, 20);
            rbLoginPassword.AutoSize = true;

            RadioButton rbEmail = new RadioButton();
            rbEmail.Text = "Почта";
            rbEmail.Location = new System.Drawing.Point(10, 50);
            rbEmail.AutoSize = true;

            RadioButton rbSocial = new RadioButton();
            rbSocial.Text = "Соцсети";
            rbSocial.Location = new System.Drawing.Point(10, 80);
            rbSocial.AutoSize = true;

            RadioButton rbTP = new RadioButton();
            rbTP.Text = "Техподдержка";
            rbTP.Location = new System.Drawing.Point(10, 110);
            rbTP.AutoSize = true;

            Label lblSupport = new Label();
            lblSupport.Text = "Техподдержка";
            lblSupport.Location = new System.Drawing.Point(10, 140);
            lblSupport.AutoSize = true;

            Button btnOk = new Button();
            btnOk.Text = "OK";
            btnOk.Location = new System.Drawing.Point(100, 190);
            btnOk.Click += BtnOk_Click;

            groupBox.Controls.Add(rbLoginPassword);
            groupBox.Controls.Add(rbEmail);
            groupBox.Controls.Add(rbSocial);
            groupBox.Controls.Add(rbTP);
            this.Controls.Add(groupBox);
            this.Controls.Add(lblSupport);
            this.Controls.Add(btnOk);
        }
        private string GetUserInfoFromDatabase(string userLogin, string type)
        {
          
            string query = string.Empty;

 
            switch (type)
            {
                case "LoginPassword":
                    query = "SELECT Login, Password FROM Users WHERE Login = @login";
                    break;
                case "Email":
                    query = "SELECT Email FROM Users WHERE Login = @login";
                    break;
                case "Social":
                    query = "SELECT Social FROM Users WHERE Login = @login";
                    break;
                default:
                    return "Неизвестный запрос.";
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Передаем параметр в запрос
                        command.Parameters.AddWithValue("@login", userLogin);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Извлекаем данные из столбцов
                                switch (type)
                                {
                                    case "LoginPassword":
                                        string login = reader["Login"].ToString();
                                        string password = reader["Password"].ToString();
                                        return $"Ваш логин: {login}, пароль: {password}";

                                    case "Email":
                                        return $"Ваш email: {reader["Email"].ToString()}";

                                    case "Social":
                                        return $"Ваши соцсети: {reader["Social"].ToString()}";

                                    default:
                                        return "Нет данных.";
                                }
                            }
                            else
                            {
                                return "Пользователь не найден.";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок
                return $"Ошибка: {ex.Message}";
            }
        }


        private void BtnOk_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3(userLogin);
            form3.Show();
            this.Hide();
        }
    }
}
