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
using System.Data.SqlClient;

namespace RecordShop2
{
    public partial class ClientForm : Form
    {
        public static string UserPswd = "";
        public static string UserName = "";
        public static string UserPhone = "";
        public static string UserMail = "";
        int n = 0;
        DataTable dTable;
        DataSet dSet;
        OleDbDataAdapter dAdapter;
        String strSQL;
        public ClientForm(string UP, string UN)
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
            strSQL = "SELECT * FROM Clients WHERE IDClient=" + UserPswd + " AND Name='" + UserName + "'";
            OleDbCommand cmdIC = new OleDbCommand(strSQL, cn);
            OleDbDataReader rdr = cmdIC.ExecuteReader();
            if (rdr.Read())
            {
                UserPhone = rdr["Phone"].ToString();
                UserMail = rdr["Email"].ToString();
            }
            rdr.Close();
            cmdIC.Dispose();
            textBox_name.Text = UserName;
            textBox_phone.Text = UserPhone;
            textBox_email.Text = UserMail;

            dSet = new DataSet();

            strSQL = "SELECT * FROM SellerDiscList";
            dAdapter = new OleDbDataAdapter(strSQL, cn);
            dAdapter.Fill(dSet, "SellerDiscList");
            dTable = dSet.Tables["SellerDiscList"];
            dataGridView1.DataSource = dTable;
            dataGridView1.Update();

            strSQL = "SELECT IDdisc, Album,IDSeller, 1 AS Number,Price FROM SellerDiscList WHERE 1=2";
            dAdapter = new OleDbDataAdapter(strSQL, cn);            
            dAdapter.Fill(dSet, "basket");
            dTable = dSet.Tables["basket"];
            dataGridView2.DataSource = dTable;
            dataGridView2.Update();

            strSQL = "SELECT IDOrder, IDSeller,StatusOrder, OrderDate,SUMMA FROM OrdersList WHERE IDClient = "+UserPswd+" ORDER BY IDOrder";
            dAdapter = new OleDbDataAdapter(strSQL, cn);
            dAdapter.Fill(dSet, "OrdersList");
            dTable = dSet.Tables["OrdersList"];
            dataGridView3.DataSource = dTable;
            dataGridView3.Update();

            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;


        }

