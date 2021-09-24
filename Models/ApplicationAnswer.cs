using System;
using System.Collections.Generic;

#nullable disable

namespace JobApplication.Models
{
    public partial class ApplicationAnswer
    {
        public int ApplicationAnswerId { get; set; }
        public int? ApplicationId { get; set; }
        public int? QuestionId { get; set; }
        public string Answer { get; set; }

        public virtual Application Application { get; set; }
        public virtual Question Question { get; set; }
    }
}
