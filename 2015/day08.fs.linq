<Query Kind="FSharpProgram" />

let input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day08.input.txt"))
let inputLength = input |> Seq.map (fun str -> str.Length) |> Seq.sum

type DecodeState =
    | Initial
    | Normal
    | Escaped
    | Hex1
    | Hex2
    | End

let getDecodedLength str = 
    let (_, cnt) = 
        str 
        |> Seq.fold (fun (state, count) c ->
            match state with
            | Initial -> 
                match c with
                | '\"' -> (Normal, count)
                | _ -> failwith "Unknown state"
            | Normal ->
                match c with
                | '\\' -> (Escaped, count)
                | '\"' -> (End, count)
                | _ -> (Normal, count + 1)
            | Escaped ->
                match c with
                | '\\' | '\"' -> (Normal, count + 1)
                | 'x' -> (Hex1, count)
                | _ -> failwith "Unknown state"
            | Hex1 -> (Hex2, count)
            | Hex2 -> (Normal, count + 1)
            | End -> failwith "Unknown State") (Initial, 0)
    cnt   

let decodeLength = input |> Seq.map getDecodedLength |> Seq.sum
(inputLength - decodeLength).Dump("Part A")

let getEncodedLength str =
    str
    |> Seq.fold (fun cnt c ->
        match c with
        | '\\' | '\"' -> cnt + 2
        | _ -> cnt + 1) 2

let encodeLength = input |> Seq.map getEncodedLength |> Seq.sum
(encodeLength - inputLength).Dump("Part B")
