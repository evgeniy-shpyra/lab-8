using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clinic
{
    public partial class Form1 : Form
    {
        string connectionString = null;
        List<int> patientId = new List<int>();
        List<int> diagnosisId = new List<int>();
        List<int> employeeId = new List<int>();

        public Form1()
        {
            InitializeComponent();
            connectionString = BuildConnectionString();
            CreateListPatient();
            CreateListDiagnosis();
            CreateListEmployee();
            CreateListReceptionId();
            PopulateDataGridView();
        }

        private string BuildConnectionString()
        {
            SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder();
            connectionStringBuilder["Data Source"] = "MACHYLOROMAN";
            connectionStringBuilder["Initial Catalog"] = "Polyclinic1";
            connectionStringBuilder["User ID"] = "Roman";
            connectionStringBuilder["Password"] = "Roman";
            connectionStringBuilder["Integrated Security"] = true;

            return connectionStringBuilder.ConnectionString;
        }

        private void CreateListPatient()
        {
            comboBoxPatient.Items.Clear();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sql = "SELECT PatientID, Surname, Name_, Patronymic FROM Patient";
                    SqlCommand command = new SqlCommand(sql, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        patientId.Add(reader.GetInt32(0));
                        comboBoxPatient.Items.Add(reader.GetString(1) + " " + reader.GetString(2) + " " + reader.GetString(3));
                    }
                    reader.Close();
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Не вийшло прочитати пацієнта із БД. " + exception.Message);
                }
            }
        }

        private void CreateListDiagnosis()
        {
            comboBoxDiagnosis.Items.Clear();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sql = "SELECT DiagnosisID, Name_ FROM Diagnosis";
                    SqlCommand command = new SqlCommand(sql, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        diagnosisId.Add(reader.GetInt32(0));
                        comboBoxDiagnosis.Items.Add(reader.GetString(1));
                    }
                    reader.Close();
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Не вийшло прочитати діагноз із БД. " + exception.Message);
                }
            }
        }

        private void CreateListEmployee()
        {
            comboBoxEmployee.Items.Clear();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sql = "SELECT EmployeeID, Surname, Name_, Patronymic FROM Employee";
                    SqlCommand command = new SqlCommand(sql, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        employeeId.Add(reader.GetInt32(0));
                        comboBoxEmployee.Items.Add(reader.GetString(1) + " " + reader.GetString(2) + " " + reader.GetString(3));
                    }
                    reader.Close();
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Не вийшло прочитати співробітника із БД. " + exception.Message);
                }
            }
        }

        private void CreateListReceptionId()
        {
            comboBoxReceptionId.Items.Clear();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sql = "SELECT ReceptionID FROM Reception";
                    SqlCommand command = new SqlCommand(sql, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        comboBoxReceptionId.Items.Add(reader.GetInt32(0));
                    }
                    reader.Close();
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Не вийшло прочитати прийом із БД. " + exception.Message);
                }
            }
        }

        private void FillFields(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sql = "SELECT * FROM Reception WHERE ReceptionID = " + id;
                    SqlCommand command = new SqlCommand(sql, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int receptionId = reader.GetInt32(0);
                        int patientId = reader.GetInt32(1);
                        int employeeId = reader.GetInt32(2);
                        DateTime dateReception = reader.GetDateTime(3);
                        int numberOffice = reader.GetInt32(4);
                        int diagnosisId = reader.GetInt32(5);

                        DateTime? dateHospital = null;
                        if (!reader.IsDBNull(6)) 
                        {
                            dateHospital = reader.GetDateTime(6); 
                        }

                        int? termHospital = null;
                        if (!reader.IsDBNull(7))
                        {
                            termHospital = reader.GetInt32(7);
                        }

                        textBoxReceptionId.Text = receptionId.ToString();
                        comboBoxPatient.Text = patientId.ToString();
                        comboBoxEmployee.Text = employeeId.ToString();
                        textBoxDateReception.Text = dateReception.ToShortDateString();
                        textBoxNumberOffice.Text = numberOffice.ToString();
                        comboBoxDiagnosis.Text = diagnosisId.ToString();
                        textBoxDateHospital.Text = dateHospital != null ? dateHospital.Value.ToShortDateString() : string.Empty;
                        textBoxTermHospital.Text = termHospital != null ? termHospital.ToString() : string.Empty;
                    }
                    reader.Close();
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Не вийшло прочитати із БД. " + exception.Message);
                }
            }
        }

        private void PopulateDataGridView()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sql = "SELECT * FROM Reception";
                    SqlCommand command = new SqlCommand(sql, connection);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    dataGridView1.DataSource = table;
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Не вийшло прочитати із БД. " + exception.Message);
                }
            }
        }

        private bool IsFieldsEmpty()
        {
            if(string.IsNullOrWhiteSpace(textBoxDateReception.Text)  || string.IsNullOrWhiteSpace(textBoxNumberOffice.Text) || 
                string.IsNullOrWhiteSpace(comboBoxPatient.Text) || string.IsNullOrWhiteSpace(comboBoxEmployee.Text) || string.IsNullOrWhiteSpace(comboBoxDiagnosis.Text))
            {
                return true;
            }
            else
            {
                return false;
            }            
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (IsFieldsEmpty())
            {
                MessageBox.Show("Заповніть поля!");
            }
            else
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand();
                        command.Connection = connection;
                        command.CommandText = @"INSERT INTO Reception VALUES (@PatientID, @EmployeeID, @DateOfReception," +
                            "@CabinetNumber, @DiagnosisID, @DateOfHospitalization, @DurationOfHospitalization)";
                        

                        command.Parameters.Add("@PatientID", SqlDbType.Int);
                        command.Parameters.Add("@EmployeeID", SqlDbType.Int);
                        command.Parameters.Add("@DateOfReception", SqlDbType.Date);
                        command.Parameters.Add("@CabinetNumber", SqlDbType.Int);
                        command.Parameters.Add("@DiagnosisID", SqlDbType.Int);
                        command.Parameters.Add("@DateOfHospitalization", SqlDbType.Date);
                        command.Parameters.Add("@DurationOfHospitalization", SqlDbType.Int);

                        command.Parameters["@PatientID"].Value = patientId[comboBoxPatient.SelectedIndex];
                        command.Parameters["@EmployeeID"].Value = employeeId[comboBoxEmployee.SelectedIndex];
                        command.Parameters["@DateOfReception"].Value = textBoxDateReception.Text;
                        command.Parameters["@CabinetNumber"].Value = textBoxNumberOffice.Text;
                        command.Parameters["@DiagnosisID"].Value = diagnosisId[comboBoxDiagnosis.SelectedIndex];
                        if (string.IsNullOrWhiteSpace(textBoxDateHospital.Text))
                        {
                            command.Parameters["@DateOfHospitalization"].Value = DBNull.Value;
                        }
                        else
                        {
                            command.Parameters["@DateOfHospitalization"].Value = textBoxDateHospital.Text;
                        }

                        if (string.IsNullOrWhiteSpace(textBoxTermHospital.Text))
                        {
                            command.Parameters["@DurationOfHospitalization"].Value = DBNull.Value;
                        }
                        else
                        {
                            command.Parameters["@DurationOfHospitalization"].Value = textBoxTermHospital.Text;
                        }

                        command.ExecuteNonQuery();
                        CreateListReceptionId();
                        PopulateDataGridView();
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show("Не вийшло додати в БД " + exception.Message);
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboBoxReceptionId.Text))
            {
                MessageBox.Show("Виберіть прийом для видалення");
            }
            else
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand();
                        command.Connection = connection;
                        command.CommandText = @"DELETE Reception WHERE ReceptionID = @ReceptionID";
                        command.Parameters.Add("@ReceptionId", SqlDbType.Int);

                        command.Parameters["@ReceptionID"].Value = comboBoxReceptionId.Text;
                        command.ExecuteNonQuery();
                        PopulateDataGridView();
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show("Не получилось удалить из БД. " + exception.Message);
                    }
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxReceptionId.Text) || IsFieldsEmpty())
            {
                MessageBox.Show("Заповніть поля!");
            }
            else
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();
                    SqlCommand command = new SqlCommand();
                    command.Transaction = transaction;
                    try
                    {
                        command.Connection = connection;
                        command.CommandText = @"UPDATE Reception SET PatientId = @PatientID, EmployeeID = @EmployeeID," +
                            "DateOfReception = @DateOfReception, CabinetNumber = @CabinetNumber," +
                            "DiagnosisID = @DiagnosisID, DateOfHospitalization = @DateOfHospitalization, DurationOfHospitalization = @DurationOfHospitalization WHERE ReceptionID = @ReceptionID";
                        command.Parameters.Add("@ReceptionID", SqlDbType.Int);
                        command.Parameters.Add("@PatientID", SqlDbType.Int);
                        command.Parameters.Add("@EmployeeID", SqlDbType.Int);
                        command.Parameters.Add("@DateOfReception", SqlDbType.Date);
                        command.Parameters.Add("@CabinetNumber", SqlDbType.Int);
                        command.Parameters.Add("@DiagnosisID", SqlDbType.Int);
                        command.Parameters.Add("@DateOfHospitalization", SqlDbType.Date);
                        command.Parameters.Add("@DurationOfHospitalization", SqlDbType.Int);

                        command.Parameters["@ReceptionID"].Value = textBoxReceptionId.Text;
                        command.Parameters["@PatientID"].Value = patientId[comboBoxPatient.SelectedIndex];
                        command.Parameters["@EmployeeID"].Value = employeeId[comboBoxEmployee.SelectedIndex];
                        command.Parameters["@DateOfReception"].Value = textBoxDateReception.Text;
                        command.Parameters["@CabinetNumber"].Value = textBoxNumberOffice.Text;
                        command.Parameters["@DiagnosisID"].Value = diagnosisId[comboBoxDiagnosis.SelectedIndex];
                        if (string.IsNullOrWhiteSpace(textBoxDateHospital.Text))
                        {
                            command.Parameters["@DateOfHospitalization"].Value = DBNull.Value;
                        }
                        else
                        {
                            command.Parameters["@DateOfHospitalization"].Value = textBoxDateHospital.Text;
                        }

                        if (string.IsNullOrWhiteSpace(textBoxTermHospital.Text))
                        {
                            command.Parameters["@DurationOfHospitalization"].Value = DBNull.Value;
                        }
                        else
                        {
                            command.Parameters["@DurationOfHospitalization"].Value = textBoxTermHospital.Text;
                        }
                        command.ExecuteNonQuery();

                        //PopulateDataGridView();
                        //string sql = "SELECT * FROM Reception";
                        //command = new SqlCommand(sql, connection);
                        command.CommandText = "SELECT * FROM Reception";
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        dataGridView1.DataSource = table;

                        transaction.Commit();
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show("Не вийшло зберегти зміни в БД. " + exception.Message);
                        transaction.Rollback();
                    }
                }
            }
        }

        private void comboBoxReceptionId_SelectedIndexChanged(object sender, EventArgs e)
        {
            int a = Convert.ToInt32(comboBoxReceptionId.Text);
            FillFields(a);
        }

        private void btnStoredProcedure_Click(object sender, EventArgs e)
        {
            string sqlExpression = "NumberRceptionsPerDayByEmployees2"; //Имя хранимой процедуры
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    //Указываем, что команда представляет хранимую процедуру
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    //Задаём параметры
                    command.Parameters.Add(new SqlParameter("@Recept_count", textBoxReceptCount.Text));
                    //var result = command.ExecuteReader();
                    //Если нам не надо возвращать id
                    //var result = command.ExecuteNonQuery();
                    //Console.WriteLine("Id добавленного объекта: {0}", result);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    dataGridView1.DataSource = table;
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Не вийшло запустити збережену процедуру. " + exception.Message);
                }
            }
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            PopulateDataGridView();
        }
    }
}
