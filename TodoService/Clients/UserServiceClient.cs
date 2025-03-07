using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using TodoService.Models.DTOs;

namespace TodoService.Clients;

public class UserServiceClient {
    private readonly HttpClient _httpClient;
    private static readonly string authKey = "CustomBearer";
    private static readonly string authValue = "Custom";
    public UserServiceClient(HttpClient httpClient) {
        _httpClient = httpClient;
    }
    public async bool UserExists(long id) {
        var request = new HttpRequestMessage(HttpMethod.Get, $"api/User/microcomm/exists/{id}");
        request.Headers.Authorization = new AuthenticationHeaderValue(authKey, authValue);
        var response = await _httpClient.SendAsync(request);
    }
    public async void TestUserMicroCommApi() {
        Console.WriteLine("TestUserMicroCommApi");
        var request = new HttpRequestMessage(HttpMethod.Get, "api/User/microcomm/5");
        request.Headers.Authorization = new AuthenticationHeaderValue(authKey, authValue);
        var response = await _httpClient.SendAsync(request);
        Console.WriteLine(response.ToString());
    }
}