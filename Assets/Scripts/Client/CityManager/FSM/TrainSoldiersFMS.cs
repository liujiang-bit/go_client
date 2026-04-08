using System;
using System.Collections.Generic;
using Client.FSM.TrainState;
using Skyunion;
using UnityEngine;
using UnityEngine.UI;

namespace Client.FSM
{
    public class TrainSoldiersFMS: WorkerFMS
    {

      
        private Vector2 OffPos;
        private Vector2 DirPos;

        private bool isTraining = false;

       
        
        public override void AddStates()
        {
            //set the custom update frequenct
            SetUpdateFrequency(0.1f);
            
            
            AddState<TrainIdeState>();
            
            AddState<TrainFindHomeState>();

            SetInitialState<TrainIdeState>();
        }


        public void WaitFor(Vector2 bornPos,Vector2 offPos,Vector2 dirPos)
        {
            this.BornPos = bornPos;
            this.OffPos = offPos;
            this.DirPos = dirPos;
            Owner.transform.localPosition = Finder.ConvertTileToLocal(BornPos.x+OffPos.x, BornPos.y+OffPos.y,true);
            
            buildDirPos = new Vector2(Owner.transform.position.x,Owner.transform.position.z)+DirPos;
            
            

            Idle();
        }

        public void Idle()
        {
            CrrWorkState = WorkerState.IDLE;


            if (isTraining)
            {
                CrrWalkState = People.ENMU_CITIZEN_STAT.FIGHT;
            }
            else
            {
                CrrWalkState = People.ENMU_CITIZEN_STAT.IDLE;
            }

            Owner.transform.localPosition = Finder.ConvertTileToLocal(BornPos.x+OffPos.x, BornPos.y+OffPos.y);

            var worldPos =
                Finder.MakeWorldPosFormLocal(Owner.transform.localPosition.x, Owner.transform.localPosition.z,true);
            buildDirPos = new Vector2(Owner.transform.position.x,Owner.transform.position.z)+DirPos;
            
            People.SetStateS(Owner, CrrWalkState, worldPos, buildDirPos, CrrMoveSpeed);
            
        }

        public void Train()
        {
            
            isTraining = true;

            if ( CrrWorkState != WorkerState.FindHome)
            {
                CrrWorkState = WorkerState.IDLE;
                CrrWalkState = People.ENMU_CITIZEN_STAT.FIGHT;

                var worldPos =
                    Finder.MakeWorldPosFormLocal(Owner.transform.localPosition.x, Owner.transform.localPosition.z,true);
                buildDirPos = new Vector2(Owner.transform.position.x,Owner.transform.position.z)+DirPos;
            
                People.SetStateS(Owner, CrrWalkState, worldPos, buildDirPos, CrrMoveSpeed);
            }
            
          
        }

        public void TrainStop()
        {
            isTraining = false;

            if (CrrWorkState != WorkerState.FindHome)
            {
                Idle();
            }

            
        }




        public void FindNewHomePos(Vector2 bornPos)
        {
            Vector2 startPos = Finder.ConvertLocalToTile(Owner.transform.localPosition);;
            this.BornPos = bornPos;
            
            Vector2 endPos = new Vector2(BornPos.x+OffPos.x, BornPos.y+OffPos.y);

            var time = (index ) * 0.2f;
            
            CrrWorkState = WorkerState.FindHome;
            CrrWalkState = People.ENMU_CITIZEN_STAT.MOVE;
           
            
            if (pathIndex == 0)
            {
                time = 0;
            }
            
            pathIndex = 0;
            FindPaths = Finder.GetFindWorldPaths( startPos, endPos,true);
            
            
            Timer.Register(time, () =>
            {
                ChangeState<TrainFindHomeState>();
            });
            
        }


    }
}