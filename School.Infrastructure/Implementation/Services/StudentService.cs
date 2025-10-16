using AutoMapper;
using School.Application.Common.Exceptions;
using School.Application.Common.Models;
using School.Application.Common.Models.QueryParams;
using School.Application.Features.Students.DTOs;
using School.Application.Interfaces.Repositories;
using School.Application.Interfaces.Services;
using School.Domain.Entities;
using School.Infrastructure.Implementation.Specifications.StudentModuleSpecifications;

namespace School.Infrastructure.Implementation.Services
{
    public class StudentService(IUnitOfWork _unitOfWork,
                                IMapper _mapper) : IStudentService
    {
        public async Task<PaginatedResult<StudentDto>> GetAllAsync(StudentQueryParams queryParams)
        {
            var spec = new StudentWithClassSpecification(queryParams);
            var students = await _unitOfWork.GetRepository<Student, int>().GetAllAsync(spec);
            var studentsCount = students.Count();
            var studentDtos = _mapper.Map<IEnumerable<StudentDto>>(students);
            var countSpec = new StudentsCountSpecification(queryParams);
            var TotalCount = await _unitOfWork.GetRepository<Student, int>().CountAsync(countSpec);
            return new PaginatedResult<StudentDto>(queryParams.PageIndex, studentsCount, TotalCount, studentDtos);
        }

        public async Task<StudentDto> GetByIdAsync(int id)
        {
            var spec = new StudentWithClassSpecification(id);
            var student = await _unitOfWork.GetRepository<Student,int>().GetByIdAsync(spec)
                                    ?? throw new StudentNotFoundException(id);
            return _mapper.Map<StudentDto>(student);
        }
        public async Task<StudentDto> CreateAsync(CreateStudentDto createStudentDto)
        {
            var student = _mapper.Map<CreateStudentDto,Student>(createStudentDto);

            await _unitOfWork.GetRepository<Student,int>().AddAsync(student);
            await _unitOfWork.SaveChangesAsync();

            if (student.ClassId.HasValue)
            {
                var spec= new StudentWithClassSpecification(student.Id);
                student = await _unitOfWork.GetRepository<Student,int>().GetByIdAsync(spec) ?? student;
            }

            return _mapper.Map<Student,StudentDto>(student);
        }
        public async Task<StudentDto> UpdateAsync(UpdateStudentDto updateStudentDto)
        {
            var existingStudent = await _unitOfWork.GetRepository<Student, int>().GetByIdAsync(updateStudentDto.Id)
                                              ?? throw new StudentNotFoundException(updateStudentDto.Id);

            _mapper.Map(updateStudentDto, existingStudent);

            _unitOfWork.GetRepository<Student, int>().Update(existingStudent);
            await _unitOfWork.SaveChangesAsync();

            if (existingStudent.ClassId.HasValue)
            {
                var spec = new StudentWithClassSpecification(existingStudent.Id);
                existingStudent = await _unitOfWork.GetRepository<Student, int>().GetByIdAsync(spec) ?? existingStudent;
            }

            return _mapper.Map<StudentDto>(existingStudent);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var student = await _unitOfWork.GetRepository<Student, int>().GetByIdAsync(id)
                                  ?? throw new StudentNotFoundException(id);
            _unitOfWork.GetRepository<Student, int>().Delete(student);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }
    }
}
