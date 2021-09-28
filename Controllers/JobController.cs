using JobApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobApplication.Controllers
{
    [ApiController]
    [Route("api/Job")]
    public class JobController : ControllerBase
    {

        private readonly ILogger<JobController> _logger;

        public JobController(ILogger<JobController> logger)
        {
            _logger = logger;
        }

        [Route("GetQuestions")]
        [HttpGet]
        public JsonResult GetQuestions()
        {
            using (var context = new JobAppSQLDBContext())
            {
                List<Question> questions = context.Questions.Where(x => x.Active == true).ToList();
                return new JsonResult(questions);
            }
        }

        [Route("GetValidApplications")]
        [HttpGet]
        public JsonResult GetValidApplications()
        {
            using (var context = new JobAppSQLDBContext())
            {

                var applications = context.Applications.Where(x => x.Valid == true).Select(x => new { x.Name, x.ApplicationAnswers, x.Valid }).ToList();
                return new JsonResult(applications);
            } 
            
        }

        public bool CheckAnswerIsValid(int? questionId, string answer)
        {
           
            using (var context = new JobAppSQLDBContext())
            {
                var anyValidAnswers = context.AnswersTypes.Where(q => q.QuestionId == questionId).Any();
                if (anyValidAnswers)
                {
                    List<string> validAnswers = context.AnswersTypes
                   .Where(x => x.QuestionId == questionId)
                   .Select(x => x.ValidAnswer.ToLower()).ToList();
                    bool valid = validAnswers.Contains(answer.ToLower());
                    return valid;
                }
                else
                {
                    //if there are no specific valid answers
                    return true;
                }
            }
        }
        [HttpPost]
        [Route("SaveApplication")]
        public async Task<JsonResult> SaveApplication(SaveApplicationModel jsonApplication)
        {
            try
            {
                bool applicationIsValid = true;
                foreach (ApplicationAnswer answer in jsonApplication.Answers)
                {
                    if (!CheckAnswerIsValid(answer.QuestionId, answer.Answer))
                    {
                        applicationIsValid = false;
                    }
                }
                await Save(jsonApplication, applicationIsValid);
                return new JsonResult(true);
            }
            catch(Exception ex)
            {
                //Log error
                return new JsonResult(false);
            }
            
        }

        public async Task<bool> Save(SaveApplicationModel application, bool valid)
        {
            using (var context = new JobAppSQLDBContext())
            {
                context.Applications.Add(new Application
                {
                    Valid = valid,
                    Name = application.Name,
                    ApplicationAnswers = application.Answers
                });
                context.SaveChanges();
                return true;
            }
        }

        
    }
}
