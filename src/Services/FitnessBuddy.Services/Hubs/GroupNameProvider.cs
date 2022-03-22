namespace FitnessBuddy.Services.Hubs
{
    using FitnessBuddy.Common;

    public class GroupNameProvider : IGroupNameProvider
    {
        public string GetGroupName(string firstSrting, string secondString)
            => firstSrting.CompareTo(secondString) > 0
                ? string.Format(GlobalConstants.HubGroupNameFormat, secondString, firstSrting)
                : string.Format(GlobalConstants.HubGroupNameFormat, firstSrting, secondString);
    }
}
