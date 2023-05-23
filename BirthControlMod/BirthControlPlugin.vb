Imports HarmonyLib
Imports TimberApi.ConsoleSystem
Imports TimberApi.ModSystem

<HarmonyPatch>
Public Class BirthControlPlugin : Implements IModEntrypoint
    Public Sub Entry([mod] As IMod, consoleWriter As IConsoleWriter) Implements IModEntrypoint.Entry
        Dim h As New Harmony("Duvet.Timberborn.BirthControl")
        h.PatchAll()
        consoleWriter.LogInfo("BirthControl plugin is loaded")
    End Sub
End Class


