namespace lab06
{
    public struct DeserializatedWeather
    {
        public string Name { get; set; }
        public Sys Sys { get; set; }
        public Main Main { get; set; }
        public Weather[] Weather { get; set; }
    }
}
