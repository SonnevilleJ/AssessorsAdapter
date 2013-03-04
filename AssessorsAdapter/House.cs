using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace AssessorsAdapter
{
    public class House
    {
        private const string QueryUrl = @"http://www.assess.co.polk.ia.us/cgi-bin/invenquery/homequery.cgi?method=GET&address={0}&photo={2}&map={3}&jurisdiction={1}";

        public House(string address, string city = "COUNTY-WIDE", bool photo = true, bool map = false)
        {
            var url = string.Format(QueryUrl, Uri.EscapeUriString(address), city.ToUpper(), photo ? "checked" : "", map ? "checked" : "");
            var client = new HtmlWeb();
            var doc = client.Load(url);

            if (NoResultsFound(doc))
            {
                Process.Start("chrome", url);
            }
            ParseHtml(doc);
            
        }

        public string Address { get; private set; }

        public string City { get; private set; }

        public string Zip { get; private set; }

        public int AssessmentTotal { get; private set; }

        public int Land { get; private set; }

        private static bool NoResultsFound(HtmlDocument doc)
        {
            return doc.DocumentNode.InnerText.Contains("0 Records");
        }

        private void ParseHtml(HtmlDocument document)
        {
            ParseAddress(document);
            ParseAssessment(document);
            ParseLand(document);
        }

        private void ParseAddress(HtmlDocument document)
        {
            var addressNode = document.DocumentNode.Descendants("a").First(node => node.InnerText == "Street Address");
            var addressTrNode = addressNode.ParentNode.ParentNode;
            var address = addressTrNode.NextSibling.NextSibling.InnerText.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            Address = RemoveDuplicateSpaces(address[0]);
            ParseCityAndZip(address[1]);
        }

        private void ParseCityAndZip(string city)
        {
            var strings = city.Split(new[] {" IA "}, StringSplitOptions.None);
            City = strings[0];
            Zip = strings[1];
        }

        private void ParseAssessment(HtmlDocument document)
        {
            var assessmentNode = document.DocumentNode.Descendants("a").First(node => node.InnerText == "Assessment");
            var assessmentTrNode = assessmentNode.ParentNode.ParentNode;
            var assessmentTds = assessmentTrNode.NextSibling.SelectNodes("td");

            AssessmentTotal = FormatInt(assessmentTds.Last().InnerText);
        }

        private void ParseLand(HtmlDocument document)
        {
            var landNode = document.DocumentNode.Descendants("a").First(node => node.InnerText == "Land");
            var landTrNode = landNode.ParentNode.ParentNode.ParentNode;
            var landTds = landTrNode.NextSibling.SelectNodes("td");

            Land = FormatInt(landTds[1].InnerText);
        }

        private static int FormatInt(string innerText)
        {
            return int.Parse(innerText.Replace(",", ""));
        }

        private static string RemoveDuplicateSpaces(string addressString)
        {
            var builder = new StringBuilder();
            var lastWasSpace = false;
            foreach (var letter in addressString)
            {
                if (letter == ' ')
                {
                    if(lastWasSpace) continue;
                    lastWasSpace = true;
                }
                else
                {
                    lastWasSpace = false;
                }
                builder.Append(letter);
            }
            return builder.ToString().Trim();
        }
    }
}
