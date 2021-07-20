module Routing

open Giraffe
open ApiProduct
open ApiBuyer


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
        
let apiBasketRoutes: HttpHandler =
    (choose [
        GET >=> routef "/%d" (fun (x: int64) -> json x)
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

