using System;
using System.Collections.Generic;
using Newtonsoft.Json;

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
        public List<string> TraitList { get; set; }
        
        public Agent()
        {
            Id = _nextId;
            _nextId++;
        }

        public string Class
        {
            get { return _class; }
            set{
                if (IsValidClass(value)) {_class = value;}
                else {throw new ArgumentException("Invalid class: " + value);}
            }
        }

        public int Age {
            get { return _Age; }
            set{
                if (value >= 18 && value <= 60) {_Age = value;}
                else {throw new ArgumentException("Invalid age: " + value);}
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

        public void AddTrait(string trait)
        {
            if (TraitList.Count < 4)
            {
                TraitList.Add(trait);
            }
        }

        public void RemoveTrait(string trait)
        {
            TraitList.Remove(trait);
        }

        // class constructor with a list of traits and specific stats
        public Agent(string name, string className, int age, int body, int cool, int intelligence, 
            int reflexes, int technical_ability, List<string> traits)
        {
            Name = name;
            Class = className;
            Age = age;
            Body = body;
            Cool = cool;
            Intelligence = intelligence;
            Reflexes = reflexes;
            Technical_Ability = technical_ability;
            TraitList = traits;
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

        // class constructor with a list of traits
        public Agent(string name, string className, List<string> traits)
        {
            Name = name;
            Class = className;
            Age = 18;
            Body = GenerateRandomStat();
            Cool = GenerateRandomStat();
            Intelligence = GenerateRandomStat();
            Reflexes = GenerateRandomStat();
            Technical_Ability = GenerateRandomStat();
            TraitList = traits;
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

        [JsonProperty("Traits")]
        public List<string> TraitNames { get; set; }

        public static List<Agent> LoadAgentList(string json)
        {
            List<Agent> agentList = JsonConvert.DeserializeObject<List<Agent>>(json);
            return agentList;
        }
    }
}

