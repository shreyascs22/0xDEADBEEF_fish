using Microsoft.VisualBasic.ApplicationServices;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace music_management
{
    public partial class Dashboard : Form
    {
        string username;
        int user_id;
        public Dashboard(string username, int user_id)
        {
            InitializeComponent();
            this.username = username;
            this.user_id = user_id;
            label2.Text = "Username: " + username;
            label3.Text = "UserID: " + user_id.ToString();
            label2.TabStop = false;
            label3.TabStop = false;
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            subscriptions sub = new subscriptions();
            sub.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string connectionString = "Server=10.86.4.89;Database=dbs_project;Uid=root;Pwd=root;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                string query = "SELECT user_id FROM subscriptions WHERE user_id = @user_id";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@user_id", user_id);
                object result = command.ExecuteScalar();
                if (result != null)
                {
                    Playlist pl = new Playlist(user_id);
                    pl.ShowDialog();
                }
            }
            catch (MySqlException ex) 
            {
                MessageBox.Show("Error: " + ex.Message);           
            }
            finally { connection.Close(); } 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Rating rate = new Rating(user_id);
            rate.ShowDialog();
        }
        private void dashboard_formclosing(object sender, FormClosingEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Listen li = new Listen(user_id);
            li.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string connectionString = "Server=10.86.4.89;Database=dbs_project;Uid=root;Pwd=root;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                string query = "SELECT user_id FROM subscriptions WHERE user_id = @user_id";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@user_id", user_id);
                object result = command.ExecuteScalar();
                if (result != null)
                {
                    Tracks tr = new Tracks(user_id);
                    tr.ShowDialog();
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally { connection.Close(); }    
        }
    }
}
