using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace music_management
{
    public partial class Tracks : Form
    {
        int user_id;
        public Tracks(int user_id)
        {
            InitializeComponent();
            this.user_id = user_id;
            LoadPlaylist();
            LoadTracks();
        }
        private void LoadTracks()
        {
            comboBox2.Items.Clear();
            string connectionString = "Server=10.86.4.89;Database=dbs_project;Uid=root;Pwd=root;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            string query = "SELECT track_name FROM tracks";
            MySqlCommand command = new MySqlCommand(query, connection);
            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string value = reader.GetString(0);
                    comboBox2.Items.Add(value);
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

        private void LoadPlaylist()
        {
            comboBox1.Items.Clear();
            string connectionString = "Server=10.86.4.89;Database=dbs_project;Uid=root;Pwd=root;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            string query = "SELECT playlist_name FROM playlists WHERE playlist_id IN (SELECT playlist_id FROM user_playlists WHERE user_id = @user_id)";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@user_id", user_id);
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
            if (comboBox1.SelectedItem != null && comboBox2.SelectedItem != null)
            {
                string track_name = comboBox2.SelectedItem.ToString();
                string playlist_name = comboBox1.SelectedItem.ToString();
                string connectionString = "Server=10.86.4.89;Database=dbs_project;Uid=root;Pwd=root;";
                MySqlConnection connection = new MySqlConnection(connectionString);
                try
                {
                    connection.Open();
                    string trackid_query = "SELECT track_id FROM tracks WHERE track_name = @track_name";
                    MySqlCommand track_cmd = new MySqlCommand(trackid_query, connection);
                    track_cmd.Parameters.AddWithValue("@track_name", track_name);
                    int track_id = Convert.ToInt32(track_cmd.ExecuteScalar());

                    string playlist_query = "SELECT playlist_id FROM playlists WHERE playlist_name = @playlist_name";
                    MySqlCommand playlist_cmd = new MySqlCommand(playlist_query, connection);
                    playlist_cmd.Parameters.AddWithValue("@playlist_name", playlist_name);
                    int playlist_id = Convert.ToInt32(playlist_cmd.ExecuteScalar());

                    string query = "INSERT INTO track_playlists (playlist_id,track_id) VALUES (@playlist_id,@track_id)";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@playlist_id", playlist_id);
                    cmd.Parameters.AddWithValue("@track_id", track_id);
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
                catch (MySqlException ex)
                {
                    MessageBox.Show("Database Error: " + ex.Message);
                }
                catch (Exception ex)
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
                MessageBox.Show("Select the playlist or tracks");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                string playlist_name = comboBox1.SelectedItem.ToString();
                string connectionString = "Server=10.86.4.89;Database=dbs_project;Uid=root;Pwd=root;";
                MySqlConnection connection = new MySqlConnection(connectionString);
                try
                {
                    connection.Open();
                    string playlistid_query = "SELECT playlist_id FROM playlists WHERE playlist_id IN (SELECT playlist_id FROM user_playlists where user_id = @user_id) and playlist_name = @playlist_name";
                    MySqlCommand playlistid_command = new MySqlCommand(playlistid_query, connection);
                    playlistid_command.Parameters.AddWithValue("@user_id", user_id);
                    playlistid_command.Parameters.AddWithValue("@playlist_name", playlist_name);
                    int playlist_id = Convert.ToInt32(playlistid_command.ExecuteScalar());

                    string count_query = "SELECT COUNT(*) FROM track_playlists WHERE playlist_id = @playlist_id";
                    MySqlCommand count_command = new MySqlCommand(count_query, connection);
                    count_command.Parameters.AddWithValue("@playlist_id", playlist_id);
                    int trackCount = Convert.ToInt32(count_command.ExecuteScalar());

                    if (trackCount > 0)
                    {
                        string artistid_query = "SELECT artist_id FROM (SELECT artist_id, COUNT(artist_id) AS artist_count FROM artist_tracks WHERE track_id IN (SELECT track_id FROM track_playlists WHERE playlist_id = @playlist_id) GROUP BY artist_id) AS subquery ORDER BY artist_count DESC LIMIT 1";
                        MySqlCommand artistid_command = new MySqlCommand(artistid_query, connection);
                        artistid_command.Parameters.AddWithValue("@playlist_id", playlist_id);
                        int artist_id = Convert.ToInt32(artistid_command.ExecuteScalar());

                        string tracks_query = "SELECT track_name FROM tracks WHERE track_id IN (SELECT track_id FROM artist_tracks WHERE track_id NOT IN (SELECT track_id FROM track_playlists WHERE playlist_id = @playlist_id) AND artist_id = @artist_id)";
                        MySqlCommand tracks_command = new MySqlCommand(tracks_query, connection);
                        tracks_command.Parameters.AddWithValue("@playlist_id", playlist_id);
                        tracks_command.Parameters.AddWithValue("@artist_id", artist_id);
                        MySqlDataReader reader;
                        reader = tracks_command.ExecuteReader();
                        StringBuilder sb = new StringBuilder();
                        while (reader.Read())
                        {
                            sb.AppendLine(reader.GetString(0));
                        }
                        MessageBox.Show("Recommended Tracks:\n" + sb.ToString());
                    }
                    else
                    {
                        string allTracks_query = "SELECT track_name FROM tracks";
                        MySqlCommand allTracks_command = new MySqlCommand(allTracks_query, connection);
                        MySqlDataReader reader;
                        reader = allTracks_command.ExecuteReader();
                        StringBuilder sb = new StringBuilder();
                        while (reader.Read())
                        {
                            sb.AppendLine(reader.GetString(0));
                        }
                        MessageBox.Show("Recommended Tracks:\n" + sb.ToString());
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
        }
    }
}
