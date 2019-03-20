using System.Text;

namespace PizzaHouseApp.Model
{
    public class Pasta : PizzaHouseItem
    {
        public string Description { get; set; }
        public string Photo { get; set; }
        public decimal Price { get; set; }

        public override string ToString()
        {
            return new StringBuilder()
                .Append(base.ToString())
                .AppendLine($"Description: {Description}")
                .AppendLine($"Photo: {Photo}")
                .AppendLine($"Price: {Price:C}")
                .ToString();
        }
    }
}