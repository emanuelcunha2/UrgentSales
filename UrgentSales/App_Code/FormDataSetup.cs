using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for FormDataSetup
/// </summary>

[Serializable]
public class FormDataSetup
{
    public string Setup { get; set; }
    public string Project { get; set; }
    public string Line { get; set; }

    public FormDataSetup()
    {

    }
}