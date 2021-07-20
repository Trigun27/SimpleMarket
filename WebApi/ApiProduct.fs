module ApiProduct

open System
open Giraffe
open Microsoft.AspNetCore.Http
open Products
open FSharp.Control.Tasks
open RepoProduct
open Models
open Common


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