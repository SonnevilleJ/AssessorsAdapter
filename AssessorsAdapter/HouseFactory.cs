using System;
using System.Linq;
using System.Net;
using System.Text;
using AssessorsAdapter.Persistence;
using HtmlAgilityPack;

namespace AssessorsAdapter
{
    public class HouseFactory : IHouseFactory
    {
        private const string QueryUrl = @"http://www.assess.co.polk.ia.us/cgi-bin/invenquery/homequery.cgi?method=GET&address={0}&photo={2}&map={3}&jurisdiction={1}";

        public IHouse ConstructHouse(string address)
        {
            return ConstructHouse(address, "COUNTY-WIDE", true, false);
        }

        public IHouse ConstructHouse(string address, string city, bool photo, bool map)
        {
            var doc = DownloadHtml(BuildHomeUrl(address, city, photo, map));

            return ConstructHouse(doc);
        }

        public IHouse ConstructHouse(HtmlDocument housePage)
        {
            return ConstructHouse(housePage, null);
        }

        public IHouse ConstructHouse(HtmlDocument housePage, HtmlDocument taxPage)
        {
            var house = new House
                {
                    NoRecordsFound = CheckNoResultsFound(housePage),
                    MultipleRecordsFound = CheckMoreThanOneResultFound(housePage)
                };

            if (house.NoRecordsFound || house.MultipleRecordsFound)
            {
                house.DataAvailable = false;
            }
            else
            {
                if(taxPage == null)
                {
                    var taxLink = housePage.DocumentNode.Descendants("a").First(node => node.InnerText == "Polk County Treasurer Tax Information").Attributes["href"].Value;
                    taxPage = DownloadHtml(taxLink);
                }
                ParseHtml(house, housePage, taxPage);
                house.DataAvailable = true;
            }
            return house;
        }

        public IHouse Clone(IHouse assessorsHouse)
        {
            return new House(assessorsHouse);
        }

        private string BuildHomeUrl(string address, string city, bool photo, bool map)
        {
            return String.Format(QueryUrl, Uri.EscapeUriString(address), city.ToUpper(), photo ? "checked" : String.Empty, map ? "checked" : String.Empty);
        }

        #region Error checking

        private bool CheckNoResultsFound(HtmlDocument doc)
        {
            return doc.DocumentNode.InnerText.Contains(@"0 Records");
        }

        private bool CheckMoreThanOneResultFound(HtmlDocument doc)
        {
            return doc.DocumentNode.InnerText.Contains(@"Click on District/Parcel Button");
        }

        #endregion

        #region Parse Methods

        private void ParseHtml(IHouse house, HtmlDocument housePage, HtmlDocument taxPage)
        {
            var address = ParseAddress(housePage);

            house.Address = RemoveDuplicateSpaces(address[0]);
            var strings = address[1].Split(new[] {" IA "}, StringSplitOptions.None);
            house.City = strings[0];
            house.Zip = strings[1];

            house.AssessmentTotal = ParseAssessment(housePage);
            house.Land = ParseLand(housePage);
            house.TSFLA = ParseTsfla(housePage);
            house.BsmtArea = ParseBsmtArea(housePage);
            house.YearBuilt = ParseYearBuilt(housePage);
            house.Fireplaces = ParseFireplaces(housePage);

            house.GrossTaxes = ParseTaxes(taxPage);
        }

        private string[] ParseAddress(HtmlDocument document)
        {
            var addressNode = document.DocumentNode.Descendants("a").First(node => node.InnerText == "Street Address");
            var addressTrNode = addressNode.ParentNode.ParentNode;
            var address = addressTrNode.NextSibling.NextSibling.InnerText.Split(new[] {'\n'}, StringSplitOptions.RemoveEmptyEntries);
            return address;
        }

        private int ParseAssessment(HtmlDocument document)
        {
            var assessmentNode = document.DocumentNode.Descendants("a").First(node => node.InnerText == "Assessment");
            var assessmentTrNode = assessmentNode.ParentNode.ParentNode;
            var assessmentTds = assessmentTrNode.NextSibling.SelectNodes("td");

            return FormatInt(assessmentTds.Last().InnerText);
        }

        private int ParseLand(HtmlDocument document)
        {
            var landNode = document.DocumentNode.Descendants("a").First(node => node.InnerText == "Land");
            var landTrNode = landNode.ParentNode.ParentNode.ParentNode;
            var landTds = landTrNode.NextSibling.SelectNodes("td");

            return FormatInt(landTds[1].InnerText);
        }

        private string ParseResidenceProperty(HtmlDocument document, string nodeName)
        {
            var tsflaNode = document.DocumentNode.Descendants("strong").First(node => node.InnerText == nodeName);
            var tsflaTdNode = tsflaNode.ParentNode;
            var tsfla = tsflaTdNode.NextSibling.NextSibling.InnerText;
            return tsfla;
        }

        private int ParseTsfla(HtmlDocument document)
        {
            return FormatInt(ParseResidenceProperty(document, "TSFLA"));
        }

        private int ParseBsmtArea(HtmlDocument document)
        {
            return FormatInt(ParseResidenceProperty(document, "BSMT AREA"));
        }

        private int ParseYearBuilt(HtmlDocument document)
        {
            return FormatInt(ParseResidenceProperty(document, "YEAR BUILT"));
        }

        private int ParseFireplaces(HtmlDocument document)
        {
            return FormatInt(ParseResidenceProperty(document, "FIREPLACES"));
        }

        private decimal ParseTaxes(HtmlDocument taxPage)
        {
            var taxTd = taxPage.DocumentNode.Descendants("td").Last(node => node.InnerText.Contains("Equals Gross Tax"));
            var grossTaxText = taxTd.NextSibling.NextSibling.InnerText.Trim(' ', '$');
            return Decimal.Parse(grossTaxText.Replace(",", ""));
        }

        #endregion

        #region Formatters

        private int FormatInt(string innerText)
        {
            return Int32.Parse(innerText.Replace(",", ""));
        }

        private string RemoveDuplicateSpaces(string addressString)
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

        private HtmlDocument DownloadHtml(string homeUrl)
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
