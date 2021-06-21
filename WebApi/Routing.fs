module Routing

open System
open System
open System.Data
open System.Net.Http
open Giraffe
open Microsoft.AspNetCore.Http
open Npgsql
open Products
open FSharp.Control.Tasks
open Repo


[<CLIMutable>]  
type ProductIo = {
    Id: Guid option
    Name: string
    SimpleInfo: string
}

let getConn (ctx: HttpContext) =
    ctx.RequestServices.GetService(typeof<NpgsqlConnection>)
                    :?> IDbConnection

let addProduct : HttpHandler =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let! productIo = ctx.BindJsonAsync<ProductIo>()
            let product = Product.create productIo.Name productIo.SimpleInfo
            let conn = getConn ctx
            let! t = addProduct conn product
            let! result = takeProduct conn product.Id
            return! Successful.OK result next ctx
        }

let getProduct (id: Guid) : HttpHandler =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let conn = getConn ctx
            let! product = takeProduct conn id
            return! json product next ctx
        }

let getAllProducts : HttpHandler =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let conn = getConn ctx
            let! products = takeAllProducts conn
            return! json products next ctx
        }

let apiProductRoutes: HttpHandler =
    subRoute "/product"
        (choose [
            GET >=> choose [
                routef "/%O" getProduct
                route "" >=> getAllProducts
            ]
            POST >=> choose [
                route "" >=> addProduct
            ]
            PUT >=> routef "/%O" (fun (guid: Guid) -> text (guid.ToString()))]
        )

let routes: HttpFunc -> HttpFunc =
    choose [
        GET >=>
            choose [
                route "/" >=> text "Hello World!"
            ]
        subRoute "/api"
            (choose [
                apiProductRoutes                
        ])
                              
        setStatusCode 404 >=> text "Not Found"
        
    ]

