using Lab4_Shop.Models;
using Lab4_Shop.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lab4_Shop
{
    class Program
    {
       
        static List<Product> inventory = new List<Product>();
       
        static List<Product> cart = new List<Product>();

        static List<User> users = new List<User>();
        static User currentUser = null;

        static void Main(string[] args)
        {
           
            Console.OutputEncoding = Encoding.UTF8;
            InitializeData();

           
            while (currentUser == null)
            {
                DrawHeader("АВТОРИЗАЦІЯ");
                Console.WriteLine("\nВхід (admin/admin) або (user/1234)\n");

                WriteColor("  Логін:  ", ConsoleColor.Cyan);
                string login = Console.ReadLine();
                WriteColor("  Пароль: ", ConsoleColor.Cyan);
                string pass = Console.ReadLine();

                currentUser = Authenticate(login, pass);
                if (currentUser == null) { PrintError("Невірні дані!"); Console.ReadKey(); }
            }

         
            bool running = true;
            while (running)
            {
                Console.Clear();
                DrawHeader($"МАГАЗИН | 👤 {currentUser.Username}");

             
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"  Роль: {(currentUser.IsAdmin ? "АДМІН" : "КЛІЄНТ")}");
                Console.Write($"  Товарів на складі: {inventory.Count}");

               
                if (!currentUser.IsAdmin)
                {
                    Console.Write(" | ");
                    Console.ForegroundColor = cart.Count > 0 ? ConsoleColor.Green : ConsoleColor.DarkGray;
                    Console.WriteLine($"У кошику: {cart.Count}");
                }
                else
                {
                    Console.WriteLine();
                }
                Console.ResetColor();
                Console.WriteLine();

                
                PrintMenuOption("1", "📋 Таблиця товарів");
                PrintMenuOption("2", "🔍 Пошук");
                PrintMenuOption("3", "💰 Сортування (за ціною)");
                PrintMenuOption("4", "📊 Статистика складу");

                Console.WriteLine();

               
                if (!currentUser.IsAdmin)
                {
                    WriteColor("  --- ПОКУПКИ ---", ConsoleColor.Cyan);
                    Console.WriteLine();
                    PrintMenuOption("5", "🛒 Додати в кошик (за ID)");
                    PrintMenuOption("6", "💳 Оформити замовлення (Чек)");
                }

               
                if (currentUser.IsAdmin)
                {
                    WriteColor("  --- АДМІН ПАНЕЛЬ ---", ConsoleColor.DarkYellow);
                    Console.WriteLine();
                    PrintMenuOption("5", "➕ Додати Книгу");
                    PrintMenuOption("6", "➕ Додати Мангу");
                    PrintMenuOption("7", "➕ Додати Закладку");
                    PrintMenuOption("8", "➕ Додати Листівку");
                    PrintMenuOption("9", "❌ Видалити товар");
                }

                Console.WriteLine();
                PrintMenuOption("0", "🚪 Вихід");
                Console.Write("\nВаш вибір > ");

                string choice = Console.ReadLine();

               
                if (currentUser.IsAdmin)
                {
                    
                    switch (choice)
                    {
                        case "1": ShowTable(); break;
                        case "2": SearchItem(); break;
                        case "3": SortItems(); break;
                        case "4": ShowStatistics(); break;
                        case "5": AddBook(); break;
                        case "6": AddManga(); break;
                        case "7": AddBookmark(); break;
                        case "8": AddPostcard(); break;
                        case "9": DeleteItem(); break;
                        case "0": running = false; break;
                        default: PrintError("Невірний вибір!"); Console.ReadKey(); break;
                    }
                }
                else
                {
                    
                    switch (choice)
                    {
                        case "1": ShowTable(); break;
                        case "2": SearchItem(); break;
                        case "3": SortItems(); break;
                        case "4": ShowStatistics(); break;
                        case "5": AddToCart(); break; 
                        case "6": Checkout(); break;    
                        case "0": running = false; break;
                        default: PrintError("Невірний вибір!"); Console.ReadKey(); break;
                    }
                }
            }
        }

     

        static void AddToCart()
        {
            DrawHeader("ДОДАТИ В КОШИК");
            ShowTable();
            Console.Write("\nВведіть ID товару: ");

            if (int.TryParse(Console.ReadLine(), out int id))
            {
                Product foundItem = null;
                
                foreach (var item in inventory)
                {
                    if (item.Id == id)
                    {
                        foundItem = item;
                        break;
                    }
                }

                if (foundItem != null)
                {
                    cart.Add(foundItem);
                    PrintSuccess($"\"{foundItem.Title}\" додано в кошик!");
                }
                else
                {
                    PrintError("Товар з таким ID не знайдено.");
                }
            }
            else
            {
                PrintError("Некоректний ID.");
            }
            Console.ReadKey();
        }

        static void Checkout()
        {
            DrawHeader("ОФОРМЛЕННЯ ЗАМОВЛЕННЯ");
            if (cart.Count == 0)
            {
                Console.WriteLine("\n Ваш кошик порожній 😢");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nВАШ ЧЕК:");
            Console.WriteLine(new string('-', 40));

            double total = 0;
            foreach (var item in cart)
            {
                Console.WriteLine($" {item.Title,-25} ... {item.Price} ₴");
                total += item.Price;
            }

            Console.WriteLine(new string('-', 40));
            WriteColor($" ДО СПЛАТИ: {total} ₴", ConsoleColor.Green);
            Console.WriteLine("\n");

            Console.WriteLine("Дякуємо за покупку! 🎉");

            cart.Clear();
            Console.ReadKey();
        }

       

        static void InitializeData()
        {
            users.Add(new User("admin", "admin", true));
            users.Add(new User("user", "1234", false));

          
            Product[] initialItems = new Product[]
            {
                new Book(1, "Кобзар", 350, "Т. Шевченко", 700),
                new Manga(2, "Наруто", 200, "М. Кішімото", 1),
                new Bookmark(3, "Котики", 50, "Картон", "Лапки"),
                new Postcard(4, "З Днем Народження", 25, true),
                new Bookmark(5, "Герб", 120, "Метал", "Тризуб"),
                new Book(6, "C# in Depth", 1200, "Jon Skeet", 500)
            };

         
            foreach (var item in initialItems)
            {
                inventory.Add(item);
            }
        }

        static void ShowStatistics()
        {
            DrawHeader("СТАТИСТИКА СКЛАДУ");
            if (inventory.Count == 0) { Console.WriteLine(" Пусто."); Console.ReadKey(); return; }

            double sum = 0;
            double max = double.MinValue;
            double min = double.MaxValue;

            foreach (var item in inventory)
            {
                sum += item.Price;
                if (item.Price > max) max = item.Price;
                if (item.Price < min) min = item.Price;
            }
        
            double average = sum / inventory.Count;

            Console.WriteLine($"  Всього товарів:    {inventory.Count}");
            Console.Write($"  Загальна вартість: "); WriteColor($"{sum:F2} ₴\n", ConsoleColor.Green);
            Console.Write($"  Середня ціна:      "); WriteColor($"{average:F2} ₴\n", ConsoleColor.Yellow);
            Console.Write($"  Найдорожчий:       "); WriteColor($"{max:F2} ₴\n", ConsoleColor.Red);
            Console.Write($"  Найдешевший:       "); WriteColor($"{min:F2} ₴\n", ConsoleColor.Cyan);
            Console.ReadKey();
        }

        static void ShowTable()
        {
            DrawHeader("АСОРТИМЕНТ");
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" {0,-4} | {1,-12} | {2,-25} | {3,-10} | {4,-30} ", "ID", "Тип", "Назва", "Ціна", "Інфо");
            Console.ResetColor();

            if (inventory.Count == 0) Console.WriteLine("\n  Список порожній.");
            else
            {
                foreach (var item in inventory)
                {
                    Console.Write(" ");
                    WriteColor($"{item.Id,-4}", ConsoleColor.DarkGray);
                    Console.Write(" | ");
                    Console.Write($"{item.GetTypeString(),-12}");
                    Console.Write(" | ");
                    string shortTitle = item.Title.Length > 24 ? item.Title.Substring(0, 21) + "..." : item.Title;
                    WriteColor($"{shortTitle,-25}", ConsoleColor.White);
                    Console.Write(" | ");
                    WriteColor($"{item.Price,8:F2} ₴", ConsoleColor.Green);
                    Console.Write(" | ");
                    WriteColor($"{item.GetDetails(),-30}", ConsoleColor.Gray);
                    Console.WriteLine();
                }
            }
            Console.WriteLine("\nНатисніть Enter...");
            Console.ReadLine();
        }

        static void SearchItem()
        {
            DrawHeader("ПОШУК");
            Console.Write("Введіть назву: ");
            string q = Console.ReadLine().ToLower();
            bool found = false;
            Console.WriteLine();
            foreach (var item in inventory)
            {
                if (item.Title.ToLower().Contains(q))
                {
                    Console.Write(" -> ");
                    WriteColor($"[{item.GetTypeString()}] ", ConsoleColor.Cyan);
                    Console.WriteLine($"{item.Title} - {item.Price} ₴");
                    found = true;
                }
            }
            if (!found) PrintError("Нічого не знайдено.");
            Console.ReadKey();
        }

        static void SortItems()
        {
            inventory.Sort((p1, p2) => p1.Price.CompareTo(p2.Price));
            PrintSuccess("Список відсортовано!");
            Console.ReadKey();
        }

        static User Authenticate(string login, string pass)
        {
            foreach (var u in users) if (u.Username == login && u.Password == pass) return u;
            return null;
        }

      
        static int GetNewId() { if (inventory.Count == 0) return 1; int max = 0; foreach (var i in inventory) if (i.Id > max) max = i.Id; return max + 1; }

        static void AddBook() { try { DrawHeader("КНИГА"); Console.Write("Назва: "); string t = Console.ReadLine(); Console.Write("Ціна: "); double p = double.Parse(Console.ReadLine()); Console.Write("Автор: "); string a = Console.ReadLine(); Console.Write("Сторінок: "); int pg = int.Parse(Console.ReadLine()); inventory.Add(new Book(GetNewId(), t, p, a, pg)); PrintSuccess("Додано!"); } catch { PrintError("Дані некоректні."); } Console.ReadKey(); }
        static void AddManga() { try { DrawHeader("МАНГА"); Console.Write("Назва: "); string t = Console.ReadLine(); Console.Write("Ціна: "); double p = double.Parse(Console.ReadLine()); Console.Write("Мангака: "); string m = Console.ReadLine(); Console.Write("Том: "); int v = int.Parse(Console.ReadLine()); inventory.Add(new Manga(GetNewId(), t, p, m, v)); PrintSuccess("Додано!"); } catch { PrintError("Дані некоректні."); } Console.ReadKey(); }
        static void AddBookmark() { try { DrawHeader("ЗАКЛАДКА"); Console.Write("Назва: "); string t = Console.ReadLine(); Console.Write("Ціна: "); double p = double.Parse(Console.ReadLine()); Console.Write("Матеріал: "); string m = Console.ReadLine(); Console.Write("Дизайн: "); string d = Console.ReadLine(); inventory.Add(new Bookmark(GetNewId(), t, p, m, d)); PrintSuccess("Додано!"); } catch { PrintError("Дані некоректні."); } Console.ReadKey(); }
        static void AddPostcard() { try { DrawHeader("ЛИСТІВКА"); Console.Write("Назва: "); string t = Console.ReadLine(); Console.Write("Ціна: "); double p = double.Parse(Console.ReadLine()); Console.Write("Конверт (+/-): "); string a = Console.ReadLine(); inventory.Add(new Postcard(GetNewId(), t, p, a.Contains("+"))); PrintSuccess("Додано!"); } catch { PrintError("Дані некоректні."); } Console.ReadKey(); }

        static void DeleteItem()
        {
            DrawHeader("ВИДАЛЕННЯ"); ShowTable();
            Console.Write("\nID: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                int idx = -1;
                for (int i = 0; i < inventory.Count; i++) if (inventory[i].Id == id) idx = i;
                if (idx != -1) { inventory.RemoveAt(idx); PrintSuccess("Видалено."); }
                else PrintError("ID не знайдено.");
            }
            Console.ReadKey();
        }

      
        static void DrawHeader(string t) { Console.Clear(); Console.ForegroundColor = ConsoleColor.Blue; Console.WriteLine("╔" + new string('═', 60) + "╗"); int s = (60 - t.Length) / 2; Console.Write("║" + new string(' ', s)); Console.ForegroundColor = ConsoleColor.White; Console.Write(t); Console.ForegroundColor = ConsoleColor.Blue; Console.WriteLine(new string(' ', 60 - s - t.Length) + "║"); Console.WriteLine("╚" + new string('═', 60) + "╝"); Console.ResetColor(); }
        static void WriteColor(string t, ConsoleColor c) { Console.ForegroundColor = c; Console.Write(t); Console.ResetColor(); }
        static void PrintError(string m) { WriteColor($"\n [X] {m}", ConsoleColor.Red); }
        static void PrintSuccess(string m) { WriteColor($"\n [OK] {m}", ConsoleColor.Green); }
        static void PrintMenuOption(string k, string t) { Console.ForegroundColor = ConsoleColor.Yellow; Console.Write($"  [{k}] "); Console.ResetColor(); Console.WriteLine(t); }
    }
}
