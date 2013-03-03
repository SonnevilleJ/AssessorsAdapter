namespace AssessorsAdapter
{
    public class House
    {
        private const string Url = "http://www.assess.co.polk.ia.us/cgi-bin/invenquery/homequery.cgi?method=GET&address={0}&photo={2}&map={3}&jurisdiction={1}";

        public House(string address, string city = "COUNTY-WIDE", bool photo = true, bool map = false)
        {
            ProcessHtmlPage(address, city, photo, map);
        }

        private void ProcessHtmlPage(string address, string city, bool photo, bool map)
        {
            Address = address;
        }

        public string Address { get; private set; }
    }
}
