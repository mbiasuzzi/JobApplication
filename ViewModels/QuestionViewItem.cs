using System;
using System.Collections.Generic;

#nullable disable

namespace JobApplication.ViewModels
{
    public class QuestionsViewItems
    {
        public List<QuestionViewItem> questionsViewItems { get; set; }
    }
    public class QuestionViewItem
    {

        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public bool Active { get; set; }
    }
}
