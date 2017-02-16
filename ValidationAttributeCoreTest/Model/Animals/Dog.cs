using ValidationAttributeCoreTest.Model.Animals.Interface;
using ValidationAttributeCoreTest.Validations;

namespace ValidationAttributeCoreTest.Model.Animals
{
    [MyValidation(typeof(DogValidation))]
    public class Dog : IAnimal
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public bool CanFly { get; set; }
        public string Type { get; set; }

        public Dog(string name, int age, bool canFly, string type)
        {
            Name = name;
            Age = age;
            CanFly = canFly;
            Type = type;
        }
    }
}