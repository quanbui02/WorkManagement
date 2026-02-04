namespace WorkManagement.Models
{
    public class AIModel
    {

    }

    public class AiIntentResult
    {
        public string Intent { get; set; }          // BOOK_MEETING_ROOM | BOOK_CAR | UNKNOWN
        public string RoomName { get; set; }         // "Phòng họp 1"
        public string CarName { get; set; }          // optional
        public string Date { get; set; }             // yyyy-MM-dd
        public string StartTime { get; set; }        // HH:mm
        public string EndTime { get; set; }          // HH:mm
    }
    public class AiIntentResultUserInfo
    {
        public string Intent { get; set; }   // GET_USER_DETAIL | UNKNOWN
        public int? UserId { get; set; }
        public string Name { get; set; }
    }
    public class OllamaResponse
    {
        public string response { get; set; }
    }

    public class AiChatRequest
    {
        public string Message { get; set; }
    }

    public class UserSearchDto
    {
        public string UserIdGuid { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }

    public class OllamaChatRequest
    {
        public string model { get; set; }
        public List<OllamaMessage> messages { get; set; }
        public bool stream { get; set; }
        public object options { get; set; }
    }

    public class OllamaMessage
    {
        public string role { get; set; }   // system | user | assistant
        public string content { get; set; }
    }

    public class OllamaChatResponse
    {
        public OllamaChatMessage message { get; set; }
    }

    public class OllamaChatMessage
    {
        public string content { get; set; }
    }

}
