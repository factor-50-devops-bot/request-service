﻿using HelpMyStreet.Contracts.UserService.Request;
using HelpMyStreet.Contracts.UserService.Response;
using HelpMyStreet.Utils.Enums;
using HelpMyStreet.Utils.Utils;
using Marvin.StreamExtensions;
using Newtonsoft.Json;
using RequestService.Core.Dto;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace RequestService.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpClientWrapper _httpClientWrapper;

        public UserService(IHttpClientWrapper httpClientWrapper)
        {
            _httpClientWrapper = httpClientWrapper;
        }

        public async Task<int> GetChampionCountByPostcode(string postcode, CancellationToken cancellationToken)
        {
            string path = $"api/GetChampionCountByPostcode?postcode={postcode}";
            GetChampionCountByPostcodeResponse championCountResponse ;
            using (HttpResponseMessage response = await _httpClientWrapper.GetAsync(HttpClientConfigName.UserService, path, cancellationToken).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();                 
                Stream stream = await response.Content.ReadAsStreamAsync();
                championCountResponse = stream.ReadAndDeserializeFromJson<GetChampionCountByPostcodeResponse>();
            }
            return championCountResponse.Count;
        }

        public async Task<GetChampionsByPostcodeResponse> GetChampionsByPostcode(string postcode, CancellationToken cancellationToken)
        {
            string path = $"api/GetChampionsByPostcode?postcode={postcode}";
            GetChampionsByPostcodeResponse championsResponse;
            using (HttpResponseMessage response = await _httpClientWrapper.GetAsync(HttpClientConfigName.UserService, path, cancellationToken).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();                
                string content = await response.Content.ReadAsStringAsync();
                championsResponse = JsonConvert.DeserializeObject<GetChampionsByPostcodeResponse>(content);
            }
            return championsResponse;
        }

        public async Task<GetVolunteersByPostcodeAndActivityResponse> GetHelpersByPostcodeAndTaskType(string postcode, List<SupportActivities> activities, CancellationToken cancellationToken)
        {
            string path = $"api/GetVolunteersByPostcodeAndActivity";
            GetVolunteersByPostcodeAndActivityResponse helperResponse;
            GetVolunteersByPostcodeAndActivityRequest request = new GetVolunteersByPostcodeAndActivityRequest
            {
                VolunteerFilter = new VolunteerFilter
                {
                    Postcode = postcode,
                    Activities = activities
                }                                
            };

            using (HttpResponseMessage response = await _httpClientWrapper.GetAsync(HttpClientConfigName.UserService, path, request, cancellationToken).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();
                string content = await response.Content.ReadAsStringAsync();
                helperResponse = JsonConvert.DeserializeObject<GetVolunteersByPostcodeAndActivityResponse>(content);
            }
            return helperResponse;
        }

        public async Task<GetUserByIDResponse> GetUser(int userID, CancellationToken cancellationToken)
        {
            string path = $"api/GetUserByID?ID={userID}";
            GetUserByIDResponse userIDResponse;
            using (HttpResponseMessage response = await _httpClientWrapper.GetAsync(HttpClientConfigName.UserService, path, cancellationToken).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();
                string content = await response.Content.ReadAsStringAsync();
                userIDResponse = JsonConvert.DeserializeObject<GetUserByIDResponse>(content);
            }
            return userIDResponse;
        }

        public async Task<GetUsersResponse> GetUsers(CancellationToken cancellationToken)
        {
            string path = $"api/GetUsers";
            GetUsersResponse usersResponse;
            using (HttpResponseMessage response = await _httpClientWrapper.GetAsync(HttpClientConfigName.UserService, path, cancellationToken).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();
                string content = await response.Content.ReadAsStringAsync();
                usersResponse = JsonConvert.DeserializeObject<GetUsersResponse>(content);
            }
            return usersResponse;
        }
    }
}
