namespace NotificationService.Application.Helpers;

internal static class NotificationTemplateHelper
{
    private const string TargetFolderName = "NotificationService";
    private const string TemplatesFolderName = "NotificationTemplates";
    private const string TemplateFileName = "Notification.html";
    private const string ContentKey = "{Content}";
    
    private static readonly string TemplateContent = LoadTemplate();

    public static string BuildTemplate(string content)
    {
        return TemplateContent.Replace(ContentKey, content);
    }

    private static string LoadTemplate()
    {
        var templateRootPath = GetTemplatesRootPath();
        var filePath = Path.Combine(templateRootPath, TemplateFileName);

        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Template file not found: {filePath}");

        return File.ReadAllText(filePath);
    }

    private static string GetTemplatesRootPath()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory != null && !directory.Name.Equals(TargetFolderName, StringComparison.CurrentCultureIgnoreCase))
            directory = directory.Parent;

        if (directory == null)
            throw new DirectoryNotFoundException($"Could not find the target folder");

        var templatePath = Path.Combine(directory.FullName, TemplatesFolderName);
        if (!Directory.Exists(templatePath))
            throw new DirectoryNotFoundException($"Templates folder not found at path: {templatePath}");

        return templatePath;
    }
}