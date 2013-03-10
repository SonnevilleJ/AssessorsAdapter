using System;
using System.Linq;
using System.Net;
using System.Text;
using AssessorsAdapter.Persistence;
using HtmlAgilityPack;

namespace AssessorsAdapter
{
    public static class HouseFactory
    {
        private const string QueryUrl = @"http://www.assess.co.polk.ia.us/cgi-bin/invenquery/homequery.cgi?method=GET&address={0}&photo={2}&map={3}&jurisdiction={1}";

        public static IHouse ConstructHouse(string address)
        {
            return ConstructHouse(address, "COUNTY-WIDE", true, false);
        }

        public static IHouse ConstructHouse(string address, string city, bool photo = true, bool map = false)
        {
            var doc = DownloadHtml(BuildHomeUrl(address, city, photo, map));

            return ConstructHouse(doc, address, city, photo, map);
        }

        public static IHouse ConstructHouse(HtmlDocument doc, string address, string city, bool photo = true, bool map = false)
        {
            var house = new House();
            house.HomeUrl = BuildHomeUrl(address, city, photo, map);
            house.NoRecordsFound = CheckNoResultsFound(doc);
            house.MultipleRecordsFound = CheckMoreThanOneResultFound(doc);

            if (house.NoRecordsFound || house.MultipleRecordsFound)
            {
                house.DataAvailable = false;
            }
            else
            {
                ParseHtml(doc, house);
                house.DataAvailable = true;
            }
            return house;
        }

        private static string BuildHomeUrl(string address, string city, bool photo, bool map)
        {
            return String.Format(QueryUrl, Uri.EscapeUriString(address), city.ToUpper(), photo ? "checked" : String.Empty, map ? "checked" : String.Empty);
        }

        public static IHouse Clone(IHouse assessorsHouse)
        {
            return new House(assessorsHouse);
        }

        #region Error checking

        private static bool CheckNoResultsFound(HtmlDocument doc)
        {
            return doc.DocumentNode.InnerText.Contains(@"0 Records");
        }

        private static bool CheckMoreThanOneResultFound(HtmlDocument doc)
        {
            return doc.DocumentNode.InnerText.Contains(@"Click on District/Parcel Button");
        }

        #endregion

        #region Parse Methods

        private static void ParseHtml(HtmlDocument document, IHouse house)
        {
            var address = ParseAddress(document);

            house.Address = RemoveDuplicateSpaces(address[0]);
            var strings = address[1].Split(new[] {" IA "}, StringSplitOptions.None);
            house.City = strings[0];
            house.Zip = strings[1];

            house.AssessmentTotal = ParseAssessment(document);
            house.Land = ParseLand(document);
            house.TSFLA = ParseTsfla(document);
            house.BsmtArea = ParseBsmtArea(document);
            house.YearBuilt = ParseYearBuilt(document);
            house.Fireplaces = ParseFireplaces(document);
            house.GrossTaxes = ParseTaxes(document);
        }

        private static string[] ParseAddress(HtmlDocument document)
        {
            var addressNode = document.DocumentNode.Descendants("a").First(node => node.InnerText == "Street Address");
            var addressTrNode = addressNode.ParentNode.ParentNode;
            var address = addressTrNode.NextSibling.NextSibling.InnerText.Split(new[] {'\n'}, StringSplitOptions.RemoveEmptyEntries);
            return address;
        }

        private static int ParseAssessment(HtmlDocument document)
        {
            var assessmentNode = document.DocumentNode.Descendants("a").First(node => node.InnerText == "Assessment");
            var assessmentTrNode = assessmentNode.ParentNode.ParentNode;
            var assessmentTds = assessmentTrNode.NextSibling.SelectNodes("td");

            return FormatInt(assessmentTds.Last().InnerText);
        }

        private static int ParseLand(HtmlDocument document)
        {
            var landNode = document.DocumentNode.Descendants("a").First(node => node.InnerText == "Land");
            var landTrNode = landNode.ParentNode.ParentNode.ParentNode;
            var landTds = landTrNode.NextSibling.SelectNodes("td");

            return FormatInt(landTds[1].InnerText);
        }

        private static string ParseResidenceProperty(HtmlDocument document, string nodeName)
        {
            var tsflaNode = document.DocumentNode.Descendants("strong").First(node => node.InnerText == nodeName);
            var tsflaTdNode = tsflaNode.ParentNode;
            var tsfla = tsflaTdNode.NextSibling.NextSibling.InnerText;
            return tsfla;
        }

        private static int ParseTsfla(HtmlDocument document)
        {
            return FormatInt(ParseResidenceProperty(document, "TSFLA"));
        }

        private static int ParseBsmtArea(HtmlDocument document)
        {
            return FormatInt(ParseResidenceProperty(document, "BSMT AREA"));
        }

        private static int ParseYearBuilt(HtmlDocument document)
        {
            return FormatInt(ParseResidenceProperty(document, "YEAR BUILT"));
        }

        private static int ParseFireplaces(HtmlDocument document)
        {
            return FormatInt(ParseResidenceProperty(document, "FIREPLACES"));
        }

        private static decimal ParseTaxes(HtmlDocument document)
        {
            var taxLink = document.DocumentNode.Descendants("a").First(node => node.InnerText == "Polk County Treasurer Tax Information").Attributes["href"].Value;
            var taxPage = DownloadHtml(taxLink);
            var taxTd = taxPage.DocumentNode.Descendants("td").Last(node => node.InnerText.Contains("Equals Gross Tax"));
            var grossTaxText = taxTd.NextSibling.NextSibling.InnerText.Trim(' ', '$');
            return Decimal.Parse(grossTaxText.Replace(",", ""));
        }

        #endregion

        #region Formatters

        private static int FormatInt(string innerText)
        {
            return Int32.Parse(innerText.Replace(",", ""));
        }

        private static string RemoveDuplicateSpaces(string addressString)
        {
            var builder = new StringBuilder();
            var lastWasSpace = false;
            foreach (var letter in addressString)
            {
                if (letter == ' ')
                {
                    if (lastWasSpace) continue;
                    lastWasSpace = true;
                }
                else lastWasSpace = false;
                builder.Append(letter);
            }
            return builder.ToString().Trim();
        }

        #endregion

        #region Data retrieval

        private static HtmlDocument DownloadHtml(string homeUrl)
        {
            var client = new WebClient {Proxy = {Credentials = CredentialCache.DefaultNetworkCredentials}};
            var html = client.DownloadString(homeUrl);

            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            return doc;
        }

        #endregion
    }
}
