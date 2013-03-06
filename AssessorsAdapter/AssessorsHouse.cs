using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using HtmlAgilityPack;

namespace AssessorsAdapter
{
#if DEBUG
    [ClassInterface(ClassInterfaceType.AutoDual)]
#endif
    public class AssessorsHouse : IHouse
    {
        private const string QueryUrl = @"http://www.assess.co.polk.ia.us/cgi-bin/invenquery/homequery.cgi?method=GET&address={0}&photo={2}&map={3}&jurisdiction={1}";

        public string HomeUrl { get; set; }

        #region Accessors

        public string Address { get; private set; }

        public string City { get; private set; }

        public string Zip { get; private set; }

        public int AssessmentTotal { get; private set; }

        public int Land { get; private set; }

        public bool MultipleRecordsFound { get; private set; }

        public bool NoRecordsFound { get; private set; }

        public int TSFLA { get; private set; }

        public bool DataAvailable { get; private set; }

        public int BsmtArea { get; private set; }

        public int YearBuilt { get; private set; }

        public int Fireplaces { get; private set; }

        public decimal GrossTaxes { get; private set; }

        #endregion

        #region Error checking

        private bool CheckNoResultsFound(HtmlDocument doc)
        {
            NoRecordsFound = doc.DocumentNode.InnerText.Contains(@"0 Records");
            return NoRecordsFound;
        }

        private bool CheckMoreThanOneResultFound(HtmlDocument doc)
        {
            MultipleRecordsFound = doc.DocumentNode.InnerText.Contains(@"Click on District/Parcel Button");
            return MultipleRecordsFound;
        }

        #endregion

        #region Parse Methods

        private void ParseHtml(HtmlDocument document)
        {
            ParseAddress(document);
            ParseAssessment(document);
            ParseLand(document);
            ParseTsfla(document);
            ParseBsmtArea(document);
            ParseYearBuilt(document);
            ParseFireplaces(document);
            ParseTaxes(document);
        }

        private void ParseAddress(HtmlDocument document)
        {
            var addressNode = document.DocumentNode.Descendants("a").First(node => node.InnerText == "Street Address");
            var addressTrNode = addressNode.ParentNode.ParentNode;
            var address = addressTrNode.NextSibling.NextSibling.InnerText.Split(new[] {'\n'}, StringSplitOptions.RemoveEmptyEntries);

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

        private static string ParseResidenceProperty(HtmlDocument document, string nodeName)
        {
            var tsflaNode = document.DocumentNode.Descendants("strong").First(node => node.InnerText == nodeName);
            var tsflaTdNode = tsflaNode.ParentNode;
            var tsfla = tsflaTdNode.NextSibling.NextSibling.InnerText;
            return tsfla;
        }

        private void ParseTsfla(HtmlDocument document)
        {
            TSFLA = FormatInt(ParseResidenceProperty(document, "TSFLA"));
        }

        private void ParseBsmtArea(HtmlDocument document)
        {
            BsmtArea = FormatInt(ParseResidenceProperty(document, "BSMT AREA"));
        }

        private void ParseYearBuilt(HtmlDocument document)
        {
            YearBuilt = FormatInt(ParseResidenceProperty(document, "YEAR BUILT"));
        }

        private void ParseFireplaces(HtmlDocument document)
        {
            Fireplaces = FormatInt(ParseResidenceProperty(document, "FIREPLACES"));
        }

        private void ParseTaxes(HtmlDocument document)
        {
            var taxLink = document.DocumentNode.Descendants("a").First(node => node.InnerText == "Polk County Treasurer Tax Information").Attributes["href"].Value;
            var taxPage = DownloadHtml(taxLink);
            var taxTd = taxPage.DocumentNode.Descendants("td").Last(node => node.InnerText.Contains("Equals Gross Tax"));
            var grossTaxText = taxTd.NextSibling.NextSibling.InnerText.Trim(' ', '$');
            GrossTaxes = decimal.Parse(grossTaxText.Replace(",", ""));
        }

        #endregion

        #region Formatters

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
                    if (lastWasSpace) continue;
                    lastWasSpace = true;
                }
                else lastWasSpace = false;
                builder.Append(letter);
            }
            return builder.ToString().Trim();
        }

        #endregion

        #region Constructors

        public void FetchData(string address)
        {
            FetchData(address, "COUNTY-WIDE", true, false);
        }

        public void FetchData(string address, string city, bool photo, bool map)
        {
            HomeUrl = string.Format(QueryUrl, Uri.EscapeUriString(address), city.ToUpper(), photo ? "checked" : "", map ? "checked" : "");
            var doc = DownloadHtml(HomeUrl);

            if (CheckNoResultsFound(doc) || CheckMoreThanOneResultFound(doc)) return;
            ParseHtml(doc);
            DataAvailable = true;
        }

        private static HtmlDocument DownloadHtml(string homeUrl)
        {
            var client = new WebClient {Proxy = {Credentials = CredentialCache.DefaultNetworkCredentials}};
            var html = client.DownloadString(homeUrl);

            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            return doc;
        }

        #endregion

        public void OpenWebPage()
        {
            Process.Start("chrome", HomeUrl);
        }
    }
}
