using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shop_Hoa
{
    public partial class Login : Form
    {
        SqlConnection conn;
        public Login()
        {
            InitializeComponent();
        }
   
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = tbUsername.Text;
            string password = tbPassword.Text;
            string role = AuthenticateUser(username, password);

            if (!string.IsNullOrEmpty(role))
            {
                MessageBox.Show($"Login successful!\nRole: {role}", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);

                OpenRoleBasedForm(role);
            }
            else
            {
                MessageBox.Show("Invalid username or password", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private string AuthenticateUser(string username, string password)
        {
            string role = "";
            string connectionString = @"Server=DESKTOP-TMCDUUR\LUCKDAT;Database=Shop;Integrated Security=true;";
            string query = "SELECT Role FROM Users WHERE Username = @Username AND Password = @Password";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                try
                {
                    con.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        role = result.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database error: " + ex.Message);
                }
            }
            return role;
        }

        private void OpenRoleBasedForm(string role)
        {
            Form formToOpen = null;

            switch (role)
            {
                case "Admin":
                    formToOpen = new Home();
                    break;
                case "Sales":
                    formToOpen = new Home();
                    break;
                case "Warehouse":
                    formToOpen = new Home();
                    break;
                default:
                    MessageBox.Show("Unauthorized role", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
            }

            this.Hide();
            formToOpen.ShowDialog();
            this.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to Exit?", "Ok", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();

            }
            else
            {
                return;
            }
        }
    }
}
