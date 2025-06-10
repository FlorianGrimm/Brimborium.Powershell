
using Brimborium.Extensions.Logging.LocalFile;

using Microsoft.Extensions.DependencyInjection;

namespace Brimborium.Powershell;

public class BrimboriumPowershellSessionState {
    public BrimboriumPowershellSessionState(ServiceProvider serviceProvider) {
        ServiceProvider = serviceProvider;
    }

    public ServiceProvider ServiceProvider { get; }


    internal void FlushFileLogger() {
        if (this.ServiceProvider.GetServices<LocalFileLoggerProvider>() is {} listFileLoggerProvider) {
            foreach (var fileLoggerProvider in listFileLoggerProvider) {
                fileLoggerProvider.Flush();
            }
        }
    }

    internal void Dispose() {
    }
}
