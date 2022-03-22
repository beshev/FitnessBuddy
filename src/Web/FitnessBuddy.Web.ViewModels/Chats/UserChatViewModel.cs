namespace FitnessBuddy.Web.ViewModels.Chats
{
    using System;
    using System.Collections.Generic;

    using AutoMapper;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;

    public class UserChatViewModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public string ReceiverId { get; set; }

        public string ReceiverUsername { get; set; }

        public string CreatedOn { get; set; }

        public IEnumerable<MessageViewModel> Messages { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, UserChatViewModel>()
                .ForMember(
                dest => dest.ReceiverId, opt => opt.MapFrom(x => x.Id))
                .ForMember(
                dest => dest.ReceiverUsername, opt => opt.MapFrom(x => x.UserName))
                .ForMember(
                dest => dest.CreatedOn, opt => opt.MapFrom(x => x.CreatedOn.ToString("MM/dd/yyyy HH:mm")));
        }
    }
}
