namespace ConsoleAPI
{
    public class ClassOfBeerSelf
    {
        public ClassOfBeerSelf(string href)
        {
            this.href = href;
        }

        public string href { get; set; }

        public static explicit operator ClassOfBeerSelf(Newtonsoft.Json.Linq.JToken t)
        {
            return new ClassOfBeerSelf((string)t["href"]);
        }
    }
}