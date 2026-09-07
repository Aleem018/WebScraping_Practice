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
    public async Task<List<Quote>> RunScraperAsync(string url)
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
            }
            else
            {
                Console.WriteLine("Parent container does not exist");
            }

            var buttonHtml = htmlDoc.DocumentNode.SelectSingleNode("//li[@class='next']/a");

            if (buttonHtml != null)
            {
                href = buttonHtml.GetAttributeValue("href", string.Empty);
            }
            else
            {
                nextBtn = false;
            }
        }

        return output;

    }
}

public partial class Form1 : Form
{
    // private Button btnScrape;
    private TextBox urlInput;
    private Button urlButton;
    // private ListBox scrapedData;

    public Form1()
    {
        this.Text = "Web Scraper";
        this.Size = new Size(700, 400);
        this.StartPosition = FormStartPosition.CenterScreen;

        urlInput = new TextBox();
        urlInput.Location = new Point(250, 120);
        urlInput.Size = new Size(200, 30);
        urlInput.PlaceholderText = "Enter the target url";
        this.Controls.Add(urlInput); //this is string url

        urlButton = new Button();
        urlButton.Text = "SCRAPE";
        urlButton.Location = new Point(250, 160);
        urlButton.Size = new Size(200, 40);
        urlButton.BackColor = Color.Green;
        urlButton.ForeColor = Color.White;
        this.Controls.Add(urlButton);
        urlButton.Click += btnScrape_Click;

    }

    private async void btnScrape_Click(object? sender, EventArgs e)
    {
        try
        {
            urlButton.Text = "Scraping in progress";

            string input = urlInput.Text;

            QuoteScraper scraper = new QuoteScraper();
            List<Quote> results = await scraper.RunScraperAsync(input);

            string userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            string targetFolder = Path.Combine(userPath, "Downloads");
            string downloadPath = Path.Combine(targetFolder, "my_scraped_data.csv");

            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
            }

            using StreamWriter writer = new StreamWriter(downloadPath, false, new UTF8Encoding(true));

            writer.WriteLine("Quote,Author");

            foreach (var quote in results)
            {
                quote.Text = quote.Text.Replace("\"", "\"\"");
                quote.Author = quote.Author.Replace("\"", "\"\"");

                writer.WriteLine($"\"{quote.Text}\",\"{quote.Author}\"");
            }

            urlButton.Text = "Done";
            MessageBox.Show($"Successfully scraped {results.Count} quotes!\n Quotes saved at {downloadPath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Execution failed: {ex.Message}");
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        Graphics g = e.Graphics;

        string textToDraw = "Welcome to my faux web scraper!";
        
        using (Font myFont = new Font("Times New Roman", 16, FontStyle.Bold))
        using (Brush myBrush = new SolidBrush(Color.Black))
        {
            float xLocation = 150;
            float yLocation = 30;

            g.DrawString(textToDraw, myFont, myBrush, xLocation, yLocation);
        }

        string anotherText = "Enter a url, and click the scrape button below";
        using (Font anotherFont = new Font("Arial", 10, FontStyle.Regular))
        using (Brush anotherBrush = new SolidBrush(Color.Chocolate))
        {
            float anotherXLocation = 170;
            float anotherYLocation = 80;

            g.DrawString(anotherText, anotherFont, anotherBrush, anotherXLocation, anotherYLocation);
        }
    }
}
