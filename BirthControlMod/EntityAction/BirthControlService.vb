
Imports Timberborn.Persistence
Imports Timberborn.TickSystem
Imports BepInEx.Logging
Imports Timberborn.BuildingsBlocking

''' <summary>
''' 对繁育罐的修饰类<br/>
''' 看起来每一个繁育罐会实例化一个对象<br/>
''' 每个对象各自持有是否开启计划生育的设置（由 <see cref="BreedingPodFragment"/> 进行修改）<br/>
''' 所有对象共同持有工作场所空位数量（由 <see cref="EventListener"/> 进行修改）<br/>
''' </summary>
Public Class BirthControlService : Inherits TickableComponent : Implements IPersistentEntity
    ' 存储配置使用的
    Private Shared ReadOnly BirthControlServiceKey As New ComponentKey(NameOf(BirthControlService))
    Private Shared ReadOnly ToggleKey As New PropertyKey(Of Boolean)("BirthControlEnabled")

    ' 工作场所空位数量，由 EventListener 进行修改
    Public Shared NumberOfFreeWorkslots As Integer = 0

    Private log As ManualLogSource

    ' 每个繁育罐的设置，由 BreedingPodFragment 进行修改，并且从存储文件中保存和读取
    Public BirthControlEnabled As Boolean = True

    Private Sub Awake()
        log = Logger.CreateLogSource("BirthControlService")
        log.LogDebug("Awake")
    End Sub

    ''' <summary>
    ''' 看起来应该是每隔一段时间会调用一次，每个繁育罐都会分别调用<br/>
    ''' 通过这个方法来轮询并进行自动操作<br/>
    ''' </summary>
    Public Overrides Sub Tick()
        Dim p = GetComponentFast(Of PausableBuilding)()
        If BirthControlEnabled Then
            ' 只有在设置为开启计划生育时才进行自动操作
            If p.Paused AndAlso NumberOfFreeWorkslots > 0 Then
                ' 已暂停 且 工作场所空位 > 0 时，启动繁育罐
                log.LogInfo($"Pause {p.name}, Free slots: {NumberOfFreeWorkslots}")
                p.Resume()
            ElseIf Not p.Paused AndAlso NumberOfFreeWorkslots <= 0 Then
                ' 未暂停 且 工作场所空位 <= 0 时，暂停繁育罐
                log.LogInfo($"Resume {p.name}, Free slots: {NumberOfFreeWorkslots}")
                p.Pause()
            End If
        End If
    End Sub

    ''' <summary>
    ''' 保存是否开启计划生育的设置
    ''' </summary>
    ''' <param name="entitySaver"></param>
    Public Sub Save(entitySaver As IEntitySaver) Implements IPersistentEntity.Save
        entitySaver.GetComponent(BirthControlServiceKey).Set(ToggleKey, BirthControlEnabled)
    End Sub

    ''' <summary>
    ''' 读取是否开启计划生育的设置
    ''' </summary>
    ''' <param name="entityLoader"></param>
    Public Sub Load(entityLoader As IEntityLoader) Implements IPersistentEntity.Load
        If entityLoader.HasComponent(BirthControlServiceKey) Then
            BirthControlEnabled = entityLoader.GetComponent(BirthControlServiceKey).Get(ToggleKey)
        End If
    End Sub
End Class

