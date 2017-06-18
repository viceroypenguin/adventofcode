<Query Kind="FSharpProgram" />

let input = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day03.input.txt"))

let nextHouse = 
    fun ((x, y): int * int) (c:char) ->
        match c with
            | '>' -> (x + 1, y)
            | '<' -> (x - 1, y)
            | '^' -> (x, y + 1)
            | 'v' -> (x, y - 1)

let santaHouses = 
    input
    |> Seq.scan nextHouse (0,0)
    |> Seq.distinct
    |> Seq.length

let bothHouses =
    input
    |> Seq.scan (
        fun ((a, b): ((int * int) * (int * int))) c ->
            (b, nextHouse a c)) ((0,0),(0,0))
    |> Seq.map (fun (_, a) -> a)
    |> Seq.distinct
    |> Seq.length

santaHouses.Dump("Part A")
bothHouses.Dump("Part B")
