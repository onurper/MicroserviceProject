using System;
using System.Collections.Generic;

namespace SharedLibrary.Dtos
{
    public class ResponseSuccess
    {
        public Data data { get; set; }
        public int statusCode { get; set; }
        public ErrorData error { get; set; }
    }

    public class Data
    {
        public string accessToken { get; set; }
        public DateTime accessTokenExpiration { get; set; }
        public string refreshToken { get; set; }
        public DateTime refreshTokenExpiration { get; set; }
    }

    public class ResponseFail
    {
        public Data data { get; set; }
        public int statusCode { get; set; }
        public ErrorData error { get; set; }
    }

    public class ErrorData
    {
        public List<String> Errors { get; private set; } = new List<string>();

        public bool IsShow { get; private set; }
    }

    public class ResponseJsonDeserializeObject<T> where T : class
    {
        public T Data { get; set; }
        public int StatusCode { get; set; }
        public int ErrorKey { get; set; }
        public Error Error { get; set; }
    }

    public class Error
    {
        public string[] Errors { get; set; }
        public bool IsShow { get; set; }
    }
}