using MySql.Data.MySqlClient;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
namespace music_management
{
    public partial class signup : Form
    {
        public signup()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string user_name = textBox1.Text;
            string email = textBox2.Text;
            string password = textBox3.Text;
            string confirm_pwd = textBox4.Text;
            if (!(string.IsNullOrEmpty(user_name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirm_pwd) ))
            {
                if (password == confirm_pwd)
                {
                    string gender = "";
                    if (radioButton1.Checked)
                    {
                        gender = "M";
                    }
                    else if (radioButton2.Checked)
                    {
                        gender = "F";
                    }
                    else
                    {
                        MessageBox.Show("PLEASE ENTER YOUR GENDER");
                        return;
                    }
                    string connectionString = "Server=192.168.43.237;Database=dbs_project;Uid=root;Pwd=root;";
                    MySqlConnection connection = new MySqlConnection(connectionString);
                    try
                    {
                        connection.Open();
                        string query = "INSERT INTO users (user_name,email,password,gender) VALUES (@user_name, @email,@password,@gender)";
                        MySqlCommand cmd = new MySqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("@user_name", user_name);
                        cmd.Parameters.AddWithValue("@email",email);
                        cmd.Parameters.AddWithValue("@password", password);
                        cmd.Parameters.AddWithValue("@gender", gender);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            cmd.CommandText = "select last_insert_id()";
                            int userid = Convert.ToInt32(cmd.ExecuteScalar());
                            MessageBox.Show("Data inserted successfully!\nYour User Id is: " + userid);
                        }
                        else
                        {
                            MessageBox.Show("Failed to insert data!");
                        }
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
                else
                {
                    MessageBox.Show("THE PASSWORD DOES NOT MATCH THE CONFRM PASSWORD");
                }
            }
            else
            {
                MessageBox.Show("ONE OF THE FIELDS IS EMPTY");
            }
        }
    }
}
