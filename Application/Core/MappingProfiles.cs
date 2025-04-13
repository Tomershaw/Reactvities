using Application.Activities;
using Application.Comments;
using AutoMapper;
using Domain;

namespace Application.Core
{
    // Defines mapping configurations for AutoMapper to transform objects between domain entities and DTOs.
    // Ensures consistent and efficient object transformation across the application.

    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // Used to capture the current user's username for context-aware mappings.
            string currentUsername = null;

            // Map Activity entity to itself (used for updating activities).
            CreateMap<Activity, Activity>();

            // Map Activity entity to ActivityDto, including the host's username.
            CreateMap<Activity, ActivityDto>()
                .ForMember(d => d.HostUsername,
                    o => o.MapFrom(s =>
                        s.Attendees.FirstOrDefault(x => x.IsHost).AppUser.UserName));

            // Map ActivityAttendee entity to AttendeeDto, including user details and follow status.
            CreateMap<ActivityAttendee, AttendeeDto>()
                .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.AppUser.DisplayName))
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.AppUser.UserName))
                .ForMember(d => d.Bio, o => o.MapFrom(s => s.AppUser.Bio))
                .ForMember(d => d.Image, o => o.MapFrom(s => s.AppUser.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(d => d.FollowersCount, o => o.MapFrom(s => s.AppUser.Followers.Count))
                .ForMember(d => d.FollowingCount, o => o.MapFrom(s => s.AppUser.Followings.Count))
                .ForMember(d => d.Following, o => o.MapFrom(s =>
                    s.AppUser.Followers.Any(x => x.Observer.UserName == currentUsername)));

            // Map AppUser entity to Profile DTO, including user details and follow status.
            CreateMap<AppUser, Profiles.Profile>()
                .ForMember(d => d.Image, o => o.MapFrom(s => s.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(d => d.FollowersCount, o => o.MapFrom(s => s.Followers.Count))
                .ForMember(d => d.FollowingCount, o => o.MapFrom(s => s.Followings.Count))
                .ForMember(d => d.Following, o => o.MapFrom(s =>
                    s.Followers.Any(x => x.Observer.UserName == currentUsername)));

            // Map Comment entity to CommentDto, including author details.
            CreateMap<Comment, CommentDto>()
                .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.Author.DisplayName))
                .ForMember(d => d.Username, o => o.MapFrom(s => s.Author.UserName))
                .ForMember(d => d.Image, o => o.MapFrom(s => s.Author.Photos.FirstOrDefault(x => x.IsMain).Url));

            // Map ActivityAttendee entity to UserActivityDto for profile activity listings.
            CreateMap<ActivityAttendee, Profiles.UserActivityDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Activity.Id))
                .ForMember(d => d.Title, o => o.MapFrom(s => s.Activity.Title))
                .ForMember(d => d.Category, o => o.MapFrom(s => s.Activity.Category))
                .ForMember(d => d.Date, o => o.MapFrom(s => s.Activity.Date))
                .ForMember(d => d.HostUsername, o => o.MapFrom(s =>
                    s.Activity.Attendees.FirstOrDefault(x => x.IsHost).AppUser.UserName));
        }
    }
}
