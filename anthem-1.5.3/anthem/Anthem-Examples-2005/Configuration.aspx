<%@ Page Language="C#" MasterPageFile="~/Sample.master" %>

<asp:Content ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <h2>
        Configuration</h2>
    <p>
        I tried really hard to make it as easy as possible to use Anthem. In order to help
        make that happen, I tried to make it my goal to require zero configuration. I think
        I did a pretty good job.</p>
    <p>
        Starting with Anthem 1.1.0, there is an optional bit of configuration you can do
        to make your Anthem experience a little more pleasant if you're still using ASP.NET
        1.1.</p>
    <p>
        By default, the JavaScript necessary for Anthem to work is injected into each page.
        This is only if you're still on ASP.NET 1.1--if you're on 2.0, it uses web resources.</p>
    <p>
        Since Anthem requires a good amount of JavaScript, you can add an option to your
        <code>appSettings</code> element in your web.config file to inject a script "include"
        into your page instead of the actual script. The key for that setting should be
        "Anthem.BaseUri". The value needs to be either an absolute URI or the empty string.</p>
    <p>
        If it's an absolute URI, it needs to point to the folder on some server that contains
        the Anthem.js file.</p>
    <p>
        If it's the empty string, it's assumed that Anthem.js exists in the same directory
        as your pages. This will obviously break if you have some of your pages in sub-directories.</p>
    <p>
        Here's an example:</p>
<pre><code>&lt;configuration&gt;
  &lt;appSettings&gt;
    &lt;add key="Anthem.BaseUri" value="~/" /&gt;
  &lt;/appSettings&gt;
&lt;/configuration&gt;</code></pre>
    <p>
        Please note that this points to a directory containing the file and not the actual
        file. In the future, Anthem might get more script files that could be optionally
        included in pages so this setting could be used for those as well.</p>
</asp:Content>
