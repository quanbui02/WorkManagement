﻿namespace WorkManagement.Model
{
    public class JwtSettings
    {
        public string DeliverySecret { get; set; }
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpiresInMinutes { get; set; }
        public int RefreshTokenExpiresInDays { get; set; }
    }
}
