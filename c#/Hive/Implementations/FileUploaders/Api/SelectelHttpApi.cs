using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace FilesCleaner;

public sealed class SelectelHttpApi
{
	private readonly HttpClient client;
	private readonly SelectelHttpApiOptions options;
	private readonly ILogger<SelectelHttpApi> logger;

	public SelectelHttpApi(
		SelectelHttpApiOptions options,
		ILogger<SelectelHttpApi> logger)
	{
		this.options = options;
		this.logger = logger;

		client = new HttpClient { BaseAddress = new Uri("https://api.selcdn.ru") };
	}

	public async Task Authorization()
	{
		client.DefaultRequestHeaders.Remove("X-Auth-Token");

		client.DefaultRequestHeaders.Add("X-Auth-User", options.PayloadUser);
		client.DefaultRequestHeaders.Add("X-Auth-Key", options.PayloadPassword);

		HttpResponseMessage response = await client.GetAsync("auth/v1.0");

		if (!response.IsSuccessStatusCode)
		{
			logger.LogError("Selectel authorization with status code {StatusCode}", response.StatusCode);

			throw new HttpRequestException();
		}

		HttpHeaders headers = response.Headers;

		String authToken = headers.GetValues("X-Storage-Token").First();

		client.DefaultRequestHeaders.Remove("X-Auth-User");
		client.DefaultRequestHeaders.Remove("X-Auth-Key");

		client.DefaultRequestHeaders.Add("X-Auth-Token", authToken);
	}

	public async Task UploadFile(StreamContent stream, String fileName)
	{
		using MultipartFormDataContent formData = new MultipartFormDataContent
		{
			{ stream, "file", fileName }
		};

		HttpResponseMessage response = await client.PutAsync($"v1/SEL_{options.AccountNumber}/{options.PayloadContainer}/{fileName}", formData);

		if (!response.IsSuccessStatusCode)
		{
			logger.LogError("Upload file: {fileName} ended with status code {StatusCode}", fileName, response.StatusCode);

			throw new HttpRequestException();
		}
		else
		{
			logger.LogInformation("Upload file: {fileName} is successed!", fileName);
		}
	}
}