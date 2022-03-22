namespace FitnessBuddy.Web.ViewModels.Chats
{
    using AutoMapper;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;

    public class MessageViewModel : IMapFrom<Message>, IHaveCustomMappings
    {
        public string AuthorUsername { get; set; }

        public string CreatedOn { get; set; }

        public string Content { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Message, MessageViewModel>()
                   .ForMember(
                   dest => dest.CreatedOn, opt => opt.MapFrom(x => x.CreatedOn.ToString("MM/dd/yyyy HH:mm")));
        }
    }
}
