using Lab4_Shop.Models;

namespace Lab4_Shop.Models
{
    public class Book : Product
    {
        public string Author { get; set; }
        public int Pages { get; set; }

        public Book(int id, string title, double price, string author, int pages)
            : base(id, title, price)
        {
            Author = author;
            Pages = pages;
        }

        public override string GetTypeString() => "Книга";

        public override string GetDetails()
        {
            return $"Автор: {Author}, {Pages} стор.";
        }
    }
}