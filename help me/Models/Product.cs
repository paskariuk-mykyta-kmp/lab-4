namespace Lab4_Shop.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }

        public Product(int id, string title, double price)
        {
            Id = id;
            Title = title;
            Price = price;
        }

        // virtual дозволяє дітям змінювати цей метод
        public virtual string GetTypeString()
        {
            return "Товар";
        }

        public virtual string GetDetails()
        {
            return "Стандартний опис";
        }
    }
}