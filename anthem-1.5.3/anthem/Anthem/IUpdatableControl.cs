using System;

namespace Anthem
{
    /// <summary>
    /// Controls that implement this interface can have their HTML
    /// updated on the client page after a call back returns.
    /// </summary>
    public interface IUpdatableControl
    {
        /// <summary>
        ///     Gets or sets a value which indicates whether the control should be updated after
        ///     the current callback. Also see <see cref="AutoUpdateAfterCallBack"/>.
        /// </summary>
        /// <value>
        /// 	<strong>true</strong> if the the control should be updated; otherwise,
        /// <strong>false</strong>. The default is <strong>false</strong>.
        /// </value>
        /// <example>
        /// 	<code lang="CS" description="This is normally used in server code as shown here.">
        /// this.Label = "Count = " + count;
        /// this.Label.UpdateAfterCallBack = true;
        ///     </code>
        /// </example>
        bool UpdateAfterCallBack { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the control should be updated after each callback.
        /// Also see <see cref="UpdateAfterCallBack"/>.
        /// </summary>
        /// <value>
        /// 	<strong>true</strong> if the the control should be updated; otherwise,
        /// <strong>false</strong>. The default is <strong>false</strong>.
        /// </value>
        /// <example>
        /// 	<code lang="CS" description="This is normally used declaratively as shown here.">
        /// &lt;anthem:Label id="label" runat="server" AutoUpdateAfterCallBack="true" /&gt;
        ///     </code>
        /// </example>
        bool AutoUpdateAfterCallBack { get; set; }
    }
}
