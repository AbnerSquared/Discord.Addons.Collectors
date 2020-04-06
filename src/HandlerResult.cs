namespace Discord.Addons.Collectors
{
    /// <summary>
    /// Defines the result of a <see cref="CollectionHandler"/> invocation.
    /// </summary>
    public enum HandlerResult
    {
        /// <summary>
        /// Closes the <see cref="CollectionHandler"/> and marks the proceeding action as a success.
        /// </summary>
        Success = 1,

        /// <summary>
        /// Closes the <see cref="CollectionHandler"/> and marks the proceeding action as a failure.
        /// </summary>
        Fail = 2,

        /// <summary>
        /// Allows the <see cref="CollectionHandler"/> to continue processing messages.
        /// </summary>
        Continue = 3
    }
}
