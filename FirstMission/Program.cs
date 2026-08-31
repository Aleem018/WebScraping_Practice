using System.Net.Http;
using HtmlAgilityPack;

HttpClient httpClient = new HttpClient();
string url = "http://quotes.toscrape.com/";

string htmlContent = await httpClient.GetStringAsync(url);

HtmlDocument htmlDocument = new HtmlDocument();
htmlDocument.LoadHtml(htmlContent);

var firstQuote = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='quote']/span[@class='text']").InnerText;

var author = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='quote']/span/small[@class='author']").InnerText;

Console.WriteLine($"Quote: {firstQuote}");
Console.WriteLine($"Author: {author}");
