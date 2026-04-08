using System;
using UnityEngine;
using Skyunion;

/// <summary>
/// 用于显示城内地表网格使用
/// </summary>
namespace Client
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class GridLines : MonoBehaviour
    {
        public static float s_fade_total_time = 0.5f;

        private enum STATE
        {
            NORMAL,
            FADE_IN,
            FADE_OUT
        }
        private STATE m_cur_state;

        private Material m_mesh_material;
        private Color m_cur_color;
        private float m_fade_time;

        private void Start()
        {
        }

        private void SetColor(Color color)
        {
            m_cur_color = color;
            if ((bool)m_mesh_material)
            {
                m_mesh_material.SetColor("_Color", m_cur_color);
            }
        }

        private void Update()
        {
            try
            {
                if (m_cur_state == STATE.FADE_IN)
                {
                    m_cur_color.a = m_fade_time / s_fade_total_time;
                    if (m_cur_color.a > 1f)
                    {
                        m_cur_color.a = 1f;
                        m_cur_state = STATE.NORMAL;
                    }
                    m_fade_time += Time.deltaTime;
                    SetColor(m_cur_color);
                }
                else if (m_cur_state == STATE.FADE_OUT)
                {
                    m_cur_color.a = 1f - m_fade_time / s_fade_total_time;
                    if (m_cur_color.a < 0f)
                    {
                        m_cur_color.a = 0f;
                        m_cur_state = STATE.NORMAL;
                        CoreUtils.assetService.Destroy(base.transform.gameObject);
                    }
                    m_fade_time += Time.deltaTime;
                    SetColor(m_cur_color);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public void Fade(bool fadeIn)
        {
            if (m_mesh_material == null)
            {
                m_mesh_material = base.transform.GetComponent<MeshRenderer>().sharedMaterial;
                m_cur_color = m_mesh_material.color;
            }
            if (fadeIn && m_cur_state != STATE.FADE_IN)
            {
                m_fade_time = 0f;
                m_cur_state = STATE.FADE_IN;
                s_fade_total_time = 0.2f;
            }
            else if (m_cur_state != STATE.FADE_OUT)
            {
                m_fade_time = 0f;
                m_cur_state = STATE.FADE_OUT;
                s_fade_total_time = 0.5f;
            }
        }

        public void CombineMeshes(float[] pos, int length)
        {
            CombineInstance[] array = new CombineInstance[length / 2];
            CoreUtils.assetService.LoadAssetAsync<GameObject>("building_move_bottom", (asset)=>
            {
                var gameObject = asset.asset() as GameObject;
                Mesh sharedMesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
                int num = 0;
                int num2 = 0;
                while (num < length)
                {
                    array[num2].mesh = sharedMesh;
                    Quaternion q = Quaternion.Euler(0f, -45f, 0f);
                    Matrix4x4 transform = Matrix4x4.TRS(new Vector3(pos[num], 0.01f, pos[num + 1]), q, Vector3.one);
                    array[num2].transform = transform;
                    num += 2;
                    num2++;
                }
                base.transform.GetComponent<MeshFilter>().mesh = new Mesh();
                base.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(array);
                base.transform.gameObject.SetActive(value: true);
                if (m_mesh_material == null)
                {
                    m_mesh_material = base.transform.GetComponent<MeshRenderer>().sharedMaterial;
                    m_cur_color = m_mesh_material.color;
                }
            }, gameObject);
        }
    }
}