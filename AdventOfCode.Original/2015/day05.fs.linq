<Query Kind="FSharpProgram">
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

let vowels = [| 'a'; 'e'; 'i'; 'o'; 'u'; |]
let hasThreeVowels = fun str ->
    (str
    |> Seq.where (fun c -> vowels.Contains(c))
    |> Seq.truncate 3
    |> Seq.length) = 3

let hasPair = fun str ->
    str
    |> Seq.windowed 2
    |> Seq.exists (fun arr -> arr.[0] = arr.[1])
    
let evilStrings = [| "ab"; "cd"; "pq"; "xy"; |]
let hasEvilStrings = fun (str: string) ->
    evilStrings
    |> Seq.exists (fun s -> str.Contains(s))

let isNicePartA = fun str ->
    not (hasEvilStrings str)
    && hasPair str
    && hasThreeVowels str

let hasRepeatLetter = fun str ->
    str
    |> Seq.windowed 3
    |> Seq.exists (fun arr -> arr.[0] = arr.[2])

let hasDuplicatePair = fun str ->
    str
    |> Seq.windowed 2
    |> Seq.mapi (fun i arr -> (i, new string(arr)))
    |> Seq.groupBy (fun (_, str) -> str)
    |> Seq.where (fun (_, x) -> (Seq.length x) > 1)
    |> Seq.exists (fun (_, x) ->
        let indexes = x |> Seq.map (fun (i, _) -> i)
        ((Seq.max indexes) - (Seq.min indexes)) > 1)
        
let isNicePartB = fun str ->
    hasRepeatLetter str
    && hasDuplicatePair str

let input = 
    File.ReadAllText(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day05.input.txt"))
        .Split([| '\r'; '\n' |], StringSplitOptions.RemoveEmptyEntries)
        .ToArray() 

let partACount = 
    input
    |> Seq.where (fun s -> isNicePartA s)
    |> Seq.length

let partBCount = 
    input
    |> Seq.where (fun s -> isNicePartB s)
    |> Seq.length


partACount.Dump("Part A")

partBCount.Dump("Part B")
