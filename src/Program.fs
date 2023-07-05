﻿namespace Examples.CounterApp

open System

open Avalonia
open Avalonia.Controls
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Themes.Fluent
open Avalonia.FuncUI.Hosts
open Avalonia.Media.Imaging

[<RequireQualifiedAccess>]
module WindowIcon =
    open Avalonia.Platform

    let loadIcon assetPath =
        AssetLoader.Open(new Uri(assetPath)) |> WindowIcon



type MainWindow() =
    inherit HostWindow()

    do
        base.Title <- "Counter Example"

        base.Icon <- WindowIcon.loadIcon "avares://Sonap/Assets/Icons/icon.ico"

        base.Height <- 400.0
        base.Width <- 400.0
        base.Content <- Main.view

type App() =
    inherit Application()

    override this.Initialize() =
        this.Styles.Add(FluentTheme())
        this.RequestedThemeVariant <- Styling.ThemeVariant.Dark

    override this.OnFrameworkInitializationCompleted() =
        match this.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as desktopLifetime ->
            let mainWindow = MainWindow()
            desktopLifetime.MainWindow <- mainWindow
        | _ -> ()

module Program =

    [<EntryPoint>]
    let main (args: string[]) =
        AppBuilder
            .Configure<App>()
            .UsePlatformDetect()
            .UseSkia()
            .StartWithClassicDesktopLifetime(args)
