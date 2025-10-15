using System;
using System.Collections.Generic;
using System.Linq;

namespace UniversityManagementSystem
{
    public abstract class Person
    {
        public int Id { get; protected set; }
        public string Name { get; protected set; }
        public int Age { get; protected set; }
        public string Email { get; protected set; }
        public string Phone { get; protected set; }

        protected Person(int id, string name, int age, string email, string phone)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Имя не может быть пустым");
            if (age <= 0 || age > 120)
                throw new ArgumentException("Возраст должен быть от 1 до 120 лет");
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
                throw new ArgumentException("Некорректный email");
            if (string.IsNullOrWhiteSpace(phone))
                throw new ArgumentException("Телефон не может быть пустым");

            Id = id;
            Name = name;
            Age = age;
            Email = email;
            Phone = phone;
        }

        public abstract void DisplayInfo();
    }
}