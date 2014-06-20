using System;
using System.Collections.Specialized;
using System.Net;
using System.Timers;
using ServiceImplementation.SignalR.Hubs;
using ServiceInterface;
using ServiceStack.ServiceClient.Web;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;
using Auth = ServiceStack.Common.ServiceClient.Web.Auth;
using AuthResponse = ServiceStack.Common.ServiceClient.Web.AuthResponse;

namespace ServiceImplementation
{
    public class SensorsService : Service, ISensorsService
    {
        private static Timer _repeatTimer;
        private static JsonServiceClient _client;

        // Sensor IDs
        private const string TemperatureSensorId =
            "4Noks_77000000-0000-0000-0000-000000000077_ZED_THL_16_Temperature_6";
        private const string HumiditySensorId =
            "4Noks_77000000-0000-0000-0000-000000000077_ZED_THL_16_Humidity_8";
        private const string Co2SensorId =
            "4Noks_77000000-0000-0000-0000-000000000077_ZED_CO2_24_CO2_6";

        // Send notification via Pushalot if one of these limits are reached.
        private static double TemperatureAlertLimit = 30;
        private static double HumidityAlertLimit = 80;
        private static double Co2AlertLimit = 750;
        private static string PushalotAuthorizationToken = "0dff86f8fc7d4e3699a728212a79be9b";
        private static bool EnableNotifications = true;

        // Last measurements received from the sensor server.
        public static LastValueResponse LastTemperature;
        public static LastValueResponse LastHumidity;
        public static LastValueResponse LastCo2;
        
        public SensorsService(){
            if (_repeatTimer == null && _client == null) {
                // Login with vubgreenhouse
                _client = new JsonServiceClient("http://www.nassist-test.com/api") { AlwaysSendBasicAuthHeader = true };
                _client.Post<AuthResponse>("/auth", new Auth { UserName = "vubgreenhouse", Password = "vubgreenhouse", RememberMe = true });

                // Create a timer with a 60 secibd interval.
                _repeatTimer = new Timer(60000);
                // Hook up the Elapsed event for the timer. 
                _repeatTimer.Elapsed += OnTimedEvent;
                _repeatTimer.Enabled = true;
                UpdateLastValues();
            }
        }

        /*
         * Update last measurements and send notifications if a certain limit has reached.
         */
        private static void UpdateLastValues(){
            LastTemperature = _client.Get(new LastValue { Id = TemperatureSensorId });
            // Broadcast new measurement from sensor.
            TemperatureHub.Send("Temperature_6", LastTemperature.LastValue, LastTemperature.LastUpdateDate);

            if (EnableNotifications && TemperatureAlertLimit <= LastTemperature.LastValue)
            {
                SendNotification("Alert: Temperature has reached " + LastTemperature.LastValue + " °C!");
            }

            LastHumidity = _client.Get(new LastValue { Id = HumiditySensorId });
            // Broadcast new measurement from sensor.
            HumidityHub.Send("Humidity_8", LastHumidity.LastValue, LastHumidity.LastUpdateDate);

            if (EnableNotifications && HumidityAlertLimit <= LastHumidity.LastValue)
            {
                SendNotification("Alert: Humidity has reached " + LastHumidity.LastValue + " %!");
            }

            LastCo2 = _client.Get(new LastValue { Id = Co2SensorId });
            // Broadcast new measurement from sensor.
            Co2Hub.Send("CO2_8", LastCo2.LastValue, LastCo2.LastUpdateDate);

            if (EnableNotifications && Co2AlertLimit <= LastCo2.LastValue)
            {
                SendNotification("Alert: CO₂ has reached " + LastCo2.LastValue + " ppm!");
            }
        }

        /*
         * Send a notification to the Pushalot server.
         */
        private static void SendNotification (string message){
            try {
                using (var client = new WebClient())
                {
                    var values = new NameValueCollection();
                    values["AuthorizationToken"] = PushalotAuthorizationToken;
                    values["Body"] = message;
                    client.UploadValues("https://pushalot.com/api/sendmessage", values);
                }
            }
            catch{}
        }
        
        private static void OnTimedEvent(Object source, ElapsedEventArgs e){
            UpdateLastValues();
        }

        /*
         *  Retrieve all values of starting from 'FromDate'
         */
        public SensorValuesResponse Get(SensorValues request) {
            // Sensor id for Temperature (default)
            var sensorId = TemperatureSensorId;

            switch (request.Id) {
                case "Humidity":
                    sensorId = HumiditySensorId ;
                    break;
                case "CO2":
                    sensorId = Co2SensorId;
                    break;
            }

            return _client.Get(new SensorValues { Id = sensorId, FromDate = DateTime.UtcNow.AddDays(-4)});
        }

        /*
         * Get current settings.
         */
        public SettingsResponse Get(Settings request)
        {
            return new SettingsResponse {
                TemperatureAlertLimit = TemperatureAlertLimit,
                HumidityAlertLimit = HumidityAlertLimit,
                Co2AlertLimit = Co2AlertLimit,
                EnableNotifications = EnableNotifications
            };
        }

        /*
         * Change notification settings.
         */
        public SettingsResponse Post(Settings request)
        {
            if (request.Password != null && request.Password.Equals("vubgreenhouse")){
                TemperatureAlertLimit = request.TemperatureAlertLimit;
                HumidityAlertLimit = request.HumidityAlertLimit;
                Co2AlertLimit = request.Co2AlertLimit;
                PushalotAuthorizationToken = request.PushalotAuthorizationToken;
                EnableNotifications = request.EnableNotifications;
                return new SettingsResponse();
            }
            return new SettingsResponse { ResponseStatus = new ResponseStatus("400") };
        }

        /*
         * Get last measurement from a sensor. (Used by Windows Phone app)
         */
        public SensorValueResponse Get(SensorValue request)
        {
            switch (request.Id)
            {
                case "Humidity":
                    return new SensorValueResponse
                    {
                        ResponseStatus = new ResponseStatus(),
                        Value = LastHumidity.LastValue,
                        Date = LastHumidity.LastUpdateDate
                    };
                case "CO2":
                    return new SensorValueResponse
                    {
                        ResponseStatus = new ResponseStatus(),
                        Value = LastCo2.LastValue,
                        Date = LastCo2.LastUpdateDate
                    };
                default:
                    return new SensorValueResponse
                    {
                        ResponseStatus = new ResponseStatus(),
                        Value = LastTemperature.LastValue,
                        Date = LastTemperature.LastUpdateDate
                    };
            }
        }
    }
}