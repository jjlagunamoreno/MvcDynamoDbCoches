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

        public async Task<List<Coche>> BuscarCochesAsync(string filtro)
        {
            var scanOptions = new ScanOperationConfig();
            var tabla = this.context.GetTargetTable<Coche>();
            var results = tabla.Scan(scanOptions);
            List<Document> data = await results.GetNextSetAsync();
            var cars = this.context.FromDocuments<Coche>(data);

            return cars
                .Where(x =>
                    (!string.IsNullOrEmpty(x.Marca) && x.Marca.Contains(filtro, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(x.Modelo) && x.Modelo.Contains(filtro, StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }

        public async Task CreateCocheAsync(Coche car)
        {
            await this.context.SaveAsync<Coche>(car);
        }

        public async Task DeleteCocheAsync(string idCoche)
        {
            await this.context.DeleteAsync<Coche>(idCoche);
        }

        public async Task<Coche> FindCocheAsync(string idCoche)
        {
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
