using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ENL_Distribution.MVVM.View
{
    public partial class MedarbejdereView : UserControl
    {
        public MedarbejdereView()
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
                            string deleteQuery = "DELETE FROM [dbo].[Medarbejdere] WHERE [MedArbejderID] = @MedArbejderID";

                            SqlCommand cmdDelete = new SqlCommand(deleteQuery, con);
                            cmdDelete.Parameters.AddWithValue("@MedArbejderID", row["ID"]);

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
                        UPDATE [dbo].[MedArbejdere]
                        SET [Navn] = @Navn,
                            [Telefon] = @Telefon,
                            [Email] = @Email,
                            [Titel] = @Titel,
                            [Ordre] = @Ordre
                        WHERE [MedArbejderID] = @MedArbejderID";

                            SqlCommand cmdUpdate = new SqlCommand(updateQuery, con);

                            cmdUpdate.Parameters.AddWithValue("@Navn", row["Navn"]);
                            cmdUpdate.Parameters.AddWithValue("@Telefon", row["Telefon"]);
                            cmdUpdate.Parameters.AddWithValue("@Email", row["Email"]);
                            cmdUpdate.Parameters.AddWithValue("@Titel", row["Titel"]);
                            cmdUpdate.Parameters.AddWithValue("@Ordre", row["Ordre"]);
                            cmdUpdate.Parameters.AddWithValue("@MedArbejderID", row["ID"]);

                            int rowsAffected = await cmdUpdate.ExecuteNonQueryAsync();

                            if (rowsAffected == 0) // No rows were updated
                            {
                                // Perform an INSERT operation instead
                                string insertQuery = @"
                            INSERT INTO [dbo].[MedArbejdere] ([Navn], [Telefon], [Email], [Titel], [Ordre])
                            VALUES (@Navn, @Telefon, @Email, @Titel, @Ordre)";

                                SqlCommand cmdInsert = new SqlCommand(insertQuery, con);

                                cmdInsert.Parameters.AddWithValue("@Navn", row["Navn"]);
                                cmdInsert.Parameters.AddWithValue("@Telefon", row["Telefon"]);
                                cmdInsert.Parameters.AddWithValue("@Email", row["Email"]);
                                cmdInsert.Parameters.AddWithValue("@Titel", row["Titel"]);
                                cmdInsert.Parameters.AddWithValue("@Ordre", row["Ordre"]);
                                cmdInsert.Parameters.AddWithValue("@MedArbejderID", row["ID"]);

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
                                                    [MedArbejderID] AS ID,
                                                    [Navn],
                                                    [Telefon],
                                                    [Email],
                                                    [Titel],
                                                    [Ordre]
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
