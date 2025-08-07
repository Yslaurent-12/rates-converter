using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace background_jobs.Services;

// Inject IHttpClientFactory instead of creating a new HttpClient
public class ExchangeRateUpdaterService(
    ILogger<ExchangeRateUpdaterService> logger,
    RedisCacheService cache,
    IHttpClientFactory httpClientFactory) : BackgroundService
{
    private readonly ILogger<ExchangeRateUpdaterService> _logger = logger;
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("CoinGecko"); // Create a client from the factory

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("ExchangeRateUpdaterService is starting.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await UpdateRatesAsync();
            }
            catch (Exception ex)
            {
                // This catch is good for catching exceptions from the Delay
                _logger.LogError(ex, "An unexpected error occurred in the execution loop.");
            }

            // Await the delay *inside* the loop
            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }

        _logger.LogInformation("ExchangeRateUpdaterService is stopping.");
    }

    private async Task UpdateRatesAsync()
    {
        _logger.LogInformation("Fetching exchange rates...");

        try
        {
            // Use a relative path since the BaseAddress is already configured
            var response = await _httpClient.GetStringAsync("coins/markets?vs_currency=usd");

            var parsedRates = JsonDocument.Parse(response);

            foreach (var coin in parsedRates.RootElement.EnumerateArray())
            {
                var name = coin.GetProperty("name").GetString();
                var symbol = coin.GetProperty("symbol").GetString() ?? "";
                var currentPrice = coin.GetProperty("current_price").GetDecimal();

                await cache.CacheSetAsync(symbol, currentPrice.ToString());

                _logger.LogInformation("Exchange rate for {Name} ({Symbol}) is {Price} USD", name, symbol, currentPrice);
            }

            _logger.LogInformation("Exchange rates updated successfully.");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch exchange rates due to a network error. Status: {StatusCode}", ex.StatusCode);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to parse the API response.");
        }
        catch (Exception ex)
        {
             _logger.LogError(ex, "An unexpected error occurred while updating rates.");
        }
    }
}