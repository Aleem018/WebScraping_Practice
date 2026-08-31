using System.Net.Http;
using HtmlAgilityPack;

HttpClient client = new HttpClient();
string url = "http://quotes.toscrape.com/";

var htmlShi = await client.GetStringAsync(url);

HtmlDocument htmlDoc = new HtmlDocument();
htmlDoc.LoadHtml(htmlShi);

var parentContainer = htmlDoc.DocumentNode.SelectNodes("//div[@class='quote']");

if (parentContainer != null)
{
    int counter = 0;
    foreach (var quote in parentContainer)
    {
        var text = quote.SelectSingleNode(".//span[@class='text']").InnerText;
        var author = quote.SelectSingleNode(".//span/small[@class='author']").InnerText;

        counter += 1;

        Console.WriteLine($"Quote {counter}: {text}");
        Console.WriteLine($"Author {counter}: {author}");
    }
} else
{
    Console.WriteLine("Parent container does not exist");
}
