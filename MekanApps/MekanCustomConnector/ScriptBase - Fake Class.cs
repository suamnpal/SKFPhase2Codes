using System.Text;
using System.Text.Json;

using System.Net.Http;
using System.Threading;
using System.Net;

public class ScriptBase
{
    public ContextClass Context { get; private set; }

    public object CancellationToken { get; private set; }

    public virtual async Task<HttpResponseMessage> ExecuteAsync()
    {
        throw new NotImplementedException();
    }

    public static StringContent CreateJsonContent(object obj)
    {
        var json = JsonSerializer.Serialize(obj);
        return new StringContent(json, Encoding.UTF8, "application/json");
    }

    public sealed class ContextClass
    {
        public string OperationId { get; private set; }

        public HttpRequestMessage Request { get; private set; }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, object token)
        {
            throw new NotImplementedException();
        }
    }
}
