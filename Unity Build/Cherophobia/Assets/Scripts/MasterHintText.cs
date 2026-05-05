using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MasterHintText : MonoBehaviour
{
    private TextMeshPro m_textMeshPro;

    // Start is called before the first frame update
    void Start()
    {
        m_textMeshPro = this.GetComponent<TextMeshPro>();

        m_textMeshPro.text = "<color=#FF9B0B>Master</color> Code:\n<color=#067265>Circle</color> > <color=#e9b00e>Square</color> > <color=#ed13a4>Circle</color> > <color=#067265>Triangle</color>";
    }
}
