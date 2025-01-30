using System.Text.Json;
using ConverterAPI.Models;
using StackExchange.Redis;

namespace ConverterAPI.Services;

public class SessionService(IConnectionMultiplexer redis)
{
    private readonly IDatabase _db = redis.GetDatabase();
    private readonly TimeSpan _sessionLifeTime = TimeSpan.FromHours(24); // Sessions TTL

    public async Task<List<Session>> GetSessionsAsync()
    {
        var keys = await _db.SetMembersAsync("sessions"); // Get all keys

        if (keys.Length == 0)
        {
            return [];
        }
        
        var redisKeys = keys.Select(k => (RedisKey)k.ToString()).ToArray();
        
        var values = await _db.StringGetAsync(redisKeys);
        return values.Where(v => !v.IsNullOrEmpty)
            .Select(v => JsonSerializer.Deserialize<Session>(v!))
            .Where(s => s != null)
            .ToList()!;
    }

    public async Task<Session?> GetSessionByUserIdAsync(int userId)
    {
        var sessionKey = $"session:{userId}";
        string? sessionData = await _db.StringGetAsync(sessionKey);
        
        return sessionData != null ? JsonSerializer.Deserialize<Session>(sessionData) : null;
    }
    
    public async Task CreateSessionAsync(Session session)
    {
        var sessionKey = $"session:{session.userid}";
        var sessionData = JsonSerializer.Serialize(session);
        
        await _db.StringSetAsync(sessionKey, sessionData, _sessionLifeTime);
        await _db.SetAddAsync("sessions", sessionKey); // Add key in set for fast GetSessions
    }

    public async Task<bool> UpdateSessionAsync(int userId, Action<Session> updateAction)
    {
        var sessionKey = $"session:{userId}";
        var sessionData = await _db.StringGetAsync(sessionKey);

        if (sessionData.IsNullOrEmpty)
        {
            return false;
        }
        
        var session = JsonSerializer.Deserialize<Session>(sessionData!);
        if (session == null)
        {
            return false;
        }
        
        updateAction(session);
        
        await _db.StringSetAsync(sessionKey, JsonSerializer.Serialize(session));
        return true;
    }

    public async Task<bool> DeleteSessionAsync(int userId)
    {
        string sessionKey = $"session:{userId}";
        
        await _db.KeyDeleteAsync(sessionKey);
        return await _db.SetRemoveAsync("sessions", sessionKey);
    }
}