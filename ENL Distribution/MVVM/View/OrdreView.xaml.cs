using System;
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
    /// </summary>
    public partial class OrdreView : UserControl
    {
        public OrdreView()
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
                            string deleteQuery = "DELETE FROM [dbo].[Ordre] WHERE [OrdreID] = @OrdreID";

                            SqlCommand cmdDelete = new SqlCommand(deleteQuery, con);
                            cmdDelete.Parameters.AddWithValue("@OrdreID", row["OrdreID"]);

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
                        UPDATE [dbo].[Ordre]
                        SET [Medarbejder fuld navn] = @MedarbejderFuldNavn,
                            [Produkt navn] = @ProduktNavn,
                            [Produkt ID] = @ProduktID,
                            [Antal] = @Antal
                        WHERE [OrdreID] = @OrdreID";

                            SqlCommand cmdUpdate = new SqlCommand(updateQuery, con);

                            cmdUpdate.Parameters.AddWithValue("@MedarbejderFuldNavn", row["Fulde Navn"]);
                            cmdUpdate.Parameters.AddWithValue("@ProduktNavn", row["Produkt navn"]);
                            cmdUpdate.Parameters.AddWithValue("@ProduktID", row["Produkt ID"]);
                            cmdUpdate.Parameters.AddWithValue("@Antal", row["Antal"]);
                            cmdUpdate.Parameters.AddWithValue("@OrdreID", row["OrdreID"]);

                            int rowsAffected = await cmdUpdate.ExecuteNonQueryAsync();

                            if (rowsAffected == 0) // No rows were updated
                            {
                                // Perform an INSERT operation instead
                                string insertQuery = @"
                            INSERT INTO [dbo].[Ordre] ([Medarbejder fuld navn], [Produkt navn], [Produkt ID], [Antal])
                            VALUES (@MedarbejderFuldNavn, @ProduktNavn, @ProduktID, @Antal)";

                                SqlCommand cmdInsert = new SqlCommand(insertQuery, con);

                                cmdInsert.Parameters.AddWithValue("@MedarbejderFuldNavn", row["Fulde Navn"]);
                                cmdInsert.Parameters.AddWithValue("@ProduktNavn", row["Produkt navn"]);
                                cmdInsert.Parameters.AddWithValue("@ProduktID", row["Produkt ID"]);
                                cmdInsert.Parameters.AddWithValue("@Antal", row["Antal"]);

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
                                                    [OrdreID],
                                                    [Medarbejder fuld navn] AS 'Fulde Navn',
                                                    [Produkt navn],
                                                    [Produkt ID],
                                                    [Antal]
                                               FROM [dbo].[Ordre]", con);
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

