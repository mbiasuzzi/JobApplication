using System;
using System.Collections.Generic;

#nullable disable

namespace JobApplication.Models
{
    public partial class AnswersType
    {
        public int AnswerId { get; set; }
        public int? QuestionId { get; set; }
        public string ValidAnswer { get; set; }
        public bool Active { get; set; }

        public virtual Question Question { get; set; }
    }
}
