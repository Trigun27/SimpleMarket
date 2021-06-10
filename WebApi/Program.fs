module Market
    

open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Giraffe
open Giraffe.Razor
open System
open System.IO


open Npgsql
open Serilog
open Routing



let errorHandler (ex : Exception) (logger : Microsoft.Extensions.Logging.ILogger) =
    logger.LogError(EventId(), ex, "An unhandled exception has occurred while executing the request.")
    clearResponse >=> setStatusCode 500 >=> text ex.Message

let configureApp (app: IApplicationBuilder) =
    let env = app.ApplicationServices.GetService<IWebHostEnvironment>()
    app.UseStaticFiles() |> ignore
    
    if env.EnvironmentName = "Development"
        then app.UseDeveloperExceptionPage()
        else app.UseGiraffeErrorHandler errorHandler
    |> ignore
    
    app.UseGiraffe routes

let configureServices (services: IServiceCollection) =
    let sp = services.BuildServiceProvider()
    let config = sp.GetService<IConfiguration>()
    let env = sp.GetService<IHostEnvironment>()
    let viewsFolderPath = Path.Combine(env.ContentRootPath, "Views")
    services.AddRazorEngine viewsFolderPath |> ignore
    
    Dapper.FSharp.OptionTypes.register()
    
    services.AddTransient<NpgsqlConnection>(fun x -> new NpgsqlConnection(config.GetConnectionString("Market"))) |> ignore
    
    // Register default Giraffe dependencies
    services.AddGiraffe() |> ignore


[<EntryPoint>]
let main _ =
    let contentRoot = Directory.GetCurrentDirectory()
    Host.CreateDefaultBuilder()
        .UseSerilog(fun host config -> config.ReadFrom.Configuration(host.Configuration) |> ignore)
        .ConfigureWebHostDefaults(
            fun webHostBuilder ->
                webHostBuilder
                    .UseKestrel()
                    .UseContentRoot(contentRoot)
                    .Configure(configureApp)
                    .ConfigureServices(configureServices)
                    |> ignore)
        
        .Build()
        .Run()
    0
