namespace SmoothLingua.WebExample.Services;

using Abstractions;

public interface IChatService
{
    Task Train(Domain domain, CancellationToken cancellationToken);

    Response Handle(Guid conversationId, string input);

    Domain GetDomain();
}
