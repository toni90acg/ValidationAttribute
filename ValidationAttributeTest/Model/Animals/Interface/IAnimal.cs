﻿namespace ValidationAttributeTest.Model.Animals
{
    public interface IAnimal
    {
        string Name { get; set; }
        int Age { get; set; }

        bool CanFly { get; set; }
        string Type { get; set; }
    }
}