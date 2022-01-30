using UnityEngine;

namespace Game.Core
{
    /// <summary>
    /// Custom attribute to require a passed interface.
    /// </summary>
    public class RequireInterfaceAttribute : PropertyAttribute
    {
        public System.Type requiredType { get; private set; }

        /// <summary>
        /// Constructor for the RequireInterfaceAttribute. Sets the requiredType
        /// data member to the passed type.
        /// </summary>
        /// <param name="type">Interface type required.</param>
        public RequireInterfaceAttribute(System.Type type)
        {
            this.requiredType = type;
        }
    }
}
