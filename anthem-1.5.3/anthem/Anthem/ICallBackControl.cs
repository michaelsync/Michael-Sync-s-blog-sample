using System;

namespace Anthem
{
    /// <summary>
    /// Controls that implement this interface will implement a callback
    /// for each supported event, unless EnableCallBack is false;
    /// </summary>
    public interface ICallBackControl
    {
        /// <summary>
        /// Gets or sets the javascript function to execute on the client if the callback is
        /// cancelled. See <see cref="PreCallBackFunction"/>.
        /// </summary>
        string CallBackCancelledFunction { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the control uses callbacks instead of postbacks to post data to the server.
        /// </summary>
        /// <value>
        /// 	<strong>true</strong> if the the control uses callbacks; otherwise,
        /// <strong>false</strong>. The default is <strong>true</strong>.
        /// </value>
        bool EnableCallBack { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the control is enabled on the client during callbacks.
        /// </summary>
        /// <value>
        /// 	<strong>true</strong> if the the control is enabled; otherwise,
        /// <strong>false</strong>. The default is <strong>true</strong>.
        /// </value>
        /// <remarks>Not all HTML elements support this property.</remarks>
        bool EnabledDuringCallBack { get; set; }

        /// <summary>
        /// Gets or sets the javascript function to execute on the client after the callback
        /// response is received.
        /// </summary>
        /// <remarks>
        /// The callback response is passed into the PostCallBackFunction as the one and only
        /// parameter.
        /// </remarks>
        /// <example>
        /// 	<code lang="JScript" description="This example shows a PostCallBackFunction that displays the error if there is one.">
        /// function AfterCallBack(result) {
        ///   if (result.error != null &amp;&amp; result.error.length &gt; 0) {
        ///     alert(result.error);
        ///   }
        /// }
        ///     </code>
        /// </example>
        string PostCallBackFunction { get; set; }

        /// <summary>
        /// Gets or sets the javascript function to execute on the client before the callback
        /// is made.
        /// </summary>
        /// <remarks>The function should return false on the client to cancel the callback.</remarks>
        string PreCallBackFunction { get; set; }

        /// <summary>Gets or sets the text to display on the client during the callback.</summary>
        /// <remarks>
        /// If the HTML element that invoked the callback has a text value (such as &lt;input
        /// type="button" value="Run"&gt;) then the text of the element is updated during the
        /// callback, otherwise the associated &lt;label&gt; text is updated during the callback.
        /// If the element does not have a text value, and if there is no associated &lt;label&gt;,
        /// then this property is ignored.
        /// </remarks>
        string TextDuringCallBack { get; set; }
    }
}
