using System;
using System.Text.Json.Serialization;


namespace ApiClient
{
    class Model
    {
        public void Write()
        {
            Console.Write(Id + ": ");
            Console.WriteLine(SomeThing);
            Console.WriteLine("");
        }

        [JsonPropertyName("id")]
        /// <summary>
        /// Identifier
        /// </summary>
        public string Id { get; set; }
        
        [JsonPropertyName("someThing")]
        /// <summary>
        /// Auto-generated Guid stored in DB under identifier Id
        /// </summary>
        public Guid SomeThing { get; set; }
    }
}
