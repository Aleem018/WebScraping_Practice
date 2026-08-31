using System.Net.Http;
using HtmlAgilityPack;
using System.IO;
using System.Net;
using System.Text;
using StreamWriter writer = new StreamWriter("scraped_quotes.csv", false, new UTF8Encoding(true));

HttpClient client = new HttpClient();

string url = "http://quotes.toscrape.com";
string nextHref = "";

bool nextBtn = true;
int pageCounter = 1;
writer.WriteLine("Quote,Author");
while (nextBtn)
{

    string tempUrl = url + nextHref;

    var htmlShi = await client.GetStringAsync(tempUrl);

    HtmlDocument htmlDoc = new HtmlDocument();
    htmlDoc.LoadHtml(htmlShi);

    var parentContainer = htmlDoc.DocumentNode.SelectNodes("//div[@class='quote']");   

    if (parentContainer != null)
    {   
        foreach (var quote in parentContainer)
        {
            var text = quote.SelectSingleNode(".//span[@class='text']").InnerText;
            var author = quote.SelectSingleNode(".//span/small[@class='author']").InnerText;

            var cleanText = WebUtility.HtmlDecode(text);
            var cleanAuthor = WebUtility.HtmlDecode(author);

            cleanText = cleanText.Replace("\"", "\"\"");
            cleanAuthor = cleanAuthor.Replace("\"", "\"\"");


            writer.WriteLine($"\"{cleanText}\",\"{cleanAuthor}\"");
        }
    }
    else
    {
        writer.WriteLine("Parent container does not exist");
    }

    var btnNode = htmlDoc.DocumentNode.SelectSingleNode("//li[@class='next']/a");
    
    if (btnNode != null)
    {
        nextHref = btnNode.GetAttributeValue("href", string.Empty);
        pageCounter += 1;
    } else
    {
        nextBtn = false;
        continue;
    }

    Console.WriteLine("Saved Successfully");

}

Console.WriteLine("Saved successfully");