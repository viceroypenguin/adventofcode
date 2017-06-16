<Query Kind="Statements" />

var input = "11011110011011101";
var length = 35651584;

var data = input.Select(c=>c == '1').ToList();

Func<IList<bool>, List<bool>> curveStep = (bits) =>
{
	return bits.Concat(new[]{false}).Concat(bits.Reverse().Select(b=>!b)).ToList();
};

while (data.Count < length)
	data = curveStep(data);
	
data = data.Take(length).ToList();

Func<IList<bool>, List<bool>> generateChecksum = (_)=>new List<bool>();
generateChecksum = (bits) =>
{
	var checksum = new List<bool>();
	for (int i = 0; i < bits.Count; i+=2)
		checksum.Add(!(bits[i] ^ bits[i + 1]));
	
	if ((checksum.Count % 2) == 0) return generateChecksum(checksum);
	return checksum;
};

data = generateChecksum(data);
string.Join("", data.Take(length).Select(b=>b?'1':'0')).Dump();
