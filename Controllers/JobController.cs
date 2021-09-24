using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobApplication.Models;

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
                List<Question> questions = context.Questions.ToList();
                return new JsonResult(questions);
            }
        }
    }
}
