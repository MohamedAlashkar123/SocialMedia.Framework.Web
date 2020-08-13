using AutoMapper;
using SocialMedia.Framework.Core;
using SocialMedia.Framework.Core.Logging;
using SocialMedia.Framework.Core.Login;
using SocialMedia.Framework.ViewModel;

namespace SocialMedia.Framework.Utilities.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserViewModel>();
            CreateMap<UserViewModel, User>();

            CreateMap<Post, PostViewModel>();
            CreateMap<PostViewModel, Post>();

            CreateMap<Like, LikeViewModel>();
            CreateMap<LikeViewModel, Like>();

            CreateMap<Deslike, DeslikeViewModel>();
            CreateMap<DeslikeViewModel, Deslike>();

            CreateMap<Friendship, FriendshipViewModel>();
            CreateMap<FriendshipViewModel, Friendship>();

            CreateMap<User, UserViewModel>();
            CreateMap<UserViewModel, User>();

            CreateMap<RegisterViewModel, User>();
            CreateMap<UpdateViewModel, User>();

            CreateMap<Log, LogViewModel>();
            CreateMap<LogViewModel, Log>();
        }
    }
}