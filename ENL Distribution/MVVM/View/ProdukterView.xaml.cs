using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ENL_Distribution.MVVM.View
{
    public partial class ProdukterView : UserControl
    {
        public ProdukterView()
        {
            InitializeComponent();
            loadGrid();
        }

        private void btnfortryd_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Er du sikker på du vil fortryde?", "Fortryd?", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                loadGrid();
                Datagrid.IsReadOnly = true;

            }
            else
            {

            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Datagrid.IsReadOnly = !Datagrid.IsReadOnly;
        }

        private void btnOpdater_Click(object sender, RoutedEventArgs e)
        {
            UpsertGrid();
        }

        private void btnSlet_Click(object sender, RoutedEventArgs e)
        {
            DeleteColum();
        }


        public async Task DeleteColum()

        {
            MessageBoxResult result = MessageBox.Show("Er du sikker på du vil slette de valgte rækker?", "Slet?", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    string connectionString = "Server=(localdb)\\local; Database=MVVMLoginDb; Integrated Security=True";

                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        await con.OpenAsync();

                        foreach (DataRowView row in Datagrid.SelectedItems)
                        {
                            string deleteQuery = "DELETE FROM [dbo].[Produkt] WHERE [ProduktID] = @ProduktID";

                            SqlCommand cmdDelete = new SqlCommand(deleteQuery, con);
                            cmdDelete.Parameters.AddWithValue("@ProduktID", row["ProduktID"]);

                            await cmdDelete.ExecuteNonQueryAsync();
                        }

                        // After deleting selected rows, reload the grid
                        await loadGrid();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }


        public async Task UpsertGrid()
        {
            MessageBoxResult result = MessageBox.Show("Er du sikker på du vil opdatere?", "Opdater?", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    string connectionString = "Server=(localdb)\\local; Database=MVVMLoginDb; Integrated Security=True";

                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        await con.OpenAsync();

                        foreach (DataRowView row in Datagrid.SelectedItems)
                        {
                            string updateQuery = @"
                        UPDATE [dbo].[Produkt]
                        SET [Navn] = @Navn,
                            [Antal] = @Antal,
                            [Beskrivelse] = @Beskrivelse,     
                            [Placering] = @Placering
                        WHERE [ProduktID] = @ProduktID";

                            SqlCommand cmdUpdate = new SqlCommand(updateQuery, con);

                            cmdUpdate.Parameters.AddWithValue("@ProduktID", row["ProduktID"]);
                            cmdUpdate.Parameters.AddWithValue("@Navn", row["Navn"]);
                            cmdUpdate.Parameters.AddWithValue("@Antal", row["Antal"]);
                            cmdUpdate.Parameters.AddWithValue("@Beskrivelse", row["Beskrivelse"]);
                            cmdUpdate.Parameters.AddWithValue("@Placering", row["Placering"]);

                            int rowsAffected = await cmdUpdate.ExecuteNonQueryAsync();

                            if (rowsAffected == 0) // No rows were updated
                            {
                                // Perform an INSERT operation instead
                                string insertQuery = @"
                            INSERT INTO [dbo].[Produkt] ([Navn], [Antal], [Beskrivelse], [Placering])
                            VALUES (@Navn, @Antal, @Beskrivelse, @Placering)";

                                SqlCommand cmdInsert = new SqlCommand(insertQuery, con);

                                cmdInsert.Parameters.AddWithValue("@Navn", row["Navn"]);
                                cmdInsert.Parameters.AddWithValue("@Antal", row["Antal"]);
                                cmdInsert.Parameters.AddWithValue("@Beskrivelse", row["Beskrivelse"]);
                                cmdInsert.Parameters.AddWithValue("@Placering", row["Placering"]);
                                cmdInsert.Parameters.AddWithValue("@ProduktID", row["ProduktID"]); // Add this line

                                await cmdInsert.ExecuteNonQueryAsync();
                            }
                        }

                        // After updating all selected rows or inserting new rows, reload the grid
                        await loadGrid();
                        Datagrid.IsReadOnly = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }



        public async Task loadGrid()
        {
            try
            {
                string connectionString = "Server=(localdb)\\local; Database=MVVMLoginDb; Integrated Security=True";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(@"SELECT 
                                                    [ProduktID],
                                                    [Navn],
                                                    [Antal],
                                                    [Beskrivelse],
                                                    [Placering]
                                               FROM [dbo].[Produkt]", con);
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

