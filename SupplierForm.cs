using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace RecordShop2
{
    public partial class SupplierForm : Form
    {
        public static string UserPswd = "";
        public static string UserName = "";
        public static string UserPhone = "";
        public static string UserMail = "";
        DataTable dTable;
        DataSet dSet;
        OleDbDataAdapter dAdapter;
        String strSQL;
        public SupplierForm(string UP, string UN)
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
            UserPswd = UP;
            UserName = UN;
            strSQL = "SELECT * FROM Suppliers WHERE IDSupplier=" + UserPswd + " AND Name='" + UserName + "'";
            OleDbCommand cmdIC = new OleDbCommand(strSQL, cn);
            OleDbDataReader rdr = cmdIC.ExecuteReader();
            if (rdr.Read())
            {
                UserPhone = rdr["Phone"].ToString();
                UserMail = rdr["Addres"].ToString();
            }
            rdr.Close();
            cmdIC.Dispose();
            textBox_name.Text = UserName;
            textBox_phone.Text = UserPhone;
            textBox_adres.Text = UserMail;



            dSet = new DataSet();

            strSQL = "SELECT IDinvoice, Album,IDSeller,Seller,StatusIn,Price,Number,SUMMA FROM InvoicesList WHERE IDSupplier="+UserPswd;
            dAdapter = new OleDbDataAdapter(strSQL, cn);
            dAdapter.Fill(dSet, "InvoicesList");
            dTable = dSet.Tables["InvoicesList"];
            dataGridView1.DataSource = dTable;
            dataGridView1.Update();

            strSQL = "SELECT IDdisc,Artist,Album,Country,Size,RecordYear,Style,TypeName,Price FROM SupplierDiscList WHERE IDSupplier=" + UserPswd;
            dAdapter = new OleDbDataAdapter(strSQL, cn);
            dAdapter.Fill(dSet, "SupplierDiscList");
            dTable = dSet.Tables["SupplierDiscList"];
            dataGridView2.DataSource = dTable;
            dataGridView2.Update();

            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;

        }

        private void button_change_Click(object sender, EventArgs e)
        {
            String strSQL = "UPDATE Suppliers SET Name=?, Phone=?, Addres=? WHERE IDSupplier=?";
            OleDbCommand cmdIC = new OleDbCommand(strSQL, cn);
            cmdIC.Parameters.Add("@Name", OleDbType.VarChar);
            cmdIC.Parameters.Add("@Phone", OleDbType.VarChar);
            cmdIC.Parameters.Add("@Addres", OleDbType.VarChar);
            cmdIC.Parameters.Add("@IDSupplier", OleDbType.Integer);

            cmdIC.Parameters[0].Value = textBox_name.Text;
            cmdIC.Parameters[1].Value = textBox_phone.Text;
            cmdIC.Parameters[2].Value = textBox_adres.Text;
            cmdIC.Parameters[3].Value = UserPswd;

            try
            {
                int iRowUp = cmdIC.ExecuteNonQuery();
                MessageBox.Show("Orders: rows updated - " + iRowUp.ToString());

            }
            catch (OleDbException exc)
            {
                MessageBox.Show(exc.ToString());
            }
            cmdIC.Dispose();

        }

        private void button_del_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Хотите удалить учетную запись?", " Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                String strSQL = "EXEC Drop_sup " + UserPswd;
                OleDbCommand cmdIC = new OleDbCommand(strSQL, cn);
                try
                {
                    int iRowUp = cmdIC.ExecuteNonQuery();
                    MessageBox.Show("Orders: rows deleted - " + iRowUp.ToString());
                    this.Close();

                }
                catch (OleDbException exc)
                {
                    MessageBox.Show(exc.ToString());
                }
            }
            else
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Хотите изменить статус?", " Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int i = comboBox1.SelectedIndex + 1;
                string strSQL = "UPDATE Invoices SET IDStatus=" + i + "WHERE IDInvoice=" + dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString();
                OleDbCommand cmdIC = new OleDbCommand(strSQL, cn);
                try
                {

                    int iRowUp = cmdIC.ExecuteNonQuery();
                    MessageBox.Show("Orders: rows updated - " + iRowUp.ToString());
                    strSQL = "SELECT IDinvoice, Album,IDSeller,Seller,StatusIn,Price,Number,SUMMA FROM InvoicesList WHERE IDSupplier=" + UserPswd;
                    dSet.Tables["InvoicesList"].Clear();
                    dAdapter = new OleDbDataAdapter(strSQL, cn);
                    dAdapter.Fill(dSet, "InvoicesList");
                    dTable = dSet.Tables["InvoicesList"];
                    dataGridView1.DataSource = dTable;
                    dataGridView1.Update();
                }
                catch (OleDbException exc)
                {
                    MessageBox.Show(exc.ToString());
                }
            }
            else
            {

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Хотите удалить диск из списка?", " Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string strSQL = "EXEC Drop_Disc " + dataGridView2.Rows[dataGridView2.CurrentRow.Index].Cells[0].Value.ToString()+", "+UserPswd;
                OleDbCommand cmdIC = new OleDbCommand(strSQL, cn);
                try
                {

                    int iRowUp = cmdIC.ExecuteNonQuery();
                    MessageBox.Show("Rows deleted - " + iRowUp.ToString());

                    strSQL = "SELECT IDdisc,Artist,Album,Country,Size,RecordYear,Style,TypeName,Price FROM SupplierDiscList WHERE IDSupplier=" + UserPswd;
                    dAdapter = new OleDbDataAdapter(strSQL, cn);
                    dSet.Tables["SupplierDiscList"].Clear();
                    dAdapter.Fill(dSet, "SupplierDiscList");
                    dTable = dSet.Tables["SupplierDiscList"];
                    dataGridView2.DataSource = dTable;
                    dataGridView2.Update();


                }
                catch (OleDbException exc)
                {
                    MessageBox.Show(exc.ToString());
                }
            }
            else
            {

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox_Album.Text=="" || textBox_Artist.Text=="" || textBox_Country.Text=="" || textBox_price.Text=="" || textBox_style.Text=="" || textBox_Year.Text=="")
            {
                MessageBox.Show("Все поля должны быть заполнены!", " Внимание!", MessageBoxButtons.OK);
            }else{
                int i1 = comboBox2.SelectedIndex + 1;
                int i2 = comboBox3.SelectedIndex + 1;
                string strSQL = "EXEC Enter_Supplier_Disc " + textBox_Artist.Text + ", " + textBox_Album.Text + ", " + textBox_Country.Text + ", " + i1+ ", " + textBox_Year.Text + ", " + textBox_style.Text + ", " + i2+ ", " + UserPswd + ", " + textBox_price.Text;
                OleDbCommand cmdIC = new OleDbCommand(strSQL, cn);
                try
                {

                    int iRowUp = cmdIC.ExecuteNonQuery();
                    MessageBox.Show("Rows inserted - " + iRowUp.ToString());

                    strSQL = "SELECT IDdisc,Artist,Album,Country,Size,RecordYear,Style,TypeName,Price FROM SupplierDiscList WHERE IDSupplier=" + UserPswd;
                    dAdapter = new OleDbDataAdapter(strSQL, cn);
                    dSet.Tables["SupplierDiscList"].Clear();
                    dAdapter.Fill(dSet, "SupplierDiscList");
                    dTable = dSet.Tables["SupplierDiscList"];
                    dataGridView2.DataSource = dTable;
                    dataGridView2.Update();


                }
                catch (OleDbException exc)
                {
                    MessageBox.Show(exc.ToString());
                }
            }
        }
    }
}
