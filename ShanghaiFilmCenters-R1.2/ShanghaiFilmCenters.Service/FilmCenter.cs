using System;
using System.Runtime.Serialization;

namespace ShanghaiFilmCenters.Service
{
    [Serializable]
    [DataContract]
    public class FilmCenter
    {
        public FilmCenter(string displayName, string code)
        {
            this.DisplayName = displayName;
            this.Code = code;
        }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string DisplayName { get; set; }

        [DataMember]
        public string DetailAddress { get; set; }

        [DataMember]
        public string Phone { get; set; }
    }
}
