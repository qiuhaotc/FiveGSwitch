namespace FiveGSwitch.Business
{
    public interface ISwitchProvider
    {
        bool Capable { get; }

        bool IsEnabled { get; }

        bool Toggle();
    }
}
