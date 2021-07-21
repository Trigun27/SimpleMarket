module Common

open System
open System.Data
open System.Net.Http
open Giraffe
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Caching.Distributed
open Npgsql
open Products
open FSharp.Control.Tasks
open RepoProduct
open Models

let getDbConn (ctx: HttpContext) =
    ctx.RequestServices.GetService(typeof<NpgsqlConnection>)
                    :?> IDbConnection

let getRedis (ctx: HttpContext) =
    ctx.RequestServices.GetService(typeof<IDistributedCache>)
        :?> IDistributedCache
