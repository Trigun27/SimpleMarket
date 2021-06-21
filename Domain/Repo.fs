module Repo

open System
open System.Data
open System.Threading.Tasks
open Dapper.FSharp
open Dapper.FSharp.PostgreSQL

open Products

let product =
    select {
        table "product"
    }
    
let addProduct (conn: IDbConnection) (product: Product) =
    insert {
        table "product"
        value product
    } |> conn.InsertAsync
    
    
let takeProduct (conn: IDbConnection) (id: Guid) =
    
    select {
        table "product"
        where (eq "Id" id)
    } |> conn.SelectAsync<Product>
    
let takeAllProducts (conn: IDbConnection) =
    select {
        table "product"
        //where (eq "Name" name)
        orderBy "id" Asc
        //skip offset
        //take takeTo        
    } |> conn.SelectAsync<Product>