namespace SmoothLingua.WebExample.Services;

using SmoothLingua.Abstractions;
using SmoothLingua.Abstractions.Stories;

public class ChatService : IChatService
{
    private Domain domain = new Domain(new List<Abstractions.NLU.Intent>()
        {
            new Abstractions.NLU.Intent("Hello", new List<string>(){ "Hello", "Hi" }),
            new Abstractions.NLU.Intent("Fine", new List<string>(){ "I am fine", "I am good" }),
            new Abstractions.NLU.Intent("Bad", new List<string>(){ "I am not good", "I feeling bad" }),
            new Abstractions.NLU.Intent("Bye", new List<string>(){ "Bye", "Good Bye" })
        }, new List<Abstractions.Stories.Story>()
        {
            new Abstractions.Stories.Story("FineStory", new List<Abstractions.Stories.Step>()
            {
                new IntentStep("Hello"),
                new ResponseStep("Hello from bot! How are you?"),
                new IntentStep("Fine"),
                new ResponseStep("I am happy to hear that!"),
            }),
            new Abstractions.Stories.Story("BadStory", new List<Abstractions.Stories.Step>()
            {
                new IntentStep("Hello"),
                new ResponseStep("Hello from bot! How are you?"),
                new IntentStep("Bad"),
                new ResponseStep("I am sorry to hear that!"),
            })
        }, new List<Abstractions.Rules.Rule>()
        {
            new Abstractions.Rules.Rule("ByeRule","Bye", "Bye from bot!")
        });

    private IAgent agent;

    public Domain GetDomain()
    {
        return domain;
    }

    public Response Handle(Guid conversationId, string input)
    {
        return agent.Handle(conversationId.ToString(), input);
    }

    public async Task Train(Domain domain, CancellationToken cancellationToken)
    {
        DomainValidator.Validate(domain);
        this.domain = domain;

        MemoryStream ms = new MemoryStream();
        var trainer = new Trainer();
        await trainer.Train(domain, ms, cancellationToken);

        agent = await AgentLoader.Load(new MemoryStream(ms.GetBuffer()), cancellationToken);
    }
}
