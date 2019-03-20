using BlogApp.Entities;
using MongoDB.Bson.Serialization;

namespace BlogApp.Database
{
    public static class EntityMappings
    {
        public static void Map()
        {
            BsonClassMap.RegisterClassMap<Blog>(x => 
            {
                x.AutoMap();
                x.SetIgnoreExtraElements(true);
                x.MapIdMember(y => y.Id);
            });

            BsonClassMap.RegisterClassMap<Author>(x => 
            {
                x.AutoMap();
                x.SetIgnoreExtraElements(true);
            });
            
            BsonClassMap.RegisterClassMap<Post>(x => 
            {
                x.AutoMap();
                x.SetIgnoreExtraElements(true);
                x.MapIdMember(y => y.Id);
            });
        }
    }
}