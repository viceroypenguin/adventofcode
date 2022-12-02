<Query Kind="FSharpProgram" />

let txt = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day01.input.txt"))

let levels = txt |> Seq.scan (fun acc x ->
        match x with
            | '(' -> acc + 1
            | _ -> acc - 1) 0

let finalLevel = levels |> System.Linq.Enumerable.Reverse |> Seq.head
let basement = levels |> Seq.findIndex (fun n -> n = -1)
            
finalLevel.Dump("Part A")
basement.Dump("Part B")