[cmdletbinding()]
param(
    [string]$Resource,
    [string]$Method = 'GET',
    [string]$Body = $null,
    [string]$ElasticHost = 'http://localhost:9200',
    [PSCredential]$Credential = (Get-Credential)
)

If ($Body) {
    If ($Method -eq 'GET') {
        $Method = 'PUT'
    }

    $result = Invoke-RestMethod `
        -Method $Method `
        -Credential $Credential `
        -ContentType 'application/json' `
        -Body $Body `
        "$ElasticHost/$Resource"

    Return $result
}
Else {
    $result = Invoke-RestMethod `
        -Method $Method `
        -Credential $Credential `
        "$ElasticHost/$Resource"

    Return $result
}