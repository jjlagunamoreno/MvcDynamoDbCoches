using Amazon.DynamoDBv2.DataModel;

namespace MvcDynamoDbCoches.Models
{
    [DynamoDBTable("coches")]
    public class Coche
    {
        [DynamoDBHashKey]
        [DynamoDBProperty("idcoche")]
        public string IdCoche { get; set; }

        [DynamoDBProperty("marca")]
        public string Marca { get; set; }

        [DynamoDBProperty("modelo")]
        public string Modelo { get; set; }

        [DynamoDBProperty("imagen")]
        public string Imagen { get; set; }
        public Motor Motor { get; set; }
    }

}
