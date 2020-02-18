using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GoodApp.Data.Models;
using Microsoft.Ajax.Utilities;

namespace GoodApp.Backend.Models
{
    public class GroupViewModel 
    {
        public string Name { get; set; }

        public string Picture { get; set; }

        public string Description { get; set; }

        public static GroupViewModel Create(Group group)
        {
            return new GroupViewModel {Name = group.Name, Description = group.Description, Picture = group.Picture};
        }
    }
}