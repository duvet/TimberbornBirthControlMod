Imports Bindito.Core
Imports TimberApi.ConfiguratorSystem
Imports TimberApi.SceneSystem
Imports Timberborn.EntityPanelSystem

<Configurator(SceneEntrypoint.InGame)>
Public Class BreedingPodConfigurator : Implements IConfigurator
    Public Sub Configure(containerDefinition As IContainerDefinition) Implements IConfigurator.Configure
        ' 绑定繁育罐 UI
        containerDefinition.Bind(Of BreedingPodFragment).AsSingleton()
        containerDefinition.MultiBind(Of EntityPanelModule).ToProvider(Of EntityPanelModuleProvider).AsSingleton()
    End Sub

    ''' <summary>
    ''' This magic class somehow adds our UI fragment into the game<br/>
    ''' 其它插件就是这么写的
    ''' </summary>
    Private Class EntityPanelModuleProvider : Implements IProvider(Of EntityPanelModule)
        Private ReadOnly _breedingPodFragment As BreedingPodFragment

        Public Sub New(breedingPodFragment As BreedingPodFragment)
            _breedingPodFragment = breedingPodFragment
        End Sub

        Public Function [Get]() As EntityPanelModule Implements IProvider(Of EntityPanelModule).Get
            Dim builder As New EntityPanelModule.Builder
            builder.AddBottomFragment(_breedingPodFragment)
            Return builder.Build
        End Function
    End Class
End Class

