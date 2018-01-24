namespace ConsoleAPI
{
    public class ClassOfBeweryBeer
    {
        public ClassOfBeweryBeer(string href)
        {
            this.href = href;
        }

        public string href { get; set; }

        public static explicit operator ClassOfBeweryBeer(Newtonsoft.Json.Linq.JToken t)
        {
            return new ClassOfBeweryBeer((string)t["href"]);
        }
    }
}