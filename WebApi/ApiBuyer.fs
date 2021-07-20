module ApiBuyer

open Giraffe
open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks
open Common

let getBuyer (phone: string) : HttpHandler =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {    
            return! json {| Guid = "0000-00000"; FirstName = "Vasiliy"; Phone = phone|} next ctx
        }