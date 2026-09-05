using Engine.QuoteScraper;
using FifthMision.Models;
using System.Text;
using System.IO;
using System.Net;

private async void btnScrape_Click(object sender, EventArgs e)
{
    // 1. Update the lblStatus text to "Scraping in progress..."
    lblStatus.Text = "Scraping in prrogress...";
    
    // 2. Instantiate your QuoteScraper and get the List<Quote> results
    QuoteScraper scraper = new QuoteScraper();

    List<Quote> results = await scraper.RunScraperAsync();
    
    // 3. Open a StreamWriter to "quotes_export.csv"
    using StreamWriter writer = new StreamWriter("quotes_export.csv", false, new UTF8Encoding(true));
    
    // 4. Write the CSV header ("Quote,Author")
    writer.WriteLine("Quote,Author");
    
    // 5. Loop through the list of quotes, format them for CSV (handle the commas/quotes), and write them to the file
    foreach (var quote in results)
    {
        quote.Text = quote.Text.Replace("\"", "\"\"");
        quote.Author = quote.Author.Replace("\"", "\"\"");

        writer.WriteLine($"\"{quote.Text}\",\"{quote.Author}\"");
        
    }
    
    // 6. Update the lblStatus text to "Saved [Count] quotes!"
    lblStatus.Text = $"Saved {results.Count} quotes!";
}