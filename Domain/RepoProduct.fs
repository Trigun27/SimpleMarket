module RepoProduct

open System
open System.Data
open System.Text
open System.Threading.Tasks
open Dapper
open FSharp.Control.Tasks

open Products
    
let addProduct (conn: IDbConnection) (product: Product) =
    let query = $"insert into public.product(\"Id\", \"Name\", \"Info\") values('{product.Id}', '{product.Name}', '{product.Info}')"
    task {
        let! data =  conn.ExecuteAsync(query)
        return data
    }
    
let updateProduct (conn: IDbConnection) (product: Product) =
    let query = $"update public.product set \"Name\" = '{product.Name}', \"Info\" = '{product.Info}' where \"Id\" = '{product.Id}'"
    task {
        let! data = conn.ExecuteAsync(query)
        return data
    }
    
let takeProduct (conn: IDbConnection) (id: Guid) =
    let query = $"select * from public.product where \"Id\" = '{id}'"
    task {
        let! data = conn.QueryFirstAsync<Product>(query)
        return data
    }
    
let takeAllProducts (conn: IDbConnection) =
    let query = "select * from public.product"
    task {
        let! data = conn.QueryAsync<Product>(query)
        return data
    }
    