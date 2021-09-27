using System;
using System.Collections.Generic;

#nullable disable

namespace JobApplication.ViewModels
{
    public partial class ValidQuestion
    {

        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public bool Active { get; set; }

       
        public virtual List<ValidApplicationAnswer> ApplicationAnswers { get; set; }
    }
}
