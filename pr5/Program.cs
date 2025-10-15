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

    public class Student : Person
    {
        private List<Course> enrolledCourses;

        public Student(int id, string name, int age, string email, string phone)
        : base(id, name, age, email, phone)
        {
            enrolledCourses = new List<Course>();
        }

        public void EnrollInCourse(Course course)
        {
            if (course == null)
                throw new ArgumentNullException(nameof(course));

            if (!enrolledCourses.Contains(course))
            {
                enrolledCourses.Add(course);
                course.EnrollStudent(this);
            }
        }

        public void DisplayEnrolledCourses()
        {
            Console.WriteLine($"\nКурсы студента {Name}:");
            if (enrolledCourses.Count == 0)
            {
                Console.WriteLine("Нет записанных курсов");
                return;
            }

            foreach (var course in enrolledCourses)
            {
                Console.WriteLine($"- {course.CourseName} ({course.CourseCode})");
            }
        }

        public List<Course> GetEnrolledCourses() => new List<Course>(enrolledCourses);

        public override void DisplayInfo()
        {
            Console.WriteLine($"Студент: {Name} (ID: {Id})");
            Console.WriteLine($"Возраст: {Age}, Email: {Email}, Телефон: {Phone}");
            Console.WriteLine($"Записан на курсов: {enrolledCourses.Count}");
        }
    }

    public class Teacher : Person
    {
        private List<Course> taughtCourses;
        public string Specialization { get; private set; }

        public Teacher(int id, string name, int age, string email, string phone, string specialization)
        : base(id, name, age, email, phone)
        {
            if (string.IsNullOrWhiteSpace(specialization))
                throw new ArgumentException("Специализация не может быть пустой");

            Specialization = specialization;
            taughtCourses = new List<Course>();
        }

        public void AssignToCourse(Course course)
        {
            if (course == null)
                throw new ArgumentNullException(nameof(course));

            if (!taughtCourses.Contains(course))
            {
                taughtCourses.Add(course);
                course.AssignInstructor(this);
            }
        }

        public void DisplayTaughtCourses()
        {
            Console.WriteLine($"\nКурсы преподавателя {Name}:");
            if (taughtCourses.Count == 0)
            {
                Console.WriteLine("Нет преподаваемых курсов");
                return;
            }

            foreach (var course in taughtCourses)
            {
                Console.WriteLine($"- {course.CourseName} ({course.CourseCode})");
            }
        }

        public List<Course> GetTaughtCourses() => new List<Course>(taughtCourses);
        public override void DisplayInfo()
        {
            Console.WriteLine($"Преподаватель: {Name} (ID: {Id})");
            Console.WriteLine($"Специализация: {Specialization}");
            Console.WriteLine($"Возраст: {Age}, Email: {Email}, Телефон: {Phone}");
            Console.WriteLine($"Преподает курсов: {taughtCourses.Count}");
        }
    }
