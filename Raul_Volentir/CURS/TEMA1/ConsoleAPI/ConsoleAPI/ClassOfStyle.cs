namespace ConsoleAPI
{
    public class ClassOfStyle
    {
        public ClassOfStyle(string href)
        {
            this.href = href;
        }

        public string href { get; set; }

        public static explicit operator ClassOfStyle(Newtonsoft.Json.Linq.JToken t)
        {
            return new ClassOfStyle((string)t["href"]);
        }
    }
}