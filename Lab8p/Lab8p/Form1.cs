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

namespace Lab8p
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //string connectionString = @"Persist Security Info=False; User ID=" + textBox1.Text +"; Password=" + textBox2.Text + "; Initial Catalog=Polyclinic; Server=MACHYLOROMAN";

            SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder();
            connectionStringBuilder["Data Source"] = "MACHYLOROMAN";
            connectionStringBuilder["Initial Catalog"] = "Polyclinic";
            connectionStringBuilder["User ID"] = textBox1.Text;
            connectionStringBuilder["Password"] = textBox2.Text;
            connectionStringBuilder["Integrated Security"] = true;

            using (SqlConnection connection = new SqlConnection(connectionStringBuilder.ConnectionString))
            {
                try
                {
                    connection.Open();
                    Form2 form = new Form2(connectionStringBuilder.ConnectionString);
                    form.Show();
                    this.Hide();
                    MessageBox.Show("Вдалося успішно підключитися до " + connection.Database);
                }
                catch (Exception exception)
                {
                    MessageBox.Show("k " + exception.Message);
                }
            }
        }
    }
}
