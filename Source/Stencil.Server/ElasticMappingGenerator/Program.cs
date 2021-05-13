using Codeable.Foundation.Core;
using Elasticsearch.Net;
using Nest;
using Newtonsoft.Json.Linq;
using System;

namespace ElasticMappingGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            CoreFoundation.Initialize(new ElasticMappingGeneratorBootstrap(), true);

            var clientFactory = CoreFoundation.Current.SafeResolve<MappingGeneratorElasticClientFactory>();

            ICreateIndexRequest request = clientFactory.BuildCreateIndexRequest();

            JsonNetSerializer serializer = new JsonNetSerializer(clientFactory.BuildConnectionSettings());
            string json = serializer.SerializeToString(request, SerializationFormatting.Indented);

            //Console.WriteLine(json);

            var client = clientFactory.CreateClient();
            var response = client.GetMapping<Program>(descriptor => descriptor.Index(clientFactory.IndexName).AllTypes());

            string mappings = serializer.SerializeToString(response.IndexTypeMappings, SerializationFormatting.Indented);

            //Console.WriteLine(mappings);

            JToken requestedMappings = JObject.Parse(json)["mappings"];
            JToken foundMappings = JObject.Parse(mappings)[clientFactory.IndexName];

            Console.WriteLine(requestedMappings);
            Console.WriteLine(foundMappings);

            var differ = new JsonDiffPatch.JsonDiffer();
            JsonDiffPatch.PatchDocument patch = differ.Diff(foundMappings, requestedMappings, useIdPropertyToDetermineEquality: false);

            Console.WriteLine(patch);
        }
    }
}
