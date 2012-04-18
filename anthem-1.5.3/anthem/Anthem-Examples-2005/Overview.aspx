<%@ Page Language="C#" MasterPageFile="~/Sample.master" %>

<%@ Register Assembly="Anthem" Namespace="Anthem" TagPrefix="anthem" %>
<%@ Import Namespace="System.Data" %>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder">
    <h2>
        History</h2>
    <p>
        Anthem was originally written as an exercise for myself during one of the ASP.NET
        classes I (Jason Diamond) teach for DevelopMentor. A student had showed me how he
        was using Michael Schwarz' Ajax.NET which seemed cool but had one serious limitation
        (in my opinion): it did not integrate with the page lifecycle which prevented you
        from accessing the server-side controls on your page during call backs.</p>
    <p>
        So I spent some time during our labs that week to write what I was originally calling
        "My Ajax.NET". I gave it to my student who started using it on his project. I also
        posted about it on my weblog where other people found it (I have no idea other people
        were even reading my weblog) and started using it.</p>
    <p>
        Eventually, I realized the name "My Ajax.NET" was too confusing so renamed the project
        to "Anthem". It's now hosted on SourceForge with a small, but active, community
        of users.</p>
    <h2>
        How It Works</h2>
    <p>
        Anthem is actually really simple. When you trigger a call back from your page, the
        XMLHttpRequest object is used to POST back to the page. I try, as much as possible,
        to emulate what a normal POST request would look like. This means that the request
        contains the values for all of the form controls on the page including the ASP.NET
        specific hidden field like __VIEWSTATE. As far as the page on the server is concerned,
        a normal post back is occuring. The page fires its Init event, transfers state into
        the controls, fires its Load event, performs validation, etc. I even let it go through
        its normal rendering process.</p>
    <p>
        After the page on the server does its thing, I capture the HTML of all the "Anthem"
        controls on the page and "return" that to the client which uses innerHTML to update
        those controls right there in the browser. This is, admittedly, a hack but it works
        surprisingly well. And, for the most part, you can just pretend you're working with
        "normal" ASP.NET pages and controls without having to learn a whole new set of APIs.</p>
    <h2>
        Support</h2>
    <p>
        I work on Anthem because I enjoy it. I'm not charging anybody any money for it which
        means that nobody has the right to expect me to support them. If you want my help,
        I'll be happy to do so but you'll have to wait until I can find the time. Note that
        you'll have a better chance of me not "losing" your bug report by submitting via
        the tracker on SourceForge. I get a <em>lot</em> of email and have a really hard
        time trying to reply to it all. I try but often fail.</p>
    <p>
        To help me help you, please submit your bug reports as a single-page example (much
        like the pages in the project you're staring at right now). If there are any dependencies
        on databases, web services, or code that I can't run, I'll immediately push your
        request to the end of my queue which means I'll probably never get to it.</p>
    <p>
        There are two mailing lists specific that you can subscribe to via the Anthem project
        summary page on SourceForge. One is intended for users of Anthem and the other is
        intended for developers. Feel free to subscribe to either or both. All SourceForge
        tracker items and CVS commit messages get emailed to the developer list which makes
        it easy for you to keep updated on how Anthem is changing.</p>
</asp:Content>
