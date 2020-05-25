using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using ScrapySharp.Network;

namespace market_rates
{
    class Program
    {
        static ScrapingBrowser _scrapingBrowser = new ScrapingBrowser();

        static void Main(string[] args)
        {
            var colorCyan = ConsoleColor.Cyan;
            var colorGreen = ConsoleColor.Green;
            var list = new List<Currency>();
           
            list = GetDolarValues("https://www.dolarhoy.com/");
            
            Console.WriteLine($"{DateTime.Now.ToShortDateString()} | {DateTime.Now.ToShortTimeString()} - Cotización DolarHoy.com");
            Console.WriteLine($"----------------------------------------------\n");
            foreach (var item in list){
                Console.ForegroundColor = item.Name.Contains("Blue")? colorCyan : colorGreen;
                Console.WriteLine($"{item.Name}");
                Console.ResetColor();
                Console.WriteLine($"Compra: {item.BuyValue} | Venta: {item.SellValue}");
                Console.WriteLine($"==================================\n");
            }
            Console.ReadLine();
        }

static List<Currency> GetDolarValues(string url){
    var values = new List<Currency>();
    var html = GetHtml(url);
    var dolarList = html.SelectNodes("//div[@class='pill pill-coti']");
    
    foreach (var dolar in dolarList ){
        
        var name = dolar.SelectSingleNode("./h4");
        var buyValue = dolar.SelectSingleNode("./div/div[1]/span");
        var sellValue = dolar.SelectSingleNode("./div/div[2]/span");
        var variation = dolar.SelectSingleNode("./div[@class='foot']/div/div[1]");
        var date = dolar.SelectSingleNode("./div[@class='foot']/div/div[2]");

        var dolarObj = new Currency();
        
        dolarObj.Name = name != null ? name.InnerText.Trim() : string.Empty;
        dolarObj.BuyValue = buyValue != null ? buyValue.InnerText.Trim() : string.Empty;
        dolarObj.SellValue = sellValue != null ? sellValue.InnerText.Trim() : string.Empty;
        dolarObj.Variation = variation != null ? variation.InnerText.Trim() : string.Empty;
        dolarObj.Date = date != null ? date.InnerText.Trim() : string.Empty;

        values.Add(dolarObj);
    }
    return values;
}
        static HtmlNode GetHtml(string url){
            WebPage webpage = _scrapingBrowser.NavigateToPage(new Uri(url));
            return webpage.Html;
        }
    }
}
