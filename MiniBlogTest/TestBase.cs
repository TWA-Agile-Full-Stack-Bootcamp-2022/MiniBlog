using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using MiniBlog;
using Newtonsoft.Json;
using Xunit;

namespace MiniBlogTest
{
    public class TestBase : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        public TestBase(CustomWebApplicationFactory<Startup> factory)
        {
            this.Factory = factory;
        }

        protected CustomWebApplicationFactory<Startup> Factory { get; }

        protected HttpClient GetClient()
        {
            return Factory.CreateClient();
        }

        protected static async Task<T> DeserializeObject<T>(HttpResponseMessage response)
        {
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }

        protected static StringContent SerializeToStringContent<T>(T obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, MediaTypeNames.Application.Json);
        }
    }
}