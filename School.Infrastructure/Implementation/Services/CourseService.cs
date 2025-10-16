using AutoMapper;
using School.Application.Common.Exceptions;
using School.Application.Common.Models;
using School.Application.Common.Models.QueryParams;
using School.Application.Features.Courses.DTOs;
using School.Application.Interfaces.Repositories;
using School.Application.Interfaces.Services;
using School.Domain.Entities;
using School.Infrastructure.Implementation.Specifications.CourseModuleSpecifications;

namespace School.Infrastructure.Implementation.Services
{
    public class CourseService(IUnitOfWork _unitOfWork,
                               IMapper _mapper) : ICourseService
    {
        public async Task<PaginatedResult<CourseDto>> GetAllAsync(CourseQueryParams queryParams)
        {
            var spec = new CourseSpecification(queryParams);
            var classes = await _unitOfWork.GetRepository<Course,int>().GetAllAsync(spec);
            var classesCount = classes.Count();
            var classesDto = _mapper.Map<IEnumerable<CourseDto>>(classes);
            var countSpec = new CoureCountSpecification(queryParams);
            var totalCount = await _unitOfWork.GetRepository<Course,int>().CountAsync(countSpec);
            return new PaginatedResult<CourseDto>(queryParams.PageIndex,classesCount,totalCount,classesDto);
        }

        public async Task<CourseDto> GetByIdAsync(int id)
        {
            var spec = new CourseSpecification(id);
            var course = await _unitOfWork.GetRepository<Course,int>().GetByIdAsync(spec)
                                         ?? throw new CourseNotFoundException(id);

            return _mapper.Map<CourseDto>(course);
        }
        public async Task<CourseDto> CreateAsync(CreateCourseDto courseDto)
        {
            var course = _mapper.Map<CreateCourseDto,Course>(courseDto);

            await _unitOfWork.GetRepository<Course,int>().AddAsync(course);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<Course,CourseDto>(course);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var course =  await _unitOfWork.GetRepository<Course,int>().GetByIdAsync(id)
                                         ?? throw new CourseNotFoundException(id);

            _unitOfWork.GetRepository<Course,int>().Delete(course);
            var result  = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }


        public async Task<CourseDto> UpdateAsync(UpdateCourseDto courseDto)
        {
            var existingCourse =  await _unitOfWork.GetRepository<Course,int>().GetByIdAsync(courseDto.Id)
                                              ?? throw new CourseNotFoundException(courseDto.Id);

            _mapper.Map(courseDto, existingCourse);

            _unitOfWork.GetRepository<Course,int>().Update(existingCourse);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<Course,CourseDto>(existingCourse);
        }
    }
}
