namespace WebApp.Helpers;

public static class EmojiHelper
{
    public static string GetEmoji(string title)
    {
        return title switch
        {
            "Kontantinsats" => "🏡",
            "Pension" => "🏦",
            "Buffert" => "💰",
            _ => "💸"
        };
    }
}