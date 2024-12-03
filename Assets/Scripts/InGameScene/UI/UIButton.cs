using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButton : MonoBehaviour
{
    /// <summary>
    /// �{�^�����������Ƃ��̂ӂ�܂�
    /// </summary>
    /// <returns>���g��n��</returns>
    public virtual UIButton PressBehave()
    {
        return this;
    }

    /// <summary>
    /// �{�^����ŗ����ꂽ���̂ӂ�܂�
    /// </summary>
    /// <returns></returns>
    public virtual int ReleaseInBehave()
    {
        return -1;
    }

    /// <summary>
    /// �{�^���O�ŗ����ꂽ���̂ӂ�܂�
    /// </summary>
    /// <returns></returns>
    public virtual int ReleaseOutBehave()
    {
        return -1;
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
