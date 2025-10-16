using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using School.Application.Common.Exceptions;
using School.Application.Interfaces;
using School.Application.Interfaces.Repositories;
using School.Domain.Entities;
using School.Infrastructure.Data;
using School.Infrastructure.Identity;
using School.Infrastructure.Implementation.Specifications.StudentModuleSpecifications;
using System.Text.Json;

namespace School.Infrastructure.Implementation
{
    internal class DataSeed(SchoolDbContext_Identity _identityDbContext,
                            SchoolDbContext _dbContext,
                            UserManager<ApplicationUser> _userManager,
                            RoleManager<IdentityRole> _roleManager,
                            IUnitOfWork _unitOfWork) : IDataSeed
    {
        public async Task DataSeedAsync()
        {
            try
            {
                var pendingMigrations = await _identityDbContext.Database.GetPendingMigrationsAsync();
                var options = new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                };

                if((pendingMigrations).Any())
                    await _dbContext.Database.MigrateAsync();

                if(!await _dbContext.Set<Class>().AnyAsync())
                {
                    var classeJsonData = File.OpenRead(@"..\School.Infrastructure\Data\SeedData\classes.json");
                    var classeData = await JsonSerializer.DeserializeAsync<List<Class>>(classeJsonData, options);
                    if(classeData is not null && classeData.Any())
                    {
                        await _dbContext.Set<Class>().AddRangeAsync(classeData);
                    }
                }

                if(!await _dbContext.Set<Student>().AnyAsync())
                {
                    var studentJsonData = File.OpenRead(@"..\School.Infrastructure\Data\SeedData\students.json");
                    var studentData = await JsonSerializer.DeserializeAsync<List<Student>>(studentJsonData, options);
                    if(studentData is not null && studentData.Any())
                    {
                        await _dbContext.Set<Student>().AddRangeAsync(studentData);
                    }
                }

                if(!await _dbContext.Set<Course>().AnyAsync())
                {
                    var courseJsonData = File.OpenRead(@"..\School.Infrastructure\Data\SeedData\courses.json");
                    var courseData = await JsonSerializer.DeserializeAsync<List<Course>>(courseJsonData, options);
                    if(courseData is not null && courseData.Any())
                    {
                        await _dbContext.Set<Course>().AddRangeAsync(courseData);
                    }
                }

                if(!_dbContext.Set<Enrollment>().Any())
                {
                    var enrollmentJsonData = File.OpenRead(@"..\School.Infrastructure\Data\SeedData\enrollments.json");
                    var enrollmentData = await JsonSerializer.DeserializeAsync<List<Enrollment>>(enrollmentJsonData, options);
                    if(enrollmentData is not null && enrollmentData.Any())
                    {
                        await _dbContext.Set<Enrollment>().AddRangeAsync(enrollmentData);
                    }
                }

                await _dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task IdentityDataSeedAsync()
        {
            try
            {
                var pendingMigrations = await _identityDbContext.Database.GetPendingMigrationsAsync();

                if (pendingMigrations.Any())
                    await _identityDbContext.Database.MigrateAsync();

                if (!await _roleManager.Roles.AnyAsync())
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("Teacher"));
                    await _roleManager.CreateAsync(new IdentityRole("Student"));
                }

                if (!await _userManager.Users.AnyAsync())
                {
                    var user01 = new ApplicationUser()
                    {
                        DisplayName = "Ali Ahmed",
                        Email = "mmms77990@gmail.com",
                        PhoneNumber = "+201022223333",
                        UserName = "mmms77990"
                    };

                    var user02 = new ApplicationUser()
                    {
                        DisplayName = "Ahmed Mohamed",
                        Email = "mmms66880@gmail.com",
                        PhoneNumber = "+201011112222",
                        UserName = "mmms66880"
                    };

                    var user03 = new ApplicationUser()
                    {
                        DisplayName = "Mohamed Saber",
                        Email = "mmms11022@gmail.com",
                        PhoneNumber = "+201022812243",
                        UserName = "mmms11022"
                    };

                    await _userManager.CreateAsync(user01, "Mo@469200989");
                    await _userManager.CreateAsync(user02, "Mo@469200989");
                    await _userManager.CreateAsync(user03, "Mo@469200989");

                    await _userManager.AddToRoleAsync(user01, "Admin");
                    await _userManager.AddToRoleAsync(user02, "Teacher");
                    await _userManager.AddToRoleAsync(user03, "Student");

                    var studentEntity = await _unitOfWork.GetRepository<Student, int>()
                                                   .GetByIdAsync(new StudentByEmailSpecification(user03.Email));
                    if (studentEntity is not null)
                    {
                        user03.StudentId = studentEntity.Id;
                        await _userManager.UpdateAsync(user03);
                    }

                        await _identityDbContext.SaveChangesAsync();
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
