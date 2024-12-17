using AutoMapper;
using NZWalks.Models.Domain;
using NZWalks.Models.DTO;

namespace NZWalks.Mappings
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Region, RegionDto>().ReverseMap();
            CreateMap<Region,AddRegionRequestDto>().ReverseMap();
            CreateMap<Region,UpdateRegionRequestDto>().ReverseMap();
            CreateMap<Walk,WalkDto>().ReverseMap();
            CreateMap<Walk,AddWalkRequestDto>().ReverseMap();
            CreateMap<Walk,UpdateWalkRequestDto>().ReverseMap();
        }
    }
}
