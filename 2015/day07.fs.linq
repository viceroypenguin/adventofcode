<Query Kind="FSharpProgram">
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

let mutable (wires: Dictionary<string, Lazy<uint16>>) = null

let getArgument arg =
    let (success, number) = UInt16.TryParse arg
    if success 
        then (fun _ -> number)
        else (fun _ -> wires.[arg].Value)

let regex = new Regex(
                    @"^\s*(
                        (?<assign>\w+) |
                        (?<not>NOT\s+(?<not_arg>\w+)) |
                        ((?<arg1>\w+)\s+(?<command>AND|OR|LSHIFT|RSHIFT)\s+(?<arg2>\w+))
                    )
                    \s*->\s*
                    (?<dest>\w+)\s*$",
                    RegexOptions.Compiled ||| RegexOptions.ExplicitCapture ||| RegexOptions.IgnorePatternWhitespace)

let resetWires = (fun _ -> 
    let tmp =
        File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day07.input.txt"))
        |> Seq.map (fun s -> 
            let m = regex.Match s
            let destination = m.Groups.["dest"].Value
            if m.Groups.["assign"].Success then 
                (destination, new Lazy<uint16> (getArgument m.Groups.["assign"].Value))
            else if m.Groups.["not"].Success then
                let arg = (getArgument m.Groups.["not_arg"].Value)
                (destination, new Lazy<uint16> (fun _ -> (~~~) (arg ())))
            else if m.Groups.["command"].Success then
                let arg1 = (getArgument m.Groups.["arg1"].Value)
                let arg2 = (getArgument m.Groups.["arg2"].Value)
                match m.Groups.["command"].Value with
                | "AND" -> (destination, new Lazy<uint16> (fun _ -> (arg1()) &&& (arg2())))
                | "OR" -> (destination, new Lazy<uint16> (fun _ -> (arg1()) ||| (arg2())))
                | "LSHIFT" -> (destination, new Lazy<uint16> (fun _ -> (arg1()) <<< int(arg2())))
                | "RSHIFT" -> (destination, new Lazy<uint16> (fun _ -> (arg1()) >>> int(arg2())))
            else invalidOp "Bad data")
    wires <- Enumerable.ToDictionary(tmp,
                            (fun (d, _) -> d),
                            (fun (_, l) -> l)))

resetWires ()
let partA = wires.["a"].Value
partA.Dump("Part A")

resetWires ()
wires.["b"] <- new Lazy<uint16> (fun _ -> partA)
wires.["a"].Value.Dump("Part B")
