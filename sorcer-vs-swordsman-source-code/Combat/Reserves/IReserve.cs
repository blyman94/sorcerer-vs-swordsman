namespace Game.Combat.Reserves
{
    public delegate void CurrentChanged(float newCurrent, bool damage);
    public delegate void MaxChanged(float newMax);
    public delegate void ReserveEmpty();

    /// <summary>
    /// Describes the behaviour of a combat reserve (such as health or mana).
    /// </summary>
    public interface IReserve
    {
        /// <summary>
        /// The reserve's current amount.
        /// </summary>
        float Current { get; set; }

        /// <summary>
        /// The reserve's maximum amount.
        /// </summary>
        float Max { get; set; }

        /// <summary>
        /// Invoked when the current reserve amount changes.
        /// </summary>
        event CurrentChanged CurrentChanged;

        /// <summary>
        /// Invoked when the maximum reserve amount changes.
        /// </summary>
        event MaxChanged MaxChanged;

        /// <summary>
        /// Invoked when the reserve is empty.
        /// </summary>
        event ReserveEmpty Empty;

        /// <summary>
        /// Changes the current amount of the reserve by the passed amount.
        /// </summary>
        /// <param name="amount">Amount by which to modify the reserve.</param>
        void Modify(float amount);
    }
}
