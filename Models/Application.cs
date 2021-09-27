using System;
using System.Collections.Generic;

#nullable disable

namespace JobApplication.Models
{
    public partial class Application
    {
        public Application()
        {
            ApplicationAnswers = new HashSet<ApplicationAnswer>();
        }

        public int ApplicationId { get; set; }
        public bool Valid { get; set; }

        public string Name { get; set; }
        public virtual ICollection<ApplicationAnswer> ApplicationAnswers { get; set; }
    }
}