        private void button_change_Click(object sender, EventArgs e)
        {
            String strSQL = "UPDATE Clients SET Name=?, Phone=?, Email=? WHERE IDClient=?";
            OleDbCommand cmdIC = new OleDbCommand(strSQL, cn);
            cmdIC.Parameters.Add("@Name", OleDbType.VarChar);
            cmdIC.Parameters.Add("@Phone", OleDbType.VarChar);
            cmdIC.Parameters.Add("@Email", OleDbType.VarChar);
            cmdIC.Parameters.Add("@IDSeller", OleDbType.Integer);

            cmdIC.Parameters[0].Value = textBox_name.Text;
            cmdIC.Parameters[1].Value = textBox_phone.Text;
            cmdIC.Parameters[2].Value = textBox_email.Text;
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
                String strSQL = "EXEC Drop_client " + UserPswd;
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

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox_count_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= 47 || e.KeyChar >= 58) && e.KeyChar != 8)
                e.Handled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string s1 = "";
            int i = 0;
            int j = 0;
            if (textBox1.Text == "")
            {
                s1 = "SELECT * FROM SellerDiscList";
                i = comboBox2.SelectedIndex;
                switch(i)
                {
                    case 0:
                        {
                            if (checkBox1.Checked)
                            {
                                s1 += " WHERE TypeName = 'Оригинал'";
                            }
                            break;
                        }
                    case 1:
                        {
                            s1 += " WHERE Size = '7\"'";
                            if (checkBox1.Checked)
                            {
                                s1 += " AND TypeName = 'Оригинал'";
                            }
                            break;
                        }
                    case 2:
                        {
                            s1 += " WHERE Size = '10\"'";
                            if (checkBox1.Checked)
                            {
                                s1 += " AND TypeName = 'Оригинал'";
                            }
                            break;
                        }
                    case 3:
                        {
                            s1 += " WHERE Size = '12\"'";
                            if (checkBox1.Checked)
                            {
                                s1 += " AND TypeName = 'Оригинал'";
                            }
                            break;
                        }
                }
                i = comboBox3.SelectedIndex;
                switch (i)
                {
                    case 1:
                        {
                            s1 += " ORDER BY Price";
                            break;
                        }
                    case 2:
                        {
                            s1 += " ORDER BY Album";
                            break;
                        }
                    case 3:
                        {
                            s1 += " ORDER BY RecordYear";
                            break;
                        }
                }
            }
            else
            {
                i = comboBox1.SelectedIndex;
                switch (i)
                {
                    case 0:
                        {
                            s1 = "SELECT * FROM SellerDiscList WHERE Album = '"+textBox1.Text+"'";
                            if (checkBox1.Checked)
                            {
                                s1 += " AND TypeName = 'Оригинал'";
                            }
                            j = comboBox2.SelectedIndex;
                            switch (j)
                            {
                                case 0:
                                    {
                                        if (checkBox1.Checked)
                                        {
                                            s1 += " AND TypeName = 'Оригинал'";
                                        }
                                        break;
                                    }
                                case 1:
                                    {
                                        s1 += " AND Size = '7\"'";
                                        break;
                                    }
                                case 2:
                                    {
                                        s1 += " AND Size = '10\"'";
                                        break;
                                    }
                                case 3:
                                    {
                                        s1 += " AND Size = '12\"'";
                                        break;
                                    }
                            }
                            j = comboBox3.SelectedIndex;
                            switch (j)
                            {
                                case 1:
                                    {
                                        s1 += " ORDER BY Price";
                                        break;
                                    }
                                case 2:
                                    {
                                        s1 += " ORDER BY Album";
                                        break;
                                    }
                                case 3:
                                    {
                                        s1 += " ORDER BY RecordYear";
                                        break;
                                    }
                            }
                            break;
                        }
                    case 1:
                        {
                            s1 = "SELECT * FROM SellerDiscList WHERE Artist = '"+textBox1.Text+"'";
                            if (checkBox1.Checked)
                            {
                                s1 += " AND TypeName = 'Оригинал'";
                            }
                            j = comboBox2.SelectedIndex;
                            switch (j)
                            {
                                case 1:
                                    {
                                        s1 += " AND Size = '7\"'";
                                        break;
                                    }
                                case 2:
                                    {
                                        s1 += " AND Size = '10\"'";
                                        break;
                                    }
                                case 3:
                                    {
                                        s1 += " AND Size = '12\"'";
                                        break;
                                    }
                            }
                            j = comboBox3.SelectedIndex;
                            switch (j)
                            {
                                case 1:
                                    {
                                        s1 += " ORDER BY Price";
                                        break;
                                    }
                                case 2:
                                    {
                                        s1 += " ORDER BY Album";
                                        break;
                                    }
                                case 3:
                                    {
                                        s1 += " ORDER BY RecordYear";
                                        break;
                                    }
                            }
                            break;
                        }
                    case 2:
                        {
                            s1 = "SELECT * FROM SellerDiscList WHERE Country = '"+textBox1.Text+"'";
                            if (checkBox1.Checked)
                            {
                                s1 += " AND TypeName = 'Оригинал'";
                            }
                            j = comboBox2.SelectedIndex;
                            switch (j)
                            {
                                case 1:
                                    {
                                        s1 += " AND Size = '7\"'";
                                        break;
                                    }
                                case 2:
                                    {
                                        s1 += " AND Size = '10\"'";
                                        break;
                                    }
                                case 3:
                                    {
                                        s1 += " AND Size = '12\"'";
                                        break;
                                    }
                            }
                            j = comboBox3.SelectedIndex;
                            switch (j)
                            {
                                case 1:
                                    {
                                        s1 += " ORDER BY Price";
                                        break;
                                    }
                                case 2:
                                    {
                                        s1 += " ORDER BY Album";
                                        break;
                                    }
                                case 3:
                                    {
                                        s1 += " ORDER BY RecordYear";
                                        break;
                                    }
                            }
                            break;
                        }
                    case 3:
                        {

                            s1 = "SELECT * FROM SellerDiscList WHERE RecordYear = '" + textBox1.Text + "'";

                            if (checkBox1.Checked)
                            {
                                s1 += " AND TypeName = 'Оригинал'";
                            }
                            j = comboBox2.SelectedIndex;
                            switch (j)
                            {
                                case 1:
                                    {
                                        s1 += " AND Size = '7\"'";
                                        break;
                                    }
                                case 2:
                                    {
                                        s1 += " AND Size = '10\"'";
                                        break;
                                    }
                                case 3:
                                    {
                                        s1 += " AND Size = '12\"'";
                                        break;
                                    }
                            }
                            j = comboBox3.SelectedIndex;
                            switch (j)
                            {
                                case 1:
                                    {
                                        s1 += " ORDER BY Price";
                                        break;
                                    }
                                case 2:
                                    {
                                        s1 += " ORDER BY Album";
                                        break;
                                    }
                                case 3:
                                    {
                                        s1 += " ORDER BY RecordYear";
                                        break;
                                    }
                            }
                            break;
                        }
                    case 4:
                        {
                            s1 = "SELECT * FROM SellerDiscList WHERE Style = '" + textBox1.Text+"'";
                            if (checkBox1.Checked)
                            {
                                s1 += " AND TypeName = 'Оригинал'";
                            }
                            j = comboBox2.SelectedIndex;
                            switch (j)
                            {
                                case 1:
                                    {
                                        s1 += " AND Size = '7\"'";
                                        break;
                                    }
                                case 2:
                                    {
                                        s1 += " AND Size = '10\"'";
                                        break;
                                    }
                                case 3:
                                    {
                                        s1 += " AND Size = '12\"'";
                                        break;
                                    }
                            }
                            j = comboBox3.SelectedIndex;
                            switch (j)
                            {
                                case 1:
                                    {
                                        s1 += " ORDER BY Price";
                                        break;
                                    }
                                case 2:
                                    {
                                        s1 += " ORDER BY Album";
                                        break;
                                    }
                                case 3:
                                    {
                                        s1 += " ORDER BY RecordYear";
                                        break;
                                    }
                            }
                            break;
                        }
                }


            }

            dAdapter = new OleDbDataAdapter(s1, cn);
            dSet.Tables["SellerDiscList"].Clear();
            dAdapter.Fill(dSet, "SellerDiscList");
            dTable = dSet.Tables["SellerDiscList"];
            dataGridView1.DataSource = dTable;
            dataGridView1.Update();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView3.Rows[dataGridView3.CurrentRow.Index].Cells[2].Value.ToString() != "Ожидает оплаты")
            {
                MessageBox.Show("Заказ нельзя отменить!", " Внимание!", MessageBoxButtons.OK);
            }
            else
            {

                if (MessageBox.Show("Хотите отменить заказ?", " Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    String strSQL = "EXEC Drop_Order " + dataGridView3.Rows[dataGridView3.CurrentRow.Index].Cells[0].Value.ToString();
                    OleDbCommand cmdIC = new OleDbCommand(strSQL, cn);
                    try
                    {
                        int iRowUp = cmdIC.ExecuteNonQuery();
                        MessageBox.Show("Orders: rows deleted - " + iRowUp.ToString());

                        strSQL = "SELECT IDOrder, IDSeller,StatusOrder, OrderDate,SUMMA FROM OrdersList WHERE IDClient = " + UserPswd + " ORDER BY IDOrder";
                        dSet.Tables["OrdersList"].Clear();
                        dAdapter = new OleDbDataAdapter(strSQL, cn);
                        dAdapter.Fill(dSet, "OrdersList");
                        dTable = dSet.Tables["OrdersList"];
                        dataGridView3.DataSource = dTable;
                        dataGridView3.Update();

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

        private void button_ok_count_Click(object sender, EventArgs e)
        {
            if (textBox_count.Text == "")
            {
                MessageBox.Show("Введите количество!", " Внимание!", MessageBoxButtons.OK);
            }
            else
            {
                string s1 = "SELECT IDdisc, Album,IDSeller, " + textBox_count.Text + " AS Number,Price FROM SellerDiscList WHERE IDdisc=" + dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString();
                dAdapter = new OleDbDataAdapter(s1, cn);

                dAdapter.Fill(dSet, "basket");
                dTable = dSet.Tables["basket"];
                dataGridView2.DataSource = dTable;
                dataGridView2.Update();
 
                if((dataGridView2.Rows.Count - 1)<0){
                       dataGridView2.CurrentCell = dataGridView2.Rows[0].Cells[0];
 
                    }else{
                    dataGridView2.CurrentCell = dataGridView2.Rows[dataGridView2.Rows.Count - 2].Cells[0];
                }
                int count = 0;
                int price = 0;
                try
                {
                    count = Convert.ToInt32(dataGridView2[3, dataGridView2.CurrentRow.Index].Value.ToString());
                    price = Convert.ToInt32(dataGridView2[4, dataGridView2.CurrentRow.Index].Value.ToString());
                   // Console.WriteLine(dataGridView2[3, dataGridView1.CurrentRow.Index-1].Value.ToString());
                }
                catch (FormatException e1)
                {
                    Console.WriteLine(e1.Message);
                }
                n += price * count;
                textBox2.Text = n.ToString();
            }
        }

        protected int SqlProcedureCall(string procName, SqlParameter[] paramArray)
        {
            using (SqlConnection cnect = new SqlConnection("Data Source=Asus\\SQLEXPRESS;Persist Security Info=True;Password=4pm;User ID=sa;Initial Catalog=RecordShop"))
            {
                cnect.Open();
                SqlCommand cmd = new SqlCommand(procName, cnect);//Установка имени хранимой процедуры
                cmd.CommandType = CommandType.StoredProcedure;//тип хранимой процедуры

                foreach (SqlParameter param in paramArray)
                    cmd.Parameters.Add(param);
                return int.Parse(cmd.ExecuteScalar().ToString());
            }
        }
        private void button_ok_order_Click(object sender, EventArgs e)
        {
            bool b = true;
            for (int i = 0; i < dataGridView2.Rows.Count-1; i++)
            {
                if (dataGridView2[2, 0].Value.ToString() != dataGridView2[2, i].Value.ToString())
                {
                    b = false;
                    break;
                }
            }
            if (b)
            {
            if (MessageBox.Show("Хотите сделать новый заказ? В заказ можно добавлять диски только одного продавца!", " Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {

                    DateTime thisDay = DateTime.Today;
                    DataTable table = new DataTable();
                    //table.Columns.Add(new DataColumn("id", typeof(int)));
                    table.Columns.Add(new DataColumn("disc", typeof(int)));
                    table.Columns.Add(new DataColumn("Num", typeof(int)));

                    for (int i = 0; i < dataGridView2.Rows.Count - 1; i++)
                    {
                        var row = table.NewRow();
                        // row["id"] = i + 1;
                        row["disc"] = Convert.ToInt32(dataGridView2[0, i].Value);
                        row["Num"] = Convert.ToInt32(dataGridView2[3, i].Value);
                        table.Rows.Add(row);
                    }

                    string connectionString = "Data Source=Asus\\SQLEXPRESS;Persist Security Info=True;Password=4pm;User ID=sa;Initial Catalog=RecordShop";
                    SqlConnection conn = new SqlConnection();
                    conn.ConnectionString = connectionString;
                    SqlCommand myCommand = conn.CreateCommand();
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.CommandText = "[Enter_Order]";
                    myCommand.Parameters.Add("@IDClient", SqlDbType.Int);
                    myCommand.Parameters["@IDClient"].Value = Int32.Parse(UserPswd);
                    myCommand.Parameters.Add("@IDSeller", SqlDbType.Int);
                    myCommand.Parameters["@IDSeller"].Value = Int32.Parse(dataGridView2[2, 0].Value.ToString());
                    myCommand.Parameters.Add("@OrderDate", SqlDbType.Date);
                    myCommand.Parameters["@OrderDate"].Value = DateTime.Now.Date;// "'2016-12-21'";
                    myCommand.Parameters.Add("@tbl1", SqlDbType.Structured);
                    myCommand.Parameters["@tbl1"].Value = table;
                    conn.Open();
                    int UspeshnoeIzmenenie = myCommand.ExecuteNonQuery();
                    if (UspeshnoeIzmenenie != 0)
                    {
                        MessageBox.Show("Изменения внесены", "Изменение записи");
                    }
                    else
                    {
                        MessageBox.Show("Не удалось внести изменения", "Изменение записи");
                    }
                    conn.Close();


                    strSQL = "SELECT IDOrder, IDSeller,StatusOrder, OrderDate,SUMMA FROM OrdersList WHERE IDClient = " + UserPswd + " ORDER BY IDOrder";
                    dAdapter = new OleDbDataAdapter(strSQL, cn);
                    dSet.Tables["OrdersList"].Clear();
                    dAdapter.Fill(dSet, "OrdersList");
                    dTable = dSet.Tables["OrdersList"];
                    dataGridView3.DataSource = dTable;
                    dataGridView3.Update();
                    n = 0;
                    textBox2.Text = "";
                    dSet.Tables["basket"].Clear();
                }
                else
                {

                }
            }
            else
            {
                MessageBox.Show("Вы не можете добавлять в заказ продукцию от разных продавцов!", "Создание заказа");
                n = 0;
                textBox2.Text = "";
                dSet.Tables["basket"].Clear();
            }

        }

        private void button_canc_Click(object sender, EventArgs e)
        {
            n = 0;
            textBox2.Text = "";
            dSet.Tables["basket"].Clear();
        }

    }
}
