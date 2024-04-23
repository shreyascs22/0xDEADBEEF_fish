using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace music_management
{
    public partial class subscriptions : Form
    {
        public subscriptions()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void subscriptions_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string user_id = textBox1.Text;
            if (!string.IsNullOrEmpty(user_id))
            {
                string type = "";
                DateTime startDate = DateTime.Now;
                DateTime endDate = DateTime.Now;

                if (radioButton1.Checked)
                {
                    type = "Monthly";
                    endDate = startDate.AddDays(30);
                }
                else if (radioButton2.Checked)
                {
                    type = "Yearly";
                    endDate = startDate.AddDays(365);
                }
                else
                {
                    MessageBox.Show("PLEASE CHOOSE THE TYPE OF SUBSCRIPTION");
                    return;
                }

                string connectionString = "Server=192.168.43.237;Database=dbs_project;Uid=root;Pwd=root;";
                MySqlConnection connection = null;
                try
                {
                    connection = new MySqlConnection(connectionString);
                    connection.Open();
                    string s = "SELECT user_id FROM users WHERE user_id = @user_id";
                    MySqlCommand command = new MySqlCommand(s, connection);
                    command.Parameters.AddWithValue("@user_id", user_id);
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        string query = "INSERT INTO subscriptions (user_id, type, start_date, end_date) VALUES (@user_id, @type, @start_date, @end_date)";
                        MySqlCommand cmd = new MySqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("@user_id", user_id);
                        cmd.Parameters.AddWithValue("@type", type);
                        cmd.Parameters.AddWithValue("@start_date", startDate);
                        cmd.Parameters.AddWithValue("@end_date", endDate);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Form submitted successfully");
                        }
                        else
                        {
                            MessageBox.Show("Failed to insert data!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("This user id does not exist");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    if (connection != null)
                    {
                        connection.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("User Id not entered");
            }
        }

    }
}
