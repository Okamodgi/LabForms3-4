using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Laborator3
{
    public partial class Form3 : Form
    {
        private TextBox txtOther;
        private CheckBox cbServerIssues;
        private CheckBox cbForgotPassword;
        private CheckBox cbForgotLogin;
        private CheckBox cbOther;
        private string userLogin;

        private string connectionString = "Server=DESKTOP-44TQDIG;Database=LABORATS; Integrated Security=True;";

        public Form3(string login)
        {
            InitializeComponent();
            userLogin = login;
            InitializeCustomComponents();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
        }

        private void InitializeCustomComponents()
        {
            GroupBox groupBox = new GroupBox();
            groupBox.Text = "Выберите проблему:";
            groupBox.Location = new System.Drawing.Point(10, 10);
            groupBox.Size = new System.Drawing.Size(250, 180);

            cbServerIssues = new CheckBox();
            cbServerIssues.Text = "Проблемы с сервером";
            cbServerIssues.Location = new System.Drawing.Point(10, 20);
            cbServerIssues.AutoSize = true;

            cbForgotPassword = new CheckBox();
            cbForgotPassword.Text = "Забыл пароль";
            cbForgotPassword.Location = new System.Drawing.Point(10, 50);
            cbForgotPassword.AutoSize = true;

            cbForgotLogin = new CheckBox();
            cbForgotLogin.Text = "Забыл логин";
            cbForgotLogin.Location = new System.Drawing.Point(10, 80);
            cbForgotLogin.AutoSize = true;

            cbOther = new CheckBox();
            cbOther.Text = "Другое";
            cbOther.Location = new System.Drawing.Point(10, 110);
            cbOther.AutoSize = true;
            cbOther.CheckedChanged += CbOther_CheckedChanged;

           
            txtOther = new TextBox();
            txtOther.Location = new System.Drawing.Point(10, 140);
            txtOther.Width = 200;
            txtOther.Enabled = false; 

            Button btnOk = new Button();
            btnOk.Text = "OK";
            btnOk.Location = new System.Drawing.Point(100, 200);
            btnOk.Click += BtnOk_Click;

            groupBox.Controls.Add(cbServerIssues);
            groupBox.Controls.Add(cbForgotPassword);
            groupBox.Controls.Add(cbForgotLogin);
            groupBox.Controls.Add(cbOther);
            groupBox.Controls.Add(txtOther); 
            this.Controls.Add(groupBox);
            this.Controls.Add(btnOk);
        }


        private void CbOther_CheckedChanged(object sender, EventArgs e)
        {
            txtOther.Enabled = cbOther.Checked;
            txtOther.Visible = cbOther.Checked;
        }


        private void BtnOk_Click(object sender, EventArgs e)
        {
            string tp = "";

            
            if (cbServerIssues.Checked) tp += "Проблемы с сервером; ";
            if (cbForgotPassword.Checked) tp += "Забыл пароль; ";
            if (cbForgotLogin.Checked) tp += "Забыл логин; ";
            if (cbOther.Checked && !string.IsNullOrEmpty(txtOther.Text)) 
            {
                tp += txtOther.Text + "; ";
            }

           
            if (string.IsNullOrEmpty(tp))
            {
                MessageBox.Show("Выберите хотя бы одну проблему или укажите её в поле 'Другое'.");
                return;
            }

    
            tp = tp.TrimEnd(new char[] { ' ', ';' });

            InsertData(tp);
            this.Close();
        }


        private void InsertData(string tp)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "UPDATE [dbo].[Users] SET TP = @tp WHERE login = @login";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@tp", tp);
                        command.Parameters.AddWithValue("@login", userLogin);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Данные успешно обновлены.");
                        }
                        else
                        {
                            MessageBox.Show("Ошибка при обновлении данных.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка подключения: {ex.Message}");
                }
            }
        }
    }
}
