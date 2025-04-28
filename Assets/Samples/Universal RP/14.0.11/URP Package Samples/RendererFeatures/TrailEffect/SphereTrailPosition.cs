using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereTrailPosition : MonoBehaviour
{
    void LateUpdate()
    {
        // Push this sphere’s world-space center into the shader each frame
        Shader.SetGlobalVector("_SpherePositionWS", transform.position);

        var texSize = 512f;
        Shader.SetGlobalVector("_ReSampleOffset", new Vector2(1 / texSize, 1 / texSize));
    }
}
