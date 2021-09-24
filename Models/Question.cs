using System;
using System.Collections.Generic;

#nullable disable

namespace JobApplication.Models
{
    public partial class Question
    {
        public Question()
        {
            AnswersTypes = new HashSet<AnswersType>();
            ApplicationAnswers = new HashSet<ApplicationAnswer>();
        }

        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public bool Active { get; set; }

        public virtual ICollection<AnswersType> AnswersTypes { get; set; }
        public virtual ICollection<ApplicationAnswer> ApplicationAnswers { get; set; }
    }
}
