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
            comboBox1.Items.Clear();
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

        private void LoadPlaylist()
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

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
