
using html_serializer;
using System.Text.RegularExpressions;

var html = await Load("http://example.com");
var cleanHtml = new Regex("\\s+").Replace(html, " ");

var htmlLines = new Regex("<(.*?)>").Split(cleanHtml).Where(s => !string.IsNullOrWhiteSpace(s));

var helper = HtmlHelper.Instance;

var root = new HtmlElement { Name = "root" };
var current = root;

var attrRegex = new Regex("([^\\s]*?)=\"(.*?)\"");

foreach (var line in htmlLines)
{
    var firstWord = line.Split(' ')[0].TrimEnd('/');

    if (firstWord == "/html")
        break;

    if (firstWord.StartsWith("/"))
    {
        if (current.Parent != null)
            current = current.Parent;
        continue;
    }
    if (helper.AllTags.Contains(firstWord))
    {
        var element = new HtmlElement
        {
            Name = firstWord,
            Parent = current
        };

        var rest = line.Substring(line.Split(' ')[0].Length);

        foreach (Match m in attrRegex.Matches(rest))
        {
            var name = m.Groups[1].Value;
            var value = m.Groups[2].Value;

            element.Attributes.Add(m.Value);

            if (name == "id")
                element.Id = value;
            else if (name == "class")
                element.Classes.AddRange(value.Split(' '));
        }
        current.Children.Add(element);

        bool selfClosing = line.EndsWith("/") || helper.SelfClosingTags.Contains(firstWord);
        if (!selfClosing)
            current = element;
    }
    else
    {
        current.InnerHtml += line;
    }
}
async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}
