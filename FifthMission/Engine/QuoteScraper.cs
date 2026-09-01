using FifthMision.Models;
using HtmlAgilityPack;
using System.Net;
using System.Text;

namespace Engine.QuoteScraper;
public class QuoteScraper
{
    public async Task<List<Quote>> RunScraperAsync()
    {

        HttpClient client = new HttpClient();

        string url = "http://quotes.toscrape.com/";
        string href = "";

        bool nextBtn = true;

        List<Quote> output = new List<Quote>();

        while (nextBtn)
        {
            string tempUrl = url + href;
            var htmlContent = await client.GetStringAsync(tempUrl);

            HtmlDocument htmlDoc = new HtmlDocument();
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
                    newQuote.text = $"\"{cleanText}\"";
                    newQuote.author = $"\"{cleanAuthor}";

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
