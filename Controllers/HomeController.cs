using JobApplication.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace JobApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public string baseUrl;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        //TODO ADD LOGGIN
        //TODO ADD TRY CATCHES 
        //TODO SET UP CONFIG

        public virtual async Task<IActionResult> IndexAsync()
        {
        
            List<QuestionViewItem> questions = await APIRequests.GetQuestionsAsync();
            return View(questions);
            

        }

        [HttpPost]
        public virtual async Task<IActionResult> SaveApplication(IFormCollection form)
        {
            
            ApplicationPostModel applicationPostModel = new ApplicationPostModel();
            List<ApplicationAnswerViewModel> answerList = new List<ApplicationAnswerViewModel>();
            foreach (string key in form.Keys)
            {
                if(key == "name")
                {
                    applicationPostModel.Name = form[key];
                }
                else
                {
                    ApplicationAnswerViewModel answer = new ApplicationAnswerViewModel();
                    answer.QuestionId = Convert.ToInt32(key);
                    answer.Answer = form[key];
                    answerList.Add(answer);
                }
                    
            }

            applicationPostModel.Answers = answerList;
            bool isSuccess = await APIRequests.PostApplicationAsync(applicationPostModel);
            if (isSuccess)
            {
                return View("Success");
            }
            else
            {
                return View("Error", new ErrorViewModel());
            }
          
        }
       
        public virtual async Task<IActionResult> ValidApplications()
        {
          
            List<ValidApplication> applications = await APIRequests.GetValidApplications();
            List<QuestionViewItem> questions = await APIRequests.GetQuestionsAsync();
           
            DataTable dt = new DataTable();

            dt.Columns.Add("Name", typeof(string));
            foreach (QuestionViewItem q in questions)
            {
                dt.Columns.Add(q.QuestionText, typeof(string));
            }

            foreach (ValidApplication ans in applications)
            {
                DataRow row = dt.NewRow();
                row[0] = ans.Name;
                for (int i = 0; i < ans.ApplicationAnswers.Count; i++)
                {
                    row[i + 1] = ans.ApplicationAnswers[i].Answer;
                }
                dt.Rows.Add(row);
            }

            return View(dt);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
