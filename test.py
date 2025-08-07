import requests
def update_rates():
    print("Fetching exchange rates...")

    response = requests.get("https://api.coingecko.com/api/v3/coins/markets?vs_currency=usd")
    response.raise_for_status()
    data = response.json()

    for rate in data:
        currency = rate.get("name", "").lower()
        value = rate.get("current_price", 0)

        # Cache it
        self.cache.cache_set(currency, str(value))

        print("Exchange rate for %s is %s", currency, value)

    print("Exchange rates updated.")

update_rates()