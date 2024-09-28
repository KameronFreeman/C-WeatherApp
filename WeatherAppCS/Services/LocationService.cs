using Flurl.Http;
using WeatherAppCS.Models;

namespace WeatherAppCS.Services
{

    public class LocationService
    {
        public async Task<ResponseModel> GetLocationData(string location)
        {
            string mapApiUrl = AppConstants.MapApiUrl;
            string mapApiKey = AppConstants.MapApiKey;
            string apiUrl = $"{mapApiUrl}{location}.json?access_token={mapApiKey}&limit=1";
            try
            {
                var response =await apiUrl.GetAsync();
                if(response.StatusCode == 200)
                {
                    var responseData = await response.GetJsonAsync<Root>();
                    return ResponseModel.Success(responseData.features[0].center);
                }
                return ResponseModel.Error(response.ResponseMessage.ToString());
            }
            catch(FlurlHttpException ex)
            {
                var errorResponse = await ex.GetResponseJsonAsync<dynamic>();
                return ResponseModel.Error(errorResponse);
            }
            
        }
    }
}