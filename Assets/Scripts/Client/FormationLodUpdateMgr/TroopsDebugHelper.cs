using UnityEngine;

namespace Client
{
    public class TroopsDebugHelper : MonoBehaviour
    {
        public GameObject m_state;

        public GameObject m_FCP;

        public GameObject m_NCP;

        public GameObject m_NFTP;

        public GameObject m_CNCDirArrow;

        public GameObject m_CNTDirArrow;

        public GameObject m_CNTNDirArrow;

        public void UpdateDebugData(Troops.ENMU_SQUARE_STAT stat, Vector2 fCurPos, Vector2 newCurPos, Vector2 newTargetPos)
        {
            string text = string.Empty;
            switch (stat)
            {
                case Troops.ENMU_SQUARE_STAT.DEAD:
                    text = "Dead";
                    break;
                case Troops.ENMU_SQUARE_STAT.FIGHT:
                    text = "Fight";
                    break;
                case Troops.ENMU_SQUARE_STAT.IDLE:
                    text = "Idle";
                    break;
                case Troops.ENMU_SQUARE_STAT.MOVE:
                    text = "Move";
                    break;
            }
            m_state.GetComponent<TextMesh>().text = text;
            m_FCP.GetComponent<TextMesh>().text = fCurPos.ToString();
            m_NCP.GetComponent<TextMesh>().text = newCurPos.ToString();
            m_NFTP.GetComponent<TextMesh>().text = newTargetPos.ToString();
            Vector2 lhs = newCurPos - fCurPos;
            if (lhs == Vector2.zero)
            {
                m_CNCDirArrow.SetActive(value: false);
            }
            else
            {
                m_CNCDirArrow.SetActive(value: true);
                m_CNCDirArrow.transform.up = new Vector3(lhs.x, lhs.y, 0f);
            }
            lhs = newTargetPos - fCurPos;
            m_CNTDirArrow.transform.up = new Vector3(lhs.x, lhs.y, 0f);
            lhs = newTargetPos - newCurPos;
            m_CNTNDirArrow.transform.up = new Vector3(lhs.x, lhs.y, 0f);
        }
    }
}