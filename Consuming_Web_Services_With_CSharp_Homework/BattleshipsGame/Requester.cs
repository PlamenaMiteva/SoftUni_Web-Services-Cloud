using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BattleshipsGame
{
    class Requester
    {
        private const string RegisterEndPoint = "http://localhost:62858/api/Account/Register";
        private const string LoginEnPoint = "http://localhost:62858/Token";
        private const string CreateGameEndPoint = "http://localhost:62858/api/Games/create";
        private const string JoinEndPoint = "http://localhost:62858/api/Games/join";
        private const string PlayEndPoint = "http://localhost:62858/api/Games/play";
        private const string AvailableGamesEndPoint = "http://localhost:62858/api/Games/available";

        private string token;
        private string currGameId;
        private IEnumerable<GameDTO> games;

        public async Task RegisterAsync(string email, string password, string confirmPassword)
        {
            var httpClient = new HttpClient();
            using (httpClient)
            {
                var body = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("Email", email),
                    new KeyValuePair<string, string>("Password", password),
                    new KeyValuePair<string, string>("ConfirmPassword", confirmPassword)
                });

                var response = await httpClient.PostAsync(RegisterEndPoint, body);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine(response.StatusCode + " " + await response.Content.ReadAsStringAsync());
                }
            }
        }

        public async Task LoginAsync(string email, string password)
        {
            var httpClient = new HttpClient();
            using (httpClient)
            {
                var body = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("Username", email),
                    new KeyValuePair<string, string>("Password", password)
                });

                var response = await httpClient.PostAsync(LoginEnPoint, body);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine(response.StatusCode + " " + await response.Content.ReadAsStringAsync());
                }

                var loginResponse = await response.Content.ReadAsAsync<LoginResponseDTO>();

                this.token = loginResponse.Access_Token;
            }
        }

        public async Task CreateGameAsync()
        {
            var httpClient=new HttpClient();
            using (httpClient)
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "bearer " + this.token);
                var response = await httpClient.PostAsync(CreateGameEndPoint, null);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine(response.StatusCode + " " + await response.Content.ReadAsStringAsync());
                }

                this.currGameId = await response.Content.ReadAsStringAsync();
            }
        }

        public async Task JoinToGameAsync(string playerEmail)
        {
            var game = this.games.FirstOrDefault(g => g.PlayerOne == playerEmail);

            if (game == null)
            {
                Console.WriteLine("Invalid game name");
                return;
            }

            string gameId = game.Id;
            var httpClient = new HttpClient();

            using (httpClient)
            {
                var body = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("GameId", gameId)
                });

                httpClient.DefaultRequestHeaders.Add("Authorization", "bearer " + this.token);

                var response = await httpClient.PostAsync(JoinEndPoint, body);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine(response.StatusCode + " " + await response.Content.ReadAsStringAsync());
                }

                this.currGameId = await response.Content.ReadAsStringAsync();
            }
        }

        public async Task PlayAsync(string positionX, string positionY)
        {
            var httpClient = new HttpClient();
            using (httpClient)
            {
                var body = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("GameId", this.currGameId),
                    new KeyValuePair<string, string>("PositionX", positionX),
                    new KeyValuePair<string, string>("PositionY", positionY)
                });

                httpClient.DefaultRequestHeaders.Add("Authorization", "bearer " + this.token);

                var response = await httpClient.PostAsync(PlayEndPoint, body);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine(response.StatusCode + " " + await response.Content.ReadAsStringAsync());
                }
            }
        }

        public async Task PrintAvailablePlayersAsync()
        {
            var httpClient = new HttpClient();
            using (httpClient)
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "bearer " + this.token);
                var response = await httpClient.GetAsync(AvailableGamesEndPoint);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine(response.StatusCode + " " + await response.Content.ReadAsStringAsync());
                }

                this.games = await response.Content.ReadAsAsync<IEnumerable<GameDTO>>();

                games.ToList().ForEach(g => Console.WriteLine(g.PlayerOne));
            }
        }
    }
}
