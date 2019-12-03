
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;

namespace WalkGame
{
    class CharacterEntity
    {
        static Texture2D characterSheetTexture;

        Animation walkDown;
        Animation walkUp;
        Animation walkLeft;
        Animation walkRight;

        Animation standDown;
        Animation standUp;
        Animation standLeft;
        Animation standRight;


        Animation currentAnimation;

        public float X { get; set; }
        public float Y { get; set; }
        
        public CharacterEntity(GraphicsDevice graphicsDevice)
        {
            if (characterSheetTexture == null)
            {
                using (var stream = TitleContainer.OpenStream("Content/charactersheet.png"))
                {
                    characterSheetTexture = Texture2D.FromStream(graphicsDevice, stream);
                }
            }

            LoadAnim();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 topLeftOfSprite = new Vector2(this.X, this.Y);
            var sourceRectangle = currentAnimation.CurrentRectangle;
            spriteBatch.Draw(characterSheetTexture, topLeftOfSprite, sourceRectangle, Color.White);
        }

        /// <summary>
        /// 加载动画
        /// </summary>
        public void LoadAnim()
        {
            walkDown = new Animation();
            walkDown.AddFrame(new Rectangle(0, 0, 16, 16), TimeSpan.FromSeconds(.25));
            walkDown.AddFrame(new Rectangle(16, 0, 16, 16), TimeSpan.FromSeconds(.25));
            walkDown.AddFrame(new Rectangle(0, 0, 16, 16), TimeSpan.FromSeconds(.25));
            walkDown.AddFrame(new Rectangle(32, 0, 16, 16), TimeSpan.FromSeconds(.25));

            walkUp = new Animation();
            walkUp.AddFrame(new Rectangle(144, 0, 16, 16), TimeSpan.FromSeconds(.25));
            walkUp.AddFrame(new Rectangle(160, 0, 16, 16), TimeSpan.FromSeconds(.25));
            walkUp.AddFrame(new Rectangle(144, 0, 16, 16), TimeSpan.FromSeconds(.25));
            walkUp.AddFrame(new Rectangle(176, 0, 16, 16), TimeSpan.FromSeconds(.25));

            walkLeft = new Animation();
            walkLeft.AddFrame(new Rectangle(48, 0, 16, 16), TimeSpan.FromSeconds(.25));
            walkLeft.AddFrame(new Rectangle(64, 0, 16, 16), TimeSpan.FromSeconds(.25));
            walkLeft.AddFrame(new Rectangle(48, 0, 16, 16), TimeSpan.FromSeconds(.25));
            walkLeft.AddFrame(new Rectangle(80, 0, 16, 16), TimeSpan.FromSeconds(.25));

            walkRight = new Animation();
            walkRight.AddFrame(new Rectangle(96, 0, 16, 16), TimeSpan.FromSeconds(.25));
            walkRight.AddFrame(new Rectangle(112, 0, 16, 16), TimeSpan.FromSeconds(.25));
            walkRight.AddFrame(new Rectangle(96, 0, 16, 16), TimeSpan.FromSeconds(.25));
            walkRight.AddFrame(new Rectangle(128, 0, 16, 16), TimeSpan.FromSeconds(.25));

            // Standing animations only have a single frame of animation:
            standDown = new Animation();
            standDown.AddFrame(new Rectangle(0, 0, 16, 16), TimeSpan.FromSeconds(.25));

            standUp = new Animation();
            standUp.AddFrame(new Rectangle(144, 0, 16, 16), TimeSpan.FromSeconds(.25));

            standLeft = new Animation();
            standLeft.AddFrame(new Rectangle(48, 0, 16, 16), TimeSpan.FromSeconds(.25));

            standRight = new Animation();
            standRight.AddFrame(new Rectangle(96, 0, 16, 16), TimeSpan.FromSeconds(.25));
        }


        public void Update(GameTime gameTime)
        {
            var velocity = GetDesiredVelocityFromInput();
            this.X += velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
            this.Y += velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(velocity == Vector2.Zero)
            {
                currentAnimation = (currentAnimation == walkRight) ? standRight :
                    (currentAnimation == walkLeft) ? standLeft :
                    (currentAnimation == walkUp) ? standUp :
                    (currentAnimation == walkDown) ? standDown :
                    standDown;
            }
            else
            {
                bool moveHorizontally = Math.Abs(velocity.X) > Math.Abs(velocity.Y);
                currentAnimation = (moveHorizontally && velocity.X > 0) ? walkRight :
                    (moveHorizontally && velocity.X < 0) ? walkLeft :
                    (!moveHorizontally && velocity.Y > 0) ? walkDown :
                    (!moveHorizontally && velocity.Y < 0) ? walkUp :
                    walkUp;
            }

            currentAnimation.Update(gameTime);
        }


        Vector2 GetDesiredVelocityFromInput()
        {

            // In Update, or some code called every frame:
            var gamePadState = GamePad.GetState(PlayerIndex.One);
            // Use gamePadState to move the character
            return new Vector2(gamePadState.ThumbSticks.Left.X, -gamePadState.ThumbSticks.Left.Y) * 200;

            //Vector2 desireVelocity = new Vector2();
            //TouchCollection touchLocations = TouchPanel.GetState();
            //if(touchLocations.Count > 0)
            //{
            //    desireVelocity.X = touchLocations[0].Position.X - this.X;
            //    desireVelocity.Y = touchLocations[0].Position.Y - this.Y;

            //    if (desireVelocity.X != 0 || desireVelocity.Y != 0)
            //    {
            //        desireVelocity.Normalize();
            //        const float desireSpeed = 200;
            //        desireVelocity *= desireSpeed;
            //    }
            //}

            //return desireVelocity;
        }

    }
}
