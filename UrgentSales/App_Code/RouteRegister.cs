using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

/// <summary>
/// Summary description for RouteRegister
/// </summary>
public class RouteRegister
{
    public int RegisterId { get; set; }
    public int RouteId { get; set; }
    public int TimePassed {  get; set; }
    public string Justification {  get; set; }
    public bool IsBreak {  get; set; }
    public bool IsLineBreak { get; set; }
    public DateTime TimeDate { get; set; }
    public string RouteStyle { get; set; }
    public string RegisterStyle { get; set; }
    public string RegisterPostionStyle { get; set; }
    public string LateRegisterStyle { get; set; }

    public RouteRegister()
    { 
    }
}