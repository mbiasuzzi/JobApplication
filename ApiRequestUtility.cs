//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Threading.Tasks;

//namespace JobApplication
//{
//    public class ApiRequestUtility
//    {



//        internal async Task<string> GetRequestAsync(string endpoint)

//        {

//            HttpClientHandler handler = new HttpClientHandler()

//            {

//                PreAuthenticate = true,

//                UseDefaultCredentials = true

//            };

//            using (var client = new HttpClient(handler))

//            {

//                client.BaseAddress = new Uri(SettingsStore.Current.CFRSEndPoint);

//                client.DefaultRequestHeaders.Accept.Clear();

//                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


//                int beg = endpoint.IndexOf("?") + 1;

//                if (beg > 0)

//                {

//                    string parms = endpoint.Substring(beg, endpoint.Length - beg);

//                    string url = endpoint.Substring(0, beg);



//                    endpoint = url + HttpUtility.UrlEncode(parms);

//                    endpoint = endpoint.Replace("%3d", "=")

//                                        .Replace("%26", "&")

//                                        .Replace("!ampersand!", "%26");

//                }



//                var response = await client.GetAsync(endpoint);

//                if (response.IsSuccessStatusCode)

//                {

//                    return await response.Content.ReadAsStringAsync();

//                }

//                else

//                {

//                    if (response.StatusCode == HttpStatusCode.Unauthorized)

//                    {

//                        throw new KeyNotFoundException("Not authorized");

//                    }



//                    var apps = await response.Content.ReadAsAsync<ResponseHeader>();

//                    if (response.StatusCode == HttpStatusCode.NotFound)

//                    {

//                    TODO: this needs to be changed in phase 2 to handle messages generically

//                        if (endpoint.Contains("GetApplicationEntitlementsByRoles"))

//                            apps.Result = "<b>The resource you requested cannot be found or does not exist.  We are sorry for this inconvenience.</b>";

//                        else

//                            apps.Result = apps.Result.Replace("Your User Name could not be found.", "<b>Your User Name could not be found.</b>");



//                        throw new KeyNotFoundException(apps.Result);

//                    }

//                    else

//                    {

//                        throw new SystemException(apps == null ? string.Empty : apps.Result);

//                    }

//                }

//            }
//        }
//    }

//}
