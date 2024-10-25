using System.Text.Json.Serialization;

namespace Serilog.Sinks.WeCom;

[JsonSerializable(typeof(WeComMessage))]
internal sealed partial class WeComMessageContent : JsonSerializerContext;

internal struct WeComMessage
{
    // ReSharper disable once StringLiteralTypo
    [JsonPropertyName("msgtype")] public string MessageType { get; set; } = "markdown";
    [JsonPropertyName("markdown")] public WeComMarkdown Markdown { get; set; }

    public WeComMessage()
    {
    }
}

internal struct WeComMarkdown
{
    [JsonPropertyName("content")] public string Content { get; set; } = string.Empty;

    public WeComMarkdown()
    {
    }
}