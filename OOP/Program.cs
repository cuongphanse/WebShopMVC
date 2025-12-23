// // See https://aka.ms/new-console-template for more information
// // Console.WriteLine("Hello, World!");
// Employee emp = new Employee{ Name="John", Age=30, Price=100, Quantity=5 };
// Console.WriteLine($"Employee: {emp.Name}, Age: {emp.Age}, Price: {emp.Price}, Quantity: {emp.Quantity}, Amount: {emp.Amount}");

Student student = new Student { Name = "Alice", Age = 20 };
Console.WriteLine($"Student: {student.Name}, Age: {student.Age}");
student.Name = "Cuong";
// student.Age = 21; // This line will cause a compile-time error because Age has an init-only setter
Console.WriteLine($"Updated Student: {student.Name}, Age: {student.Age}");
