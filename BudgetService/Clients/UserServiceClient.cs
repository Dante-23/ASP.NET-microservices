using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using BudgetService.Models.DTOs;

namespace BudgetService.Clients;

public class UserServiceClient {
    private readonly HttpClient _httpClient;
    private static readonly string authKey = "CustomBearer";
    private static readonly string authValue = "Custom";
    public UserServiceClient(HttpClient httpClient) {
        _httpClient = httpClient;
    }
    public async Task<bool> UserExists(long id) {
        var request = new HttpRequestMessage(HttpMethod.Get, $"api/User/microcomm/exists/{id}");
        request.Headers.Authorization = new AuthenticationHeaderValue(authKey, authValue);
        try {
            HttpResponseMessage response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode) {
                return true;
            } else return false;
        } catch (HttpRequestException e) {
            Console.WriteLine(e);
            return false;
        }
    }
}