namespace Market

open System

[<CLIMutable>]
type Buyer = {
    Id: Guid
    FirstName: string
    LastName: string
    IsVerified: bool
    Email: string
}

[<RequireQualifiedAccess>]
module Buyer =
    let private isValidEmail email =
        try
            System.Net.Mail.MailAddress(email) |> ignore
            true
        with
        | _ -> false
        
    let validate user =
        let msg = " should not be empty"
        let errors = seq {
            if (String.IsNullOrEmpty(user.FirstName)) then yield "FirstName" + msg
            if (String.IsNullOrEmpty(user.LastName)) then yield "LastName" + msg
            if (isValidEmail user.Email |> not) then yield "Not valid Email"
        }
        
        if (Seq.isEmpty errors) then Ok user else Error errors
        
    let create fname lname email =
        let u = {Id = Guid.NewGuid(); FirstName = fname; LastName = lname; Email = email; IsVerified = false}
        validate u
    

