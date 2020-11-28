module Routing

open Giraffe

let routes: HttpFunc -> HttpFunc =
    choose [
        GET >=>
            choose [
                route "/" >=> text "Hello World!"
            ]
        setStatusCode 404 >=> text "Not Found"
    ]

