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
    public partial class SellerForm : Form
    {
        public static string UserPswd = "";
        public static string UserName = "";
        public static string UserPhone = "";
        public static string UserMail = "";
        DataTable dTable;
        DataSet dSet;
        OleDbDataAdapter dAdapter;
        String strSQL;
        public SellerForm(string UP, string UN)
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
            strSQL = "SELECT * FROM Sellers WHERE IDSeller=" + UserPswd + " AND Name='" + UserName + "'";
            OleDbCommand cmdIC = new OleDbCommand(strSQL, cn);
            OleDbDataReader rdr = cmdIC.ExecuteReader();
            if (rdr.Read())
            {
                UserPhone = rdr["Phone"].ToString();
                UserMail = rdr["Email"].ToString();
            }
            rdr.Close();
            cmdIC.Dispose();
            textNameBox.Text = UserName;
            textPhoneBox.Text = UserPhone;
            textEmailBox.Text = UserMail;



            dSet = new DataSet();

            strSQL = "SELECT * FROM Sellers WHERE IDSeller<>"+UserPswd;
            dAdapter = new OleDbDataAdapter(strSQL, cn);
            dAdapter.Fill(dSet, "Sellers");
            dTable = dSet.Tables["Sellers"];
            dataGridView1.DataSource = dTable;
            dataGridView1.Update();
            strSQL = "SELECT * FROM Suppliers";
            dAdapter = new OleDbDataAdapter(strSQL, cn);
            dAdapter.Fill(dSet, "Suppliers");
            dTable = dSet.Tables["Suppliers"];
            dataGridView2.DataSource = dTable;
            dataGridView2.Update();
            strSQL = "SELECT * FROM Clients";
            dAdapter = new OleDbDataAdapter(strSQL, cn);
            dAdapter.Fill(dSet, "Clients");
            dTable = dSet.Tables["Clients"];
            dataGridView3.DataSource = dTable;
            dataGridView3.Update();
            strSQL = "SELECT IDdisc,IDSupplier,Supplier, Artist,Album,Country,Size,RecordYear,Style,TypeName,Price FROM SupplierDiscList";
            dAdapter = new OleDbDataAdapter(strSQL, cn);
            dAdapter.Fill(dSet, "SupplierDiscList");
            dTable = dSet.Tables["SupplierDiscList"];
            dataGridView4.DataSource = dTable;
            dataGridView4.Update();
            strSQL = "SELECT IDdisc,Artist, Album, Country, Size, RecordYear,Style,TypeName, Price FROM SellerDiscList WHERE IDSeller = " + UserPswd;
            dAdapter = new OleDbDataAdapter(strSQL, cn);
            dAdapter.Fill(dSet, "SellerDiscList");
            dTable = dSet.Tables["SellerDiscList"];
            dataGridView5.DataSource = dTable;
            dataGridView5.Update();
            strSQL = "SELECT IDOrder, IDClient,StatusOrder, OrderDate,SUMMA FROM OrdersList WHERE IDSeller =" + UserPswd;
            dAdapter = new OleDbDataAdapter(strSQL, cn);
            dAdapter.Fill(dSet, "OrderList");
            dTable = dSet.Tables["OrderList"];
            dataGridView6.DataSource = dTable;
            dataGridView6.Update();
            strSQL = "SELECT IDInvoice, Album,Supplier, StatusIn,Price,Number,SUMMA FROM InvoicesList WHERE IDSeller =" + UserPswd;
            dAdapter = new OleDbDataAdapter(strSQL, cn);
            dAdapter.Fill(dSet, "InvoicesList");
            dTable = dSet.Tables["InvoicesList"];
            dataGridView7.DataSource = dTable;
            dataGridView7.Update();
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;

            //string s = "SELECT MIN(OrderDate) AS MIN FROM Orders";
            //cmdIC = new OleDbCommand(s, cn);
            //rdr = cmdIC.ExecuteReader();
            //String Rmin = "";
            //if (rdr.Read())
            //{
            //    maskedTextBox1.Text = rdr["MIN"].ToString();
            //}
            //rdr.Close();
            ////maskedTextBox1.Text = Rmin;

            //s = "SELECT MAX(OrderDate) AS MAX FROM Orders";
            //cmdIC = new OleDbCommand(s, cn);
            //rdr = cmdIC.ExecuteReader();
            //String Rmax = "";
            //if (rdr.Read())
            //{
            //    maskedTextBox2.Text = rdr["MAX"].ToString();
            //}
            //rdr.Close();
            ////maskedTextBox2.Text = Rmax;

        }

        public void SellerRecieve(string UP, string UN)
        {
                    }

        private void SellerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            cn.Close();
        }

        private void buttonChange_Click(object sender, EventArgs e)
        {
            String strSQL = "UPDATE Sellers SET Name=?, Phone=?, Email=? WHERE IDSeller=?";
            OleDbCommand cmdIC = new OleDbCommand(strSQL, cn);
            cmdIC.Parameters.Add("@Name", OleDbType.VarChar);
            cmdIC.Parameters.Add("@Phone", OleDbType.VarChar);
            cmdIC.Parameters.Add("@Email", OleDbType.VarChar);
            cmdIC.Parameters.Add("@IDSeller", OleDbType.Integer);

            cmdIC.Parameters[0].Value = textNameBox.Text;
            cmdIC.Parameters[1].Value = textPhoneBox.Text;
            cmdIC.Parameters[2].Value = textEmailBox.Text;
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

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Хотите удалить учетную запись?", " Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                String strSQL = "EXEC Drop_Sellers "+UserPswd;
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

        private void SellerForm_Load(object sender, EventArgs e)
        {
            maskedTextBox1.ValidatingType = typeof(System.DateTime);
            maskedTextBox1.TypeValidationCompleted += new TypeValidationEventHandler(maskedTextBox1_TypeValidationCompleted);
            maskedTextBox2.ValidatingType = typeof(System.DateTime);
            maskedTextBox2.TypeValidationCompleted += new TypeValidationEventHandler(maskedTextBox2_TypeValidationCompleted);
        }

        private void button_Seller_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Хотите удалить учетную запись продавца?", " Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string strSQL = "EXEC Drop_Sellers " + dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString();
                OleDbCommand cmdIC = new OleDbCommand(strSQL, cn);
                try
                {
 
                    int iRowUp = cmdIC.ExecuteNonQuery();
                    MessageBox.Show("Orders: rows deleted - " + iRowUp.ToString());

                    strSQL = "SELECT * FROM Sellers WHERE IDSeller<>"+UserPswd;
                    dAdapter = new OleDbDataAdapter(strSQL, cn);
                    dSet.Tables["Sellers"].Clear();
                    dAdapter.Fill(dSet, "Sellers");
                    dTable = dSet.Tables["Sellers"];
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

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button_sup_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Хотите удалить учетную запись поставщика?", " Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string strSQL = "EXEC Drop_Sup " + dataGridView2.Rows[dataGridView2.CurrentRow.Index].Cells[0].Value.ToString();
                OleDbCommand cmdIC = new OleDbCommand(strSQL, cn);
                try
                {

                    int iRowUp = cmdIC.ExecuteNonQuery();
                    MessageBox.Show("Orders: rows deleted - " + iRowUp.ToString());

                    strSQL = "SELECT * FROM Suppliers";
                    dAdapter = new OleDbDataAdapter(strSQL, cn);
                    dSet.Tables["Suppliers"].Clear();
                    dAdapter.Fill(dSet, "Suppliers");
                    dTable = dSet.Tables["Suppliers"];
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

        private void button_myCl_Click(object sender, EventArgs e)
        {
            strSQL = "SELECT * FROM Clients WHERE IDClient IN (SELECT IDClient FROM Orders WHERE IDSeller="+UserPswd+")";
                dAdapter = new OleDbDataAdapter(strSQL, cn);
                dSet.Tables["Clients"].Clear();
                dAdapter.Fill(dSet, "Clients");
                dTable = dSet.Tables["Clients"];
                dataGridView3.DataSource = dTable;
                dataGridView3.Update();
        }

        private void button_ch_St_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Хотите изменить статус?", " Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int i = comboBox1.SelectedIndex+1;
                string strSQL = "UPDATE Orders SET IDStatus="+i+"WHERE IDOrder=" + dataGridView6.Rows[dataGridView6.CurrentRow.Index].Cells[0].Value.ToString();
                OleDbCommand cmdIC = new OleDbCommand(strSQL, cn);
                try
                {

                    int iRowUp = cmdIC.ExecuteNonQuery();
                    MessageBox.Show("Orders: rows updated - " + iRowUp.ToString());

                    strSQL = "SELECT IDOrder, IDClient,StatusOrder, OrderDate,SUMMA FROM OrdersList WHERE IDSeller =" + UserPswd;
                    dSet.Tables["OrderList"].Clear();
                    dAdapter = new OleDbDataAdapter(strSQL, cn);
                    dAdapter.Fill(dSet, "OrderList");
                    dTable = dSet.Tables["OrderList"];
                    dataGridView6.DataSource = dTable;
                    dataGridView6.Update();
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

        private void button_OK_Click(object sender, EventArgs e)
        {
            int i = comboBox3.SelectedIndex;
            string s1="";
            switch (i)
            {
                case 0:
                    {
                        s1 = "SELECT IDdisc,IDSupplier,Supplier, Artist,Album,Country,Size,RecordYear,Style,TypeName,Price FROM SupplierDiscList ";
                        if (checkBox1.Checked)
                        {
                            s1 += " WHERE TypeName = 'Оригинал'";
                        }
                        break;
                    }
                case 1:
                    {
                        s1 = "SELECT IDdisc,IDSupplier,Supplier, Artist,Album,Country,Size,RecordYear,Style,TypeName,Price FROM SupplierDiscList WHERE Size='7\"' ";
                        if (checkBox1.Checked)
                        {
                            s1 += " AND TypeName = 'Оригинал'";
                        }
                        break;
                    }
                case 2:
                    {
                        s1 = "SELECT IDdisc,IDSupplier,Supplier, Artist,Album,Country,Size,RecordYear,Style,TypeName,Price FROM SupplierDiscList WHERE Size='10\"' ";
                        if (checkBox1.Checked)
                        {
                            s1 += " AND TypeName = 'Оригинал'";
                        }
                        break;
                    }
                case 3:
                    {
                        s1 = "SELECT IDdisc,IDSupplier,Supplier, Artist,Album,Country,Size,RecordYear,Style,TypeName,Price FROM SupplierDiscList WHERE Size='12\"' ";
                        if (checkBox1.Checked)
                        {
                            s1 += " AND TypeName = 'Оригинал'";
                        }
                        break;
                    }
            }


            i = comboBox2.SelectedIndex;
            switch (i)
            {
                case 1 :{
                    s1 += " ORDER BY Price";
                    break;
                }
                case 2 :{
                    s1 += " ORDER BY Album";
                    break;
                }
                case 3:{
                    s1 += " ORDER BY RecordYear";
                    break;
                }
            }

            

            dAdapter = new OleDbDataAdapter(s1, cn);
            dSet.Tables["SupplierDiscList"].Clear();
            dAdapter.Fill(dSet, "SupplierDiscList");
            dTable = dSet.Tables["SupplierDiscList"];
            dataGridView4.DataSource = dTable;
            dataGridView4.Update();

        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            checkBox1.Checked = false;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            string s1 = "SELECT IDdisc,IDSupplier,Supplier, Artist,Album,Country,Size,RecordYear,Style,TypeName,Price FROM SupplierDiscList";
            dAdapter = new OleDbDataAdapter(s1, cn);
            dSet.Tables["SupplierDiscList"].Clear();
            dAdapter.Fill(dSet, "SupplierDiscList");
            dTable = dSet.Tables["SupplierDiscList"];
            dataGridView4.DataSource = dTable;
            dataGridView4.Update();

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= 47 || e.KeyChar >= 58) && e.KeyChar != 8)
                e.Handled = true;
        }

        private void button_KOL_OK_Click(object sender, EventArgs e)
        {
            String SupP = "";
            string s = "SELECT Price*"+textBox1.Text +" AS SUMMA FROM SupplierPrices WHERE IDdisc="+dataGridView4.Rows[dataGridView4.CurrentRow.Index].Cells[0].Value.ToString();
            OleDbCommand cmdIC = new OleDbCommand(s, cn);
            OleDbDataReader rdr = cmdIC.ExecuteReader();
            String Rsum = "";
            if (rdr.Read())
            {
                Rsum = rdr["SUMMA"].ToString();
            }
            rdr.Close();
            string str = "SELECT IDSupplierPrice FROM SupplierPrices WHERE IDdisc = " + dataGridView4.Rows[dataGridView4.CurrentRow.Index].Cells[0].Value.ToString() + " AND IDSupplier = "+dataGridView4.Rows[dataGridView4.CurrentRow.Index].Cells[1].Value.ToString();
            cmdIC = new OleDbCommand(str, cn);
            try
            {
                OleDbDataReader rdr1 = cmdIC.ExecuteReader();
                
                if (rdr1.Read())
                {
                    SupP = rdr1["IDSupplierPrice"].ToString();
                }
                rdr1.Close();
            }
            catch (OleDbException exc)
            {
                MessageBox.Show(exc.ToString());
            }
            if (MessageBox.Show("Хотите сделать новый заказ? Сумма заказа составляет :"+Rsum+"р.", " Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                strSQL = "EXEC Enter_Invoices " + SupP + ", " + UserPswd + ", " + textBox1.Text;
                cmdIC = new OleDbCommand(strSQL, cn);
                try
                {
                    int iRowUp = cmdIC.ExecuteNonQuery();
                    MessageBox.Show("Rows inserted - " + iRowUp.ToString());

                    strSQL = "SELECT IDInvoice, Album,Supplier, StatusIn,Price,Number,SUMMA FROM InvoicesList WHERE IDSeller =" + UserPswd;
                    dAdapter = new OleDbDataAdapter(strSQL, cn);
                    dSet.Tables["InvoicesList"].Clear();
                    dAdapter.Fill(dSet, "InvoicesList");
                    dTable = dSet.Tables["InvoicesList"];
                    dataGridView7.DataSource = dTable;
                    dataGridView7.Update();
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

        private void button_del_inv_Click(object sender, EventArgs e)
        {
            if (dataGridView7.Rows[dataGridView7.CurrentRow.Index].Cells[3].Value.ToString()!="Принято")
            {
                MessageBox.Show("Заказ уже выполнен!", " Внимание!", MessageBoxButtons.OK);
            }
            else{
            if (MessageBox.Show("Хотите отменить заказ?", " Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string strSQL = "DELETE Invoices WHERE IDinvoice=" + dataGridView7.Rows[dataGridView7.CurrentRow.Index].Cells[0].Value.ToString() + " AND IDSeller="+UserPswd;
                OleDbCommand cmdIC = new OleDbCommand(strSQL, cn);
                try
                {

                    int iRowUp = cmdIC.ExecuteNonQuery();
                    MessageBox.Show("Rows deleted: " + iRowUp.ToString());

                    strSQL = "SELECT IDInvoice, Album,Supplier, StatusIn,Price,Number,SUMMA FROM InvoicesList WHERE IDSeller =" + UserPswd;
                    dAdapter = new OleDbDataAdapter(strSQL, cn);
                    dSet.Tables["InvoicesList"].Clear();
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
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= 47 || e.KeyChar >= 58) && e.KeyChar != 8)
                e.Handled = true;
        }

        private void button_ch_pr_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
            {
                string strSQL = "UPDATE SellerPrices SET Price=" + textBox2.Text + "WHERE IDSeller=" + UserPswd + " AND IDdisc=" + dataGridView5.Rows[dataGridView5.CurrentRow.Index].Cells[0].Value.ToString();
                OleDbCommand cmdIC = new OleDbCommand(strSQL, cn);
                try
                {

                    int iRowUp = cmdIC.ExecuteNonQuery();
                    MessageBox.Show("Rows updated: " + iRowUp.ToString());

                    strSQL = "SELECT IDdisc,Artist, Album, Country, Size, RecordYear,Style,TypeName, Price FROM SellerDiscList WHERE IDSeller = " + UserPswd;
                    dSet.Tables["SellerDiscList"].Clear();
                    dAdapter = new OleDbDataAdapter(strSQL, cn);
                    dAdapter.Fill(dSet, "SellerDiscList");
                    dTable = dSet.Tables["SellerDiscList"];
                    dataGridView5.DataSource = dTable;
                    dataGridView5.Update();
                    textBox2.Text = "";
                }
                catch (OleDbException exc)
                {
                    MessageBox.Show(exc.ToString());
                }
            }
        }

        private void button_del_d_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Хотите удалить диск из списка?", " Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string strSQL = "DELETE SellerPrices WHERE IDdisc=" + dataGridView5.Rows[dataGridView5.CurrentRow.Index].Cells[0].Value.ToString() + " AND IDSeller=" + UserPswd;
                OleDbCommand cmdIC = new OleDbCommand(strSQL, cn);
                try
                {

                    int iRowUp = cmdIC.ExecuteNonQuery();
                    MessageBox.Show("Rows deleted: " + iRowUp.ToString());

                    strSQL = "SELECT IDdisc,Artist, Album, Country, Size, RecordYear,Style,TypeName, Price FROM SellerDiscList WHERE IDSeller = " + UserPswd;
                    dSet.Tables["SellerDiscList"].Clear();
                    dAdapter = new OleDbDataAdapter(strSQL, cn);
                    dAdapter.Fill(dSet, "SellerDiscList");
                    dTable = dSet.Tables["SellerDiscList"];
                    dataGridView5.DataSource = dTable;
                    dataGridView5.Update();


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

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void button_ins_d_Click(object sender, EventArgs e)
        {
            if (textBox3.Text != "")
            {
                string s1 = "SELECT IDdisc FROM SupplierPrices WHERE IDSupplierPrice=(SELECT IDSupplierPrice FROM Invoices WHERE IDinvoice = " + dataGridView7.Rows[dataGridView7.CurrentRow.Index].Cells[0].Value.ToString() + " AND IDSeller =" + UserPswd + ")";
                OleDbCommand cmdIC = new OleDbCommand(s1, cn);
                OleDbDataReader rdr = cmdIC.ExecuteReader();
                String Rid = "";
                if (rdr.Read())
                {
                    Rid = rdr["IDdisc"].ToString();
                }
                rdr.Close();
                string s2 = "SELECT COUNT(IDdisc) As COUNT FROM SellerDiscList WHERE IDdisc IN (SELECT IDdisc FROM SellerDiscList WHERE IDdisc="+Rid+" AND IDSeller="+UserPswd+")";
                cmdIC = new OleDbCommand(s2, cn);
                rdr = cmdIC.ExecuteReader();
                String Rcount = "";
                if (rdr.Read())
                {
                    Rcount = rdr["COUNT"].ToString();
                }
                rdr.Close();
                if (Rcount != "0") {
                    MessageBox.Show("Диск уже существует в списке товаров!", " Внимание!", MessageBoxButtons.OK);
                }else{
                if (MessageBox.Show("Хотите добавить диск в список своих товаров?", " Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    string strSQL = "Enter_Seller_Disc " + UserPswd + ", " + Rid + ", " + textBox3.Text;
                    cmdIC = new OleDbCommand(strSQL, cn);
                    try
                    {

                        int iRowUp = cmdIC.ExecuteNonQuery();
                        MessageBox.Show("Rows inserted: " + iRowUp.ToString());

                        strSQL = "SELECT IDdisc,Artist, Album, Country, Size, RecordYear,Style,TypeName, Price FROM SellerDiscList WHERE IDSeller = " + UserPswd;
                        dAdapter = new OleDbDataAdapter(strSQL, cn);
                        dSet.Tables["SellerDiscList"].Clear();
                        dAdapter.Fill(dSet, "SellerDiscList");
                        dTable = dSet.Tables["SellerDiscList"];
                        dataGridView5.DataSource = dTable;
                        dataGridView5.Update();
                        textBox3.Text = "";


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
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= 47 || e.KeyChar >= 58) && e.KeyChar != 8)
                e.Handled = true;
        }

        private void maskedTextBox1_TypeValidationCompleted(object sender, TypeValidationEventArgs e)
        {
        //    if (!e.IsValidInput)
        //    {
        //        toolTip1.ToolTipTitle = "Invalid Date Value";
        //        toolTip1.Show("We're sorry, but the value you entered is not a valid date. Please change the value.", maskedTextBox1, 5000);
        //        e.Cancel = true;
        //    }
        }

        private void maskedTextBox2_TypeValidationCompleted(object sender, TypeValidationEventArgs e)
        {
            //if (!e.IsValidInput)
            //{
            //    toolTip1.ToolTipTitle = "Invalid Date Value";
            //    toolTip1.Show("We're sorry, but the value you entered is not a valid date. Please change the value.", maskedTextBox1, 5000);
            //    e.Cancel = true;
            //}
        }

        private void button_OK_date_Click(object sender, EventArgs e)
        {
            maskedTextBox1.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
            maskedTextBox2.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
            if (maskedTextBox1.Text == "" || maskedTextBox2.Text == "")
            {
                MessageBox.Show("Введите даты!", " Внимание!", MessageBoxButtons.OK);
            } else { 
                string s1 = "'" + maskedTextBox1.Text + "'";
                string s2 = "'" + maskedTextBox2.Text + "'";
                chart1.Series["Sizes"].Points.Clear();
                string s = "EXEC Sellss_by_sizes " + s1 + ", " + s2 + ", " + UserPswd;
                OleDbCommand cmdIC = new OleDbCommand(s, cn);
                OleDbDataReader rdr = cmdIC.ExecuteReader();
                while (rdr.Read())
                {
                    chart1.Series["Sizes"].Points.AddXY(rdr["Size"].ToString(), rdr["Num"].ToString());
                }
                chart1.Visible = true;
                rdr.Close();


                chart2.Series["Types"].Points.Clear();
                s = "EXEC Sells_by_type " + s1 + ", " + s2 + ", " + UserPswd;
                cmdIC = new OleDbCommand(s, cn);
                rdr = cmdIC.ExecuteReader();
                while (rdr.Read())
                {
                    chart2.Series["Types"].Points.AddXY(rdr["TypeName"].ToString(), rdr["Num"].ToString());
                }
                chart2.Visible = true;
                rdr.Close();

                chart3.Series["Clients"].Points.Clear();
                s = "EXEC Sells_by_client " + s1 + ", " + s2 + ", " + UserPswd;
                cmdIC = new OleDbCommand(s, cn);
                rdr = cmdIC.ExecuteReader();
                while (rdr.Read())
                {
                    chart3.Series["Clients"].Points.AddXY(rdr["Name"].ToString(), rdr["Num"].ToString());
                }
                chart3.Visible = true;
                rdr.Close();

            }
        }
    }
}
