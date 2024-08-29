using Drifter.Class.Factory;
using Drifter.Class.Tools;
using Drifter.Class.Tools.CollisionShapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drifter.Class.AbstractClass
{
    internal abstract class GameObject
    {
        protected int travelSpeed { get; set; }

        protected Texture2D ObjectTexture { get; set; }

        public Texture2D Texture { get { return ObjectTexture; } }

        protected Vector2 Position;

        public CollisionCircle collisionCircle;

        public bool isMovingLeft = false;

        protected AnimationPlayer animationPlayer;

        public Rectangle CurrentAnimationRectangle { get; protected set; }

        protected virtual void UpdateCollisionCircle()
        {
            this.collisionCircle.Centre = this.Position + new Vector2(8, 8);
        }

        public Vector2 CurrentPosition { get { return Position; } }


        protected SoundEffectInstance soundEffectInstance;

        //IsMovingNegative only used for player
        public virtual void Run(bool IsMovingNegative, float EndOfScreenPosition)
        {
            UpdateCollisionCircle();
            DidExitScreen(EndOfScreenPosition);
        }
        
        public int TravelSpeed { get {  return travelSpeed; } }


        public virtual void CollidedWithOtherGameObject(GameObject gameObject = null) { 
            DestroyMyself();
        }


        public virtual void SetSoundEffectInstance(string soundEffectName)
        {
            this.soundEffectInstance = Globals.GetSoundEffect(soundEffectName).CreateInstance();
        }

        protected virtual void DidExitScreen(float EndOfScreenPosition)
        {
            if (Position.Y > EndOfScreenPosition + 128 || Position.Y < 0 - 128)
            {
                DestroyMyself();
            }
        }

        protected void DestroyMyself()
        {


            if (soundEffectInstance is not null)
            {
                soundEffectInstance.IsLooped = false;
            }
            GameObjectSpawner.AddToDeleteList(this);
        }


        protected virtual void CheckObjectAtEdge()
        {

            if (CurrentPosition.X < 0)
            {
                SetPositionAtEdgeOfScreen(0);
            }

            else if (CurrentPosition.X + 16 > Globals.ScreenWidth - 16)
            {
                SetPositionAtEdgeOfScreen(Globals.ScreenWidth - 32);
            }
        }


        public void SetPositionAtEdgeOfScreen(float edgePosition)
        {
            this.Position.X = edgePosition;
        }

        public abstract void PlayAnimation();

        public void PlayDeathAnimation()
        {

        }
    }
}
