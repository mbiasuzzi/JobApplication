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
using Microsoft.Extensions.Configuration;


namespace JobApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger )
        {
            _logger = logger;
         
        }

        //TODO SET UP CONFIG

        public async Task<IActionResult> IndexAsync()
        {
            APIRequests api = new APIRequests();
            List<QuestionViewItem> questions = await api.GetQuestionsAsync();
            if(questions.Count == 0)
            {
                return View("Error", new ErrorViewModel());
            }
            else
            {
                return View(questions);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveApplication(IFormCollection form)
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
            APIRequests api = new APIRequests();
            bool isSuccess = await api.PostApplicationAsync(applicationPostModel);
            if (isSuccess)
            {
                return View("Success");
            }
            else
            {
                return View("Error", new ErrorViewModel());
            }
          
        }
       
        public async Task<IActionResult> ValidApplications()
        {
            APIRequests api = new APIRequests();
            List<ValidApplication> applications = await api.GetValidApplications();
            List<QuestionViewItem> questions = await api.GetQuestionsAsync();
            if(questions.Count == 0)
            {
                return View("Error", new ErrorViewModel());
            }
           
            DataTable dt = new DataTable();

            dt.Columns.Add("Name", typeof(string));
            foreach (QuestionViewItem q in questions)
            {
                dt.Columns.Add(q.QuestionText, typeof(string));
            }

            foreach (ValidApplication app in applications)
            {
                DataRow row = dt.NewRow();
                row[0] = app.Name;
                for (int i = 0; i < app.ApplicationAnswers.Count; i++)
                {
                    row[i + 1] = app.ApplicationAnswers[i].Answer;
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
