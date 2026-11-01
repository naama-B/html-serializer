using System.Text.RegularExpressions;

namespace html_serializer
{
    public class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; } = new List<string>();

        public Selector Parent { get; set; }
        public Selector Child { get; set; }

        public static Selector Parse(string query)
        {
            // 1. פירוק לפי רווחים – כל חלק הוא רמה
            var levels = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            // 2. שורש + סלקטור נוכחי
            var root = new Selector();
            var current = root;

            for (int i = 0; i < levels.Length; i++)
            {
                // 3. פירוק לפי # ו-. (תוך שמירה על הסימן)
                var parts = Regex.Split(levels[i], "(?=[#.])")
                                 .Where(p => p.Length > 0);

                // 4. עדכון המאפיינים לפי הסימן
                foreach (var part in parts)
                {
                    if (part.StartsWith("#"))
                        current.Id = part.Substring(1);
                    else if (part.StartsWith("."))
                        current.Classes.Add(part.Substring(1));
                    else if (HtmlHelper.Instance.AllTags.Contains(part))
                        current.TagName = part;
                }

                // 5. סלקטור חדש כבן – רק אם יש עוד רמה
                if (i < levels.Length - 1)
                {
                    var child = new Selector { Parent = current };
                    current.Child = child;
                    current = child;
                }
            }

            return root;
        }
    }
}