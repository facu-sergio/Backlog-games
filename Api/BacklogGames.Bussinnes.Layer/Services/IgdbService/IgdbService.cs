using AutoMapper;
using BacklogGames.Bussinnes.Layer.DTOs.Game;
using BacklogGames.Bussinnes.Layer.DTOs.Igdb;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace BacklogGames.Bussinnes.Layer.Services.IgdbService
{
    public class IgdbService : IIgdbService
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private string? _accessToken;
        private DateTime _tokenExpirationTime;

        public IgdbService(HttpClient httpClient, IMapper mapper, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _mapper = mapper;
            _clientId = configuration["Igdb:ClientId"] ?? throw new ArgumentNullException("Igdb:ClientId");
            _clientSecret = configuration["Igdb:ClientSecret"] ?? throw new ArgumentNullException("Igdb:ClientSecret");
            _httpClient.BaseAddress = new Uri("https://api.igdb.com/v4/");
        }

        public async Task<List<GameInfoDto>> SearchGamesByNameAsync(string searchTerm)
        {
            await EnsureAccessTokenAsync();

            var query = $"search \"{searchTerm}\"; fields name, cover.image_id, first_release_date, summary, rating; limit 10;";

            var request = new HttpRequestMessage(HttpMethod.Post, "games")
            {
                Content = new StringContent(query, Encoding.UTF8, "text/plain")
            };

            request.Headers.Add("Client-ID", _clientId);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var igdbGames = JsonSerializer.Deserialize<List<IgdbGameResponseDto>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<IgdbGameResponseDto>();

            return _mapper.Map<List<GameInfoDto>>(igdbGames);
        }

        private async Task EnsureAccessTokenAsync()
        {
            if (!string.IsNullOrEmpty(_accessToken) && DateTime.UtcNow < _tokenExpirationTime)
            {
                return;
            }

            var tokenRequest = new HttpRequestMessage(HttpMethod.Post,
                $"https://id.twitch.tv/oauth2/token?client_id={_clientId}&client_secret={_clientSecret}&grant_type=client_credentials");

            var response = await _httpClient.SendAsync(tokenRequest);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<IgdbTokenResponse>(content);

            if (tokenResponse == null)
            {
                throw new Exception("Failed to obtain access token from IGDB");
            }

            _accessToken = tokenResponse.AccessToken;
            _tokenExpirationTime = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn - 300); // 5 minutes buffer
        }

        private class IgdbTokenResponse
        {
            [System.Text.Json.Serialization.JsonPropertyName("access_token")]
            public string AccessToken { get; set; } = string.Empty;

            [System.Text.Json.Serialization.JsonPropertyName("expires_in")]
            public int ExpiresIn { get; set; }
        }
    }
}
