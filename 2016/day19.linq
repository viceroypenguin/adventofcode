<Query Kind="Statements" />

Func<int, int> nextPowerOfTwo = (n)=>
{
	n|=n>>1;
	n|=n>>2;
	n|=n>>4;
	n|=n>>8;
	n|=n>>16;
	
	return n+1;
};

Func<int, int> partAElfHoldingPresents = (n)=>
	((n * 2) % nextPowerOfTwo(n) + 1);
	
Func<int, int> nextPowerOfThree = (n) =>
{
	int x = 3;
	for (; x < n; x *= 3)
		;
	return x;
};

Func<int, int> partBElfHoldingPresents = (n)=>
{
	var roundUp = nextPowerOfThree(n);
	var roundDown = roundUp / 3;
	if (n <= roundDown * 2) return n - roundDown;
	if (n == roundUp) return n;
	return (n * 2) % roundUp;
};

var input = 3005290;

partAElfHoldingPresents(input).Dump("Part A");
partBElfHoldingPresents(input).Dump("Part B");
