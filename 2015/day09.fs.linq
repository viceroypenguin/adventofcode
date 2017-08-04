<Query Kind="FSharpProgram" />

let regex = new Regex(@"(?<from>\w+) to (?<to>\w+) = (?<distance>\d+)", RegexOptions.Compiled)

let input = 
    File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day09.input.txt"))
    |> Seq.map (
        fun str -> 
            let m = regex.Match str
            (
                m.Groups.["from"].Value,
                m.Groups.["to"].Value,
                Convert.ToInt32(m.Groups.["distance"].Value)
            ))

let cities =
    input
    |> Seq.collect (fun (from, _to, _) -> [from; _to])
    |> Seq.distinct
    |> Seq.toList
    
let distMatrix =
    dict (
        input
        |> Seq.collect (fun (from, _to, dist) -> [ ((from, _to), dist); ((_to, from), dist) ]))
        
let rec distribute e = function
  | [] -> [[e]]
  | x::xs' as xs -> (e::xs)::[for xs in distribute e xs' -> x::xs]
  
let rec permute = function
  | [] -> [[]]
  | e::xs -> List.collect (distribute e) (permute xs)
  
let paths =
    cities 
        |> permute
        |> Seq.map (
            fun path ->
                path
                |> Seq.windowed 2
                |> Seq.map (fun pair -> (pair.[0], pair.[1], distMatrix.[(pair.[0], pair.[1])])))
        |> Seq.map (
            fun path ->
                (path, path |> Seq.map (fun (_, _, d) -> d) |> Seq.sum))
        |> Seq.toList

(paths
    |> Seq.sortBy (fun (_, dist) -> dist)
    |> Seq.head).Dump("Part A")

(paths
    |> Seq.sortBy (fun (_, dist) -> -dist)
    |> Seq.head).Dump("Part B")
