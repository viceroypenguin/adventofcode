<Query Kind="Statements">
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

using (var md5 = MD5.Create())
{
    var input = "zpqevtbw";
    var partA = false;
    var numHashes = partA ? 1 : 2017;
    
    var threeMatchingRegex = new Regex(@"(\w|\d)\1{2}", RegexOptions.Compiled);
    
    var queue = new Queue<string>();
    var index = 0;
    var numKeys = 0;
    
    var counter = 0;
    Func<string> getNextHash = () =>
    {
        var hashSrc = input + counter;
        counter++;
        
        var hashText = hashSrc;
        for (int i = 0; i < numHashes; i++)
        {
            var bytes = Encoding.ASCII.GetBytes(hashText);
            var hash = md5.ComputeHash(bytes);
            hashText = BitConverter.ToString(hash).ToLower().Replace("-", "");
        }
        return hashText;
    };
    
    Action ensureQueueLength = () =>
    {
        while (queue.Count < 1000)
            queue.Enqueue(getNextHash());
    };
    
    ensureQueueLength();
    
    while (numKeys < 64)
    {
        var possibleKey = new { hash = queue.Dequeue(), index };
        index++;
        ensureQueueLength();
        
        var match = threeMatchingRegex.Match(possibleKey.hash);
        if (!match.Success) continue;
        
        var fiveLetter = new string(match.Value[0], 5);
        
        if (queue.Any(h=>h.Contains(fiveLetter)))
        {
            numKeys++;
            possibleKey.Dump();
        }
    }
}