using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TitleText : MonoBehaviour
{
    private TMP_Text _tmpText;
    [Range(0.5f, 120f)]
    [SerializeField] private float speed;
    [SerializeField] private float amplitude;

    private void Awake()
    {
        _tmpText = GetComponent<TMP_Text>();
    }

    void Update()
    {
        _tmpText.ForceMeshUpdate();

        TMP_TextInfo textInfo = _tmpText.textInfo;


        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo cInfo = textInfo.characterInfo[i];

            if (!cInfo.isVisible)
            {
                continue;
            }

            Vector3[] vertices = textInfo.meshInfo[cInfo.materialReferenceIndex].vertices;

            int vIndex0 = cInfo.vertexIndex;
            for (int j = 0; j < 4; j++)
            {
                Vector3 origin = vertices[vIndex0 + j];
                float offsetY = Mathf.Sin((Time.time * speed + origin.x) * Mathf.PI) * amplitude;
                vertices[vIndex0 + j] = origin + new Vector3(0, offsetY, 0);
            }
        }
        _tmpText.UpdateVertexData();
    }
}
