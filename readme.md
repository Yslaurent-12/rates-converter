# Crypto-Fiat Converter Service

This service allows users to **register cryptocurrencies**, and **convert between fiat and crypto currencies** using real-time exchange rates. It integrates with external APIs for accurate pricing and runs a **background worker** to update exchange rates regularly.

## Features

### 🔁 Currency Conversion
- **Fiat to Crypto:** Convert any amount in fiat currency (e.g., USD, EUR, GHS) to its equivalent in a registered cryptocurrency (e.g., BTC, ETH).
- **Crypto to Fiat:** Convert cryptocurrency amounts to their fiat currency equivalent.

### 💰 Coin Registration
- Easily register new coins you want to support in your application.
- Each registered coin is tracked and updated with the latest exchange rate.

### 🔄 Background Rate Updater
A background worker runs at scheduled intervals to fetch and update exchange rates from two main sources:
- [CoinGecko](https://api.coingecko.com/api/v3/coins/markets?vs_currency=usd) – for **cryptocurrency rates**.
- [OpenExchangeRates](https://openexchangerates.org/) – for **fiat currency rates**.

Rates are stored in the cache for fast access during conversions.

---

## Example Use Case

### Register a Coin
```http
POST /api/CoinData/create-coin
{
  "symbol": "btc",
  "name": "Bitcoin"
}
```

### Convert Btc to Eth
```http
POST /api/CoinData/convert-coin-to-coin
{
  "amount":0.25,
  "fromCryptoSymbol": "btc",
  "toCryptoSymbol": "eth"
}
```

### Convert ghs to btc
```http POST /api/CoinData/convert-coin-to-fiat
{
  "amount": 100,
  "cryptoSymbol": "btc",
  "toCurrency": "ghs"
}

```
