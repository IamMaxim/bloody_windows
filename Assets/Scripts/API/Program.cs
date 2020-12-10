using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace API
{
    class Program
    {
        static async Task Main(string[] args)
        {
            OSU a = new OSU();
            //await a.CreateUser("Insaf", "aaaaaaa@ssss.ru", "prettypassword");
            //User newUser = await a.CreateUser("Vitaliy", "aaa@ssss.ru", "prettypassword");
            Token b = await a.GetToken("Insaf", "aaaaaaa@ssss.ru", "prettypassword");
            Console.WriteLine(b.access);
            string accessToken = b.access;
            List<Profile> pr = await a.SearchUser(accessToken, "Insaf");
            Profile profile = await a.SubmitPlay(accessToken, "5", "true", "204", "1");
            Console.WriteLine(1);
            Console.WriteLine(profile.hours_played);
            Console.WriteLine(2);
            profile = await a.AddFriend(accessToken, "Vitaliy");
            profile = await a.MyProfile(accessToken);
            profile = await a.UpdateProfile(accessToken, "Inno", "",
                "https://cdn.business2community.com/wp-content/uploads/2017/08/blank-profile-picture-973460_640.png");
            List<Play> plays = await a.MyPlays(accessToken);
        }
    }

    public class OSU
    {
        string URL = "http://mywarmplace.tk:8000/";

        public static OSU Instance = new OSU();
        public static Token Token;
        public static string Username;


        public OSU()
        {
        }

        public OSU(string url)
        {
            URL = url;
        }

        static readonly HttpClient client = new HttpClient();

        public async Task<Token> GetToken(string username, string email, string password)
        {
            try
            {
                Person person = new Person(username, email, password);
                var json = JsonConvert.SerializeObject(person);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var url = URL + "auth/jwt/create/";
                HttpResponseMessage response = await client.PostAsync(url, data);
                string responseBody = await response.Content.ReadAsStringAsync();
                Token token = JsonConvert.DeserializeObject<Token>(responseBody);
                return token;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

            return null;
        }

        public async Task<User> CreateUser(string username, string email, string password)
        {
            try
            {
                Person person = new Person(username, email, password);
                var json = JsonConvert.SerializeObject(person);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var url = URL + "auth/users/";
                HttpResponseMessage response = await client.PostAsync(url, data);
                string responseBody = await response.Content.ReadAsStringAsync();
                User user = JsonConvert.DeserializeObject<User>(responseBody);
                return user;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

            return null;
        }

        public async Task<List<Profile>> SearchUser(string accessToken, string search = "")
        {
            try
            {
                var url = URL + "all-profiles" + "?search=" + search;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                HttpResponseMessage response = await client.GetAsync(url);
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
                List<Profile> profiles = JsonConvert.DeserializeObject<List<Profile>>(responseBody);
                return profiles;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

            return null;
        }


        public async Task<Profile> SubmitPlay(string accessToken, string length, string passed, string score,
            string mapid)
        {
            try
            {
                var url = URL + "play" +
                          "?length=" + length +
                          "&passed=" + passed +
                          "&score=" + score +
                          "&mapid=" + mapid;

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                HttpResponseMessage response = await client.PutAsync(url, null);
                string responseBody = await response.Content.ReadAsStringAsync();
                Profile profile = JsonConvert.DeserializeObject<Profile>(responseBody);
                return profile;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

            return null;
        }


        public async Task<Profile> AddFriend(string accessToken, string name)
        {
            try
            {
                var url = URL + "add-friend" +
                          "?name=" + name;

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                HttpResponseMessage response = await client.PutAsync(url, null);
                string responseBody = await response.Content.ReadAsStringAsync();
                Profile profile = JsonConvert.DeserializeObject<Profile>(responseBody);
                return profile;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

            return null;
        }

        public async Task<Profile> MyProfile(string accessToken)
        {
            try
            {
                var url = URL + "myprofile";
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                HttpResponseMessage response = await client.GetAsync(url);
                string responseBody = await response.Content.ReadAsStringAsync();
                Profile profile = JsonConvert.DeserializeObject<Profile>(responseBody);
                return profile;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

            return null;
        }

        public async Task<Profile> UpdateProfile(string accessToken, string location, string description,
            string profilePictureUrl)
        {
            try
            {
                var url = URL + "myprofile";

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                ;
                Content content = new Content(location, description, profilePictureUrl);
                var json = JsonConvert.SerializeObject(content);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(url, data);
                string responseBody = await response.Content.ReadAsStringAsync();
                Profile profile = JsonConvert.DeserializeObject<Profile>(responseBody);
                return profile;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

            return null;
        }

        public async Task<List<Play>> MyPlays(string accessToken)
        {
            try
            {
                var url = URL + "myplays";
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                HttpResponseMessage response = await client.GetAsync(url);
                string responseBody = await response.Content.ReadAsStringAsync();
                List<Play> plays = JsonConvert.DeserializeObject<List<Play>>(responseBody);
                return plays;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

            return null;
        }
    }
}