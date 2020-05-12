using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OrangeVote.Utils
{
    class HttpHelper
    {
        private static HttpHelper _httpHelper = null;

        private HttpClient _httpClient;

        /// <summary>
        /// 建立單例
        /// </summary>
        /// <returns></returns>
        public static HttpHelper GetInstance()
        {
            if (_httpHelper != null)
            {
                return _httpHelper;
            }
            else
            {
                HttpHelper helper = new HttpHelper();
                HttpClientHandler handler = new HttpClientHandler() {UseCookies = false};
                helper._httpClient = new HttpClient(handler);
                return helper;
            }
        }

        /// <summary>
        /// 設置預設請求標頭檔
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetDefaultHeader(string key, string value)
        {
            _httpClient.DefaultRequestHeaders.Add(key, value);
        }

        /// <summary>
        /// 移除請求標頭檔
        /// </summary>
        /// <param name="key"></param>
        public void RemoveDefaultHeader(string key)
        {
            _httpClient.DefaultRequestHeaders.Remove(key);
        }

        /// <summary>
        /// Get 方法
        /// </summary>
        /// <param name="url">Api地址</param>
        /// <param name="headers">HeaderList</param>
        /// <returns></returns>
        public HttpResponseMessage Get(string url, List<KeyValuePair<string, string>> headers = null)
        {
            HttpRequestMessage request = InitGet(url, headers);
            HttpResponseMessage response = _httpClient.SendAsync(request).Result;
            return response;
        }

        /// <summary>
        /// Get 方法 配合同步
        /// </summary>
        /// <param name="url">Api地址</param>
        /// <param name="headers">HeaderList</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> GetAsync(string url, List<KeyValuePair<string, string>> headers = null)
        {
            HttpRequestMessage request = InitGet(url, headers);
            return await _httpClient.SendAsync(request);
        }

        /// <summary>
        /// Post 方法(formUrlEncoded)
        /// </summary>
        /// <param name="url">Api地址</param>
        /// <param name="paramList">參數List</param>
        /// <param name="headers">HeaderList</param>
        /// <returns></returns>
        public HttpResponseMessage Post(string url, List<KeyValuePair<string, string>> paramList,
            List<KeyValuePair<string, string>> headers = null)
        {
            return InitPost(url, paramList, headers).Result;
        }

        /// <summary>
        /// Post 方法(formUrlEncoded) 配合同步
        /// </summary>
        /// <param name="url">Api地址</param>
        /// <param name="paramList">參數List</param>
        /// <param name="headers">HeaderList</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> PostAsync(string url, List<KeyValuePair<string, string>> paramList,
            List<KeyValuePair<string, string>> headers = null)
        {
            return await InitPost(url, paramList, headers);
        }

        /// <summary>
        /// Post 方法(Row Data)
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public HttpResponseMessage Post(string url, string content, List<KeyValuePair<string, string>> headers)
        {
            return InitPost(url, content, headers).Result;
        }

        /// <summary>
        /// Post 方法(Row Data) 配合同步
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> PostAsync(string url, string content,
            List<KeyValuePair<string, string>> headers = null)
        {
            return await InitPost(url, content, headers);
        }

        /// <summary>
        /// 釋放HttpClient
        /// </summary>
        public void Release()
        {
            _httpClient.Dispose();
        }

        /// <summary>
        /// 初始化Get 方法
        /// </summary>初始化 Post RowData
        /// <param name="url">Api地址</param>
        /// <param name="headers">HeaderList</param>
        /// <returns></returns>
        private static HttpRequestMessage InitGet(string url, List<KeyValuePair<string, string>> headers = null)
        {
            HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Get,
            };
            if (headers != null && headers.Count > 0)
            {
                request.Headers.Clear();
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            return request;
        }

        /// <summary>
        /// 初始化Post FormUrlEncoded
        /// </summary>
        /// <param name="url">Api地址</param>
        /// <param name="paramList">formdata List</param>
        /// <param name="headers">HeaderList</param>
        /// <returns></returns>
        private Task<HttpResponseMessage> InitPost(string url, List<KeyValuePair<string, string>> paramList,
            List<KeyValuePair<string, string>> headers = null)
        {
            FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(paramList);
            if (headers != null && headers.Count > 0)
            {
                formUrlEncodedContent.Headers.Clear();
                foreach (var header in headers)
                {
                    formUrlEncodedContent.Headers.Add(header.Key, header.Value);
                }
            }

            return _httpClient.PostAsync(new Uri(url), formUrlEncodedContent);
        }

        /// <summary>
        /// 初始化 Post RowData
        /// </summary>
        /// <param name="url">Api地址</param>
        /// <param name="content">Row data</param>
        /// <param name="headers">HeaderList</param>
        /// <returns></returns>
        private Task<HttpResponseMessage> InitPost(string url, string content,
            List<KeyValuePair<string, string>> headers = null)
        {
            StringContent stringContent = new StringContent(content, Encoding.UTF8);
            if (headers != null && headers.Count > 0)
            {
                stringContent.Headers.Clear();
                foreach (var header in headers)
                {
                    stringContent.Headers.Add(header.Key, header.Value);
                }
            }

            return _httpClient.PostAsync(new Uri(url), stringContent);
        }
    }
}