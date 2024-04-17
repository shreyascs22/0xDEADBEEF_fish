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

namespace music_management
{
    public partial class Listen : Form
    {
        private Dictionary<string, int> trackDictionary = new Dictionary<string, int>();
        private int user_id;
        public Listen(int user_id)
        {
            InitializeComponent();
            this.user_id = user_id;
            LoadTracks();
        }

        private void Listen_Load(object sender, EventArgs e)
        {

        }
        private void LoadTracks()
        {
            comboBox1.Items.Clear();
            trackDictionary.Clear();

            string connectionString = "Server=10.86.4.89;Database=dbs_project;Uid=root;Pwd=root;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT track_id, track_name FROM tracks";

            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int track_id = reader.GetInt32(0);
                    string track_name = reader.GetString(1);
                    comboBox1.Items.Add(track_name); 
                    trackDictionary.Add(track_name, track_id); 
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
        private void button1_Click_1(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                string track_name = comboBox1.SelectedItem.ToString();
                int track_id = trackDictionary[track_name];
                string connectionString = "Server=10.86.4.89;Database=dbs_project;Uid=root;Pwd=root;";
                MySqlConnection connection = new MySqlConnection(connectionString);
                try
                {
                    connection.Open();
                    string query = "INSERT INTO listens (user_id,track_id) VALUES (@user_id,@track_id)";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@user_id", user_id);
                    command.Parameters.AddWithValue("@track_id", track_id);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Form submitted successfully");
                    }
                    else
                    {
                        MessageBox.Show("Failed to insert data!");
                    }
                }
                catch(MySqlException ex)
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
                MessageBox.Show("Please select a track.");
            }
        }
    }
}
