using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp8
{
    public partial class Form2 : Form
    {
        DataBase dataBase = new DataBase();

        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataBase.OpenConnection();

            string namebook = textBox1.Text;
            string author = textBox2.Text;
            string path = textBox3.Text;
            var price =numericUpDown1.Value;

            var query = $"insert into books (name_book,author,photobook,pricebook) values ('{namebook}','{author}','{path}','{price}')";

            NpgsqlCommand command = new NpgsqlCommand(query,dataBase.GetConnection());
            
            command.ExecuteNonQuery();

            MessageBox.Show("данные добавлены", "успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

            dataBase.CloseConnection();

            Form1 form1 = new Form1();
            this.Hide();
            form1.ShowDialog();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                textBox3.Text = openFileDialog1.FileName;
        }
    }
}
