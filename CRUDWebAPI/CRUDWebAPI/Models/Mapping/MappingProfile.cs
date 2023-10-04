using AutoMapper;

namespace CRUDWebAPI.Models.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        { 
            this.CreateMap<Event, EventDto>();
            this.CreateMap<EventDto, Event>();
        }
    }
}
