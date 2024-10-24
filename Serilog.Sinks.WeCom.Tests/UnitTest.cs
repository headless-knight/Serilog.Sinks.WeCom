using Serilog.Events;

namespace Serilog.Sinks.WeCom.Tests;

public class UnitTest
{
    [Fact]
    public async Task Test()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.WeCom("https://qyapi.weixin.qq.com/cgi-bin/webhook/send?key=example",
                properties: new Dictionary<string, string>
                {
                    ["key1"] = "value1",
                    ["key2"] = "value2"
                },
                minimumLevel: LogEventLevel.Warning)
            .CreateLogger();
        Log.Information(new Exception("Test Exception1"), "Test Message1!");
        Log.Warning(new Exception("Test Exception2"), "Test Message2!");
        Log.Error(new Exception("Test Exception3"), "Test Message3!");
        await Log.CloseAndFlushAsync();
    }
}