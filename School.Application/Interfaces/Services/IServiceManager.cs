namespace School.Application.Interfaces.Services
{
    public interface IServiceManager
    {
        public IAuthenticationService AuthenticationService { get; }  
        public IStudentService StudentService { get; }

        public IClassService ClassService { get; }

        public ICourseService CourseService { get; }

        public IChatService ChatService { get; }

        public IChatNotificationService ChatNotificationService { get; }
        
        public IFirebaseNotificationService FirebaseNotificationService { get; }

        public IMailService MailService { get; }
    }
}
