using System;
using System.Collections.Generic;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;

// Serverside Client
[Route("/sensors/{Id}/lastvalue", Summary = "Get the last measured value", Verbs = "GET")]
public class LastValue : IReturn<LastValueResponse>{
    [ApiMember(Name = "Id", Description = "Sensor id", ParameterType = "path", DataType = "string", IsRequired = true)]
    public string Id { get; set; }
}

// Serverside Server
[Route("/sensors/{Id}/current", Summary = "Get the last measured value", Verbs = "GET")]
public class SensorValue : IReturn<SensorValueResponse>
{
    [ApiMember(Name = "Id", Description = "Sensor id", ParameterType = "path", DataType = "string", IsRequired = true)]
    public string Id { get; set; }
}

[Route("/sensors/{Id}/values", Summary = "Insert sensor", Verbs = "GET")]
public class SensorValues : IReturn<SensorValuesResponse>
{
    [ApiMember(Name = "Id", Description = "Sensor id", ParameterType = "path", DataType = "string", IsRequired = true)]
    public string Id { get; set; }
    [ApiMember(Name = "FromDate", Description = "From date", ParameterType = "query", DataType = "string", IsRequired = false)]
    public DateTime? FromDate { get; set; }
}

[Route("/settings", Summary = "Change notification settings", Verbs = "POST")]
[Route("/settings", Summary = "Get notification settings", Verbs = "GET")]
public class Settings : IReturn<SettingsResponse>
{
    [ApiMember(Name = "TemperatureAlertLimit", Description = "Send notification if this limit is reached.", ParameterType = "body", DataType = "string", IsRequired = true)]
    public double TemperatureAlertLimit { get; set; }
    [ApiMember(Name = "HumidityAlertLimit", Description = "Send notification if this limit is reached.", ParameterType = "body", DataType = "string", IsRequired = true)]
    public double HumidityAlertLimit { get; set; }
    [ApiMember(Name = "Co2AlertLimit", Description = "Send notification if this limit is reached.", ParameterType = "body", DataType = "string", IsRequired = true)]
    public double Co2AlertLimit { get; set; }
    [ApiMember(Name = "PushalotAuthorizationToken", Description = "Pushalot authorization token for sending notifications.", ParameterType = "body", DataType = "string", IsRequired = true)]
    public string PushalotAuthorizationToken { get; set; }
    [ApiMember(Name = "Password", Description = "Protection", ParameterType = "body", DataType = "string", IsRequired = true)]
    public string Password { get; set; }
    [ApiMember(Name = "EnableNotifications", Description = "Enable notifications", ParameterType = "body", DataType = "string", IsRequired = true)]
    public bool EnableNotifications { get; set; }
}

public class SensorValueResponse
{
    public ResponseStatus ResponseStatus { get; set; }
    public double Value { get; set; }
    public DateTime? Date { get; set; }

    public SensorValueResponse()
    {
        ResponseStatus = new ResponseStatus();
    }
}

public class LastValueResponse
{
    public ResponseStatus ResponseStatus { get; set; }
    public double LastValue { get; set; }
    public DateTime LastUpdateDate { get; set; }

    public LastValueResponse()
    {
        ResponseStatus = new ResponseStatus();
    }
}

public class SensorValuesResponse
{
    public ResponseStatus ResponseStatus { get; set; }
    public List<SensorValueResponse> Values { get; set; } 

    public SensorValuesResponse()
    {
        ResponseStatus = new ResponseStatus();
    }
}

public class AuthenticateResponse
{
    public ResponseStatus ResponseStatus { get; set; }
    public string UserId { get; set; }
    public string SessionId { get; set; }
    public string UserName { get; set; }

    public AuthenticateResponse()
    {
        ResponseStatus = new ResponseStatus();
    }
}

public class SettingsResponse {
    public ResponseStatus ResponseStatus { get; set; }
    public double TemperatureAlertLimit { get; set; }
    public double HumidityAlertLimit { get; set; }
    public double Co2AlertLimit { get; set; }
    public bool EnableNotifications { get; set; }

    public SettingsResponse()
    {
        ResponseStatus = new ResponseStatus();
    }
}
