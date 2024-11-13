// https://nekojara.city/unity-input-system-press-position

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

public class PressPositionComposite : InputBindingComposite<Vector2>
{
    // ボタンの押下状態
    [InputControl(layout = "Button")] public int press;

    // ポインタの位置
    [InputControl(layout = "Vector2")] public int position;

    /// <summary>
    /// 初期化
    /// </summary>
#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoadMethod]
#else
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
#endif
    private static void Initialize()
    {
        // 初回にCompositeBindingを登録する必要がある
        InputSystem.RegisterBindingComposite<PressPositionComposite>("PressPosition");
    }

    public override Vector2 ReadValue(ref InputBindingCompositeContext context)
    {
        // ポインタの位置を返す
        return context.ReadValue<Vector2, Vector2MagnitudeComparer>(position);
    }

    public override float EvaluateMagnitude(ref InputBindingCompositeContext context)
    {
        // ボタンの押下状態を大きさとして返す
        return context.ReadValue<float>(press);
    }
}