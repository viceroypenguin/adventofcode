<Query Kind="FSharpProgram" />

let regex = new Regex(@"(?<l>\d+)x(?<w>\d+)x(?<h>\d+)", RegexOptions.Compiled);
let boxes = 
    File.ReadLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day02.input.txt"))
    |> Seq.map regex.Match
    |> Seq.map (fun m -> 
        [|
            Convert.ToInt32(m.Groups.["l"].Value);
            Convert.ToInt32(m.Groups.["w"].Value);
            Convert.ToInt32(m.Groups.["h"].Value);
        |] |> Seq.sort |> Seq.toList)
    
let totalWrappingPaper =
    boxes
    |> Seq.map (fun b -> [| b.[0] * b.[1]; b.[0] * b.[2]; b.[1] * b.[2]; |] |> Seq.sort |> Seq.toList)
    |> Seq.map (fun a -> 3 * a.[0] + 2 * a.[1] + 2 * a.[2])
    |> Seq.sum
        
let totalRibbonLength =
    boxes
    |> Seq.map (fun b -> b.[0] * b.[1] * b.[2] + 2 * b.[0] + 2 * b.[1])
    |> Seq.sum
        
totalWrappingPaper.Dump("Part A");
totalRibbonLength.Dump("Part B");
