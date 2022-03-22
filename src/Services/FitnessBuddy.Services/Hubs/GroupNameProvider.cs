namespace FitnessBuddy.Services.Hubs
{
    public class GroupNameProvider : IGroupNameProvider
    {
        public string GetGroupName(string firstSrting, string secondString)
            => firstSrting.CompareTo(secondString) > 0
                ? $"{secondString}{firstSrting}"
                : $"{firstSrting}{secondString}";
    }
}
