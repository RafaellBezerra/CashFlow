using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;

namespace CashFlow.Application.AutoMapper;
public class AutoMapping : Profile
{
    public AutoMapping()
    {
        MapRequestToEntity();
        MapEntityToResponse();
    }
    private void MapRequestToEntity()
    {
        CreateMap<RequestRegisterUser, User>().ForMember(dest => dest.Password, config => config.Ignore());

        CreateMap<RequestExpense, Expense>()
            .ForMember(dest => dest.Tags, config => config.MapFrom(src => src.Tags.Distinct()));

        CreateMap<Communication.Enum.Tag, Tag>()
            .ForMember(dest => dest.TagValue, config => config.MapFrom(src => src));
    }
    private void MapEntityToResponse()
    {
        CreateMap<Expense, ResponseRegisterExpense>();
        CreateMap<Expense, ResponseShortGetAllExpense>();

        CreateMap<Expense, ResponseGetById>()
            .ForMember(dest => dest.Tags, config => config.MapFrom(src => src.Tags.Select(tag => tag.TagValue)));

        CreateMap<User, ResponseUserProfile>();
    }
}
