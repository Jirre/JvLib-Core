namespace UnityEngine.Playables
{
    public static class PlayableDirectorExtensions
    {
        public static void SetSpeed(this PlayableDirector timeline, float speed)
        {
            timeline.playableGraph.GetRootPlayable(0).SetSpeed(speed);
        }
        
        public static float GetSpeed(this PlayableDirector timeline)
        {
            return (float)timeline.playableGraph.GetRootPlayable(0).GetSpeed();
        }
        
        /// <summary>
        /// The original timeline pause and resume is more like a stop. This stop certain behavior. Setting the
        /// speed to zero will react more like a pause. It will be more expensive because it keeps playing.
        /// </summary>
        public static void PauseSpeedBased(this PlayableDirector timeline)
        {
            timeline.SetSpeed(0);
        }

        /// <summary>
        /// The original timeline pause and resume is more like a stop. This stop certain behavior. Setting the
        /// speed to zero will react more like a pause. It will be more expensive because it keeps playing.
        /// </summary>
        public static void ResumeSpeedBased(this PlayableDirector timeline)
        {
            timeline.SetSpeed(1);
        }
    }

}
