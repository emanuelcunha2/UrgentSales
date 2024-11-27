using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Route
/// </summary>
public class Route
{
    public int Id { get; set; }
    public int Minutes { get; set; }
    public string Name { get; set; }
    public DateTime Date { get; set; }
    public string Building {  get; set; }
    public List<RouteRegister> Registers { get; set; }
    public List<RouteBreak> Breaks { get; set; }
    public Route()
    {
        Registers = new List<RouteRegister>();
    }
}