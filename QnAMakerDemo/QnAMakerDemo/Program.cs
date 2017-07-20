using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QnAMakerDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            string flag = string.Empty;
            string responseString = string.Empty;
            Start:
            Console.WriteLine("Ask your question.");
            var query = Console.ReadLine(); //User Query
            var knowledgebaseId = "your knowledgebase id"; // Use knowledge base id created.
            var qnamakerSubscriptionKey = "your subscription key"; //Use subscription key assigned to you.

            //Build the URI
            Uri qnamakerUriBase = new Uri("https://westus.api.cognitive.microsoft.com/qnamaker/v2.0");
            var builder = new UriBuilder($"{qnamakerUriBase}/knowledgebases/{knowledgebaseId}/generateAnswer");

            //Add the question as part of the body
            var postBody = $"{{\"question\": \"{query}\"}}";

            //Send the POST request
            using (WebClient client = new WebClient())
            {
                //Set the encoding to UTF8
                client.Encoding = System.Text.Encoding.UTF8;

                //Add the subscription key header
                client.Headers.Add("Ocp-Apim-Subscription-Key", qnamakerSubscriptionKey);
                client.Headers.Add("Content-Type", "application/json");
                responseString = client.UploadString(builder.Uri, postBody);
                var response = JsonConvert.DeserializeObject<RootObject>(responseString);
                Console.WriteLine("Answer: " + response.answers[0].answer);
            }
            flag = Console.ReadLine();
            if (string.IsNullOrEmpty(flag))
            {
                goto Start;
            }
        }
    }

    public class Answer
    {
        public string answer { get; set; }
        public List<string> questions { get; set; }
        public double score { get; set; }
    }

    public class RootObject
    {
        public List<Answer> answers { get; set; }
    }
}
