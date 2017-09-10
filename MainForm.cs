using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using System.Data;
using System.Data.OleDb;


namespace RecordShop2
{
    public partial class MainForm : Form
    {
        //private System.Data.OleDb.OleDbDataAdapter dAdapter;
        public static string UserName = "";
        public static string UserPswd = "";


        public MainForm()
        {
            InitializeComponent();
            try
            {
                cn.Open();
            }
            catch (Exception)
            {
                MessageBox.Show("Connection failed!!", "Connection", MessageBoxButtons.OK);
            }
        }
        
        private void textName_TextChanged(object sender, EventArgs e)
        {
            UserName = textName.Text;
        }
        
        private void textPswd_TextChanged(object sender, EventArgs e)
        {
            UserPswd = textPswd.Text;
        }

        private void EnterBtn_Click(object sender, EventArgs e)
        {
            String strSQL = "SELECT IDSeller,Name FROM Sellers WHERE IDSeller=" + UserPswd + " AND Name='" + UserName + "'";
            OleDbCommand cmdIC = new OleDbCommand(strSQL, cn);
            OleDbDataReader rdr = cmdIC.ExecuteReader();
            String Rid = "";
            String Rname = "";
            if (rdr.Read())
            {
                Rid = rdr["IDSeller"].ToString();
                Rname = rdr["Name"].ToString();
            }
            rdr.Close();
            if (UserPswd == Rid && UserName == Rname)
            {
                MessageBox.Show("Правильные имя и ID!");
              SellerForm  selform = new SellerForm(UserPswd, UserName);
                selform.Show();
            }
            if (Rid == "" && Rname == "")
            {
                strSQL = "SELECT IDClient,Name FROM Clients WHERE IDClient=" + UserPswd + " AND Name='" + UserName + "'";
                cmdIC = new OleDbCommand(strSQL, cn);
                rdr = cmdIC.ExecuteReader();
                if (rdr.Read())
                {
                    Rid = rdr["IDClient"].ToString();
                    Rname = rdr["Name"].ToString();
                }
                rdr.Close();
                if (UserPswd == Rid && UserName == Rname)
                {
                    MessageBox.Show("Правильные имя и ID!");
                    ClientForm cliform = new ClientForm(UserPswd, UserName);
                    cliform.Show();
                }
                if (Rid == "" && Rname == "")
                {
                    strSQL = "SELECT IDSupplier,Name FROM Suppliers WHERE IDSupplier=" + UserPswd + " AND Name='" + UserName + "'";
                    cmdIC = new OleDbCommand(strSQL, cn);
                    rdr = cmdIC.ExecuteReader();
                    if (rdr.Read())
                    {
                        Rid = rdr["IDSupplier"].ToString();
                        Rname = rdr["Name"].ToString();
                    }
                    rdr.Close();
                    if (UserPswd == Rid && UserName == Rname)
                    {
                        MessageBox.Show("Правильные имя и ID!");
                        SupplierForm supform = new SupplierForm(UserPswd, UserName);
                        supform.Show();
                    }
                }
            }
                if (UserPswd == Rid && UserName == Rname)
                {
                //    MessageBox.Show("Правильные имя и ID!");
                }
                else
                {
                    MessageBox.Show("Имя или ID введены неверно!");
                }
                textName.Text = "";
                textPswd.Text = "";
            }

        private void RegBtn_Click(object sender, EventArgs e)
        {

                RegForm regform = new RegForm();
                regform.Show();
         
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            cn.Close();
        }

    }
}
