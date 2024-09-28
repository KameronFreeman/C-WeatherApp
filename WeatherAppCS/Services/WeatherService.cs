using Flurl.Http;
using WeatherAppCS.Models;

namespace WeatherAppCS.Services
{

    public class WeatherService
    {
        public async Task<ResponseModel> GetWeatherData(string location)
        {
            string weatherApiUrl = AppConstants.WeatherApiUrl;
            string weatherApiKey = AppConstants.WeatherApiKey;
            

            
            try
            {
                LocationService locationService = new LocationService();
                var res = locationService.GetLocationData(location);
                dynamic coordinates;
                if(res.Result.IsSuccess)
                {
                    coordinates = res.Result.JsonData;
                    var longitude = coordinates[0];
                    var latitude = coordinates[1];
                    string apiUrl = $"{weatherApiUrl}?access_key={weatherApiKey}&query={latitude},{longitude}";
                
                    var response =await apiUrl.GetAsync();
                    if(response.StatusCode == 200)
                    {
                        var responseData = await response.GetJsonAsync<WeatherModel>();
                        return ResponseModel.Success(responseData);
                    }
                
                    return ResponseModel.Error(response.ResponseMessage.ToString());
                }
                return ResponseModel.Error(res.Result.ToString());
            }
            catch(FlurlHttpException ex)
            {
                var errorResponse = await ex.GetResponseJsonAsync<dynamic>();
                return ResponseModel.Error(errorResponse);
            }
            
        }
    }
}