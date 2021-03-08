using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//using System.Text.Json;
//using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TestServer.Model;

namespace TestServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserDataController : ControllerBase
    {
        //GET: UserData
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: UserData/5
        [HttpGet("GetLeader")]
        public string GetLeader()
        {
            List<User> studentsList;
            LeaderboardData leaderData = new LeaderboardData
            {
                Code = 0,
                Message = "Successfull",
                Data = null,
            };

            using (var context = new UsersDBContext())
            {

                IQueryable<User> students = from s in context.Users
                               select s;
                students = students.OrderByDescending(s => s.Score);
                studentsList = students.ToList();

            }
            leaderData.Data = studentsList;

            return JsonConvert.SerializeObject(leaderData);
        }

        // POST: UserData
        [HttpPost]
        public string Post([FromBody] System.Text.Json.JsonElement rawQuery)
        {
            string rawJson = rawQuery.ToString();

            error err = new error
            {
                Code = 0,
                Message = "Successfull"
            };

            string response;

            User userData = JsonConvert.DeserializeObject<User>(rawJson);
            using (var context = new UsersDBContext())
            {
                User user = context.Users.Where(x => x.Name == userData.Name).FirstOrDefault();
                if (user != null)
                {
                    //err.Code = 1;
                    //err.Message = "Name already naken";
                    //Already have user
                }
                else
                {
                    context.Users.Add(userData);
                    context.SaveChanges();
                }
            }

            response = JsonConvert.SerializeObject(err);

            //string jsonData = JsonConvert.SerializeObject(query);
            //Newtonsoft.Json.Linq.JObject
            //var aa = JsonConvert.DeserializeObject(value.ToString());
            return response;
        }

        [HttpPost("{id}")]
        public string UpdateScore([FromBody] System.Text.Json.JsonElement rawQuery)
        {
            string rawJson = rawQuery.ToString();
            User scoreData = JsonConvert.DeserializeObject<User>(rawJson);
            
            error err = new error
            {
                Code = 0,
                Message = "Successfull"
            };

            using (var context = new UsersDBContext())
            {


                User user = context.Users.Where(x => x.Name == scoreData.Name).FirstOrDefault();
                if (user == null)
                {
                    err.Code = 1;
                    err.Message = "Error update values, player doesnt exist";
                }
                else
                {
                    user.Score += scoreData.Score;
                    user.Win += scoreData.Win;
                    user.Lose += scoreData.Lose;
                    
                    context.SaveChanges();
                }
            }

            string response = JsonConvert.SerializeObject(err);

            return response;
        }

        // PUT: UserData/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }  

   
}
