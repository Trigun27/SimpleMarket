module Products 
    
open System

type MagicType =
    | Common
    | Uncommon
    | Rare
        
type TypeOfGood =
    | Sword
    | Bow 
    | Knives
    | Book
    | MagicBook
    | Food

//ToDo взять в option? Может можно все же как то через member
let convert (str: string) =
    match str with
    | "Sword" -> Sword
    | "Bow" -> Bow
    | "Knives" -> Knives
    | "Book" -> Book
    | "MagicBook" -> MagicBook
    | "Food" -> Food
            
    
type Product = {
        Id: Guid
        Name: string
        SimpleInfo: string
        Quantity: uint
        Type: TypeOfGood
    }
    
[<RequireQualifiedAccess>]
module Product =
    let validate product =
        let errors = seq {
            if (String.IsNullOrEmpty(product.Name)) then yield "Name should not be empty"
            if (String.IsNullOrEmpty(product.SimpleInfo)) then yield "Info should not by empty"
            if (product.Quantity < 0u) then yield "Quantity should be equal to 0 or more"
        }
        
        if (Seq.isEmpty errors) then Ok product else Error errors
    
    let create typeOfGood name info quantity  =
        let p = {Id = Guid.NewGuid(); Name = name; SimpleInfo = info; Quantity = quantity; Type = typeOfGood}
        p
       
    let print product =
        sprintf "%A;%s;%s;%d;%A" product.Id product.Name product.SimpleInfo product.Quantity product.Type

    //ToDo Полная херня. Надо подумать как делать лучше
    let readFromLine (line: string) =
        let data = line.Split(";")
        create
            (data.[1] |> convert)
            data.[2]
            data.[2]
            (data.[3] |> UInt32.Parse)
    
    let deserialize (line: string) =
        let data = line.Split(";")
        {
            Id = data.[0] |> Guid.Parse
            Name = data.[1]
            SimpleInfo = data.[2]
            Quantity = data.[3] |> UInt32.Parse
            Type = data.[4] |> convert
        }
        
    //ToDo Проверить что при сумме дельты выражение не будет меньше 0. Как сделать правильно?
    let updateQuantity delta product =
        let newQuantity = {
            product with Quantity = product.Quantity + delta
        }
        newQuantity