using Lab4_Shop.Models;

namespace Lab4_Shop.Models
{
    public class Bookmark : Product
    {
        public string Material { get; set; } // Папір, Пластик, Метал
        public string Design { get; set; }   // Наприклад: "З котиками"

        public Bookmark(int id, string title, double price, string material, string design)
            : base(id, title, price)
        {
            Material = material;
            Design = design;
        }

        public override string GetTypeString() => "Закладка";

        public override string GetDetails()
        {
            return $"Матеріал: {Material}, Дизайн: {Design}";
        }
    }
}