using GoogleMapGeoLocation;
using Microsoft.SqlServer.Types;
using Newtonsoft.Json;
using System.Net;

string apiKey = "GoogleAPIKey";

List<Address> addresses = StaticMethods.GetAddressesFromDatabase();

CancellationTokenSource source = new CancellationTokenSource();
var options = new ParallelOptions { MaxDegreeOfParallelism = 10, CancellationToken = source.Token };
try
{
    await Parallel.ForEachAsync(addresses, options, async (address, token) =>
    {
        // Create a WebClient to make the request
        using (WebClient client = new WebClient())
        {
            string fullAddress = $"{address.AddressLine1}, {address.City}, {address.State} {address.ZipCode}";
            string apiUrl = $"https://maps.googleapis.com/maps/api/geocode/json?address={WebUtility.UrlEncode(fullAddress)}&key={apiKey}";

            string jsonResult = client.DownloadString(apiUrl);

            dynamic result = JsonConvert.DeserializeObject(jsonResult);

            double latitude = result.results[0].geometry.location.lat;
            double longitude = result.results[0].geometry.location.lng;
            address.Coordinates = SqlGeography.Point(latitude, longitude, 4326);
            //Console.WriteLine($"Latitude: {latitude}");
            //Console.WriteLine($"Longitude: {longitude}");
            //Console.ReadLine();
        }
    });

    foreach (Address address in addresses)
    {
        StaticMethods.UpdateCoordinatesInDatabase(address.Id, address.Coordinates);
    }
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
}


