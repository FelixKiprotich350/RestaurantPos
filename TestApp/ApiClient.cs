using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http; 
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    public static class ApiClient
    {
        static string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJKV1RTZXJ2aWNlQWNjZXNzVG9rZW4iLCJqdGkiOiI5OWRkYTVlOS0xZmUwLTRiNmQtYTYyMi0wYjdmMTk3NjdjZmMiLCJpYXQiOiIyNS8xMS8yMDIzIDE3OjM2OjIyIiwiVXNlcklkIjoiQWRtaW4iLCJEaXNwbGF5TmFtZSI6IkFkbWluLURlZmF1bHQiLCJVc2VyTmFtZSI6IkFkbWluIiwiRW1haWwiOiJBZG1pbiIsImV4cCI6MTcwMTc5Nzc4MiwiaXNzIjoiSldUQXV0aGVudGljYXRpb25TZXJ2ZXIiLCJhdWQiOiJKV1RTZXJ2aWNlUG9zdG1hbkNsaWVudCJ9.agkN2jZ4mn_DSwJv7PEvH-zzUG1s5fLJvbYRSj-fgWA";
        public static HttpResponseMessage GetResponse(string Uri)
        {
            
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress =new Uri( "https://localhost:7014/");
                // Add any headers needed for authentication, etc.
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var responseTask = client.GetAsync(Uri);
                responseTask.Wait();
                var result = responseTask.Result;
                return result;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "API ERROR", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return null;
            }
        }
        public static HttpResponseMessage PostResponse(string Uri,HttpContent content)
        {
            
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress =new Uri( "https://localhost:7014/");
                // Add any headers needed for authentication, etc.
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                

                var responseTask = client.PostAsync(Uri,content);
                responseTask.Wait();
                var result = responseTask.Result;
                return result;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "API ERROR", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return null;
            }
        }
    }
}
