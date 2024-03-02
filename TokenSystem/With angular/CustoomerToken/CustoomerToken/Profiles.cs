using AutoMapper;
using CustoomerToken.Models;

public class Profiles : Profile
{
    public Profiles()
    {
        CreateMap<Token, TokenListModel>()
            .ForMember(x => x.Status, opt => opt.MapFrom(src => src.StatusId))
            .ForMember(x => x.Query, opt => opt.MapFrom(src => src.QueryId))
            .ReverseMap()
            .ForMember(x => x.QueryId, opt => opt.MapFrom(src => src.Query))
            .ForMember(x => x.StatusId, opt => opt.MapFrom(src => src.Status));

        CreateMap(typeof(PaginationResult<>), typeof(PaginationResult<>));
    }
}