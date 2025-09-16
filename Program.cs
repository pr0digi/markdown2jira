using System.Text.RegularExpressions;
using TextCopy;

namespace MarkdownToJiraConverter;

class MarkdownToJiraConverter
{
    static async Task Main()
    {
        var markdown = await ClipboardService.GetTextAsync();

        if (markdown is null)
        {
            Console.WriteLine("No text in clipboard!");
            return;
        }

        markdown = Regex.Replace(markdown, "^# ", "h1. ", RegexOptions.Multiline);
        markdown = Regex.Replace(markdown, "^## ", "h2. ", RegexOptions.Multiline);
        markdown = Regex.Replace(markdown, "^### ", "h3. ", RegexOptions.Multiline);
        markdown = Regex.Replace(markdown, "^#### ", "h4. ", RegexOptions.Multiline);
        markdown = Regex.Replace(markdown, "^##### ", "h5. ", RegexOptions.Multiline);
        markdown = Regex.Replace(markdown, "^###### ", "h6. ", RegexOptions.Multiline);
        markdown = Regex.Replace(markdown, @"\*\*", "*", RegexOptions.Multiline);
        markdown = Regex.Replace(markdown, "^- ", "* ", RegexOptions.Multiline);
        markdown = Regex.Replace(markdown, "^  - ", "** ", RegexOptions.Multiline);
        markdown = Regex.Replace(markdown, "^    - ", "*** ", RegexOptions.Multiline);
        markdown = Regex.Replace(markdown, "^      - ", "**** ", RegexOptions.Multiline);
        markdown = Regex.Replace(markdown, "^```sql(.*?)```", "{code:sql}$1{code}", RegexOptions.Multiline | RegexOptions.Singleline);
        markdown = Regex.Replace(markdown, "^```csharp(.*?)```", "{code:c#}$1{code}", RegexOptions.Multiline | RegexOptions.Singleline);
        markdown = Regex.Replace(markdown, "^```json(.*?)```", "{noformat}$1{noformat}", RegexOptions.Multiline | RegexOptions.Singleline);
        markdown = Regex.Replace(markdown, "^```[^\n]*(.*?)```", "{noformat}$1{noformat}", RegexOptions.Multiline | RegexOptions.Singleline);

        // Remove second line of table
        markdown = Regex.Replace(markdown, "^\\|( -+ \\|)+\r?\n", "", RegexOptions.Multiline | RegexOptions.ExplicitCapture);

        // Convert || to | in table values
        markdown = Regex.Replace(
            markdown,
            "^(\r?\n)(\\|.*\\|\r?\n)",
            match => match.Groups[1].Value + match.Groups[2].Value.Replace("|", "||"),
            RegexOptions.Multiline);

        await ClipboardService.SetTextAsync(markdown);
        Console.WriteLine("Converted text has been inserted into clipboard.");
    }
}