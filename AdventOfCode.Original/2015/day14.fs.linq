<Query Kind="FSharpProgram" />

let regex = new Regex(@"(?<name>\w+) can fly (?<speed>\d+) km/s for (?<fly_time>\d+) seconds, but then must rest for (?<rest_time>\d+) seconds.", RegexOptions.Compiled)

type reinder =
    {
        name: string
        speed: int
        fly_time: int
        rest_time: int
        total_time: int
    }
let reindeer = 
    File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day14.input.txt"))
    |> Seq.map (fun str -> regex.Match(str))
    |> Seq.map (
        fun m -> 
            {
                name = m.Groups.["name"].Value
                speed = Convert.ToInt32(m.Groups.["speed"].Value)
                fly_time = Convert.ToInt32(m.Groups.["fly_time"].Value)
                rest_time = Convert.ToInt32(m.Groups.["rest_time"].Value)
                total_time = Convert.ToInt32(m.Groups.["fly_time"].Value) + Convert.ToInt32(m.Groups.["rest_time"].Value)
            })

let time = 2503
let finishLine = 
    [1..time]
    |> Seq.fold (
        fun deerProgress t ->
            let newProgress = 
                reindeer |> Seq.fold (
                    fun dp deer ->
                        let loop_time = (t-1) % deer.total_time
                        let (progress, points) = Map.find deer.name dp
                        Map.add 
                            deer.name 
                            (progress + (if loop_time < deer.fly_time then deer.speed else 0), points)
                            dp)
                    deerProgress
            let maxProgress = 
                Map.toSeq newProgress
                |> Seq.map (fun (_, (progress, _)) -> progress)
                |> Seq.max
            Map.toSeq newProgress
            |> Seq.filter (fun (_, (progress, _)) -> progress = maxProgress)
            |> Seq.fold (
                fun dp (name, (progress, points)) -> 
                    Map.add 
                        name
                        (progress, points + 1)
                        dp)
                newProgress)
        (reindeer |> Seq.map (fun d -> (d.name, (0, 0))) |> Map.ofSeq)

let finalDeer = 
    finishLine 
    |> Map.toSeq 
    |> Seq.map (fun (name, (progress, points)) -> (name, progress, points))

(finalDeer
    |> Seq.map (fun (_, progress, _) -> progress)
    |> Seq.max).Dump("Part A")
    
(finalDeer
    |> Seq.map (fun (_, _, points) -> points)
    |> Seq.max).Dump("Part B")