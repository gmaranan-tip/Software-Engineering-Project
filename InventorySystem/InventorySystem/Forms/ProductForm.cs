using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using FontAwesome.Sharp;

namespace InventorySystem.Forms
{
    public partial class ProductForm : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security=True");
        private IconButton currentBtn;
        public ProductForm()
        {
            InitializeComponent();
        }

        private struct RGBColors
        {
            public static Color color1 = Color.FromArgb(69, 120, 170);
        }
        private void ActiveButton(object senderBtn, Color color)
        {
            if (senderBtn != null)
            {
                DisableButton();

                currentBtn = (IconButton)senderBtn;
                currentBtn.BackColor = Color.FromArgb(36, 37, 81);
                currentBtn.ForeColor = color;
                currentBtn.IconColor = color;
                currentBtn.IconChar = IconChar.FolderOpen;
            }
        }

        private void DisableButton()
        {
            if (currentBtn != null)
            {
                currentBtn.BackColor = Color.FromArgb(20, 35, 70);
                currentBtn.ForeColor = Color.Gainsboro;
                currentBtn.IconColor = Color.Gainsboro;
                currentBtn.IconChar = IconChar.FolderPlus;
            }
        }

        public void fill_product_name()
        {
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Select * from productTable order by Product_Name";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                comboBox1.Items.Add(dr["Product_Name"].ToString());
            }
        }

        public void fill_product_units()
        {
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Select * from unitsTable order by Unit_List";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                comboBox2.Items.Add(dr["Unit_List"].ToString());
            }
        }

        public void fill_dg()
        {
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from stockTable order by Product_Name";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            metroGrid2.DataSource = dt;
        }



        private void iconButton1_Click(object sender, EventArgs e)
        {

            int i;
            SqlCommand cmd1 = con.CreateCommand();
            cmd1.CommandType = CommandType.Text;
            cmd1.CommandText = "Select * from stockTable where Product_Name='" + comboBox1.Text + "' and Product_Unit='" + comboBox2.Text + "'";
            cmd1.ExecuteNonQuery();
            DataTable dt1 = new DataTable();
            SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
            da1.Fill(dt1);
            i = Convert.ToInt32(dt1.Rows.Count.ToString());

            if (i == 0)
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "insert into quantityTable values('" + comboBox1.SelectedItem.ToString() + "','" + Convert.ToInt32(textBox1.Text) + "','" + comboBox2.SelectedItem.ToString() + "')";
                cmd.ExecuteNonQuery();

                SqlCommand cmd3 = con.CreateCommand();
                cmd3.CommandType = CommandType.Text;
                cmd3.CommandText = "insert into stockTable values('" + comboBox1.SelectedItem.ToString() + "','" + Convert.ToInt32(textBox1.Text) + "','" + comboBox2.SelectedItem.ToString() + "')";
                cmd3.ExecuteNonQuery();

                fill_dg();
                metroGrid2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                MessageBox.Show("Item '" + comboBox1.SelectedItem.ToString() + "' '" + comboBox2.SelectedItem.ToString() + "' Successfully Updated!");
            }
            else
            {
                SqlCommand cmd2 = con.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.CommandText = "insert into quantityTable values('" + comboBox1.SelectedItem.ToString() + "','" + Convert.ToInt32(textBox1.Text) + "','" + comboBox2.SelectedItem.ToString() + "')";
                cmd2.ExecuteNonQuery();

                SqlCommand cmd4 = con.CreateCommand();
                cmd4.CommandType = CommandType.Text;
                cmd4.CommandText = "update stockTable set Product_Qty=Product_Qty + " + textBox1.Text + " where Product_Name='" + comboBox1.Text + "' and Product_Unit='" + comboBox2.Text + "'";
                cmd4.ExecuteNonQuery();

                fill_dg();
                metroGrid2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                MessageBox.Show("Item '" + comboBox1.SelectedItem.ToString() + "' '" + comboBox2.SelectedItem.ToString() + "' Successfully Updated!");
            }
        }

        private void ProductForm_Load(object sender, EventArgs e)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            fill_product_name();
            fill_product_units();
            fill_dg();
            metroGrid2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            int id;
            id = Convert.ToInt32(metroGrid2.SelectedCells[0].Value.ToString());
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "delete from stockTable where Id= " + id + "";
            cmd.ExecuteNonQuery();
            fill_dg();
        }

        private void iconButton3_Click(object sender, EventArgs e)
        {
            ActiveButton(sender, RGBColors.color1);

            NewProduct np = new NewProduct();
            np.TopMost = true;
            np.Show();
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            iconButton2.Visible = false;
            DisableButton();
        }

        private void panel6_MouseClick(object sender, MouseEventArgs e)
        {
            iconButton2.Visible = false;
            DisableButton();
        }

        private void panel2_MouseClick(object sender, MouseEventArgs e)
        {
            iconButton2.Visible = false;
            DisableButton();
        }

        private void comboBox1_MouseClick(object sender, MouseEventArgs e)
        {
            iconButton2.Visible = false;
            DisableButton();
        }

        private void metroGrid2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            iconButton2.Visible = true;
        }
    }
}
