module Products 
    
open System
        
[<CLIMutable>]  
type Product = {
        Id: Guid
        Name: string
        Info: string
    }
    
[<RequireQualifiedAccess>]
module Product =
    let validate product =
        let errors = seq {
            if (String.IsNullOrEmpty(product.Name)) then yield "Name should not be empty"
            if (String.IsNullOrEmpty(product.Info)) then yield "Info should not by empty"
        }
        
        if (Seq.isEmpty errors) then Ok product else Error errors
    
    let create name info =
        let p = {Id = Guid.NewGuid(); Name = name; Info = info;  } //Type = typeOfGood
        p
       
    let print product =
        sprintf "%A;%s;%s;%d" product.Id product.Name product.Info //product.Type
