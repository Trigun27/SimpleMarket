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
    Info: string
}

let getConn (ctx: HttpContext) =
    ctx.RequestServices.GetService(typeof<NpgsqlConnection>)
                    :?> IDbConnection

let addProduct : HttpHandler =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let! productIo = ctx.BindJsonAsync<ProductIo>()
            let product = Product.create productIo.Name productIo.Info
            let conn = getConn ctx
            let! t = addProduct conn product
            let! product = takeProduct conn product.Id
            return! json product next ctx
        }

let getProduct (id: Guid) : HttpHandler =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let conn = getConn ctx
            let! product = takeProduct conn id
            return! json product next ctx
        }
        
let updateProduct (id: Guid) : HttpHandler =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let! productIo = ctx.BindJsonAsync<ProductIo>()
            let conn = getConn ctx
            let request = Product.build productIo.Id.Value productIo.Name productIo.Info 
            let! result = updateProduct conn request
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
            PUT >=> routef "/%O" updateProduct
            ]
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

