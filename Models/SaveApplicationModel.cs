using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobApplication.Models
{
    public class SaveApplicationModel
    {
            public string Name { get; set; }

            public List<ApplicationAnswer> Answers { get; set; }       
    }
}
