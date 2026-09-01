using Engine.QuoteScraper;
using FifthMision.Models;

QuoteScraper scraper = new QuoteScraper();

List<Quote> results = await scraper.RunScraperAsync();

Console.WriteLine($"Successfully scraped {results.Count} quotes!");
