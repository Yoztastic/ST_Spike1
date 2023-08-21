
function Execute ($action) {
    if($action -eq "database") {
        Database
        return
    }

    if($action -eq "devcert") {
      DevCert
      return
    }

    Write-Host "Unknown action: $action. Known actions are 'urlacl'"
}

function Database {
    $create_db_script="CREATE DATABASE StorageSpike"
    & docker exec -it dc-sql /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P p@ssw0rd -Q "$create_db_script"
}

function DevCert {
    $password = New-Guid

    if ($IsWindows) {
      Invoke-Expression "dotnet dev-certs https --clean"
      Invoke-Expression "dotnet dev-certs https -ep $env:APPDATA\ASP.NET\https\SecureDataInterceptor.pfx -p ${password}"
      Invoke-Expression "dotnet dev-certs https --trust"
      Invoke-Expression "dotnet user-secrets -p src\Host\Host.csproj set ""Kestrel:Certificates:Development:Password"" ""${password}"""
    } else {
      Invoke-Expression "dotnet dev-certs https --clean"
      Invoke-Expression "dotnet dev-certs https -ep $env:HOME/.aspnet/https/SecureDataInterceptor.pfx -p ${password}"
      Invoke-Expression "dotnet dev-certs https --trust"
      Invoke-Expression "dotnet user-secrets -p src/Host/Host.csproj set ""Kestrel:Certificates:Development:Password"" ""${password}"""
    }
}

Execute $args
