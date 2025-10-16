using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using School.Application.Common.Exceptions;
using School.Application.Features.Auth.DTOs;
using School.Application.Features.Email.Dtos;
using School.Application.Interfaces.Repositories;
using School.Application.Interfaces.Services;
using School.Domain.Entities;
using School.Infrastructure.Identity;
using School.Infrastructure.Implementation.Specifications.StudentModuleSpecifications;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace School.Infrastructure.Implementation.Services
{
    public class AuthenticationService(UserManager<ApplicationUser> _userManager,
                                       IConfiguration _configuration,
                                       IUnitOfWork _unitOfWork,
                                       IServiceManager _serviceManager) : IAuthenticationService
    {

        public async Task<string> GetUserIdByStudentIdAsync(int studentId)
        {
            // Find the user by studentId
            var studentRepo = _unitOfWork.GetRepository<Student, int>();
            var student = await studentRepo.GetByIdAsync(studentId)
                                   ?? throw new StudentNotFoundException(studentId);

            var user = await _userManager.FindByEmailAsync(student.Email)
                ?? throw new UserNotFoundException(student.Email);

            return user.Id;
        }
        public async Task<UserDto> GetCurrentUserAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email)
                        ?? throw new UserNotFoundException(email);

            var roles = await _userManager.GetRolesAsync(user);

            var studentInfo = await GetStudentInfoAsync(user, roles);

            return new UserDto()
            {
                UserId = user.Id,
                Email = user.Email!,
                DisplayName = user.DisplayName,
                Token = await GenerateJwtTokenAsync(user),
                Roles = roles,
                StudentInfo = studentInfo
            };
        }

        public async Task<UserDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email)
                             ?? throw new UserNotFoundException(loginDto.Email);

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!isPasswordValid)
                throw new UnauthorizedException();

            if (user.IsFirstLogin)
            {
                var mailMessage = new MailMessageDto()
                {
                    To = user.Email!,
                    Subject = "Welcome to School Management System",
                    Body = $"Welcome {user.DisplayName}, you have successfully logged in for the first time."
                };
                await _serviceManager.MailService.SendEmailAsync(mailMessage);

                user.IsFirstLogin = false;
                await _userManager.UpdateAsync(user);
            }

            var roles = await _userManager.GetRolesAsync(user);

            var studentInfo = await GetStudentInfoAsync(user, roles);

            return new UserDto()
            {
                UserId = user.Id,
                Email = user.Email ?? string.Empty,
                DisplayName = user.DisplayName,
                Token = await GenerateJwtTokenAsync(user),
                Roles = roles,
                StudentInfo = studentInfo
            };
        }

        public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
        {
            var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existingUser != null)
                throw new BadRequestException(new List<string> { "Email is already registered" });
            
            var existingUserName = await _userManager.FindByNameAsync(registerDto.UserName);
            if(existingUserName != null)
                throw new BadRequestException(new List<string> { "Username is already taken" });

            var user = new ApplicationUser()
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                PhoneNumber = registerDto.Phone,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if(!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                throw new BadRequestException(errors);
            }

            var roles = await _userManager.GetRolesAsync(user);

            return new UserDto()
            {
                UserId = user.Id,
                Email = registerDto.Email,
                DisplayName = registerDto.DisplayName,
                Token = await GenerateJwtTokenAsync(user),
                Roles = roles,
                StudentInfo = null
            };
        }

        public async Task<StudentAccountDto> CreateStudentAccountAsync(
           CreateStudentAccountDto createDto,
           string adminUserId)
        {
            var admin = await _userManager.FindByIdAsync(adminUserId);
            if (admin == null)
                throw new UnauthorizedException();

            var adminRoles = await _userManager.GetRolesAsync(admin);
            if (!adminRoles.Contains("Admin"))
                throw new UnauthorizedException();

            var existingUser = await _userManager.FindByEmailAsync(createDto.Email);
            if (existingUser != null)
                throw new BadRequestException(new List<string> { "Email is already registered" });

            var existingUsername = await _userManager.FindByNameAsync(createDto.UserName);
            if (existingUsername != null)
                throw new BadRequestException(new List<string> { "Username is already taken" });

            var temporaryPassword = createDto.Password ?? GenerateSecurePassword();

            var student = new Student
            {
                Name = createDto.Name,
                Email = createDto.Email,
                ClassId = createDto.ClassId
            };

            await _unitOfWork.GetRepository<Student, int>().AddAsync(student);
            await _unitOfWork.SaveChangesAsync();

            var user = new ApplicationUser
            {
                DisplayName = createDto.Name,
                Email = createDto.Email,
                UserName = createDto.UserName,
                PhoneNumber = createDto.PhoneNumber,
                StudentId = student.Id,
                EmailConfirmed = true,
                IsFirstLogin = true
            };

            var result = await _userManager.CreateAsync(user, temporaryPassword);

            if (!result.Succeeded)
            {
                _unitOfWork.GetRepository<Student, int>().Delete(student);
                await _unitOfWork.SaveChangesAsync();

                var errors = result.Errors.Select(e => e.Description).ToList();
                throw new BadRequestException(errors);
            }

            var roleResult = await _userManager.AddToRoleAsync(user, "Student");

            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                _unitOfWork.GetRepository<Student, int>().Delete(student);
                await _unitOfWork.SaveChangesAsync();

                var roleErrors = roleResult.Errors.Select(e => e.Description).ToList();
                throw new BadRequestException(roleErrors);
            }

            return new StudentAccountDto
            {
                UserId = user.Id,
                StudentId = student.Id,
                Name = student.Name,
                Email = user.Email!,
                UserName = user.UserName!,
                TemporaryPassword = temporaryPassword,
                ClassId = student.ClassId,
                CreatedBy = admin.DisplayName
            };
        }

        private string GenerateSecurePassword()
        {
            const string uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowercase = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string special = "@#$%";

            var random = RandomNumberGenerator.Create();
            var password = new StringBuilder();

            password.Append(uppercase[GetRandomNumber(random, uppercase.Length)]);
            password.Append(lowercase[GetRandomNumber(random, lowercase.Length)]);
            password.Append(digits[GetRandomNumber(random, digits.Length)]);
            password.Append(special[GetRandomNumber(random, special.Length)]);

            var allChars = uppercase + lowercase + digits + special;
            for (int i = 4; i < 10; i++)
            {
                password.Append(allChars[GetRandomNumber(random, allChars.Length)]);
            }

            return new string(password.ToString().OrderBy(c => GetRandomNumber(random, 100)).ToArray());
        }

        private int GetRandomNumber(RandomNumberGenerator random, int max)
        {
            byte[] randomBytes = new byte[4];
            random.GetBytes(randomBytes);
            return Math.Abs(BitConverter.ToInt32(randomBytes, 0)) % max;
        }

        private async Task<StudentInfoDto?> GetStudentInfoAsync(ApplicationUser user,IList<string> roles)
        {
            if (!roles.Contains("Student"))
                return null;

            if (user.StudentId.HasValue)
            {
                var spec = new StudentWithClassSpecification(user.StudentId.Value);
                var student = await _unitOfWork.GetRepository<Student,int>().GetByIdAsync(spec)
                                           ?? throw new StudentNotFoundException(user.StudentId.Value);

                return new StudentInfoDto()
                {
                    StudentId = student.Id,
                    Name = student.Name,
                    Email = user.Email!,
                    ClassId = student.ClassId
                };
            }

            var studentByEmail = await _unitOfWork.GetRepository<Student,int>()
                                                 .GetByIdAsync(new StudentByEmailSpecification(user.Email!));

            if (studentByEmail != null)
            {
                user.StudentId = studentByEmail.Id;
                await _userManager.UpdateAsync(user);

                return new StudentInfoDto()
                {
                    StudentId = studentByEmail.Id,
                    Name = studentByEmail.Name,
                    Email = user.Email!,
                    ClassId = studentByEmail.ClassId
                };
            }

            return null;
        }

        private async Task<string> GenerateJwtTokenAsync(ApplicationUser user)
        {
            var claims = new List<Claim>()
            {
                new (ClaimTypes.NameIdentifier, user.Id),
                new (ClaimTypes.Email, user.Email!),
                new (ClaimTypes.Name, user.UserName!)
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach(var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            if(user.StudentId.HasValue)
                claims.Add(new Claim("StudentId", user.StudentId.Value.ToString()));

            var secretKey = _configuration["JwtSettings:SecretKey"]!;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(8),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
