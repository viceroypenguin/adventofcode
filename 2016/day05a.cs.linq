<Query Kind="Statements">
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

using (var md5 = MD5.Create())
{
    var input = "ojvtpuvg";
    var password = "";
    
    var cnt = 0L;
    while (password.Length < 8)
    {
        cnt++;
        var hashSrc = input + cnt.ToString();
        var bytes = Encoding.ASCII.GetBytes(hashSrc);
        var hash = md5.ComputeHash(bytes);
        if (hash[0] == 0x00 &&
            hash[1] == 0x00 &&
            (hash[2] & 0xf0) == 0x00)
        {
            password = password + (hash[2] & 0x0f).ToString("x");
            password.Dump();
        }
    }
}