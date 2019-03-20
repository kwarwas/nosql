using System.Collections.Generic;
using System.Text;

namespace PizzaHouseApp.Model
{
    public class Drink : PizzaHouseItem
    {
        public double Size { get; set; }
        public decimal Price { get; set; }
        public ICollection<string> Kind { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder()
                .Append(base.ToString())
                .AppendLine($"Size: {Size}")
                .AppendLine($"Price: {Price:C}");

            foreach (var kind in Kind)
            {
                sb.AppendLine($" - {kind}");
            }

            return sb.ToString();
        }
    }
}