namespace FixerGame
{
    public class EndEvent: StoryEvent
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string TeamDeath { get; set; }
        public string Success { get; set; }
        public string NodeType { get; set; } = "End";

        public void End()
        {
            Console.WriteLine("Thanks for playing!");
        }
    }
}
