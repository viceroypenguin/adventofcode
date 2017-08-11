<Query Kind="FSharpProgram" />

let input = "1113122113"

let processInput arr =
    let (cur, cnt, newStr) =
        arr 
        |> Seq.fold (
            fun (cur, cnt, newStr) c ->
                if cur = c 
                    then (cur, cnt + 1, newStr)
                    else (c, 1, cur::((string cnt).[0])::newStr))
            (' ', 0, [])
    (cur::((string cnt).[0])::newStr)
    |> List.rev
    |> List.tail
    |> List.tail
    

let mutable answer = input |> Seq.toList

for i = 1 to 40 do answer <- processInput answer
(answer.Length).Dump("Part A")

for i = 1 to 10 do answer <- processInput answer
(answer.Length).Dump("Part B")
