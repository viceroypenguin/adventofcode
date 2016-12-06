<Query Kind="Statements">
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

using (var md5 = MD5.Create())
{
    var input = "ojvtpuvg";
    var password = new char?[8];
    
    var cnt = 0L;
    while (password.Any(c=>!c.HasValue))
    {
        cnt++;
        var hashSrc = input + cnt.ToString();
        var bytes = Encoding.ASCII.GetBytes(hashSrc);
        var hash = md5.ComputeHash(bytes);
        if (hash[0] == 0x00 &&
            hash[1] == 0x00 &&
            (hash[2] & 0xf0) == 0x00)
        {
            var idx = hash[2] & 0x0f;
            if (idx >= 8) continue;
            if (password[idx].HasValue) continue;
            
            password[idx] = ((hash[3] & 0xf0) >> 4).ToString("x")[0];
            string.Join("", password.Select(c=>c ?? '_')).Dump();
        }
    }
}
