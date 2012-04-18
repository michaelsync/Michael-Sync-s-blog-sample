#if V2

using System;
using System.ComponentModel;
using System.Drawing;
using System.Web.UI;
using ASP = System.Web.UI.WebControls;

namespace Anthem
{
    /// <summary>
    /// Represents a control that acts as a container for a group of controls within a <see cref="MultiView"/> control.
    /// </summary>
    [ToolboxBitmap(typeof(ASP.View))]
    public class View : ASP.View
    {
        // No new functionality...this is just here to help intellisense 
        // recognize the classes in the designer source view
    }
}

#endif