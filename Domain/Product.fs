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
        
    let create name info quantity typeOfGood =
        let p = {Id = Guid.NewGuid(); Name = name; SimpleInfo = info; Quantity = quantity; Type = typeOfGood}
        validate p
        
    //ToDo Проверить что при сумме дельты выражение не будет меньше 0. Как сделать правильно?
    let updateQuantity delta product =
        let newQuantity = {
            product with Quantity = product.Quantity + delta
        }
        newQuantity