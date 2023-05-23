Imports TimberApi.UiBuilderSystem
Imports Timberborn.EntityPanelSystem
Imports UnityEngine.UIElements
Imports UnityEngine
Imports Timberborn.CoreUI

''' <summary>
''' 繁育罐的 UI 类
''' </summary>
Public Class BreedingPodFragment : Implements IEntityPanelFragment
    Private ReadOnly _builder As UIBuilder
    Private _root As VisualElement
    ' 持有 UI 开关实例来设置状态
    Private _birthControlEnabled As Toggle
    ' 持有繁育罐实例来设置逻辑开关
    Private _component As BirthControlService

    Public Sub New(builder As UIBuilder)
        _builder = builder
    End Sub

    Public Sub ShowFragment(entity As GameObject) Implements IEntityPanelFragment.ShowFragment
        ' 如果当前 UI 是繁育罐的，GetComponent() 会返回一个 BirthControlService 对象，否则返回 Nothing
        _component = entity.GetComponent(Of BirthControlService)
    End Sub

    Public Sub ClearFragment() Implements IEntityPanelFragment.ClearFragment
        _root.ToggleDisplayStyle(False)
    End Sub

    Public Sub UpdateFragment() Implements IEntityPanelFragment.UpdateFragment
        If _component IsNot Nothing Then
            ' 只有当前是繁育罐的时候，才显示开关 UI，并且使用逻辑开关来初始化 UI
            _birthControlEnabled.SetValueWithoutNotify(_component.BirthControlEnabled)
            _root.ToggleDisplayStyle(True)
        End If
    End Sub

    Public Function InitializeFragment() As VisualElement Implements IEntityPanelFragment.InitializeFragment
        ' 添加 UI 开关
        _root = _builder.CreateComponentBuilder.CreateVisualElement.AddPreset(
                        Function(factory) factory.Toggles.Checkmark(
                            name:="BirthControlEnabled",
                            locKey:="BirthControl.BreedingPod.BirthControlEnabled"
                        )
                    ).BuildAndInitialize
        _birthControlEnabled = _root.Q(Of Toggle)("BirthControlEnabled")
        ' UI 开关改变时回调修改逻辑开关
        _birthControlEnabled.RegisterValueChangedCallback(
                    Sub(e As ChangeEvent(Of Boolean))
                        _component.BirthControlEnabled = e.newValue
                    End Sub
                )
        _root.ToggleDisplayStyle(False)
        Return _root
    End Function
End Class

