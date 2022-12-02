<Query Kind="FSharpProgram">
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

let input = "yzbqklnj"

let md5 = MD5.Create()

let hasLeadingZeros = 
    fun (numZeros: int) (b: byte []) ->
        Seq.initInfinite (fun i -> i)
        |> Seq.map (fun i ->
            b.[i / 2] &&& if i % 2 = 1 then 0x0fuy else 0xf0uy)
        |> Seq.take numZeros
        |> Seq.exists (fun x -> int(x) <> 0)
        |> (fun b -> not b)
   
let getPassword = 
    fun numZeros ->
        Seq.initInfinite (fun i -> i)
        |> Seq.where (fun n ->
            input + string n
            |> Encoding.ASCII.GetBytes
            |> md5.ComputeHash
            |> hasLeadingZeros numZeros)
        |> Seq.head
        
(getPassword 5).Dump("Part A")
(getPassword 6).Dump("Part B")
