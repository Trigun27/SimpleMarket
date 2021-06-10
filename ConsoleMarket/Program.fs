// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open System.Net.Sockets
open Products
open Repo

type AdminCommand =
    | Add
    | Print
    | Del

type Command =
    | AdminCommand of AdminCommand
    | Exit

[<AutoOpen>]
module CommandParsing =
    let tryParse c : Command option =
        match c with
        | 'a' -> Some (AdminCommand Add)
        | 'p' -> Some (AdminCommand Print)
        | 'e' -> Some Exit
        | _ -> None
        
    let tryGetCommand cmd =
        match cmd with
        | Exit -> None
        | AdminCommand cmd -> Some cmd

[<AutoOpen>]
module UserInput =
    let commands = seq {
        while true do
            Console.Write "(a)dd, (p)rint or (e)xit: "
            yield Console.ReadKey().KeyChar
            Console.WriteLine()
    }
    
    

let private printErrors errs =
        printfn "ERRORS"
        errs |> Seq.iter (printfn "%s")

let inputProduct products =
    Console.WriteLine "Input product in line: Type;Name;Info;Quantity: "
    
    let p = Console.ReadLine()
            |> Product.readFromLine
            
    p::products
    
    

let processCommand products (adminCommand: AdminCommand)  =
    match adminCommand with
    | Add -> inputProduct products
    | Print -> products
    | Del -> products

[<EntryPoint>]
let main argv =
    
    let p: Product list = []
    
    let products = 
        commands
        |> Seq.choose tryParse
        |> Seq.takeWhile ((<>) Exit)
        |> Seq.choose tryGetCommand
        |> Seq.fold processCommand p
     
    
    0 // return an integer exit code