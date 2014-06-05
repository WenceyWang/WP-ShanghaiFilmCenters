using System;
using System.Runtime.Serialization;

namespace ShanghaiFilmCenters.Service
{
    [Serializable]
    [DataContract]
    public class MovieInfo
    {
        [DataMember]
        public string FilmCenterName { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Language { get; set; }

        [DataMember]
        public DateTime? StartTime { get; set; }

        [DataMember]
        public DateTime? Date { get; set; }

        [DataMember]
        public string Price { get; set; }

        [DataMember]
        public string Hall { get; set; }

        public MovieInfo()
        {

        }
    }
}
