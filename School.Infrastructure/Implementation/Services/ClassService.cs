using AutoMapper;
using School.Application.Common.Exceptions;
using School.Application.Common.Models;
using School.Application.Common.Models.QueryParams;
using School.Application.Features.Classes.DTOs;
using School.Application.Interfaces.Repositories;
using School.Application.Interfaces.Services;
using School.Domain.Entities;
using School.Infrastructure.Implementation.Specifications.ClassModuleSpecifications;

namespace School.Infrastructure.Implementation.Services
{
    public class ClassService(IUnitOfWork _unitOfWork,
                              IMapper _mapper) : IClassService
    {
        public async Task<PaginatedResult<ClassDto>> GetAllAsync(ClassQueryParams queryParams)
        {
            var spec = new ClassSpecification(queryParams);
            var classes = await _unitOfWork.GetRepository<Class,int>().GetAllAsync(spec);
            var classesCount = classes.Count();
            var classDtos = _mapper.Map<IEnumerable<ClassDto>>(classes);
            var countSpec = new ClassCountSpecification(queryParams);
            var TotalCount = await _unitOfWork.GetRepository<Class,int>().CountAsync(countSpec);
            return new PaginatedResult<ClassDto>(queryParams.PageIndex,classesCount, TotalCount, classDtos);
        }

        public async Task<ClassDto> GetByIdAsync(int id)
        {
            var spec = new ClassSpecification(id);
            var classEntity = await _unitOfWork.GetRepository<Class,int>().GetByIdAsync(spec)
                                         ?? throw new ClassNotFoundException(id);

            return _mapper.Map<ClassDto>(classEntity);
        }

        public async Task<ClassDto> CreateAsync(CreateClassDto createClassDto)
        {
            var classEntity = _mapper.Map<CreateClassDto,Class>(createClassDto);

            await _unitOfWork.GetRepository<Class,int>().AddAsync(classEntity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<Class,ClassDto>(classEntity);    
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var classEntity = await _unitOfWork.GetRepository<Class,int>().GetByIdAsync(id)
                                         ?? throw new ClassNotFoundException(id);

            _unitOfWork.GetRepository<Class,int>().Delete(classEntity);
            var result  = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }

        public async Task<ClassDto> UpdateAsync(UpdateClassDto updateClassDto)
        {
            var existingClass = await _unitOfWork.GetRepository<Class,int>().GetByIdAsync(updateClassDto.Id)
                                              ?? throw new ClassNotFoundException(updateClassDto.Id);

            _mapper.Map(updateClassDto, existingClass);
            _unitOfWork.GetRepository<Class,int>().Update(existingClass);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<Class,ClassDto>(existingClass);
        }
    }
}
