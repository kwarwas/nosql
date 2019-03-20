namespace PizzaHouseApp.Model
{
    public class PizzaPrice
    {
        public decimal Small { get; set; }
        public decimal Big { get; set; }
        public decimal Family { get; set; }

        public override string ToString()
        {
            return $"Small: {Small:C}, Big: {Big:C}, Family: {Family:C}";
        }
    }
}