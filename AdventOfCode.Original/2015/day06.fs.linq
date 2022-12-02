<Query Kind="FSharpProgram">
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

type Command =
    | TurnOn
    | TurnOff
    | Toggle

type Action = 
    {
        Command: Command
        StartX: int
        StartY: int
        EndX: int
        EndY: int
    }
    
let processActions =
    fun getLightProcessor actions ->
        let lights = Array2D.create 1000 1000 0
        actions
        |> Seq.iter (fun a ->
            let lightProcessor = (getLightProcessor a.Command)
            for x = a.StartX to a.EndX do
                for y = a.StartY to a.EndY do
                    lights.[x, y] <- (lightProcessor lights.[x, y]))
        
        let mutable sum = 0
        Array2D.iter (fun i -> sum <- sum + i) lights
        sum
   
let partA = processActions (fun c ->
    match c with
    | TurnOn -> (fun _ -> 1)
    | TurnOff -> (fun _ -> 0)
    | Toggle -> (fun i -> 1 - i))
    
let partB = processActions (fun c ->
    match c with
    | TurnOn -> (fun i -> i + 1)
    | TurnOff -> (fun i -> Math.Max(i - 1, 0))
    | Toggle -> (fun i -> i + 2))
    
let regex = new Regex(
                    @"((?<on>turn on)|(?<off>turn off)|(?<toggle>toggle)) (?<startX>\d+),(?<startY>\d+) through (?<endX>\d+),(?<endY>\d+)",
                    RegexOptions.Compiled ||| RegexOptions.ExplicitCapture)

let actions = 
    File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day06.input.txt"))
    |> Seq.map regex.Match
    |> Seq.map (fun m -> 
        {
            Command = 
                if m.Groups.["on"].Success then Command.TurnOn else
                if m.Groups.["off"].Success then Command.TurnOff else
                if m.Groups.["toggle"].Success then Command.Toggle else
                invalidOp "bad data"
            StartX = Int32.Parse(m.Groups.["startX"].Value)
            StartY = Int32.Parse(m.Groups.["startY"].Value)
            EndX = Int32.Parse(m.Groups.["endX"].Value)
            EndY = Int32.Parse(m.Groups.["endY"].Value)
        })
    |> Seq.toList

(partA actions).Dump("Part A")
(partB actions).Dump("Part B")