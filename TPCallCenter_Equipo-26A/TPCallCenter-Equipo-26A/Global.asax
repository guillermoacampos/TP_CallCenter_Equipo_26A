<%@ Application Language="C#" %>

<script runat="server">
    void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup
        RegisterJQuery();
    }

    private void RegisterJQuery()
    {
        ScriptManager.ScriptResourceMapping.AddDefinition(
            "jquery",
            new ScriptResourceDefinition
            {
                Path = "~/Scripts/jquery-3.7.0.slim.js",
                DebugPath = "~/Scripts/jquery-3.7.0.slim.js",
                CdnPath = "https://code.jquery.com/jquery-3.7.0.slim.min.js",
                CdnDebugPath = "https://code.jquery.com/jquery-3.7.0.slim.js",
                CdnSupportsSecureConnection = true,
                LoadSuccessExpression = "window.jQuery"
            });
    }
</script>