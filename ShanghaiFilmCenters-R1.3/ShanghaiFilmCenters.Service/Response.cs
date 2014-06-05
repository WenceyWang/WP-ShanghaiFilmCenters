using System;
using System.Runtime.Serialization;
using System.Text;

namespace ShanghaiFilmCenters.Service
{
    [Serializable]
    [DataContract]
    public class Response
    {
        [DataMember]
        public bool IsSuccess { get; set; }

        [DataMember]
        public string Message { get; set; }

        public Response()
        {
            IsSuccess = false;
            Message = string.Empty;
        }
    }

    [Serializable]
    [DataContract]
    public class Response<T> : Response
    {
        [DataMember]
        public T Item { get; set; }
    }
}
