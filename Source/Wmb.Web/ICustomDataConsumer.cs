namespace Wmb.Web {
    /// <summary>
    /// This interface enable the developer to pass custom parameters to custom ImageRetrievers, ImageTransforms and CacheControllers
    /// </summary>
    public interface ICustomDataConsumer {
        /// <summary>
        /// Sets the custom data that this consumer consumes.
        /// </summary>
        /// <param name="data">The data.</param>
        void SetCustomData(string data);
    }
}
