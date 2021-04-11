// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open Domain.Products

[<EntryPoint>]
let main argv =
    let product = Product.create "test" "test" 0u Sword
    printfn "%A" product

    let p = {Id = Guid.NewGuid(); Name = "str"; SimpleInfo = ""; Quantity = 10u; Type = Sword}
    printfn "%A" p
    0 // return an integer exit code