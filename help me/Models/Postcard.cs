namespace Lab4_Shop.Models
{
    public class Postcard : Product
    {
        public bool HasEnvelope { get; set; } // Чи йде конверт у комплекті

        public Postcard(int id, string title, double price, bool hasEnvelope)
            : base(id, title, price)
        {
            HasEnvelope = hasEnvelope;
        }

        public override string GetTypeString() => "Листівка";

        public override string GetDetails()
        {
            // Тернарний оператор: якщо true - "Так", якщо false - "Ні"
            string envelopeStatus = HasEnvelope ? "Є конверт" : "Без конверта";
            return $"Комплектація: {envelopeStatus}";
        }
    }
}