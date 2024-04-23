using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace music_management
{
    public partial class Rating : Form
    {
        private Dictionary<string, int> trackDictionary = new Dictionary<string, int>();
        private int user_id;

        public Rating(int user_id)
        {
            InitializeComponent();
            this.user_id = user_id;
            LoadTracks();
        }

        private void LoadTracks()
        {
            comboBox1.Items.Clear();
            trackDictionary.Clear();

            MySqlConnection connection = new MySqlConnection("Server=192.168.43.237;Database=dbs_project;Uid=root;Pwd=root;");
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT track_id, track_name FROM tracks";

            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int trackId = reader.GetInt32(0);
                    string trackName = reader.GetString(1);
                    trackDictionary.Add(trackName, trackId);
                    comboBox1.Items.Add(trackName);
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

        private void comboBoxTracks_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedTrackName = comboBox1.SelectedItem.ToString();
            int selectedTrackId = trackDictionary[selectedTrackName];
            MessageBox.Show($"Selected Track ID: {selectedTrackId}, Track Name: {selectedTrackName}");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                string track_name = comboBox1.SelectedItem.ToString();
                int track_id = trackDictionary[track_name];
                string input = textBox1.Text;
                int ratings = Convert.ToInt32(input);
                string connectionString = "Server=192.168.43.237;Database=dbs_project;Uid=root;Pwd=root;";
                MySqlConnection connection = new MySqlConnection(connectionString);
                string chk = "SELECT * FROM ratings where track_id = @track_id and user_id = @user_id";
                MySqlCommand command = new MySqlCommand(chk, connection);
                command.Parameters.AddWithValue("@track_id", track_id);
                command.Parameters.AddWithValue("@user_id", user_id);
                try
                {
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Close();
                        string query = "UPDATE ratings SET ratings = @ratings WHERE user_id = @user_id AND track_id = @track_id";
                        MySqlCommand cmd = new MySqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("@user_id", user_id);
                        cmd.Parameters.AddWithValue("@track_id", track_id);
                        cmd.Parameters.AddWithValue("@ratings", ratings);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Value updated successfully");
                        }
                        else
                        {
                            MessageBox.Show("Value was not updated");
                        }
                    }
                    else
                    {
                        reader.Close();
                        string query = "INSERT INTO ratings (user_id,track_id,ratings) VALUES (@user_id,@track_id,@ratings)";
                        MySqlCommand cmd = new MySqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("@user_id", user_id);
                        cmd.Parameters.AddWithValue("@track_id", track_id);
                        cmd.Parameters.AddWithValue("@ratings", ratings);
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
                MessageBox.Show("Choose a track to rate");
            }
        }

        private void Rating_Load(object sender, EventArgs e)
        {

        }
    }
}
