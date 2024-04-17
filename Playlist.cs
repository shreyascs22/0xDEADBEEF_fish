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

namespace music_management
{
    public partial class Playlist : Form
    {
        int user_id;
        public Playlist(int user_id)
        {
            InitializeComponent();
            this.user_id = user_id;
        }

        private void Playlist_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string playlist_name = textBox1.Text;
            string connectionString = "Server=10.86.4.89;Database=dbs_project;Uid=root;Pwd=root;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                string query = "INSERT INTO playlists (playlist_name) VALUES (@playlist_name)";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@playlist_name", playlist_name);
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    string last = "SELECT LAST_INSERT_ID()";
                    MySqlCommand command = new MySqlCommand(last, connection);
                    int playlist_id = Convert.ToInt32(command.ExecuteScalar());

                    string query_user = "INSERT INTO user_playlists (playlist_id,user_id) VALUES (@playlist_id,@user_id)";
                    MySqlCommand cmd1 = new MySqlCommand(query_user, connection);
                    cmd1.Parameters.AddWithValue("@user_id", user_id);
                    cmd1.Parameters.AddWithValue("@playlist_id", playlist_id);
                    int userPlaylistsRowsAffected = cmd1.ExecuteNonQuery();

                    if (userPlaylistsRowsAffected > 0)
                    {
                        MessageBox.Show("Playlist added successfully");
                    }
                    else
                    {
                        MessageBox.Show("Failed to add playlist for the user");
                    }
                }
                else
                {
                    MessageBox.Show("Failed to insert playlist");
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
