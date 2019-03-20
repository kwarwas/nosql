using System.Text;

namespace PizzaHouseApp.Model
{
    public abstract class PizzaHouseItem
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return new StringBuilder()
                .AppendLine($"Id: {Id}")
                .AppendLine($"Name: {Name}")
                .ToString();
        }
    }
}