using TMPro;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(TMP_Text))]
public class TextOnArc : MonoBehaviour
{
    public float radius = 200f;
    public float angleMultiplier = 1f;
    public bool faceCenter = true;

    private TMP_Text text;

    void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    void LateUpdate()
    {
        if (text == null)
            text = GetComponent<TMP_Text>();

        text.ForceMeshUpdate();

        TMP_TextInfo textInfo = text.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;

            int vertexIndex = charInfo.vertexIndex;
            int materialIndex = charInfo.materialReferenceIndex;

            Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;

            Vector3 bottomLeft = vertices[vertexIndex + 0];
            Vector3 topLeft = vertices[vertexIndex + 1];
            Vector3 topRight = vertices[vertexIndex + 2];
            Vector3 bottomRight = vertices[vertexIndex + 3];

            Vector3 charMid = (bottomLeft + topRight) * 0.5f;

            for (int j = 0; j < 4; j++)
                vertices[vertexIndex + j] -= charMid;

            float angle = (charMid.x / radius) * angleMultiplier;

            Vector3 circlePos = new Vector3(
                Mathf.Sin(angle) * radius,
                Mathf.Cos(angle) * radius,
                0f
            );

            float rotationZ = faceCenter ? -Mathf.Rad2Deg * angle : 0f;
            Matrix4x4 matrix = Matrix4x4.TRS(
                circlePos,
                Quaternion.Euler(0f, 0f, rotationZ),
                Vector3.one
            );

            for (int j = 0; j < 4; j++)
                vertices[vertexIndex + j] = matrix.MultiplyPoint3x4(vertices[vertexIndex + j]);
        }

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
            text.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }
    }
}
