using AutoMapper;
using EmployeeManagement.Data;
using EmployeeManagement.Models;

namespace EmployeeManagement.Config
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EmployeeDetail,EmployeeListModel>().ReverseMap();
            CreateMap<Designation,DesignationModel>().ReverseMap();
        }
    }
}
