<Query Kind="FSharpProgram" />

let input = "hxbxwxba"

let password = 
    input
    |> Seq.map (fun c -> (int c) - (int 'a'))
    |> Seq.toList
    |> List.rev
    
let invalidChars = 
    ['i'; 'l'; 'o']
    |> Seq.map (fun c -> (int c) - (int 'a'))
    |> Seq.toArray

let rec iterateCurrent = function
    | x::xs when x = 25 -> 0::(iterateCurrent xs)
    | x::xs when invalidChars.Contains(x + 1) -> (x + 2)::xs
    | x::xs -> (x + 1)::xs
    | [] -> []

let hasTripleIncrement current =
    current
    |> Seq.windowed 3
    // NB: list is reversed, so arr.[0] is highest number instead of lowest
    |> Seq.exists (fun arr -> arr.[0] - 1 = arr.[1] && arr.[1] - 1 = arr.[2])

let hasTwoPair current =
    (
        current
        |> Seq.windowed 2
        |> Seq.filter (fun arr -> arr.[0] = arr.[1])
        |> Seq.distinct
        |> Seq.length) >= 2

let passwords =
    Seq.unfold (
        fun curPass ->
            let newPass = iterateCurrent curPass
            Some (newPass, newPass)) password
    |> Seq.filter (
        fun pass -> 
            let asSeq = pass |> List.toSeq
            (hasTripleIncrement asSeq) && (hasTwoPair asSeq))
        
let passToString pass = 
    let chars =
        pass 
        |> List.rev
        |> List.toSeq
        |> Seq.map (fun c -> char (c + (int 'a')))
    String.Join("", chars)    

(passwords 
|> Seq.take 2
|> Seq.map passToString).Dump()
