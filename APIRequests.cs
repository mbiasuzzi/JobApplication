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

namespace JobApplication
{
    public class APIRequests
    {
       
        public static string GetUrl()
        {
            bool isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
            string url = string.Empty;
            if (isDevelopment)
            {
                return "http://localhost:14633/api/";
            }
            else
            {
                return "http://jobapplicationv1.azurewebsites.net/api/";
            }
        }
        public static async Task<List<QuestionViewItem>> GetQuestionsAsync()
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


        public static async Task<bool> PostApplicationAsync(ApplicationPostModel applicationPostModel)
        {
            HttpClient client = new HttpClient();
            string url = GetUrl();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await client.PostAsJsonAsync(Routes.SaveApplicationEndpoint,applicationPostModel);
            return response.IsSuccessStatusCode;
        }

        public static async Task<List<ValidApplication>> GetValidApplications()
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
    }

    


}
