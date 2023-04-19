//namespace LINQDemo
//{
//    internal class Person
//    {
//        public string Name { get; set; }
//        public int Age { get; set; }


//        static void Main(string[] args)
//        {
//            List<int> ints = new List<int>() { 1, 4, 3, 2, 6, 7, 9, 0, 11, 5, 19 };

//            ints.TakeWhile(i => i < 10).ToList().ForEach(j => { Console.WriteLine(j); });
//            ints.Take(5).ToList().ForEach(i => { Console.WriteLine(i); });
//            List<Person> people = new List<Person>
//            {
//            new Person { Name = "Alice", Age = 25 },
//            new Person { Name = "Bob", Age = 30 },
//            new Person { Name = "Charlie", Age = 35 },
//            new Person { Name = "David", Age = 40 },
//            new Person { Name = "Anna", Age = 42 },
//            new Person { Name = "Keith", Age = 50 },
//            new Person { Name = "Keith2", Age = 55 },
//            };

//            people.Take(people.Count).Where(q => q.Age > 40).Take(2).ToList().ForEach(i => { Console.WriteLine(i.Name); });
//            people.Take(people.Count).Where(q => q.Age > 40).Take(2).ToList().ForEach(i => { Console.WriteLine(i.Name); });
//            var names = from p in people where p.Age > 32 select p;
//            foreach (var p in names)
//            {
//                Console.WriteLine(p.Name);
//            }
//        }
//    }
//}

