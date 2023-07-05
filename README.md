# Package an F# Avalonia app as a Snap

> ### Snaps are not your thing?
>
> Try the packaging F# apps as a [flatpak](https://github.com/AngelMunoz/package-fsharp-to-flatpak/tree/main) then!

> **_Note_**: While this example is written for F#, it should work for any .NET app.

You're done with your app. Providing binaries is the next step, but you want to make it easy for your users to install and run your app. You also want to make it easy for you to distribute your app and even maybe auto-updates. You've heard about Snaps and want to try it out.

This project is based on this _https://snapcraft.io/docs/dotnet-apps_ tutorial which has better information in regards of the yaml file content.

> **_Note_**:For this you should have set already LXD and Snapcraft snaps installed.

## The Snap Parts

You simply need to have a directory named `snap` with a `snapcraft.yaml` file in it. The `snapcraft.yaml` file is the recipe for your snap. It tells snapcraft how to build your snap. In this case, we're going to use the `dotnet` plugin to build our snap.

```yaml
name: sonap # name of the snap
version: "1.0.0"
grade: devel # these act like _release_ channels you can use to signal the maturity of your snap
summary: Sample thing # short description
# multi line string description
description: |
  Just an F# Sample.

base: core22

# snaps usually use the strict confinement, devmode helps with debugging
confinement: strict

parts:
  sonap: # our app build definition
    plugin: dotnet
    dotnet-build-configuration: Release
    dotnet-self-contained-runtime-identifier: linux-x64
    source: ./src # where the source code is
    build-packages:
      # the sdk we're using to build this app
      - dotnet-sdk-7.0
    stage-packages:
      # These are packages that may be required by
      # Avalonia or dotnet to run in linux natively
      - libicu70
      - libfontconfig
      - libx11-6
      - libice6
      - libsm6

apps:
  sonap: # If this matches the name of the part, it will be the command name
    # otherwise you'd need to call your app something like sonap.myappname
    # The output directory of the dotnet build command is the root of the container
    # which matches as the root of the repository
    # So the command is the name of the project solution without the .fsproj extension
    # Also keep in mind that we've used '<PublishSingleFile>true</PublishSingleFile>'
    # in the .fsproj file, so the output is a single file that we can call directly
    # otherwise we'd need to adjust the call to something like ./Sonap/Sonap
    command: ./Sonap
    # We require these plugins so that our app can talk to the window manager
    # of the platform it's running on
    plugs:
      - desktop
      - desktop-legacy
      - wayland
      - x11
      # Please visit https://snapcraft.io/docs/supported-interfaces for the full list
      # just in case you need more interfaces like:
      # - home # to access the user's home directory
      # - alsa # to play/record sound
      # - network
      # - camera
```

Once you've done your app and are ready to package it, you can follow the steps below.

- sudo snapcraft --debug

  - debug allows you to drop into the container if the snap build process fails.

- sudo snap install `<snap-name>_1.0.0_amd64.snap` --devmode --dangerous
- `<snap name>` # in this case it's the name of the command

Snaps are also supported in arm64, so you can build your snap for arm64 as well but you will need to change the runtime identifier.

### Extra Notes

If you'd like to know how the dotnet build/publish command are executed, please check out https://github.com/canonical/craft-parts/blob/main/craft_parts/plugins/dotnet_plugin.py

> It was quite hard to find there doesn't seem to be any documentation on where plugins are hosted, I went to the snapcraft repo countless times!
