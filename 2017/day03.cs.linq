<Query Kind="Statements" />

var input = 325489;

var maxRoot = (int)Math.Sqrt(input - 1);
var rank = (maxRoot + 1) / 2;
var rankRoot = (rank * 2) + 1;
var botRight = rankRoot * rankRoot;
var sideLength = rank * 2;

var sideNum = (botRight - input) / sideLength;
var lastInSide = botRight - (sideLength * sideNum);
var distance = Math.Abs(lastInSide - rank - input) + rank;
distance.Dump("Part A");
