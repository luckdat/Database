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
    public partial class Invoices : UserControl
    {
        SqlConnection conn;
        public Invoices()
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
                string sql = "select * from Invoices";
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

        private void btnView_Click(object sender, EventArgs e)
        {
            DisplayData();
        }

        private void Create()
        {
            try
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                string sql = "INSERT INTO Invoices (CustomerID, EmployeeID, TotalAmount) VALUES (@CustomerID, @EmployeeID, @TotalAmount)";

                cmd.Parameters.Add("@CustomerID", SqlDbType.Int).Value = Convert.ToInt32(tbCustomerID.Text);
                cmd.Parameters.Add("@EmployeeID", SqlDbType.Int).Value = string.IsNullOrEmpty(tbEmployeeID.Text) ? (object)DBNull.Value : Convert.ToInt32(tbEmployeeID.Text);
                cmd.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = Convert.ToDecimal(tbTotalAmount.Text);

                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                MessageBox.Show("Create Invoice Successful");
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Creating Invoice: " + ex.Message);
            }
        }

   
        private void Edit()
        {
            try
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                string sql = "UPDATE Invoices SET CustomerID = @CustomerID, EmployeeID = @EmployeeID, TotalAmount = @TotalAmount WHERE InvoiceID = @InvoiceID";

                cmd.Parameters.Add("@InvoiceID", SqlDbType.Int).Value = Convert.ToInt32(tbID.Text);
                cmd.Parameters.Add("@CustomerID", SqlDbType.Int).Value = Convert.ToInt32(tbCustomerID.Text);
                cmd.Parameters.Add("@EmployeeID", SqlDbType.Int).Value = string.IsNullOrEmpty(tbEmployeeID.Text) ? (object)DBNull.Value : Convert.ToInt32(tbEmployeeID.Text);
                cmd.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = Convert.ToDecimal(tbTotalAmount.Text);

                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                MessageBox.Show("Edit Invoice Successful");
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Editing Invoice: " + ex.Message);
            }
        }

       

        // Delete Invoice
        private void Delete()
        {
            try
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                string sql = "DELETE FROM Invoices WHERE InvoiceID = @InvoiceID";

                cmd.Parameters.Add("@InvoiceID", SqlDbType.Int).Value = Convert.ToInt32(tbID.Text);

                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                MessageBox.Show("Delete Invoice Successful");
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Deleting Invoice: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DisplayData();
            Delete();
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

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            DisplayData();
            Delete();
        }
    }
}
