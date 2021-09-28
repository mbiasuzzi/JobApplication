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
                return "http://localhost:14633/api/";  //TODO this needs to go in the config file
            }
            else
            {
                return "http://jobapplicationv1.azurewebsites.net/api/";//TODO this needs to go in the config file
            }
        }
        public static async Task<List<QuestionViewItem>> GetQuestionsAsync()
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
                    //TODO log ex
                    return new List<QuestionViewItem>();
                }
               
            }


        public static async Task<bool> PostApplicationAsync(ApplicationPostModel applicationPostModel)
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
                //TODO log exception
                return false;
            }
            
        }

        public static async Task<List<ValidApplication>> GetValidApplications()
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
                //TODO log ex
                return new List<ValidApplication>();
            }
        }
    }

    


}
