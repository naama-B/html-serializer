namespace html_serializer
{
    public static class HtmlElementExtensions
    {
        public static IEnumerable<HtmlElement> FindElements(this HtmlElement element, Selector selector)
        {
            var result = new HashSet<HtmlElement>();
            FindRecursive(element, selector, result);
            return result;
        }

        private static void FindRecursive(HtmlElement element, Selector selector, HashSet<HtmlElement> result)
        {
            // צאצאים שמתאימים לסלקטור הנוכחי (בלי האלמנט עצמו)
            var matches = element.Descendants()
                                 .Where(d => d != element && IsMatch(d, selector));

            // תנאי עצירה – הסלקטור האחרון
            if (selector.Child == null)
            {
                foreach (var m in matches)
                    result.Add(m);
                return;
            }

            // אחרת – ממשיכים לרמה הבאה
            foreach (var m in matches)
                FindRecursive(m, selector.Child, result);
        }

        private static bool IsMatch(HtmlElement el, Selector selector)
        {
            if (selector.TagName != null && el.Name != selector.TagName)
                return false;

            if (selector.Id != null && el.Id != selector.Id)
                return false;

            if (selector.Classes.Any(c => !el.Classes.Contains(c)))
                return false;

            return true;
        }
    }
}