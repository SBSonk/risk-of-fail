namespace RiskOfFail.Combat
{
    public class Enemy : Alive
    {
        public static float globalEnemyHealthScale = 1;
        
        protected override void Initialize()
        {
            base.Initialize();
            
            health *= globalEnemyHealthScale;
            maxHealth *= globalEnemyHealthScale;
        }
    }
}