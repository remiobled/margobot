namespace MargoBot
{
    internal class LUISAnswer
    {
        public string query{ get; set; }
        public Intent topScoringIntent { get; set; }
        public Entity[] entities { get; set; }
    }
    internal class Intent
    {
        public string intent { get; set; }
        public double score { get; set; }
    }
    internal class Entity

    {
        public string entity { get; set; }
        public string type { get; set; }
        public int startIndex { get; set; }
        public int endIndex { get; set; }
        public double score { get; set; }
    }
}