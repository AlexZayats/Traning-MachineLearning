using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JARVIS.Core.Networking.Clients
{
    public class ApiClient : IDisposable
    {
        protected HttpMessageHandler _messageHandler;
        protected HttpClient _client;
        protected readonly ILogger<ApiClient> _logger;

        public ApiClient(ILogger<ApiClient> logger = null)
        {
            _messageHandler = GetHttpMessageHandler();
            _client = new HttpClient();
            _logger = logger;
        }

        public Task<TResponse> ApiGetAsync<TResponse>(Uri uri, CancellationToken token = default(CancellationToken))
        {
            return ApiVerbAsync<TResponse, object>(uri, HttpMethod.Get, null, token);
        }

        public Task<TResponse> ApiPostAsync<TResponse, TRequest>(Uri uri, TRequest requestData, CancellationToken token = default(CancellationToken))
        {
            return ApiVerbAsync<TResponse, TRequest>(uri, HttpMethod.Post, requestData, token);
        }

        public Task ApiPutAsync(Uri uri, CancellationToken token = default(CancellationToken))
        {
            return ApiVerbAsync<object, object>(uri, HttpMethod.Put, null, token);
        }

        public Task<TResponse> ApiPutAsync<TResponse>(Uri uri, CancellationToken token = default(CancellationToken))
        {
            return ApiVerbAsync<TResponse, object>(uri, HttpMethod.Put, null, token);
        }

        public Task<TResponse> ApiPutAsync<TResponse, TRequest>(Uri uri, TRequest requestData, CancellationToken token = default(CancellationToken))
        {
            return ApiVerbAsync<TResponse, TRequest>(uri, HttpMethod.Put, requestData, token);
        }

        public Task ApiDeleteAsync(Uri uri, CancellationToken token = default(CancellationToken))
        {
            return ApiVerbAsync<object, object>(uri, HttpMethod.Delete, null, token);
        }

        public Task<TResponse> ApiDeleteAsync<TResponse>(Uri uri, CancellationToken token = default(CancellationToken))
        {
            return ApiVerbAsync<TResponse, object>(uri, HttpMethod.Delete, null, token);
        }

        public Task<TResponse> ApiDeleteAsync<TResponse, TRequest>(Uri uri, TRequest requestData, CancellationToken token = default(CancellationToken))
        {
            return ApiVerbAsync<TResponse, TRequest>(uri, HttpMethod.Delete, requestData, token);
        }

        protected async Task<TResponse> ApiVerbAsync<TResponse, TRequest>(Uri uri, HttpMethod method, TRequest requestData, CancellationToken token = default(CancellationToken))
        {
            using (var response = await GetResponseAsync(uri, method, requestData, token))
            {
                var buffer = await response.Content.ReadAsByteArrayAsync();
                var content = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                return JsonConvert.DeserializeObject<TResponse>(content, new JsonSerializerSettings
                {
                    Error = delegate (object sender, ErrorEventArgs args)
                    {
                        Debug.WriteLine($"JSON error: {args.ErrorContext.Error.Message}");
                        args.ErrorContext.Handled = true;
                    }
                });
            }
        }

        protected async Task<HttpResponseMessage> GetResponseAsync<TRequest>(Uri uri, HttpMethod method, TRequest requestData, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var request = new HttpRequestMessage(method, uri))
            {
                HttpResponseMessage response = null;
                var stopwatch = new Stopwatch();
                _logger?.LogTrace($"{method.Method} [{uri}]");
                try
                {
                    if (requestData != null)
                    {
                        var data = JsonConvert.SerializeObject(requestData);
                        request.Content = new StringContent(data, Encoding.UTF8, "application/json");
                    }
                    ApplyHeaders(request);
                    stopwatch.Start();
                    response = await _client.SendAsync(request, cancellationToken);
                    return response;
                }
                catch (OperationCanceledException)
                {
                    _logger?.LogTrace($"Canceled [{uri}]");
                    throw;
                }
                catch (Exception exception)
                {
                    _logger?.LogTrace($"Error [{uri}]: {exception}");
                    _logger?.LogError(exception, $"Error [{uri}]");
                    throw;
                }
                finally
                {
                    stopwatch.Stop();
                    _logger?.LogTrace($"OK [{uri}]: {stopwatch.Elapsed}");
                }
            }
        }

        protected virtual HttpMessageHandler GetHttpMessageHandler()
        {
            var handler = new HttpClientHandler();
            handler.UseProxy = true;
            handler.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            return handler;
        }

        protected virtual void ApplyHeaders(HttpRequestMessage request)
        {
        }

        public void Dispose()
        {
            if (_messageHandler != null)
            {
                _messageHandler.Dispose();
                _messageHandler = null;
            }

            if (_client != null)
            {
                _client.Dispose();
                _client = null;
            }
        }
    }
}
