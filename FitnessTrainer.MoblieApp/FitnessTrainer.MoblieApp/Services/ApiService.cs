using FitnessTrainer.MoblieApp.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using FitnessTrainer.MoblieApp.Settings;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTrainer.MoblieApp.Services
{
    class ApiService
    {
        public async Task<bool> RegisterUserAsync(string userName, string password, string confirmPassword)
        {
            var client = new HttpClient();
            var model = new RegisterViewModel
            {
                UserName = userName,
                Password = password,
                ConfirmPassword = confirmPassword
            };

            var json = JsonConvert.SerializeObject(model);

            HttpContent httpContent = new StringContent(json);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.PostAsync(Setts.GetWebServiceAddress + "/api/Account/Register", httpContent);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }
    }
}
