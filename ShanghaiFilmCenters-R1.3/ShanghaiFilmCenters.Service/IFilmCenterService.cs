using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace ShanghaiFilmCenters.Service
{
    [ServiceContract]
    public interface IFilmCenterService
    {
        [OperationContract]
        Response<List<MovieInfo>> GetMovies(string filmCenterCode, DateTime selectedDate);

        [OperationContract]
        Response<List<FilmCenter>> GetAllFilmCenters();
    }
}
