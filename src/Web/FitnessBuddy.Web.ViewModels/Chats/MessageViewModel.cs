namespace FitnessBuddy.Web.ViewModels.Chats
{
    using System.Globalization;

    using AutoMapper;
    using FitnessBuddy.Common;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;
    using Ganss.XSS;

    public class MessageViewModel : IMapFrom<Message>, IHaveCustomMappings
    {
        private readonly IHtmlSanitizer htmlSanitizer;

        public MessageViewModel()
        {
            this.htmlSanitizer = new HtmlSanitizer();
        }

        public int Id { get; set; }

        public string AuthorUsername { get; set; }

        public string CreatedOn { get; set; }

        public string Content { get; set; }

        public string SanitizedContent => this.htmlSanitizer.Sanitize(this.Content);

        public bool IsDeleted { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Message, MessageViewModel>()
                   .ForMember(
                   dest => dest.CreatedOn,
                   opt =>
                   opt.MapFrom(x => x.CreatedOn.ToString(GlobalConstants.DateTimeFormat, CultureInfo.InvariantCulture)));
        }
    }
}
