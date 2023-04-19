using System;
using System.Collections.Generic;

namespace LINQDemo
{
  
    interface IAnimalVisitor
    {
        void Visit(Dog dog);
        void Visit(Cat cat);
        void Visit(Bird bird);
    }

    interface IAnimal
    {
        void Accept(IAnimalVisitor visitor);
    }

    class Dog : IAnimal
    {
        public void Bark()
        {
            Console.WriteLine("Woof!");
        }

        public void Accept(IAnimalVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    class Cat : IAnimal
    {
        public void Meow()
        {
            Console.WriteLine("Meow!");
        }

        public void Accept(IAnimalVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    class Bird : IAnimal
    {
        public void Chirp()
        {
            Console.WriteLine("Chirp chirp!");
        }

        public void Accept(IAnimalVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    class AnimalFeeder : IAnimalVisitor
    {
        public void Visit(Dog dog)
        {
            Console.WriteLine("Feeding dog.");
            dog.Bark();
        }

        public void Visit(Cat cat)
        {
            Console.WriteLine("Feeding cat.");
            cat.Meow();
        }

        public void Visit(Bird bird)
        {
            Console.WriteLine("Feeding bird.");
            bird.Chirp();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<IAnimal> animals = new List<IAnimal> { new Dog(), new Cat(), new Bird() };

            AnimalFeeder feeder = new AnimalFeeder();
            foreach (var animal in animals)
            {
                animal.Accept(feeder);
            }
        }
    }


}
