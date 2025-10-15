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

    public class MenuManager
    {
        private UniversityManager universityManager;

        public MenuManager()
        {
            universityManager = new UniversityManager();
        }

        public void DisplayMainMenu()
        {
            Console.WriteLine("\n=== Главное меню ===");
            Console.WriteLine("1. Управление студентами");
            Console.WriteLine("2. Управление преподавателями");
            Console.WriteLine("3. Управление курсами");
            Console.WriteLine("4. Показать все данные");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите опцию: ");
        }

        public void DisplayStudentMenu()
        {
            Console.WriteLine("\n=== Меню студентов ===");
            Console.WriteLine("1. Добавить студента");
            Console.WriteLine("2. Показать всех студентов");
            Console.WriteLine("3. Записать студента на курс");
            Console.WriteLine("4. Показать курсы студента");
            Console.WriteLine("0. Назад");
            Console.Write("Выберите опцию: ");
        }

        public void DisplayTeacherMenu()
        {
            Console.WriteLine("\n=== Меню преподавателей ===");
            Console.WriteLine("1. Добавить преподавателя");
            Console.WriteLine("2. Показать всех преподавателей");
            Console.WriteLine("3. Назначить преподавателя на курс");
            Console.WriteLine("4. Показать курсы преподавателя");
            Console.WriteLine("0. Назад");
            Console.Write("Выберите опцию: ");
        }

        public void DisplayCourseMenu()
        {
            Console.WriteLine("\n=== Меню курсов ===");
            Console.WriteLine("1. Создать курс");
            Console.WriteLine("2. Показать все курсы");
            Console.WriteLine("3. Показать студентов курса");
            Console.WriteLine("0. Назад");
            Console.Write("Выберите опцию: ");
        }

        public void ProcessUserInput()
        {
            bool running = true;

            while (running)
            {
                try
                {
                    DisplayMainMenu();
                    string input = Console.ReadLine();

                    switch (input)
                    {
                        case "1":
                            ProcessStudentMenu();
                            break;
                        case "2":
                            ProcessTeacherMenu();
                            break;
                        case "3":
                            ProcessCourseMenu();
                            break;
                        case "4":
                            DisplayAllData();
                            break;
                        case "0":
                            running = false;
                            Console.WriteLine("Выход из программы...");
                            break;
                        default:
                            Console.WriteLine("Неверный выбор. Попробуйте снова.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }
        }

        private void ProcessStudentMenu()
        {
            bool inStudentMenu = true;

            while (inStudentMenu)
            {
                DisplayStudentMenu();
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        AddNewStudent();
                        break;
                    case "2":
                        universityManager.DisplayAllStudents();
                        break;
                    case "3":
                        HandleEnrollment();
                        break;
                    case "4":
                        DisplayStudentCourses();
                        break;
                    case "0":
                        inStudentMenu = false;
                        break;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }
            }
        }

        private void ProcessTeacherMenu()
        {
            bool inTeacherMenu = true;

            while (inTeacherMenu)
            {
                DisplayTeacherMenu();
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        AddNewTeacher();
                        break;
                    case "2":
                        universityManager.DisplayAllTeachers();
                        break;
                    case "3":
                        HandleTeacherAssignment();
                        break;
                    case "4":
                        DisplayTeacherCourses();
                        break;
                    case "0":
                        inTeacherMenu = false;
                        break;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }
            }
        }

        private void ProcessCourseMenu()
        {
            bool inCourseMenu = true;

            while (inCourseMenu)
            {
                DisplayCourseMenu();
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        CreateNewCourse();
                        break;
                    case "2":
                        universityManager.DisplayAllCourses();
                        break;
                    case "3":
                        DisplayCourseStudents();
                        break;
                    case "0":
                        inCourseMenu = false;
                        break;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }
            }
        }

        private void AddNewStudent()
        {
            try
            {
                Console.Write("Введите имя студента: ");
                string name = Console.ReadLine();

                Console.Write("Введите возраст: ");
                int age = int.Parse(Console.ReadLine());

                Console.Write("Введите email: ");
                string email = Console.ReadLine();

                Console.Write("Введите телефон: ");
                string phone = Console.ReadLine();

                universityManager.AddStudent(name, age, email, phone);
            }
            catch (FormatException)
            {
                Console.WriteLine("Ошибка: Неверный формат возраста");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при добавлении студента: {ex.Message}");
            }
        }

        private void AddNewTeacher()
        {
            try
            {
                Console.Write("Введите имя преподавателя: ");
                string name = Console.ReadLine();

                Console.Write("Введите возраст: ");
                int age = int.Parse(Console.ReadLine());

                Console.Write("Введите email: ");
                string email = Console.ReadLine();

                Console.Write("Введите телефон: ");
                string phone = Console.ReadLine();

                Console.Write("Введите специализацию: ");
                string specialization = Console.ReadLine();

                universityManager.AddTeacher(name, age, email, phone, specialization);
            }
            catch (FormatException)
            {
                Console.WriteLine("Ошибка: Неверный формат возраста");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при добавлении преподавателя: {ex.Message}");
            }
        }

        private void CreateNewCourse()
        {
            try
            {
                Console.
                Write("Введите код курса: ");
                string code = Console.ReadLine();

                Console.Write("Введите название курса: ");
                string name = Console.ReadLine();

                Console.Write("Введите описание курса: ");
                string description = Console.ReadLine();

                Console.Write("Введите количество кредитов: ");
                int credits = int.Parse(Console.ReadLine());

                universityManager.CreateCourse(code, name, description, credits);
            }
            catch (FormatException)
            {
                Console.WriteLine("Ошибка: Неверный формат кредитов");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при создании курса: {ex.Message}");
            }
        }

        private void HandleEnrollment()
        {
            try
            {
                universityManager.DisplayAllStudents();
                if (!universityManager.GetAllStudents().Any()) return;

                universityManager.DisplayAllCourses();
                if (!universityManager.GetAllCourses().Any()) return;

                Console.Write("Введите ID студента: ");
                int studentId = int.Parse(Console.ReadLine());

                Console.Write("Введите код курса: ");
                string courseCode = Console.ReadLine();

                universityManager.EnrollStudentInCourse(studentId, courseCode);
            }
            catch (FormatException)
            {
                Console.WriteLine("Ошибка: Неверный формат ID студента");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при записи на курс: {ex.Message}");
            }
        }

        private void HandleTeacherAssignment()
        {
            try
            {
                universityManager.DisplayAllTeachers();
                if (!universityManager.GetAllTeachers().Any()) return;

                universityManager.DisplayAllCourses();
                if (!universityManager.GetAllCourses().Any()) return;

                Console.Write("Введите ID преподавателя: ");
                int teacherId = int.Parse(Console.ReadLine());

                Console.Write("Введите код курса: ");
                string courseCode = Console.ReadLine();

                universityManager.AssignTeacherToCourse(teacherId, courseCode);
            }
            catch (FormatException)
            {
                Console.WriteLine("Ошибка: Неверный формат ID преподавателя");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при назначении преподавателя: {ex.Message}");
            }
        }

        private void DisplayStudentCourses()
        {
            try
            {
                universityManager.DisplayAllStudents();
                if (!universityManager.GetAllStudents().Any()) return;

                Console.Write("Введите ID студента: ");
                int studentId = int.Parse(Console.ReadLine());

                universityManager.DisplayStudentCourses(studentId);
            }
            catch (FormatException)
            {
                Console.WriteLine("Ошибка: Неверный формат ID студента");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        private void DisplayTeacherCourses()
        {
            try
            {
                universityManager.DisplayAllTeachers();
                if (!universityManager.GetAllTeachers().Any()) return;

                Console.Write("Введите ID преподавателя: ");
                int teacherId = int.Parse(Console.ReadLine());

                var teacher = universityManager.GetAllTeachers().FirstOrDefault(t => t.Id == teacherId);
                if (teacher != null)
                {
                    teacher.DisplayTaughtCourses();
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Ошибка: Неверный формат ID преподавателя");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        private void DisplayCourseStudents()
        {
            try
            {
                universityManager.DisplayAllCourses();
                if (!universityManager.GetAllCourses().Any()) return;

                Console.Write("Введите код курса: ");
                string courseCode = Console.ReadLine();

                universityManager.DisplayCourseStudents(courseCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        private void DisplayAllData()
        {
            universityManager.DisplayAllStudents();
            universityManager.DisplayAllTeachers();
            universityManager.DisplayAllCourses();
        }
    }
