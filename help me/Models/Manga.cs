using Lab4_Shop.Models;

namespace Lab4_Shop.Models
{
    public class Manga : Product
    {
        public string Mangaka { get; set; }
        public int Volume { get; set; } // Номер тому

        public Manga(int id, string title, double price, string mangaka, int volume)
            : base(id, title, price)
        {
            Mangaka = mangaka;
            Volume = volume;
        }

        public override string GetTypeString() => "Манга";

        public override string GetDetails()
        {
            return $"Мангака: {Mangaka}, Том {Volume}";
        }
    }
}