using MongoDB.Bson.Serialization;
using PizzaHouseApp.Model;

namespace PizzaHouseApp.Database
{
    class EntityMappings
    {
        public static void Map()
        {
            BsonClassMap.RegisterClassMap<PizzaHouseItem>(x =>
            {
                x.AutoMap();
                x.GetMemberMap(y => y.Name).SetElementName("name");
            });

            BsonClassMap.RegisterClassMap<Pizza>(x =>
            {
                x.AutoMap();
                x.SetIgnoreExtraElements(true);
                x.SetDiscriminator("Pizza");
                x.GetMemberMap(y => y.Ingredients).SetElementName("ingredients");
                x.GetMemberMap(y => y.Photo).SetElementName("photo");
                x.GetMemberMap(y => y.Price).SetElementName("price");
            });

            BsonClassMap.RegisterClassMap<PizzaPrice>(x =>
            {
                x.AutoMap();
                x.GetMemberMap(y => y.Small).SetElementName("small");
                x.GetMemberMap(y => y.Big).SetElementName("big");
                x.GetMemberMap(y => y.Family).SetElementName("family");
            });

            BsonClassMap.RegisterClassMap<Pasta>(x =>
            {
                x.AutoMap();
                x.SetIgnoreExtraElements(true);
                x.SetDiscriminator("Pasta");
                x.GetMemberMap(y => y.Description).SetElementName("description");
                x.GetMemberMap(y => y.Photo).SetElementName("photo");
                x.GetMemberMap(y => y.Price).SetElementName("price");
            });

            BsonClassMap.RegisterClassMap<Drink>(x =>
            {
                x.AutoMap();
                x.SetIgnoreExtraElements(true);
                x.SetDiscriminator("Drink");
                x.GetMemberMap(y => y.Size).SetElementName("size");
                x.GetMemberMap(y => y.Kind).SetElementName("kind");
                x.GetMemberMap(y => y.Price).SetElementName("price");
            });
        }
    }
}