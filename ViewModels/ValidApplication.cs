using System;
using System.Collections.Generic;

#nullable disable

namespace JobApplication.ViewModels
{
    public partial class ValidApplication
    {
   
        public int ApplicationId { get; set; }
        public bool Valid { get; set; }

        public string Name { get; set; }
        public virtual List<ValidApplicationAnswer> ApplicationAnswers { get; set; }
    }
}
