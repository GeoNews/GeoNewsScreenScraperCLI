using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using GeoNewsScreenScraperCLI.Database;
using System.Net;
using System.Diagnostics;

namespace GeoNewsScreenScraperCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            ISet<string> list = FrountPageStories();
            foreach (string url in list)
            {
                Debug.WriteLine(url);
                //GetAndSaveStory(url);
            }
        }

        static ISet<string> FrountPageStories()
        {
            ISet<string> storiesOnFrontPage = new HashSet<string>();
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load("http://www.stuff.co.nz/");
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//div[contains(@class,'display-asset')]//a");
            foreach (HtmlNode node in nodes)
            {
                string url = node.Attributes["href"].Value;
                bool validStory = false;
                foreach (string s in url.Split('/'))
                {
                    int i = 0;
                    if (int.TryParse(s,out i))
                    {
                        validStory = true;
                    }
                }
                if (!validStory)
                {
                    continue;
                }
                if (url.StartsWith("/"))
                {
                    url = "http://www.stuff.co.nz" + url;
                }
                storiesOnFrontPage.Add(url);
            }
            return storiesOnFrontPage;
        }

        static void GetAndSaveStory(int StoryId)
        {
            HtmlDocument doc = GetDocFromStroyId(StoryId);
            NewsItem newsItem = CreateNewsItem(doc,false);
            Debug.WriteLine(newsItem.HeadLine);
            Debug.WriteLine(newsItem.Paragraphs.Count);
            SaveNewsItem(newsItem);
        }

        static void GetAndSaveStory(string url)
        {
            HtmlDocument doc = GetDocFromString(url);
            NewsItem newsItem = CreateNewsItem(doc, true);
            Debug.WriteLine(newsItem.HeadLine);
            Debug.WriteLine(newsItem.Paragraphs.Count);
            SaveNewsItem(newsItem);
        }

        static HtmlDocument GetDocFromStroyId(int StoryId)
        {
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create("http://www.stuff.co.nz/" + StoryId);
            Console.WriteLine("Requestion data from http://www.stuff.co.nz/" + StoryId);
            myHttpWebRequest.MaximumAutomaticRedirections = 1;
            myHttpWebRequest.AllowAutoRedirect = true;
            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
            HtmlDocument doc = new HtmlDocument();
            doc.Load(myHttpWebResponse.GetResponseStream());
            return doc;
        }

        static HtmlDocument GetDocFromString(string url)
        {
            HtmlWeb web = new HtmlWeb();
            return web.Load(url); 
        }

        static NewsItem CreateNewsItem(HtmlDocument doc, bool ArticalDefined)
        {
            HtmlNode articleRoot = doc.DocumentNode.SelectSingleNode("//article");
            HtmlNodeCollection paragraphs;
            HtmlNode headline;
            if (ArticalDefined)
            {
                headline = doc.DocumentNode.SelectSingleNode("//article//h1");
                paragraphs = doc.DocumentNode.SelectNodes("//article/p/text()");
            }
            else
            {
                headline = doc.DocumentNode.SelectSingleNode("//h1");
                paragraphs = doc.DocumentNode.SelectNodes("//div[contains(@id,'left_col')]//p");
            }
            HtmlNode author = doc.DocumentNode.SelectSingleNode("//article//p[@itemprop=\"author\"]");
            HtmlNode articleDate = doc.DocumentNode.SelectSingleNode("//article//span[@itemprop=\"datePublished\"]");
            DateTime dt = DateTime.Now;
            NewsItem newsItem = new NewsItem();
            string[] hl = headline.InnerText.Split('<');
            newsItem.HeadLine = hl[0].Trim();
            newsItem.NewsItemDate = DateTime.Now;
            if (paragraphs == null)
            {
                return newsItem;
            }
            int i = 1;
            foreach (HtmlNode paragraph in paragraphs)
            {
                if (!String.IsNullOrWhiteSpace(paragraph.InnerText))
                {
                    newsItem.Paragraphs.Add(new Paragraph() { Text = paragraph.InnerText, ParagraphNumber = i }); 
                }
                i++;
            }
            return newsItem;
        }

        static void SaveNewsItem(NewsItem item)
        {
            Model1 db = new Model1();
            db.NewsItems.Add(item);
            db.SaveChanges();
        }
    }
}
