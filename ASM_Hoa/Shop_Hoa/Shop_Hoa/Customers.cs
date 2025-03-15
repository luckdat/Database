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

    public partial class Customers : UserControl
    {
        SqlConnection conn;
        public Customers()
        {
            InitializeComponent();
            createConnection();
        }
        private void createConnection()
        {
            try
            {
                String stringConnection = "Server=DESKTOP-TMCDUUR\\LUCKDAT;Database=Shop; Integrated Security = true";
                conn = new SqlConnection(stringConnection);
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Erorr createconnection " + ex.Message);
            }

        }

        private void DisplayData()
        {

            try
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                string sql = "select * from Customers";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                DataTable data = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(data);
                dgv1.DataSource = data;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erorr DisplayData " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }

        }

        private void Create()
        {
            try
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                string sql = "INSERT INTO Customers (CustomerName, Phone, Address, Email, DateOfBirth) " +
                             "VALUES (@CustomerName, @Phone, @Address, @Email, @DateOfBirth)";

                cmd.Parameters.Add("@CustomerName", SqlDbType.NVarChar).Value = tbCustomerName.Text;
                cmd.Parameters.Add("@Phone", SqlDbType.NVarChar).Value = tbPhone.Text;
                cmd.Parameters.Add("@Address", SqlDbType.NVarChar).Value = tbAddress.Text;
                cmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = tbEmail.Text;
                cmd.Parameters.Add("@DateOfBirth", SqlDbType.Date).Value = Convert.ToDateTime(tbDateOfBirth.Text);


                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                MessageBox.Show("Create Successful");
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Create: " + ex.Message);
            }
        }

        private void Edit()
        {
            try
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                string sql = "UPDATE Customers SET CustomerName = @CustomerName, Phone = @Phone, " +
                             "Address = @Address, Email = @Email, DateOfBirth = @DateOfBirth WHERE CustomerID = @CustomerID";

                cmd.Parameters.Add("@CustomerID", SqlDbType.Int).Value = Convert.ToInt32(tbID.Text);
                cmd.Parameters.Add("@CustomerName", SqlDbType.NVarChar).Value = tbCustomerName.Text;
                cmd.Parameters.Add("@Phone", SqlDbType.NVarChar).Value = tbPhone.Text;
                cmd.Parameters.Add("@Address", SqlDbType.NVarChar).Value = tbAddress.Text;
                cmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = tbEmail.Text;
                cmd.Parameters.Add("@DateOfBirth", SqlDbType.Date).Value = Convert.ToDateTime(tbDateOfBirth.Text);


                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                MessageBox.Show("Edit Successful");
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Edit: " + ex.Message);
            }
        }

        private void Delete()
        {
            try
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                string sql = "DELETE FROM Customers WHERE CustomerID = @CustomerID";

                cmd.Parameters.Add("@CustomerID", SqlDbType.Int).Value = Convert.ToInt32(tbID.Text);
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                MessageBox.Show("Delete Successful");
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Delete: " + ex.Message);
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            DisplayData();
            Create();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            DisplayData();
            Edit();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DisplayData();
            Delete();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            DisplayData();
        }
    }
}
