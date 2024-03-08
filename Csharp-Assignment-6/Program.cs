
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

using HtmlAgilityPack;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Enter url to read");
        string url=Console.ReadLine();

        string content=await ReadUrlAsync(url);
        if (!string.IsNullOrEmpty(content))
        {
            await WriteToFileAsync("A.txt",content);
            Console.WriteLine("Writing to file A.txt successfully completed");
        }
        else
        {
            Console.WriteLine("Failed to read content from the URL.");
        }
    }
    static async Task<string> ReadUrlAsync(string url)
    {
        try
        {
            using(var client=new WebClient())
            {
                string htmlContent= await client.DownloadStringTaskAsync(url);
                return ExtractMainContent(htmlContent);
                //return ExtractMainContent(htmlContent);

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading {ex.Message}");
            return null;
        }
    }
    static string ExtractMainContent(string htmlContent)
    {
        HtmlDocument doc = new HtmlDocument();  
        doc.LoadHtml(htmlContent);
        var mainContent = doc.DocumentNode.SelectNodes("//p");
        if (mainContent != null)
        {
            return string.Join("\n",mainContent.Select(node=>node.InnerText));

        }
        return null;
    }
    static async Task WriteToFileAsync(string filename,string content)
    {
        try
        {
            await File.WriteAllTextAsync(filename, content);

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writting file operation {ex.Message}");
        }
    }
}