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
    public partial class Form1 : Form
    {

        DataBase dataBase = new DataBase();

        int selected;

        enum RowState
        {
            New,
            Deleted,
            ModifiedNew,
            Modified
        }

        public Form1()
        {
            InitializeComponent();
        }

        public void CreateColumns()
        {
            dataGridView1.Columns.Add("id_book", "number");
            dataGridView1.Columns.Add("name_book", "название книги");
            dataGridView1.Columns.Add("author", "автор");
            dataGridView1.Columns.Add("pricebook", "цена");
            dataGridView1.Columns.Add("photobook", "фото");  
           
            dataGridView1.Columns.Add("IsNew", String.Empty);

            dataGridView1.Columns["IsNew"].Visible = false;
            dataGridView1.Columns["photobook"].Visible = false;
            dataGridView1.Columns["id_book"].Visible = false;

        }

        public void ReadSingleRow(DataGridView gridView,IDataRecord record)
        {

            gridView.Rows.Add(record.GetInt64(0),record.GetString(1),record.GetString(2),record.GetInt32(3),record.GetString(4),RowState.Modified);
        }

        public void RefreshData(DataGridView gridView)
        {
            gridView.Rows.Clear();

            dataBase.OpenConnection();

            string query = $"select id_book,name_book,author,pricebook,photobook from books";

            NpgsqlCommand command = new NpgsqlCommand(query,dataBase.GetConnection());

            NpgsqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow(gridView,reader);
            }

            reader.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CreateColumns();
            RefreshData(dataGridView1);
            textBox3.Visible = false;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selected = e.RowIndex;

            if(e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[selected];
                string path = row.Cells[4].Value.ToString();

                if(!string.IsNullOrEmpty(path))
                {
                    pictureBox1.ImageLocation = path;
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }

            if(selected >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[selected];

                textBox1.Text = row.Cells[1].Value.ToString();
                textBox2.Text = row.Cells[2].Value.ToString();
                textBox3.Text = row.Cells[0].Value.ToString();
            }
        }

        public void DeleteData()
        {
            int index = dataGridView1.CurrentCell.RowIndex;

            dataGridView1.Rows[index].Visible = false;

            if (dataGridView1.Rows[index].Cells[0].Value.ToString() == string.Empty)
            {
                dataGridView1.Rows[index].Cells[4].Value = RowState.Deleted;
                return;
            }
            dataGridView1.Rows[index].Cells[4].Value = RowState.Deleted;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DeleteData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataBase.OpenConnection();

            string id = textBox3.Text;

            string query = $"delete from books where id_book = " + id;

            NpgsqlCommand cmd = new NpgsqlCommand(query,dataBase.GetConnection());

            cmd.ExecuteNonQuery();

            dataBase.CloseConnection();
        }

      

        private void button3_Click(object sender, EventArgs e)
        {
          
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dataBase.OpenConnection();

            var id = Convert.ToInt64(textBox3.Text);
            var namebook = textBox1.Text;
            var author = textBox2.Text;

            var query = $"UPDATE books SET name_book = '{namebook}',author = '{author}' where id_book = '{id}'";

            NpgsqlCommand cmd = new NpgsqlCommand(query, dataBase.GetConnection());

            cmd.ExecuteNonQuery();

            MessageBox.Show("данные изменены","успех",MessageBoxButtons.OK, MessageBoxIcon.Information);

            RefreshData(dataGridView1);

            dataBase.CloseConnection();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            this.Hide();
            form2.ShowDialog();
            this.Close();
        }
    }
}
