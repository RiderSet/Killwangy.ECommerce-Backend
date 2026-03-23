using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Concurrent;
using System.Text;

namespace GBastos.Casa_dos_Farelos.AuthService.Infrastructure.Security;

public class KeyRotationService
{
    private readonly IConfiguration _configuration;

    // armazena chaves ativas
    private readonly ConcurrentDictionary<string, SecurityKey> _keys = new();

    // chave atual
    private string _currentKeyId = string.Empty;

    public KeyRotationService(IConfiguration configuration)
    {
        _configuration = configuration;
        Initialize();
    }

    private void Initialize()
    {
        var key = _configuration["Jwt:Key"]!;
        var keyId = Guid.NewGuid().ToString();

        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(key))
        {
            KeyId = keyId
        };

        _keys[keyId] = securityKey;
        _currentKeyId = keyId;
    }

    public SecurityKey GetCurrentKey()
    {
        return _keys[_currentKeyId];
    }

    public string GetCurrentKeyId()
    {
        return _currentKeyId;
    }

    public IEnumerable<SecurityKey> GetAllKeys()
    {
        return _keys.Values;
    }

    // rotação manual
    public void RotateKey(string newKey)
    {
        var keyId = Guid.NewGuid().ToString();

        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(newKey))
        {
            KeyId = keyId
        };

        _keys[keyId] = securityKey;
        _currentKeyId = keyId;
    }

    // opcional: remover chaves antigas
    public void RemoveOldKeys(TimeSpan maxAge)
    {
        // exemplo simples — pode evoluir com timestamp
        if (_keys.Count <= 2) return;

        var first = _keys.Keys.First();
        _keys.TryRemove(first, out _);
    }
}