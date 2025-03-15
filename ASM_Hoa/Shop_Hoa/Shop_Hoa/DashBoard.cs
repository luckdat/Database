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
    public partial class DashBoard : UserControl
    {
    
        private Dictionary<PictureBox, Color> borderColors = new Dictionary<PictureBox, Color>();
        private int borderWidth = 1;
        SqlConnection conn;
        private DatabaseHelper dbHelper = new DatabaseHelper();


        public DashBoard()
        {
            InitializeComponent();
           
            AddPictureBoxEvents(pictureBox1);
            AddPictureBoxEvents(pictureBox2);
            AddPictureBoxEvents(pictureBox3);
            AddPictureBoxEvents(pictureBox4);
            AddPictureBoxEvents(pictureBox5);
            createConnection();
            LoadData();
            LoadRevenue1();
            LoadRevenue2();
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
                MessageBox.Show("Lỗi kết nối: " + ex.Message);
            }
        }

        // 1. Thống kê số lượng sản phẩm nhập theo mã sản phẩm
        private void LoadData()
        {
            string query = @"
                SELECT 
                    P.ProductCode, 
                    P.ProductName, 
                    SUM(ID.Quantity) AS TotalImported
                FROM Products P
                LEFT JOIN InvoiceDetails ID ON P.ProductID = ID.ProductID
                GROUP BY P.ProductCode, P.ProductName
                ORDER BY TotalImported DESC;";
            dgv1.DataSource = dbHelper.GetStatistics(query);
        }

        // 2. Thống kê doanh thu theo thời gian (ngày)
        private void LoadRevenue1()
        {
            string query = @"
                SELECT 
                    FORMAT(InvoiceDate, 'yyyy-MM-dd') AS SaleDate, 
                    SUM(TotalAmount) AS Revenue
                FROM Invoices
                GROUP BY FORMAT(InvoiceDate, 'yyyy-MM-dd')
                ORDER BY SaleDate DESC;";
            dgv2.DataSource = dbHelper.GetStatistics(query);
        }

        // 3. Thống kê lợi nhuận theo nhân viên
        private void LoadRevenue2()
        {
            string query = @"
                SELECT 
                    E.EmployeeID, 
                    E.EmployeeName, 
                    SUM(I.TotalAmount) AS Revenue,
                    SUM(ID.Quantity * (ID.UnitPrice - 0)) AS Profit
                FROM Employees E
                JOIN Invoices I ON E.EmployeeID = I.EmployeeID
                JOIN InvoiceDetails ID ON I.InvoiceID = ID.InvoiceID
                JOIN Products P ON ID.ProductID = P.ProductID
                GROUP BY E.EmployeeID, E.EmployeeName
                ORDER BY Profit DESC;";
            dgv3.DataSource = dbHelper.GetStatistics(query);
        }
    }
}
