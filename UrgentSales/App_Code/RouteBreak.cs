using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for RouteBreak
/// </summary>
/// 

[Serializable]
public class RouteBreak
{
    public int Id { get; set; }
    public int RouteId { get; set; }
    public int Shift { get; set; }
    public TimeSpan TimeStart { get; set; }
    public TimeSpan TimeEnd { get; set; }
    public bool IsBreakFill { get; set; }
    public bool IsStoppedLineBreak {  get; set; }
    public string BreakStyle { get; set; }
    public string BreakType { get; set; }
    public RouteBreak()
    {
        IsBreakFill = false;
    }
}