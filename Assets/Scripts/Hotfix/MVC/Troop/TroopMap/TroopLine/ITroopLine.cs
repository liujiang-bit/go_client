using System.Collections.Generic;
using Game;
using UnityEngine;

namespace Hotfix
{
    public interface ITroopLine
    {
        Color GetLineColor(int id);
        void AddSummonerTroopLine(int lineId, List<Vector2> path);
        void RemoveSummonerTroopLine(int lineId);
        void AddTroopLine(int lineId, List<Vector2> path, Color color);
        void UpdateLineColor(int id);
        void UpdateAllLineColor();
        void UpdateTroopLine(MapViewLevel level);
        void RemoveTroopLine(int id);
        void RemoveLine(int id);
        void RemoveTroopLines();
        void SetAoiTroopLines(int id,List<Vector2> path,Color color);
        void RemoveAoiTroopLines(int id);
        void UpdateAoiTroopLinesColor(int id,Color color);
    }
}