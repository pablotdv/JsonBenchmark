using System.Text.Json;
using System.Text.Json.Serialization;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Newtonsoft.Json;

namespace JsonBenchmark
{
    public class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<JsonComparisons>();
        }
    }

        [MemoryDiagnoser]
        [RankColumn]
        public class JsonComparisons
        {
            private readonly JsonSerializerOptions _jsonOptions;
            private readonly List<Produto> _produtos;

            public JsonComparisons()
            {
                _jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);

                _produtos = new List<Produto>()
                {
                    new Produto()
                    {
                        Id= 1,
                        Nome = "Name 1",
                        Categoria = "Category 1",
                        Descricao= "Description 1",
                        Preco = 1,                        
                    },
                    new Produto()
                    {
                        Id= 2,
                        Nome = "Name 2",
                        Categoria = "Category 2",
                        Descricao= "Description 2",
                        Preco = 2,                        
                    },
                    new Produto()
                    {
                        Id= 3,
                        Nome = "Name 3",
                        Categoria = "Category 3",
                        Descricao= "Description 3",
                        Preco = 3,
                    }
                };
            }

            [Benchmark]
            public string Newtonsoft() => JsonConvert.SerializeObject(_produtos);

            [Benchmark]
            public string SystemTextJson() => System.Text.Json.JsonSerializer.Serialize(_produtos, _jsonOptions);


            [Benchmark]
            public string SourceGenerator() => System.Text.Json.JsonSerializer.Serialize(_produtos, ProdutoGenerationContext.Default.ListProduto);

        }
    }

    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public double Preco { get; set; }
        public string Categoria { get; set; }
    }

    [JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
    [JsonSerializable(typeof(List<Produto>))]
    public partial class ProdutoGenerationContext : JsonSerializerContext { }
}