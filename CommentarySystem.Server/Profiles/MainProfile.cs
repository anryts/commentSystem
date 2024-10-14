using AutoMapper;
using CommentarySystem.Server.Data.Entities;
using CommentarySystem.Server.Model;
using File = CommentarySystem.Server.Data.Entities.File;

namespace CommentarySystem.Server.Profiles;

public class MainProfile : Profile
{
    public MainProfile()
    {
        CreateMap<Comment, CommentResponseModel>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
            .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.Replies, opt => opt.MapFrom(src => src.ChildComments))
            .ForMember(dest => dest.Files, opt => opt.MapFrom(src => src.Files));

        CreateMap<File, FileResponse>()
            .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.FileName))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
            .ForMember(dest => dest.CommentId, opt => opt.MapFrom(src => src.CommentId))
            .ForMember(dest => dest.FileType, opt => opt.MapFrom(src => src.FileType));
    }
}