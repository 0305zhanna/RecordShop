using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;

namespace RecordShop2
{
    public partial class RegForm : Form
    {
        private System.Data.OleDb.OleDbDataAdapter dAdapter;
        public static string UserName = "";
        public static string UserPhone = "";
        public static string UserD = "";

        public RegForm()
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

        private void buttonOk_Click(object sender, EventArgs e)
        {
            
            if (UserName == "")
            {
                MessageBox.Show("Введите имя!", "failed", MessageBoxButtons.OK);
                return;
            }
            if (UserPhone == "")
            {
                MessageBox.Show("Введите телефон!", "failed", MessageBoxButtons.OK);
                return;
            }
            if (UserD== "")
            {
                MessageBox.Show("Выберите деятельность!", "failed", MessageBoxButtons.OK);
                return;
            }
            int i = comboBox1.SelectedIndex;
            String Nid = "";
            switch (i)
            {
                case 0:
                    {
                        String strSQL = "EXEC Enter_Sellers ?,?";
                        
                        OleDbCommand cmdIC = new OleDbCommand(strSQL, cn);
                        cmdIC.Parameters.Add("@Name", OleDbType.VarChar);
                        cmdIC.Parameters.Add("@Phone", OleDbType.VarChar);
                        cmdIC.Parameters[0].Value = UserName;
                        cmdIC.Parameters[1].Value = UserPhone;
                        try{
                            int iRowAff = cmdIC.ExecuteNonQuery();
                            strSQL = "SELECT TOP 1 IDSeller FROM Sellers order by IDSeller desc";
                            OleDbCommand cmdIC2 = new OleDbCommand(strSQL, cn);
                            OleDbDataReader rdr = cmdIC2.ExecuteReader();
                            if (rdr.Read())
                            {
                               Nid = rdr["IDSeller"].ToString();
                            }
                            rdr.Close();
                            MessageBox.Show("Пароль:" + Nid/*iRowAff.ToString()*/);
                            this.Close();
                            cmdIC.Dispose();
                            iRowAff = 0;
                             }catch (OleDbException exc) {
                                 MessageBox.Show(exc.ToString());
                              }
                        break;
                    }
                case 1:
                    {
                        String strSQL = "EXEC Enter_Clients ?,?";
                        OleDbCommand cmdIC = new OleDbCommand(strSQL, cn);
                        cmdIC.Parameters.Add("@Name", OleDbType.VarChar);
                        cmdIC.Parameters.Add("@Phone", OleDbType.VarChar);
                        cmdIC.Parameters[0].Value = UserName;
                        cmdIC.Parameters[1].Value = UserPhone;
                        try
                        {
                            int iRowAff = cmdIC.ExecuteNonQuery();
                            strSQL = "SELECT TOP 1 IDClient FROM Clients order by IDClient desc";
                            OleDbCommand cmdIC2 = new OleDbCommand(strSQL, cn);
                            OleDbDataReader rdr = cmdIC2.ExecuteReader();
                            if (rdr.Read())
                            {
                                Nid = rdr["IDClient"].ToString();
                            }
                            rdr.Close();
                            MessageBox.Show("Пароль:" + Nid/*iRowAff.ToString()*/);
                            this.Close();
                            cmdIC.Dispose();
                            iRowAff = 0;
                        }
                        catch (OleDbException exc)
                        {
                            MessageBox.Show(exc.ToString());
                        }
                        break;
                    }
                case 2:
                    {
                        String strSQL = "EXEC Enter_Suppliers ?,?";
                        OleDbCommand cmdIC = new OleDbCommand(strSQL, cn);
                        cmdIC.Parameters.Add("@Name", OleDbType.VarChar);
                        cmdIC.Parameters.Add("@Phone", OleDbType.VarChar);
                        cmdIC.Parameters[0].Value = UserName;
                        cmdIC.Parameters[1].Value = UserPhone;
                        try
                        {
                            int iRowAff = cmdIC.ExecuteNonQuery();
                            strSQL = "SELECT TOP 1 IDSupplier FROM Suppliers order by IDSupplier desc";
                            OleDbCommand cmdIC2 = new OleDbCommand(strSQL, cn);
                            OleDbDataReader rdr = cmdIC2.ExecuteReader();
                            if (rdr.Read())
                            {
                                Nid = rdr["IDSupplier"].ToString();
                            }
                            rdr.Close();
                            MessageBox.Show("Пароль:" + Nid/*iRowAff.ToString()*/);
                            this.Close();
                            cmdIC.Dispose();
                            iRowAff = 0;
                        }
                        catch (OleDbException exc)
                        {
                            MessageBox.Show(exc.ToString());
                        }
                        break;
                    }

              }

                        
        }

        private void textName_TextChanged(object sender, EventArgs e)
        {
            UserName = textName.Text;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            UserPhone = textBox1.Text;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UserD = comboBox1.SelectedItem.ToString();
        }

        private void RegForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            cn.Close();
        }

        private void RegForm_Load(object sender, EventArgs e)
        {

        }
    }
}
