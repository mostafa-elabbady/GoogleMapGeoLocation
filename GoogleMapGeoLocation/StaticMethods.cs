using Microsoft.SqlServer.Types;
using Microsoft.Data.SqlClient;

namespace GoogleMapGeoLocation;

static class StaticMethods
{
    // Method to get addresses from the database
    public static List<Address> GetAddressesFromDatabase()
    {
        List<Address> addresses = new List<Address>();
        string connectionString = "Add Connection String here";
        using (var connection = new SqlConnection(connectionString))
        {
            string query = "SELECT Id, AddressLine1, City, State, Country, ZipCode FROM AddressTable";

            using (var command = new SqlCommand(query, connection))
            {
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Address address = new Address
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            AddressLine1 = reader["AddressLine1"] == DBNull.Value ? null : (string)reader["AddressLine1"],
                            City = reader["City"] == DBNull.Value ? null : (string)reader["City"],
                            State = reader["State"] == DBNull.Value ? null : (string)reader["State"],
                            Country = reader["Country"] == DBNull.Value ? null : (string)reader["Country"],
                            ZipCode = reader["ZipCode"] == DBNull.Value ? null : (string)reader["ZipCode"],
                            Coordinates = null
                        };

                        addresses.Add(address);
                    }
                }
            }
        }

        return addresses;
    }

    // Method to update Coordinates in the database
    public static void UpdateCoordinatesInDatabase(int Id, SqlGeography coordinates)
    {
        Console.WriteLine($"Updating Address Id {Id} and Coordinates {coordinates.STAsBinary()}");

        string connectionString = "Add Connection String here";

        using (var connection = new SqlConnection(connectionString))
        {
            string query = "UPDATE AddressTable SET Coordinates = @Coordinates WHERE Id = @AddressId";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Coordinates", coordinates.STAsBinary());
                command.Parameters.AddWithValue("@AddressId", Id);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }

}
