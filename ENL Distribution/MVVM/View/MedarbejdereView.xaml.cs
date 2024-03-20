﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;

namespace ENL_Distribution.MVVM.View
{
    /// <summary>
    /// Interaction logic for Medarbejdere.xaml
    /// </summary>
    public partial class MedarbejdereView : UserControl
    {
        public MedarbejdereView()
        {
            InitializeComponent();
            loadGrid();
        }



        public async Task loadGrid()
        {
            try
            {
                string connectionString = "Server=(localdb)\\local; Database=MVVMLoginDb; Integrated Security=True";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(@"SELECT 
                                    [MedArbejderID],
                                    [Fulde Navn],
                                    [Telefon nr.],
                                    [Email],
                                    [Titel],
                                    [Udførte Ordre]
                                FROM [dbo].[Medarbejdere]", con);
                    DataTable dt = new DataTable();

                    await con.OpenAsync();

                    using (SqlDataReader sdr = await cmd.ExecuteReaderAsync())
                    {
                        dt.Load(sdr);
                    }

                    if (dt.Rows.Count > 0)
                    {
                        Datagrid.ItemsSource = dt.DefaultView;
                    }
                    else
                    {
                        MessageBox.Show("No records found.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

    }

}

