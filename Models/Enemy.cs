namespace FixerGame 
{
    public class CombatProcess
    {
        public List<Enemy> Enemies { get; set; }
    }

    public class Enemy
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Health { get; set; }
        public int Level { get; set; }
        public int Body { get; set; }
        public int Cool { get; set; }
        public int Intelligence { get; set; }
        public int Reflex { get; set; }
        public int Tech { get; set; }
        public List<Weapon> weapons { get; set; }
        public int Armor { get; set; }
        public int CritChance { get; set; }
        public int Accuracy { get; set; }
        public int Evasion { get; set; }
    }
}

