using System;
using UnityEngine;

namespace ROK
{
    public class FormationDebugHelper : MonoBehaviour
    {
        public GameObject m_state;

        public GameObject m_FCP;

        public GameObject m_NCP;

        public GameObject m_NFTP;

        public GameObject m_CNCDirArrow;

        public GameObject m_CNTDirArrow;

        public GameObject m_CNTNDirArrow;

        public void UpdateDebugData(Formation.ENMU_SQUARE_STAT stat, Vector2 fCurPos, Vector2 newCurPos, Vector2 newTargetPos)
        {
            string text = string.Empty;
            switch (stat)
            {
                case Formation.ENMU_SQUARE_STAT.IDLE:
                    text = "Idle";
                    break;
                case Formation.ENMU_SQUARE_STAT.MOVE:
                    text = "Move";
                    break;
                case Formation.ENMU_SQUARE_STAT.FIGHT:
                    text = "Fight";
                    break;
                case Formation.ENMU_SQUARE_STAT.DEAD:
                    text = "Dead";
                    break;
            }
            this.m_state.GetComponent<TextMesh>().text = text;
            this.m_FCP.GetComponent<TextMesh>().text = fCurPos.ToString();
            this.m_NCP.GetComponent<TextMesh>().text = newCurPos.ToString();
            this.m_NFTP.GetComponent<TextMesh>().text = newTargetPos.ToString();
            Vector2 lhs = newCurPos - fCurPos;
            if (lhs == Vector2.zero)
            {
                this.m_CNCDirArrow.SetActive(false);
            }
            else
            {
                this.m_CNCDirArrow.SetActive(true);
                this.m_CNCDirArrow.transform.up = new Vector3(lhs.x, lhs.y, 0f);
            }
            lhs = newTargetPos - fCurPos;
            this.m_CNTDirArrow.transform.up = new Vector3(lhs.x, lhs.y, 0f);
            lhs = newTargetPos - newCurPos;
            this.m_CNTNDirArrow.transform.up = new Vector3(lhs.x, lhs.y, 0f);
        }
    }
}