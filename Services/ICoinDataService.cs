using background_jobs.models;

namespace background_jobs.Services
{
    public interface ICoinDataService
    {
        Task<List<CoinDataDto>> GetCoinsAsync();
        Task<CoinDataDto> GetCoinAsync(Guid id);

        Task<CoinDataDto> CreateCoinAsync(CreateCoinDto createCoinDto);

        Task<CoinDataDto> FetchCoinAsync(FetchCoinDto fetchCoinDto);
        Task<ConversionFiatResponseDto> ConvertCoinToFiatCurrencyAsync(ConvertCoinDto convertCoinDto);

        Task<ConversionCoinResponseDto> ConvertCoinToCoinAsync(ConvertCoinDto2 convertCoinDto2);
        
    }
}