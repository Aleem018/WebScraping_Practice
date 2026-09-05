using System.Drawing;
using System.Net;
using System.Security.Policy;
using System.Text;
using HtmlAgilityPack;

namespace SixthMission;

public class Quote
{
    public string Text { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty; 
}

public class QuoteScraper
{
    public async Task<List<Quote>> RunScraperAsync(string url = "")
    {

        HttpClient client = new HttpClient();

        string href = "";

        bool nextBtn = true;

        List<Quote> output = new List<Quote>();

        while (nextBtn)
        {
            string tempUrl = url + href;
            var htmlContent = await client.GetStringAsync(tempUrl);

            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(htmlContent);

            var parentContainer = htmlDoc.DocumentNode.SelectNodes("//div[@class='quote']");

            if (parentContainer != null)
            {
                foreach (var quote in parentContainer)
                {
                    string text = quote.SelectSingleNode(".//span[@class='text']").InnerText;
                    string author = quote.SelectSingleNode(".//small[@class='author']").InnerText;

                    string cleanText = WebUtility.HtmlDecode(text);
                    string cleanAuthor = WebUtility.HtmlDecode(author);

                    Quote newQuote = new Quote();
                    newQuote.Text = $"\"{cleanText}\"";
                    newQuote.Author = $"\"{cleanAuthor}";

                    output.Add(newQuote);
                }
            } else
            {
                Console.WriteLine("Parent container does not exist");
            }

            var buttonHtml = htmlDoc.DocumentNode.SelectSingleNode("//li[@class='next']/a");

            if (buttonHtml != null)
            {
                href = buttonHtml.GetAttributeValue("href", string.Empty);
            } else
            {
                nextBtn = false;
            }
        }

        return output;

    }
}

public partial class Form1 : Form
{
    private Button btnScrape;
    private TextBox urlInput;
    private Button urlButton;
    private ListBox scrapedData;

    public Form1()
    {
        this.Text = "Web Scraper";
        this.Size = new Size(700, 400);
        this.StartPosition = FormStartPosition.CenterScreen;

        urlInput = new TextBox();
        urlInput.Location = new Point(60, 40);
        urlInput.Size = new Size(200, 30);
        urlInput.PlaceholderText = "Enter the target url";
        this.Controls.Add(urlInput); //this is string url

        urlButton = new Button();
        urlButton.Text = "SCRAPE";
        urlButton.Location = new Point(60, 80);
        urlButton.Size = new Size(200, 40);
        urlButton.BackColor = Color.Green;
        urlButton.ForeColor = Color.White;
        this.Controls.Add(urlButton);
        urlButton.Click += btnScrape_Click;
        
        
    }

    private async void btnScrape_Click(object sender, EventArgs e)
    {
        urlButton.Text = "Scraping in progress";

        QuoteScraper scraper = new QuoteScraper();
        List<Quote> results = await scraper.RunScraperAsync();

        using StreamWriter writer = new StreamWriter("quotes_export.csv", false, new UTF8Encoding(true));

        writer.WriteLine("Quote,Author");

        foreach (var quote in results)
        {
            quote.Text = quote.Text.Replace("\"", "\"\"");
            quote.Author = quote.Author.Replace("\"", "\"\"");

            writer.WriteLine($"\"{quote.Text}\",\"{quote.Author}\"");
        }

        urlButton.Text = $"Saved {results.Count} quotes!";
    }
}
