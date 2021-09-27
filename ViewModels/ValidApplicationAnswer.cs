using System;
using System.Collections.Generic;

#nullable disable

namespace JobApplication.ViewModels
{
    public partial class ValidApplicationAnswer
    {
        public int ApplicationAnswerId { get; set; }
        public int? ApplicationId { get; set; }
        public int? QuestionId { get; set; }
        public string Answer { get; set; }

        public virtual ValidApplication Application { get; set; }
        public virtual ValidQuestion Question { get; set; }
    }
}
