using System.Security.Claims;
using System.Text.Json;
using WorkManagement.Models;
using WorkManagement.Services.Clients;

namespace WorkManagement.Services.AI
{
    public interface IAIAsissTantService
    {
        Task<string> HandleAsync(string message, ClaimsPrincipal user);
    }
    public class AIAsissTantService : IAIAsissTantService
    {
        private readonly HttpClient _http;
        private readonly IWmUsersService _wmUsersService;

        public AIAsissTantService(IHttpClientFactory factory,IWmUsersService wmUsersService)
        {
            _http = factory.CreateClient();
            _wmUsersService = wmUsersService;
            _http.Timeout = TimeSpan.FromSeconds(15);
        }


        public async Task<string> HandleAsync(string message, ClaimsPrincipal user)
        {
            var mode = await DetectMode(message);

            if (mode == "CHAT")
            {
                return await ChatNaturally(message);
            }

            // TOOL MODE
            var intent = await ExtractIntent(message);
            if (intent == null || intent.Intent == "UNKNOWN")
                return "Tôi chưa hiểu yêu cầu, bạn có thể nói rõ hơn không?";

            return await HandleTool(intent, user);
        }

        private async Task<string> DetectMode(string message)
        {
            var prompt = $@"
                Bạn là bộ phân loại yêu cầu.
                Chỉ trả JSON.

                Schema:
                {{ ""mode"": ""CHAT"" | ""TOOL"" }}

                Quy tắc:
                - Chào hỏi, nói chuyện, hỏi chung chung → CHAT
                - Hỏi thông tin user, tìm user, xem chi tiết → TOOL

                Câu người dùng:
                {message}
                ";

            var raw = await CallOllama(prompt);
            var json = ExtractJson(raw);

            try
            {
                using var doc = JsonDocument.Parse(json);
                return doc.RootElement.GetProperty("mode").GetString() ?? "CHAT";
            }
            catch
            {
                return "CHAT"; // fail-safe
            }
        }

        private async Task<string> ChatNaturally(string message)
        {
            var prompt = $@"
                            Bạn là trợ lý AI thân thiện trong hệ thống nội bộ.
                            Trả lời tự nhiên, ngắn gọn, tiếng Việt.
                            Không trả JSON.

                            Câu người dùng:
                            {message}
                            ";

            return await CallOllama(prompt);
        }

        private async Task<AiIntentResultUserInfo?> ExtractIntent(string message)
        {
            var prompt = $@"
                            Bạn là AI hỗ trợ hệ thống quản lý người dùng.
                            Chỉ trả JSON, không giải thích.

                            Schema:
                            {{
                              intent: ""GET_USER_DETAIL | SEARCH_USER_BY_NAME | UNKNOWN"",
                              userId: number | null,
                              name: string | null
                            }}

                            Quy tắc:
                            - Hỏi theo ID → GET_USER_DETAIL
                            - Hỏi theo tên → SEARCH_USER_BY_NAME
                            - Không rõ → UNKNOWN

                            Câu người dùng:
                            {message}
                            ";

            var raw = await CallOllama(prompt);
            var json = ExtractJson(raw);

            return ParseIntent(json);
        }

        private async Task<string> HandleTool(AiIntentResultUserInfo intent, ClaimsPrincipal user)
        {
            switch (intent.Intent)
            {
                case "GET_USER_DETAIL":
                    return await HandleGetUserDetail(intent);

                case "SEARCH_USER_BY_NAME":
                    return await HandleSearchUserByName(intent);

                default:
                    return "Không hiểu yêu cầu";
            }
        }

        private string ExtractJson(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return raw;

            var start = raw.IndexOf('{');
            var end = raw.LastIndexOf('}');
            if (start >= 0 && end > start)
                return raw.Substring(start, end - start + 1);

            return raw;
        }

        private string BuildPrompt(string userMessage)
        {
            return $@"
                        Bạn là AI hỗ trợ hệ thống quản lý người dùng.
                        Chỉ trả JSON, không giải thích.

                        Schema:
                        {{
                          intent: ""GET_USER_DETAIL | SEARCH_USER_BY_NAME | UNKNOWN"",
                          userId: number | null,
                          name: string | null
                        }}

                        Quy tắc:
                        - Nếu người dùng hỏi user theo ID → GET_USER_DETAIL
                        - Nếu người dùng hỏi theo TÊN → SEARCH_USER_BY_NAME
                        - name là phần tên người dùng hỏi (ví dụ: Quân)

                        Câu người dùng:
                        {userMessage}
                        ";
        }

        private async Task<string> CallOllama(string prompt)
        {
            var payload = new
            {
                model = "qwen2.5:1.5b",
                prompt = prompt,
                stream = false
            };

            var res = await _http.PostAsJsonAsync(
                "http://localhost:11434/api/generate",
                payload
            );

            var json = await res.Content.ReadFromJsonAsync<OllamaResponse>();
            return json?.response;
        }

        private AiIntentResultUserInfo ParseIntent(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<AiIntentResultUserInfo>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }
            catch
            {
                return null;
            }
        }

        private async Task<string> HandleGetUserDetail(AiIntentResultUserInfo intent)
        {
            if (!intent.UserId.HasValue)
                return "Bạn chưa cung cấp user id.";

            var result = await _wmUsersService.GetDetail(intent.UserId.Value);

            // Vì GetDetail đã trả Result<object>
            return JsonSerializer.Serialize(result);
        }
        
        private async Task<string> HandleSearchUserByName(AiIntentResultUserInfo intent)
        {
            if (string.IsNullOrWhiteSpace(intent.Name))
                return "Bạn muốn tìm user tên gì?";

            var users = await _wmUsersService.SearchByName(intent.Name);

            if (users.Count == 0)
                return $"Không tìm thấy user tên {intent.Name}";

            if (users.Count == 1)
            {
                var user = users.First();
                return JsonSerializer.Serialize(user);
            }

            // >1 user
            return JsonSerializer.Serialize(new
            {
                message = $"Có {users.Count} người tên {intent.Name}",
                users
            });
        }

        private int GetUserId(ClaimsPrincipal user)
        {
            return int.Parse(user.FindFirst("UserId")!.Value);
        }
    }
}
