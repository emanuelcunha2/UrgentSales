
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

/// <summary>
/// Summary description for GapRecord
/// </summary>
/// 

[Serializable]
public class GapRecord
{
    public string Building { get; set; }
    public string PartNumber { get; set; }
    public string Designation { get; set; }
    public int QuantityAvailable { get; set; }
    public int ProdQuantityAvailable { get; set; }
    public int QuantityGap { get; set; }
    public List<ShipmentRecord> Records { get; set; }
    public string StringRecords { get; set; }
}