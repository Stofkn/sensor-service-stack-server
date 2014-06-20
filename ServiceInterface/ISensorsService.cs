namespace ServiceInterface
{
    public interface ISensorsService
    {
        // Get last measurement.
        SensorValueResponse Get(SensorValue request);
        // Get last measurements from past days.
        SensorValuesResponse Get(SensorValues request);
        // Get current notification settings.
        SettingsResponse Get(Settings request);
        // Change current notification settings.
        SettingsResponse Post(Settings request);
    }
}
