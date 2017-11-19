using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Amon.Web.Models.ComponentesForm
{
    public class ItemDropDown
    {
        public String Name { get; set; }
        public String Value { get; set; }

        public ItemDropDown(String name, String value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}