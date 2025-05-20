using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2;
using MvcDynamoDbCoches.Models;

namespace MvcDynamoDbCoches.Services
{
    public class ServiceDynamoDb
    {
        private DynamoDBContext context;

        public ServiceDynamoDb()
        {
            //TENEMOS UN CLIENT PARA CREAR EL CONTEXT DE DYNAMO
            AmazonDynamoDBClient client = new AmazonDynamoDBClient();
            //DynamoDBContextBuilder builder = new DynamoDBContextBuilder();
            //builder.;
            this.context = new DynamoDBContext(client);
        }

        public async Task CreateCocheAsync(Coche car)
        {
            await this.context.SaveAsync<Coche>(car);
        }

        public async Task DeleteCocheAsync(int idCoche)
        {
            await this.context.DeleteAsync<Coche>(idCoche);
        }

        public async Task<Coche> FindCocheAsync(int idCoche)
        {
            //SI ESTAMOS BUSCANDO POR PARTITION KEY TENEMOS 
            //UN METODO QUE LO REALIZA POR NOSOTROS
            return await this.context.LoadAsync<Coche>(idCoche);
        }

        public async Task<List<Coche>> GetCochesAsync()
        {
            //DEBEMOS BUSCAR LA TABLA QUE CORRESPONDE A NUESTRO MODEL
            ITable tabla = this.context.GetTargetTable<Coche>();
            //PARA BUSCAR, NO SABEMOS SI BUSCAMOS TODOS O FILTRAMOS
            //DEBEMOS CREAR UN OBJETO LLAMADO ScanOptions QUE LLEVARIA LOS
            //FILTROS
            var scanOptions = new ScanOperationConfig();
            var results = tabla.Scan(scanOptions);
            //DYNAMO DB DENOMINA A LO QUE ALMACENA COMO Document
            List<Document> data = await results.GetNextSetAsync();
            //CONVERTIMOS DOCUMENT A NUESTRO MODEL
            var cars =
                this.context.FromDocuments<Coche>(data);
            return cars.ToList();
        }
    }
}
