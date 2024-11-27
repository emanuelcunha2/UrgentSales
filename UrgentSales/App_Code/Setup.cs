using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Setup
/// </summary>
/// 

[Serializable]
public class Setup
{
    public int Id { get; set; }
    public string Designation { get; set; }
    public string Project { get; set; }
    public string Line {  get; set; } 
    public bool IsChecked { get; set; }
    public string Pn { get; set; }
    public string PnDescription { get; set; }
    public int Qty { get; set; }
    public int Priority {  get; set; }
    public Setup()
    { 

    }
}