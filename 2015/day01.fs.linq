<Query Kind="FSharpProgram" />

let mutable level = 0
let mutable basement = 0
File.ReadAllText(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day01.input.txt")) 
    |> Seq.iteri (fun i c -> 
        level <- level + 
            match c with
                | '(' -> +1
                | ')' -> -1
        if basement = 0 && level = -1 then basement <- i + 1)
 
level.Dump("Part A")
basement.Dump("Part B")