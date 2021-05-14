using System.Net.Http;
using System.Threading.Tasks;

namespace Stencil.Plugins.GitHub.Integration
{
    public interface IGitHubWebHookValidator
    {
        Task<GitHubWebHookValidationResult> ValidateEventPayloadAsync(HttpRequestMessage request);
    }
}