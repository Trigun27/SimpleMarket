module Routing

open System
open Giraffe
open ApiProduct
open ApiBuyer
open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks
open Common
open Microsoft.Extensions.Caching.Distributed
open Newtonsoft
open Newtonsoft.Json
open Models



let apiProductRoutes: HttpHandler =  
    (choose [
        GET >=> choose [
            routef "/%O" getProduct
            route "" >=> getAllProducts
        ]
        POST >=> choose [
            route "" >=> addProduct
        ]
        PUT >=> routef "/%O" updateProduct
    ])
        
let addToBusket (userId: Guid) : HttpHandler =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let redis = getRedis ctx
//            let! data = redis.GetStringAsync(userId.ToString())
//            let result = JsonConvert.DeserializeObject<Busket> data
//            let newResult = {result with Count = result.Count + add }
            let newResult = {
                ProductIn = {
                    Id = Some (Guid.NewGuid())
                    Name = "Test"
                    Info = "Test Info"
                }
                Count = 2
            }
            let toCache = JsonConvert.SerializeObject newResult
            let! stringAsync = redis.SetStringAsync(userId.ToString(), toCache)
            return! json newResult next ctx
        }
        
let getBusket (userId: Guid) : HttpHandler =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let redis = getRedis ctx
            let! data = redis.GetStringAsync(userId.ToString())
            let result = JsonConvert.DeserializeObject<Busket> data
            return! json result next ctx
        }

let apiBasketRoutes: HttpHandler =
    (choose [
        GET >=> routef "/%O" getBusket
        PUT >=> routef "/%O" addToBusket 
    ])
    
    
    
let apiBuyerRoutes: HttpHandler =
    (choose [
        GET >=> routef "/%s" getBuyer
    ])

let routes: HttpFunc -> HttpFunc =
    choose [
        GET >=>
            choose [
                route "/" >=> text "Hello World!"
            ]
        subRoute "/api"
            (choose [
                subRoute "/product"
                    apiProductRoutes
                subRoute "/basket"
                    apiBasketRoutes
                subRoute "/buyer"
                    apiBuyerRoutes
        ])
                              
        setStatusCode 404 >=> text "Not Found"
        
    ]

