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

    public class Course
    {
        public string CourseCode { get; private set; }
        public string CourseName { get; private set; }
        public string Description { get; private set; }
        public int Credits { get; private set; }

        private Teacher instructor;
        private List<Student> enrolledStudents;

        public Course(string courseCode, string courseName, string description, int credits)
        {
            if (string.IsNullOrWhiteSpace(courseCode))
                throw new ArgumentException("Код курса не может быть пустым");
            if (string.IsNullOrWhiteSpace(courseName))
                throw new ArgumentException("Название курса не может быть пустым");
            if (credits <= 0 || credits > 10)
                throw new ArgumentException("Кредиты должны быть от 1 до 10");

            CourseCode = courseCode;
            CourseName = courseName;
            Description = description ?? "";
            Credits = credits;
            enrolledStudents = new List<Student>();
        }

        public void AssignInstructor(Teacher teacher)
        {
            instructor = teacher;
        }

        public void EnrollStudent(Student student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student));

            if (!enrolledStudents.Contains(student))
            {
                enrolledStudents.Add(student);
            }
        }

        public void DisplayCourseInfo()
        {
            Console.WriteLine($"\nКурс: {CourseName} ({CourseCode})");
            Console.WriteLine($"Описание: {Description}");
            Console.WriteLine($"Кредиты: {Credits}");
            Console.WriteLine($"Преподаватель: {(instructor != null ? instructor.Name : "Не назначен")}");
            Console.WriteLine($"Записанных студентов: {enrolledStudents.Count}");
        }

        public void DisplayEnrolledStudents()
        {
            Console.WriteLine($"\nСтуденты курса {CourseName}:");
            if (enrolledStudents.Count == 0)
            {
                Console.WriteLine("Нет записанных студентов");
                return;
            }

            foreach (var student in enrolledStudents)
            {
                Console.WriteLine($"- {student.Name} (ID: {student.Id})");
            }
        }

        public Teacher GetInstructor() => instructor;
        public List<Student> GetEnrolledStudents() => new List<Student>(enrolledStudents);
        public bool HasInstructor() => instructor != null;
    }

    public class UniversityManager
    {
        private List<Student> students;
        private List<Teacher> teachers;
        private List<Course> courses;
        private int nextStudentId;
        private int nextTeacherId;

        public UniversityManager()
        {
            students = new List<Student>();
            teachers = new List<Teacher>();
            courses = new List<Course>();
            nextStudentId = 1;
            nextTeacherId = 1;
        }

        public void AddStudent(string name, int age, string email, string phone)
        {
            var student = new Student(nextStudentId++, name, age, email, phone);
            students.Add(student);
            Console.WriteLine($"Студент {name} добавлен с ID: {student.Id}");
        }

        public void AddTeacher(string name, int age, string email, string phone, string specialization)
        {
            var teacher = new Teacher(nextTeacherId++, name, age, email, phone, specialization);
            teachers.Add(teacher);
            Console.WriteLine($"Преподаватель {name} добавлен с ID: {teacher.Id}");
        }
        public void CreateCourse(string courseCode, string courseName, string description, int credits)
        {
            var course = new Course(courseCode, courseName, description, credits);
            courses.Add(course);
            Console.WriteLine($"Курс {courseName} создан с кодом: {courseCode}");
        }

        public void DisplayAllStudents()
        {
            Console.WriteLine("\n=== Все студенты ===");
            if (students.Count == 0)
            {
                Console.WriteLine("Нет студентов в системе");
                return;
            }

            foreach (var student in students)
            {
                student.DisplayInfo();
                Console.WriteLine("---");
            }
        }

        public void DisplayAllTeachers()
        {
            Console.WriteLine("\n=== Все преподаватели ===");
            if (teachers.Count == 0)
            {
                Console.WriteLine("Нет преподавателей в системе");
                return;
            }

            foreach (var teacher in teachers)
            {
                teacher.DisplayInfo();
                Console.WriteLine("---");
            }
        }

        public void DisplayAllCourses()
        {
            Console.WriteLine("\n=== Все курсы ===");
            if (courses.Count == 0)
            {
                Console.WriteLine("Нет курсов в системе");
                return;
            }

            foreach (var course in courses)
            {
                course.DisplayCourseInfo();
                Console.WriteLine("---");
            }
        }

        public void EnrollStudentInCourse(int studentId, string courseCode)
        {
            var student = students.FirstOrDefault(s => s.Id == studentId);
            var course = courses.FirstOrDefault(c => c.CourseCode == courseCode);

            if (student == null)
                throw new ArgumentException($"Студент с ID {studentId} не найден");
            if (course == null)
                throw new ArgumentException($"Курс с кодом {courseCode} не найден");

            student.EnrollInCourse(course);
            Console.WriteLine($"Студент {student.Name} записан на курс {course.CourseName}");
        }

        public void AssignTeacherToCourse(int teacherId, string courseCode)
        {
            var teacher = teachers.FirstOrDefault(t => t.Id == teacherId);
            var course = courses.FirstOrDefault(c => c.CourseCode == courseCode);

            if (teacher == null)
                throw new ArgumentException($"Преподаватель с ID {teacherId} не найден");
            if (course == null)
                throw new ArgumentException($"Курс с кодом {courseCode} не найден");

            teacher.AssignToCourse(course);
            Console.WriteLine($"Преподаватель {teacher.Name} назначен на курс {course.CourseName}");
        }

        public void DisplayStudentCourses(int studentId)
        {
            var student = students.FirstOrDefault(s => s.Id == studentId);
            if (student == null)
                throw new ArgumentException($"Студент с ID {studentId} не найден");

            student.DisplayEnrolledCourses();
        }

        public void DisplayCourseStudents(string courseCode)
        {
            var course = courses.FirstOrDefault(c => c.CourseCode == courseCode);
            if (course == null)
                throw new ArgumentException($"Курс с кодом {courseCode} не найден");

            course.DisplayEnrolledStudents();
        }

        public bool StudentExists(int studentId) => students.Any(s => s.Id == studentId);
        public bool TeacherExists(int teacherId) => teachers.Any(t => t.Id == teacherId);
        public bool CourseExists(string courseCode) => courses.Any(c => c.CourseCode == courseCode);
        public List<Student> GetAllStudents() => new List<Student>(students);
        public List<Teacher> GetAllTeachers() => new List<Teacher>(teachers);
        public List<Course> GetAllCourses() => new List<Course>(courses);
    }
