module Models

open System


[<CLIMutable>]  
type ProductIo = {
    Id: Guid option
    Name: string
    Info: string
}

[<CLIMutable>]
type Busket = {
    ProductIn: ProductIo
    Count: int
}

[<CLIMutable>]
type Buyer = {
    Id: Guid option
    FirstName: string
    LastName: string option
    IsVerified: bool
    Email: string
    Phone: string
}

