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
    public partial class History : UserControl
    {
        private Dictionary<PictureBox, Color> borderColors = new Dictionary<PictureBox, Color>(); 
        private int borderWidth = 1;
        SqlConnection conn;

        public History()
        {
            InitializeComponent();
            createConnection();


            AddPictureBoxEvents(pictureBox1);
            AddPictureBoxEvents(pictureBox2);
            AddPictureBoxEvents(pictureBox3);
            AddPictureBoxEvents(pictureBox4);
        }

        private void AddPictureBoxEvents(PictureBox pictureBox)
        {
            if (pictureBox != null)
            {
                borderColors[pictureBox] = Color.Orange; 
                pictureBox.Paint += PictureBox_Paint;
                pictureBox.Click += PictureBox_Click;
            }
        }

        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            if (pb != null && borderColors.ContainsKey(pb))
            {
                Color borderColor = borderColors[pb];

           
                ControlPaint.DrawBorder(e.Graphics, pb.ClientRectangle,
                                        borderColor, borderWidth, ButtonBorderStyle.Solid,
                                        borderColor, borderWidth, ButtonBorderStyle.Solid,
                                        borderColor, borderWidth, ButtonBorderStyle.Solid,
                                        borderColor, borderWidth, ButtonBorderStyle.Solid);
            }
        }

        private void PictureBox_Click(object sender, EventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            if (pb != null && borderColors.ContainsKey(pb))
            {
  
                borderColors[pb] = borderColors[pb] == Color.Blue ? Color.Orange : Color.Blue;
                pb.Invalidate(); 
            }
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
                string sql = "select * from CustomerPurchaseHistory";
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

        private void Delete()
        {
            try
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                string sql = "DELETE FROM CustomerPurchaseHistory WHERE HistoryID = @HistoryID";

                cmd.Parameters.Add("@HistoryID", SqlDbType.Int).Value = Convert.ToInt32(tbID.Text);
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DisplayData();
            Delete();
        }
    }
}
