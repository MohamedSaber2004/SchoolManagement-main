using School.Application.Interfaces.Services;

namespace School.Infrastructure.Implementation.Services
{
    public class ServiceManager(Func<IAuthenticationService> authenticationServiceFactory,
                                Func<IStudentService> _studentServiceFactory,
                                Func<IClassService> _classServiceFactory,
                                Func<ICourseService> _courseServiceFactory,
                                Func<IChatService> _chatServiceFactory,
                                Func<IChatNotificationService> _chatNotificationServiceFactory,
                                Func<IFirebaseNotificationService> _firebaseNotificationServiceFactory,
                                Func<IMailService> _mailServiceFactory) : IServiceManager
    {
        public IAuthenticationService AuthenticationService => authenticationServiceFactory.Invoke();

        public IStudentService StudentService => _studentServiceFactory.Invoke();

        public IClassService ClassService => _classServiceFactory.Invoke();

        public ICourseService CourseService => _courseServiceFactory.Invoke();

        public IChatService ChatService => _chatServiceFactory.Invoke();

        public IChatNotificationService ChatNotificationService => _chatNotificationServiceFactory.Invoke();

        public IFirebaseNotificationService FirebaseNotificationService => _firebaseNotificationServiceFactory.Invoke();

        public IMailService MailService => _mailServiceFactory.Invoke();
    }
}
