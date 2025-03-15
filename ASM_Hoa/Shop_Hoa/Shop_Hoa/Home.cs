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
    public partial class Home : Form
    {
        private List<Button> menuButtons;
        public Home()
        {
            InitializeComponent();
       
            menuButtons = new List<Button> { btnHistory, btnDashBoard, btnInvoices, btnProducts, btnCustomers, btnLogout };

            foreach (Button btn in menuButtons)
            {
                btn.Click += Button_Click;
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            foreach (Button btn in menuButtons)
            {
                btn.BackColor = SystemColors.Control;
                btn.ForeColor = Color.Blue;
            }

            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                clickedButton.BackColor = Color.DarkOrange;
                clickedButton.ForeColor = Color.White;
            }
        }

        private void LoadUserControl(UserControl uc)
        {
            panelMain.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            panelMain.Controls.Add(uc);
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to Exit", "Ok", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();

            }
            else
            {
                return;
            }
        }


        private void panelHeader_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, panelHeader.ClientRectangle, Color.Black, ButtonBorderStyle.Solid);
        }

        private void btnLogout_Click_1(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to Logout?", "Ok", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                //Application.Exit();
                this.Hide(); 
                Login loginForm = new Login();
                loginForm.Show();

            }
            else
            {
                return;
            }
        }

        private void btnProducts_Click_1(object sender, EventArgs e)
        {
            LoadUserControl(new Products());
        }

        private void btnCustomers_Click_1(object sender, EventArgs e)
        {
            LoadUserControl(new Customers());
        }

        private void btnInvoices_Click_1(object sender, EventArgs e)
        {
            LoadUserControl(new Invoices());
        }

        private void btnDashBoard_Click_1(object sender, EventArgs e)
        {
            LoadUserControl(new DashBoard());
        }

        private void btnHistory_Click_1(object sender, EventArgs e)
        {
            LoadUserControl(new History());
        }

 
      
    }
}

