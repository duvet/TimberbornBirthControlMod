Imports Bindito.Core
Imports TimberApi.ConfiguratorSystem
Imports TimberApi.SceneSystem
Imports Timberborn.Reproduction
Imports Timberborn.TemplateSystem

<Configurator(SceneEntrypoint.InGame)>
Public Class BirthControlConfigurator : Implements IConfigurator
    Public Sub Configure(containerDefinition As IContainerDefinition) Implements IConfigurator.Configure
        ' 注册 EventListner
        containerDefinition.Bind(Of EventListener)().AsSingleton()
        ' 将 BirthControlService 绑定到 BreedingPod
        containerDefinition.MultiBind(Of TemplateModule)().ToProvider(AddressOf ProvideTemplateModule).AsSingleton()
    End Sub

    Private Shared Function ProvideTemplateModule() As TemplateModule
        Dim builder As New TemplateModule.Builder
        builder.AddDecorator(Of BreedingPod, BirthControlService)()
        Return builder.Build
    End Function

End Class

