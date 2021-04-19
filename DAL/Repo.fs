module Repo

open System
open System.IO


let typePath (typeName: string) =
    let result = Directory.CreateDirectory typeName
    typeName
    
let private buildPath(typeName, id:Guid) = sprintf @"%s\%s_%O" (typePath typeName) typeName id

    
let tryFindTypeNameFolder typeName =
    let mkdir = typePath typeName   
    let folders = Directory.EnumerateDirectories(mkdir, sprintf "%s_*" typeName)
    let l = folders |> Seq.toList
    match l with
    | [] -> None
    | head::tail -> Some (DirectoryInfo(head).Name)

let write typeName (id: Guid) line =
    let path = typePath typeName
    let filePath = sprintf "%s\%A.txt" path id
    File.WriteAllText(filePath, line)

let read typeName (id: Guid) =
    let filePath = sprintf "%s\%A.txt" typeName id
    File.ReadAllLines(filePath)
    
let writeProduct = write "product"

let readProduct = read "product"