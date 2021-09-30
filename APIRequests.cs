using JobApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using Microsoft.Extensions.Logging;

namespace JobApplication
{
    public class APIRequests
    {
        public APIRequests()
        {
        }
        private readonly ILogger<APIRequests> _logger;
        private readonly IConfiguration configuration;
        public APIRequests(ILogger<APIRequests> logger, IConfiguration iConfig)
        {
            _logger = logger;
            configuration = iConfig;

        }

        public string GetUrl()
        {
            
            bool isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
           
            if (isDevelopment)
            {
                //string url = configuration.GetValue<string>("Enviroment:ApiAddress");
                //return url;
                return "http://localhost:14633/api/";  //TODO this needs to go in the config file
            }
            else
            {
                return "http://jobapplicationv1.azurewebsites.net/api/";//TODO this needs to go in the config file
            }
        }
        public async Task<List<QuestionViewItem>> GetQuestionsAsync()
            {
            try
                {
                    HttpClient client = new HttpClient();
                    string url = GetUrl();
                    client.BaseAddress = new Uri(url);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                    List<QuestionViewItem> questions = new List<QuestionViewItem>();
                    HttpResponseMessage response = await client.GetAsync(Routes.GetQuestionsEndpoint);
                    if (response.IsSuccessStatusCode)
                    {
                        questions = await response.Content.ReadAsAsync<List<QuestionViewItem>>();
                    }
                    return questions;
                }
                catch(Exception ex)
                {
                    _logger.Log(LogLevel.Error, ex, "Error getting questions from API");
                    return new List<QuestionViewItem>();
                }
               
            }


        public async Task<bool> PostApplicationAsync(ApplicationPostModel applicationPostModel)
        {
            try
            {
                HttpClient client = new HttpClient();
                string url = GetUrl();
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.PostAsJsonAsync(Routes.SaveApplicationEndpoint, applicationPostModel);
                return response.IsSuccessStatusCode;
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, "Error posting Application to API");
                return false;
            }
            
        }

        public async Task<List<ValidApplication>> GetValidApplications()
        {
            try
            {
                HttpClient client = new HttpClient();
                string url = GetUrl();
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                List<ValidApplication> applications = new List<ValidApplication>();
                HttpResponseMessage response = await client.GetAsync(Routes.GetValidApplicationsEndpoint);
                if (response.IsSuccessStatusCode)
                {
                    applications = await response.Content.ReadAsAsync<List<ValidApplication>>();
                }
                return applications;
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, "Error getting valid applications from API");
                return new List<ValidApplication>();
            }
        }
    }

    


}
