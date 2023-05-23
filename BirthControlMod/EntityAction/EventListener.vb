Imports BepInEx.Logging
Imports Bindito.Core
Imports TimberApi.DependencyContainerSystem
Imports Timberborn.Population
Imports Timberborn.SingletonSystem

''' <summary>
''' 事件监听类，通过它获得人口变更的通知
''' </summary>
Public Class EventListener : Implements ILoadableSingleton
    Private _eventBus As EventBus
    Private log As ManualLogSource

    <Inject>
    Public Sub InjectDependencies(eventBus As EventBus)
        _eventBus = eventBus
    End Sub

    Public Sub Load() Implements ILoadableSingleton.Load
        log = Logger.CreateLogSource("EventListener")
        log.LogDebug("Load")
        ' 注册通知订阅
        _eventBus.Register(Me)
    End Sub

    ''' <summary>
    ''' 人口变更时获得的通知回调<br/>
    ''' 通过 <see cref="PopulationService"/> 来获得人口数据，并赋值给 <see cref="BirthControlService.NumberOfFreeWorkslots"/><br/>
    ''' </summary>
    ''' <param name="e">里面啥也没有</param>
    <OnEvent>
    Public Sub OnPopulationChanged(e As PopulationChangedEvent)
        Dim instance = DependencyContainer.GetInstance(Of PopulationService)
        BirthControlService.NumberOfFreeWorkslots = instance.GlobalPopulationData.BeaverWorkplaceData.NumberOfFreeWorkslots
    End Sub
End Class
