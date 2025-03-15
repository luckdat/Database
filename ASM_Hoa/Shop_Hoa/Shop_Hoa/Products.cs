using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Shop_Hoa
{
    public partial class Products : UserControl
    {
        SqlConnection conn;
        public Products()
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
                string sql = "select * from Products";
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

                string sql = "INSERT INTO Products (ProductCode, ProductName, SellingPrice, InventoryQuantity, SupplierID, Image) " +
                             "VALUES (@ProductCode, @ProductName, @SellingPrice, @InventoryQuantity, @SupplierID, @Image)";

                cmd.Parameters.Add("@ProductCode", SqlDbType.NVarChar).Value = tbProductCode.Text;
                cmd.Parameters.Add("@ProductName", SqlDbType.NVarChar).Value = tbProductName.Text;
                cmd.Parameters.Add("@SellingPrice", SqlDbType.Decimal).Value = Convert.ToDecimal(tbSellingPrice.Text);
                cmd.Parameters.Add("@InventoryQuantity", SqlDbType.Int).Value = Convert.ToInt32(tbInventoryQuantity.Text);
                cmd.Parameters.Add("@SupplierID", SqlDbType.Int).Value = string.IsNullOrEmpty(tbSupplierID.Text) ? (object)DBNull.Value : Convert.ToInt32(tbSupplierID.Text);

                if (!string.IsNullOrEmpty(tbImages.Text) && File.Exists(tbImages.Text))
                {
                    byte[] ImageData = File.ReadAllBytes(tbImages.Text);
                    cmd.Parameters.Add("@Image", SqlDbType.VarBinary).Value = ImageData;
                }
                else
                {
                    cmd.Parameters.Add("@Image", SqlDbType.VarBinary).Value = DBNull.Value;
                }

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

                string sql = "UPDATE Products SET ProductCode = @ProductCode, ProductName = @ProductName, SellingPrice = @SellingPrice, " +
                             "InventoryQuantity = @InventoryQuantity, SupplierID = @SupplierID, Image = @Image WHERE ProductID = @ProductID";

                cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = Convert.ToInt32(tbID.Text);
                cmd.Parameters.Add("@ProductCode", SqlDbType.NVarChar).Value = tbProductCode.Text;
                cmd.Parameters.Add("@ProductName", SqlDbType.NVarChar).Value = tbProductName.Text;
                cmd.Parameters.Add("@SellingPrice", SqlDbType.Decimal).Value = Convert.ToDecimal(tbSellingPrice.Text);
                cmd.Parameters.Add("@InventoryQuantity", SqlDbType.Int).Value = Convert.ToInt32(tbInventoryQuantity.Text);
                cmd.Parameters.Add("@SupplierID", SqlDbType.Int).Value = string.IsNullOrEmpty(tbSupplierID.Text) ? (object)DBNull.Value : Convert.ToInt32(tbSupplierID.Text);

                if (!string.IsNullOrEmpty(tbImages.Text) && File.Exists(tbImages.Text))
                {
                    byte[] ImageData = File.ReadAllBytes(tbImages.Text);
                    cmd.Parameters.Add("@Image", SqlDbType.VarBinary).Value = ImageData;
                }
                else
                {
                    cmd.Parameters.Add("@Image", SqlDbType.VarBinary).Value = DBNull.Value;
                }

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
                string sql = "DELETE FROM Products WHERE ProductID = @ProductID";

                cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = Convert.ToInt32(tbID.Text);

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

        private void btnImages_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();


            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif|All Files|*.*";
            openFileDialog.Title = "Select a File";


            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {

                tbImages.Text = openFileDialog.FileName;
            }
        }
    }
}
