using System;

namespace Anthem
{
    /// <summary>
    /// Controls that implement this interface act as a container for
    /// other controls that may generate postbacks. When AddCallBacks
    /// is true, this control will inject callbacks into all the
    /// child controls.
    /// </summary>
    public interface ICallBackContainerControl
    {
        /// <summary>
        /// Gets or sets a value indicating whether the control should convert child control postbacks into callbacks.
        /// </summary>
        /// <value>
        /// 	<strong>true</strong> if the control should convert child control postbacks to
        /// callbacks; otherwise, <strong>false</strong>. The default is
        /// <strong>true</strong>.
        /// </value>
        /// <remarks>
        /// Only controls that <see cref="Anthem.Manager"/> recognizes will be converted.
        /// </remarks>
        bool AddCallBacks { get; set; }
    }
}
