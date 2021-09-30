using JobApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

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
        public async Task<JsonResult> GetQuestions()
        {
            try
            {
                using (var context = new JobAppSQLDBContext())
                {
                    List<Question> questions = await context.Questions.Where(x => x.Active == true).ToListAsync();
                    return new JsonResult(questions);
                }
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error,ex,"Error getting questions from azure database");
                return new JsonResult(new List<Question>());
            }
        }

        [Route("GetValidApplications")]
        [HttpGet]
        public async Task<JsonResult> GetValidApplications()
        {
            try
            {
                using (var context = new JobAppSQLDBContext())
                {

                    var applications = await context.Applications.Where(x => x.Valid == true)
                        .Select(x => new { x.Name, x.ApplicationAnswers, x.Valid }).ToListAsync();
                    return new JsonResult(applications);
                }
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, "Error getting valid applications from azure database");
                return new JsonResult(new List<Application>());
            }

        }

        public async Task<bool> CheckAnswerIsValid(int? questionId, string answer)
        {
           
            using (var context = new JobAppSQLDBContext())
            {
                try
                {
                    var anyValidAnswers = await context.AnswersTypes.Where(q => q.QuestionId == questionId).AnyAsync();
                    if (anyValidAnswers)
                    {
                        List<string> validAnswers = await context.AnswersTypes
                       .Where(x => x.QuestionId == questionId)
                       .Select(x => x.ValidAnswer.ToLower()).ToListAsync();
                        bool valid = validAnswers.Contains(answer.ToLower());
                        return valid;
                    }
                    else
                    {
                        //if there are no specific valid answers
                        return true;
                    }
                }
                catch(Exception ex)
                {
                    _logger.Log(LogLevel.Error, ex, "Error checking valid answers from azure database");
                    return false;
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
                    if (await CheckAnswerIsValid(answer.QuestionId, answer.Answer) == false)
                    {
                        applicationIsValid = false;
                    }
                }
                bool isSucces = await Save(jsonApplication, applicationIsValid);
                return new JsonResult(isSucces);
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, "Error while trying to save application");
                return new JsonResult(ex.Message);
            }
            
        }

        public async Task<bool> Save(SaveApplicationModel application, bool valid)
        {
            try
            {
                using (var context = new JobAppSQLDBContext())
                {
                    context.Applications.Add(new Application
                    {
                        Valid = valid,
                        Name = application.Name,
                        ApplicationAnswers = application.Answers
                    });
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, "Error saving application to azure database");
                return false;
            }
           
        }

        
    }
}
