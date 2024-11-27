using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for IncompleteBoxesRequest
/// </summary>
public class IncompleteBoxesRequest
{
    public int Id { get; set; }
    public string Building { get; set; }
    public string PartNumber { get; set; }
    public string Designation { get; set; } 
    public string Locations { get; set; }
    public int Quantity { get; set; }
    public int ProdQuantity { get; set; }
    public int AvailableQuantity { get; set; }
    public DateTime RequestDate { get; set; }
    public DateTime SoldDate { get; set; }
    public DateTime SendDate { get; set; }
    private bool _isCompleted { get; set; }  
    public bool IsCompleted
    { 
        get { return _isCompleted; }
        set 
        {
            if (value)
            {
                CompletionStatus = "Concluído";
            } 
            else
            {
                CompletionStatus = "Não Concluído";
            } 

            _isCompleted = value; 
        } 
    }  

    public string CompletionStatus { get; set; }
 
    public string Comment { get; set; }
    public IncompleteBoxesRequest()
    {
        IsCompleted = false;
    }
}