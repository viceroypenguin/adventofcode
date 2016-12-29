<Query Kind="Statements" />

var input = ".^.^..^......^^^^^...^^^...^...^....^^.^...^.^^^^....^...^^.^^^...^^^^.^^.^.^^..^.^^^..^^^^^^.^^^..^";
var rows = 400000;

var tiles = new List<IList<bool>>();
tiles.Add(input.Select(c=>c=='^').ToArray());

while (tiles.Count < rows)
{
    var row = tiles[tiles.Count - 1];
    var newRow = new bool[row.Count];
    
    for (int i = 0; i < row.Count; i++)
    {
        var left = i > 0 ? row[i - 1] : false;
        var right = i < row.Count - 1 ? row[i + 1] : false;
        newRow[i] = left ^ right;
    }
    
    tiles.Add(newRow);
}

//foreach (var r in tiles)
//    string.Join("", r.Select(b=>b?'^':'.')).Dump();

tiles.SelectMany(x=>x).Where(b=>!b).Count().Dump();
