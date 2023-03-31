using System;
using RestSharp;
using System.Threading;
using Newtonsoft.Json;
using demoapp.Models;

namespace demoapp.Services
{
	public class UserService
	{
		private readonly RestClient _restClient;
        public UserService()
		{
            _restClient = new RestClient("https://jsonplaceholder.typicode.com");

        }

		public async Task<User> GetUser(int id)
		{
            User user = null; 
            var request = new RestRequest("/users/"+id);
            var response = await this._restClient.GetAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK){
                var userObject = JsonConvert.DeserializeObject<User>(response.Content.ToString());
                var customuserObject = new User()
                {
                    //user
                    Id = (int)userObject.Id,
                    Name = userObject.Name,
                    UserName = userObject.UserName,
                    Email = userObject.Email,
                    Phone = userObject.Phone,
                    Website = userObject.Website,
                    //company
                    Company = userObject.Company,
                    ////address 
                    Address = userObject.Address,

                };
                user = customuserObject;
            }
            return user;
        }
    }
}

