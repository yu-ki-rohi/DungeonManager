using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButton : MonoBehaviour
{
    public enum  UIType
    {
        Default,
        MonsterRepository,
        ItemRepository
    }
    /// <summary>
    /// �{�^�����������Ƃ��̂ӂ�܂�
    /// </summary>
    /// <returns>���g��n��</returns>
    public virtual UIButton PressBehave(out UIType uiType, out int index)
    {
        index = 0;
        uiType = UIType.Default;
        return this;
    }

    /// <summary>
    /// �{�^����ŗ����ꂽ���̂ӂ�܂�
    /// </summary>
    /// <returns></returns>
    public virtual UIType ReleaseInBehave(out int index)
    {
        index = 0;
        return UIType.Default;
    }

    /// <summary>
    /// �{�^���O�ŗ����ꂽ���̂ӂ�܂�
    /// </summary>
    /// <returns></returns>
    public virtual UIType ReleaseOutBehave(out int index)
    {
        index = 0;
        return UIType.Default;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
}
