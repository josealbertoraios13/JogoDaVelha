using System.Security.Cryptography;

namespace model;

public abstract class ModelStructure //rever nome
{
    public string id {get; set;} = string.Empty;

    public abstract bool Valid();

    public string GenerateCode(int length = 8)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVXZ0123456789";
        var bytes = new byte[length];

        RandomNumberGenerator.Fill(bytes);

        return new string(bytes.Select(b => chars[b % chars.Length]).ToArray());
    }
    
    public virtual void GenerateId()
    {
        this.id = GenerateCode();
    }
}