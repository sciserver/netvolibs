$xsd = "C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\xsd.exe"

$xlink = "dll\Jhu.VO\Stc\Resources\xlink.xsd"
$stc = "dll\Jhu.VO\Stc\Resources\stc-v1.30.xsd"
$tapregext = "dll\Jhu.VO\TapRegExt\Resources\tapregext-v1.0.xsd"
$vodataservice = "dll\Jhu.VO\VoDataService\Resources\vodataservice-v1.1.xsd"
$voresource = "dll\Jhu.VO\VoResource\Resources\voresource-v1.0.xsd"
$vosi_availability = "dll\Jhu.VO\Vosi\Resources\vosi-availability-v1.0.xsd"
$vosi_capabilities = "dll\Jhu.VO\Vosi\Resources\vosi-capabilities-v1.0.xsd"
$vosi_tables = "dll\Jhu.VO\Vosi\Resources\vosi-tables-v1.0.xsd"

& "$xsd" /c "$stc" `
    "$xlink" `
    "$stc" `
    "$tapregext" `
    "$vodataservice" `
    "$voresource" `
    "$vosi_availability" `
    "$vosi_capabilities" `
    "$vosi_tables"