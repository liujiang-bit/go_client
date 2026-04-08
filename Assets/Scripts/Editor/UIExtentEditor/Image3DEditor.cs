using UnityEditor;
using UnityEditor.UI;
using Client;

[CustomEditor(typeof(Image3D))]
public class Image3DEditor : ImageEditor
{
    private Image3D image3D;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var pos = EditorGUILayout.Vector3Field("坐标", image3D.pos);
        if(!pos.Equals(image3D.pos))
        {
            image3D.pos = pos;
            image3D.SetVerticesDirty();
        }
        var rotate = EditorGUILayout.Vector3Field("旋转", image3D.rotate);
        if (!rotate.Equals(image3D.rotate))
        {
            image3D.rotate = rotate;
            image3D.SetVerticesDirty();
        }
        var scale = EditorGUILayout.Vector3Field("缩放", image3D.scale);
        if (!scale.Equals(image3D.scale))
        {
            image3D.scale = scale;
            image3D.SetVerticesDirty();
        }

        var fov = EditorGUILayout.FloatField("fov", image3D.fov);
        if (!fov.Equals(image3D.fov))
        {
            image3D.fov = fov;
            image3D.SetVerticesDirty();
        }
        var aspect = EditorGUILayout.FloatField("aspect", image3D.aspect);
        if (!aspect.Equals(image3D.aspect))
        {
            image3D.aspect = aspect;
            image3D.SetVerticesDirty();
        }
        var near = EditorGUILayout.FloatField("near", image3D.near);
        if (!near.Equals(image3D.near))
        {
            image3D.near = near;
            image3D.SetVerticesDirty();
        }
        var far = EditorGUILayout.FloatField("far", image3D.far);
        if (!far.Equals(image3D.far))
        {
            image3D.far = far;
            image3D.SetVerticesDirty();
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        image3D = (Image3D)target;
    }
}
