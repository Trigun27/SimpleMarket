module Common

open System
open System.Data
open System.Net.Http
open Giraffe
open Microsoft.AspNetCore.Http
open Npgsql
open Products
open FSharp.Control.Tasks
open RepoProduct
open Models

let getConn (ctx: HttpContext) =
    ctx.RequestServices.GetService(typeof<NpgsqlConnection>)
                    :?> IDbConnection

