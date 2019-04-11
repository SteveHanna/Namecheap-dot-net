# Artifacts Folder

The artifacts folder is used for packaging.

All files in here are, and should be, ignored.

To package the project:

    dotnet pack -c Release --output ../artifacts

`nupkg` is in the `$/artifacts` folder.

    cd artifacts
    dotnet nuget push *.nupkg -k API_KEY -s https://api.nuget.org/v3/index.json

