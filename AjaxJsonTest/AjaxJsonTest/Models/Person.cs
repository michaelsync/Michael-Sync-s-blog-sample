using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AjaxJsonTest.Models {
public class Person {
    //must be in property. field cannot be deserialized. 
    public string Name;// { get; set; }
    public string Occupation { get; set; }
    public int Salary { get; set; }
    public int[] NumArr { get; set; }
}
}