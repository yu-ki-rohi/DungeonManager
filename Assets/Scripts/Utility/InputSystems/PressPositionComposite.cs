// https://nekojara.city/unity-input-system-press-position

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

public class PressPositionComposite : InputBindingComposite<Vector2>
{
    // �{�^���̉������
    [InputControl(layout = "Button")] public int press;

    // �|�C���^�̈ʒu
    [InputControl(layout = "Vector2")] public int position;

    /// <summary>
    /// ������
    /// </summary>
#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoadMethod]
#else
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
#endif
    private static void Initialize()
    {
        // �����CompositeBinding��o�^����K�v������
        InputSystem.RegisterBindingComposite<PressPositionComposite>("PressPosition");
    }

    public override Vector2 ReadValue(ref InputBindingCompositeContext context)
    {
        // �|�C���^�̈ʒu��Ԃ�
        return context.ReadValue<Vector2, Vector2MagnitudeComparer>(position);
    }

    public override float EvaluateMagnitude(ref InputBindingCompositeContext context)
    {
        // �{�^���̉�����Ԃ�傫���Ƃ��ĕԂ�
        return context.ReadValue<float>(press);
    }
}