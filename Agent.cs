using System;

namespace FixerGame
{
    public class Agent
    {
        public int Id { get; set; }
        private static int _nextId = 1;
        public string Name { get; set; }
        private string _class;
        private int _Age;
        private int _Body;
        private int _Cool;
        private int _Intelligence;
        private int _Reflexes;
        private int _Technical_Ability;
        public List<Trait> traitList { get; set; }
        public string Class
        {
            get { return _class; }
            set{
                if (IsValidClass(value)) {_class = value;}
                else {throw new ArgumentException("Invalid class");}
            }
        }

        public int Age {
            get { return _Age; }
            set{
                if (value >= 18 && value <= 60) {_Age = value;}
                else {throw new ArgumentException("Invalid age");}
            }
        }

 
        public int Body {
            get { return _Body; }
            set{
                if (IsValidStat(value)) {_Body = value;}
                else {throw new ArgumentException("Invalid Body");}
            }
        }

        public int Cool {
            get { return _Cool; }
            set {
                if (IsValidStat(value)){_Cool = value;}
                else {throw new ArgumentException("Invalid Cool");}
            }
        }


        public int Intelligence {
            get { return _Intelligence; }
            set{
                if (IsValidStat(value)){_Intelligence = value;}
                else{throw new ArgumentException("Invalid Intelligence");}
            }
        }


        public int Reflexes {
            get { return _Reflexes; }
            set{ if (IsValidStat(value)){_Reflexes = value;}
                else{throw new ArgumentException("Invalid Reflexes");}
            }
        }


        public int Technical_Ability {
            get { return _Technical_Ability; }
            set{ if (IsValidStat(value)){_Technical_Ability = value;}
                else{throw new ArgumentException("Invalid Technical Ability");}
            }
        }
    
        private bool IsValidClass(string className)
        {
            if (
                className == "Solo" || 
                className == "Netrunner" || 
                className == "Techie" ||
                className == "Medtech" ||
                className == "Media" ||
                className == "Rockerboy" ||
                className == "Cop"
                )
            {return true;}
            else
            {return false;}
        }

        private bool IsValidStat(int stat)
        {
            if (stat >= 1 && stat <= 20)
            {return true;}
            else
            {return false;}
        }

        public void AddTrait(Trait trait)
        {
            if (traitList.Count < 4)
            {
                traitList.Add(trait);
            }
        }

        public void RemoveTrait(Trait trait)
        {
            traitList.Remove(trait);
        }

        public Agent(string name, string className, int age, int body, int cool, int intelligence, int reflexes, int technical_ability)
        {
            Name = name;
            Class = className;
            Age = age;
            Body = body;
            Cool = cool;
            Intelligence = intelligence;
            Reflexes = reflexes;
            Technical_Ability = technical_ability;    
        }

        private int GenerateRandomStat()
        {
            Random rnd = new Random();
            int stat = rnd.Next(1, 11);
            return stat;
        }

        public Agent(string name, string className)
        {
            Name = name;
            Class = className;
            Age = 18;
            Body = GenerateRandomStat();
            Cool = GenerateRandomStat();
            Intelligence = GenerateRandomStat();
            Reflexes = GenerateRandomStat();
            Technical_Ability = GenerateRandomStat();
        }

        public void PrintStats()
        {
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Age: {Age}");
            Console.WriteLine($"Body: {Body}");
            Console.WriteLine($"Cool: {Cool}");
            Console.WriteLine($"Intelligence: {Intelligence}");
            Console.WriteLine($"Reflexes: {Reflexes}");
            Console.WriteLine($"Technical Ability: {Technical_Ability}");
        }
    }
}

public class Trait{
    public string Name { get; set; }
    public string Description { get; set; }
}