module FileRepo

open System.IO

open Products



let writeProduct product =
    let line = sprintf "%A; %s; %s; %d; %A" product.Id product.Name product.SimpleInfo product.Quantity product.Type
    let filePath = sprintf "%s.txt" "product"
    File.WriteAllText(filePath, line)

    

    