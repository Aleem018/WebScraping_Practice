using System.Net.Http;
using HtmlAgilityPack;

HttpClient client = new HttpClient();

string url = "http://quotes.toscrape.com";
string nextHref = "";

bool nextBtn = true;
int pageCounter = 1;
while (nextBtn)
{

    string tempUrl = url + nextHref;

    var htmlShi = await client.GetStringAsync(tempUrl);

    HtmlDocument htmlDoc = new HtmlDocument();
    htmlDoc.LoadHtml(htmlShi);

    var parentContainer = htmlDoc.DocumentNode.SelectNodes("//div[@class='quote']");   

    if (parentContainer != null)
    {
        int counter = 0;
        Console.WriteLine($"Page {pageCounter}");
        foreach (var quote in parentContainer)
        {
            var text = quote.SelectSingleNode(".//span[@class='text']").InnerText;
            var author = quote.SelectSingleNode(".//span/small[@class='author']").InnerText;

            counter += 1;

            Console.WriteLine($"Quote {counter}: {text}");
            Console.WriteLine($"Author {counter}: {author}");
        }
    }
    else
    {
        Console.WriteLine("Parent container does not exist");
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
    Console.WriteLine("-----------------------------------------------------");

}





