﻿using Microsoft.SqlServer.Types;

namespace GoogleMapGeoLocation;

internal class Address
{
    public int Id { get; set; }
    public string? AddressLine1 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? ZipCode { get; set; }
    public SqlGeography? Coordinates { get; set; }

}
