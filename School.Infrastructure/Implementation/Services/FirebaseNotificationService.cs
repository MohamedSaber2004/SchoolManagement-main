using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Firebase.Database;
using Firebase.Database.Query;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using School.Application.Interfaces.Services;
using School.Infrastructure.Identity;
using System.Text.Json;

namespace School.Infrastructure.Implementation.Services
{
    public class FirebaseNotificationService : IFirebaseNotificationService
    {
        private readonly FirebaseApp _firebaseApp;
        private readonly FirebaseClient _realtimeDb;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<FirebaseNotificationService> _logger;

        public FirebaseNotificationService
            (IConfiguration _configuration,
             UserManager<ApplicationUser> _userManager,
             ILogger<FirebaseNotificationService> _logger)
        {
            this._logger = _logger;
            this._userManager = _userManager;

            try
            {
                var firebaseSettings = _configuration.GetSection("FirebaseSettings");
                var projectId = firebaseSettings["ProjectId"];
                var databaseUrl = firebaseSettings["DatabaseUrl"];

                var credentialJson = JsonSerializer.Serialize(new
                {
                    type = "service_account",
                    project_id = projectId,
                    private_key_id = firebaseSettings["PrivateKeyId"],
                    private_key = firebaseSettings["PrivateKey"],
                    client_email = firebaseSettings["ClientEmail"],
                    client_id = firebaseSettings["ClientId"],
                    auth_uri = "https://accounts.google.com/o/oauth2/auth",
                    token_uri = firebaseSettings["TokenUri"],
                    auth_provider_x509_cert_url = "https://www.googleapis.com/oauth2/v1/certs",
                    client_x509_cert_url = $"https://www.googleapis.com/robot/v1/metadata/x509/{firebaseSettings["ClientEmail"]}"
                });

                var credential = GoogleCredential.FromJson(credentialJson);

                _firebaseApp = FirebaseApp.DefaultInstance ?? FirebaseApp.Create(new AppOptions()
                {
                    Credential = credential
                });

                _realtimeDb = new FirebaseClient(databaseUrl);

                _logger.LogInformation("Firebase initialized successfully for project: {ProjectId}", projectId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing Firebase");
                throw;
            }
        }

        public async Task SendChatNotificationAsync(string receiverUserId, string senderUserId, string senderName, string message)
        {
            _logger.LogInformation("Sending chat notification to user: {ReceiverId} from {SenderId}", receiverUserId, senderUserId);

            try
            {
                var user = await _userManager.FindByIdAsync(receiverUserId);
                if (user is null)
                {
                    _logger.LogWarning("User {UserId} not found", receiverUserId);
                    return;
                }

                var title = $"New message from {senderName}";
                var data = new Dictionary<string, string>()
                {
                    {"type", "chat_message" },
                    {"senderId", senderUserId },
                    {"senderName", senderName },
                    {"timestamp", DateTime.UtcNow.ToString("O")}
                };

                var storedInDatabase = await StoreNotificationInRealtimeDbAsync(
                    receiverUserId,
                    title,
                    message,
                    "chat_message",
                    data
                );

                if (storedInDatabase)
                    _logger.LogInformation("Notification stored in Realtime Database");

                if (!string.IsNullOrEmpty(user.FirebaseDeviceToken))
                {
                    _logger.LogInformation("Sending FCM push notification");

                    var fcmMessageId = await SendNotificationAsync(
                        user.FirebaseDeviceToken,
                        title,
                        message,
                        data
                    );

                    if (!string.IsNullOrEmpty(fcmMessageId))
                    {
                        _logger.LogInformation("Push notification sent: {MessageId}", fcmMessageId);
                    }
                }
                else
                    _logger.LogWarning("User {UserId} has no device token - notification stored but not pushed", receiverUserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending chat notification to user {UserId}", receiverUserId);
            }
        }

        public async Task<string?> SendNotificationAsync(string deviceToken, string title, string body, Dictionary<string, string>? data = null)
        {
            if (string.IsNullOrEmpty(deviceToken))
            {
                _logger.LogWarning("Device token is null or empty");
                return null;
            }

            try
            {
                _logger.LogInformation("Sending FCM notification - Title: '{Title}'", title);

                var message = new Message()
                {
                    Token = deviceToken,
                    Notification = new Notification()
                    {
                        Title = title,
                        Body = body
                    },
                    Data = data ?? new Dictionary<string, string>(),
                    Android = new AndroidConfig()
                    {
                        Priority = Priority.High,
                        Notification = new AndroidNotification()
                        {
                            Sound = "default",
                            ChannelId = "chat_messages",
                            ClickAction = "FLUTTER_NOTIFICATION_CLICK"
                        }
                    },
                    Apns = new ApnsConfig()
                    {
                        Aps = new Aps()
                        {
                            Sound = "default",
                            Badge = 1,
                            Alert = new ApsAlert
                            {
                                Title = title,
                                Body = body
                            }
                        }
                    },
                    Webpush = new WebpushConfig()
                    {
                        Notification = new WebpushNotification()
                        {
                            Title = title,
                            Body = body,
                            Icon = "/icon-192x192.png"
                        }
                    }
                };

                var response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                _logger.LogInformation("FCM notification sent successfully: {Response}", response);

                return response;
            }
            catch (FirebaseMessagingException ex)
            {
                _logger.LogError(ex, "FCM Error. Code: {ErrorCode}, Message: {Message}",
                    ex.MessagingErrorCode, ex.Message);

                if (ex.MessagingErrorCode == MessagingErrorCode.Unregistered)
                    _logger.LogWarning("Device token is invalid or expired");

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error sending FCM notification");
                return null;
            }
        }

        public async Task<bool> StoreNotificationInRealtimeDbAsync(string userId, string title, string body, string type, Dictionary<string, string>? data = null)
        {
            try
            {
                _logger.LogInformation("Storing notification in Realtime Database for user: {UserId}", userId);

                var notificationData = new
                {
                    title = title,
                    body = body,
                    type = type,
                    createdAt = DateTime.UtcNow.ToString("O"),
                    isRead = false,
                    userId = userId,
                    senderId = data?.ContainsKey("senderId") == true ? data["senderId"] : "",
                    senderName = data?.ContainsKey("senderName") == true ? data["senderName"] : "",
                    data = data ?? new Dictionary<string, string>()
                };

                await _realtimeDb
                    .Child("notifications")
                    .Child(userId)
                    .Child("userNotifications")
                    .PostAsync(notificationData);

                _logger.LogInformation("Notification stored in Realtime Database successfully");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error storing notification in Realtime Database");
                return false;
            }
        }

        public async Task<IEnumerable<object>> GetUserNotificationsFromRealtimeDbAsync(string userId)
        {
            try
            {
                _logger.LogInformation("Retrieving notifications for user: {UserId}", userId);

                var notifications = await _realtimeDb
                    .Child("notifications")
                    .Child(userId)
                    .Child("userNotifications")
                    .OrderByKey()
                    .LimitToLast(50)
                    .OnceAsync<Dictionary<string, object>>();

                var result = notifications
                    .Select(n => new Dictionary<string, object>(n.Object)
                    {
                        ["notificationId"] = n.Key
                    })
                    .Reverse()
                    .ToList();

                _logger.LogInformation("Retrieved {Count} notifications", result.Count);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving notifications from Realtime Database");
                return Enumerable.Empty<object>();
            }
        }

        public async Task<bool> MarkNotificationAsReadAsync(string userId, string notificationId)
        {
            try
            {
                _logger.LogInformation("Marking notification as read: {NotificationId}", notificationId);

                await _realtimeDb
                    .Child("notifications")
                    .Child(userId)
                    .Child("userNotifications")
                    .Child(notificationId)
                    .PatchAsync(new
                    {
                        isRead = true,
                        readAt = DateTime.UtcNow.ToString("O")
                    });

                _logger.LogInformation("Notification marked as read");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking notification as read");
                return false;
            }
        }
    }
}