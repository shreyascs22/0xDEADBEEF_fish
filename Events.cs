using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace music_management
{
    public partial class Events : Form
    {
        int user_id;
        public Events(int user_id)
        {
            InitializeComponent();
            this.user_id = user_id;
            LoadData();
        }

        private void LoadData()
        {
            comboBox1.Items.Clear();
            string connectionString = "Server=192.168.43.237;Database=dbs_project;Uid=root;Pwd=root;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            string query = "SELECT event_name FROM events";
            MySqlCommand command = new MySqlCommand(query, connection);
            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string value = reader.GetString(0);
                    comboBox1.Items.Add(value);
                }

                reader.Close();
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                string event_name = comboBox1.SelectedItem.ToString();
                string connectionString = "Server=192.168.43.237;Database=dbs_project;Uid=root;Pwd=root;";
                MySqlConnection connection = new MySqlConnection(connectionString);
                try
                {
                    connection.Open();
                    string eventid_query = "SELECT event_id FROM events WHERE event_name = @event_name";
                    MySqlCommand command = new MySqlCommand(eventid_query, connection);
                    command.Parameters.AddWithValue("@event_name", event_name);
                    int event_id = Convert.ToInt32(command.ExecuteScalar());

                    string query = "INSERT INTO attending (user_id,event_id) VALUES (@user_id,@event_id)";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@user_id", user_id);
                    cmd.Parameters.AddWithValue("@event_id", event_id);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("User inserted into attending table successfully.");
                    }
                    else
                    {
                        MessageBox.Show("Failed to insert user into attending table.");
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally { connection.Close(); }
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Events_Load(object sender, EventArgs e)
        {

        }
    }
}
