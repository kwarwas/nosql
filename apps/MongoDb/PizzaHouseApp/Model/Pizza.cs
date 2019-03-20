using System.Collections.Generic;
using System.Text;

namespace PizzaHouseApp.Model
{
    public class Pizza : PizzaHouseItem
    {
        public ICollection<string> Ingredients { get; set; }
        public string Photo { get; set; }
        public PizzaPrice Price { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder()
                .Append(base.ToString())
                .AppendLine($"Photo: {Photo}")
                .AppendLine($"Price: {Price:C}")
                .AppendLine("Ingredients:");

            foreach (var ingredient in Ingredients)
            {
                sb.AppendLine($" - {ingredient}");
            }

            return sb.ToString();
        }
    }
}