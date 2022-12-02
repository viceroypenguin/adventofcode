<Query Kind="FSharpProgram" />

let input = (
    File.ReadAllText(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day01.input.txt"))
    |> Seq.map (fun c -> int c - int '0')).ToList()

((input
    |> Seq.windowed 2
    |> Seq.filter (fun x -> x.[0] = x.[1])
    |> Seq.map (fun x -> x.[0])
    |> Seq.sum) + Seq.last input).Dump("Part A")
    
let rotInput = 
    input.Skip(input.Count / 2)
        .Concat(input.Take(input.Count / 2))
(Seq.zip input rotInput
    |> Seq.filter (fun (a,b) -> a = b)
    |> Seq.map (fun (a, _) -> a)
    |> Seq.sum).Dump("Part B")

