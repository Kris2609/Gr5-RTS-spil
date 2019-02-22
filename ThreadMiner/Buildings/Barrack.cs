using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ThreadMiner
{
    class Barrack : Building
    {
        private float trainingTime;
        public static new int cost = 100;

        public Barrack(GameWorld currentGame, Vector2 pos, string spriteName) : base(200, currentGame, pos, spriteName)
        {
            this.trainingTime = 5000;
            accesLimiter = new Mutex();
        }

        public Mutex accesLimiter;

        public float TrainingTime { get => trainingTime; set => trainingTime = value; }

        public void TrainUnit(Worker worker)
        {
            currentGame.units.Remove(worker);
            Swordsman sMan = new Swordsman(currentGame, worker.Pos, 30, Swordsman.WarriorJob.Idle);
            sMan.targetBuilding = this;
            currentGame.units.Add(sMan);

        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
